using System;
using CryptoCA.Core.Annotations;
using OpenSSL.Core;
using OpenSSL.Crypto;
using OpenSSL.X509;

namespace CryptoCA.Core
{
    /// <summary>
    /// THE FUCKING BIBLE : http://marvinalpaca.com/blog/index.php/tag/c/
    /// SAME TRUC : http://marvinalpaca.com/blog/index.php/creating-self-signed-x-509-certificates-using-openssl-net/
    /// </summary>
    public class CertificationAutority
    {
        //Chemin d'enregistrement des fichiers
        private const string PATH = @"S:\Data\NDG_A_RECUPERER\WebSiteCA\";
        //Mot de passe des certificats
        private const string MDP = "BADASSdu94";
        public X509CertificateAuthority MetaCertificateAuthority { get; private set; }

        #region Creation de l'autorité de certification

        public void GenerateCACertificate()
        {
            // Initialize the following with your information
            var serial = 1234;
            var issuer = new X509Name("Flop Corp");
            var subject = new X509Name("Flop");

            // Creates the key pair
            var rsa = new RSA();
            rsa.GenerateKeys(2048, 0x10001, null, null);

            // Creates the certificate
            var key = new CryptoKey(rsa);
            var cert = new X509Certificate(serial, subject, issuer, key, DateTime.Now, DateTime.Now.AddYears(1));

            // Dumps the certificate into a .cer file
            //var bio = BIO.File("C:/temp/cert.cer", "w");
            //var bio = BIO.File(@"S:\Data\NDG_A_RECUPERER\WebSiteCA\cert.cer", "w");
            var bio = BIO.File(PATH + "cert.cer", "w");

            cert.Write(bio);
        }

        /// <summary>
        /// Creation du certificat de l'autorité de certification
        /// </summary>
        public void GenerateCACertificateV2()
        {
            //Fabrication de la clé privée en RSA
            var cryptoKey = CreateNewRSAKey(2048);
            //Les infos sur la CA
            var subject = GetCertificateAuthoritySubject();
            //Options supplémentaires
            var extensions = GetCertificateAuthorityExtensions();
            //Création du certificat auto-signé représentant la CA
            var ca = X509CertificateAuthority.SelfSigned(new SimpleSerialNumber(),
                cryptoKey, MessageDigest.SHA512, subject, DateTime.UtcNow,
                TimeSpan.FromDays(365), extensions);

            MetaCertificateAuthority = ca;

            //Enregistrement du certificat dans un fichier
            using (var bio = BIO.File(PATH + "CA Certificate.cer", "w"))
            {
                ca.Certificate.Write(bio);
            }

            //Enregistrement de la clé privée
            using (var bio = BIO.File(PATH + "CA private key.key", "w"))
            {
                cryptoKey.WritePrivateKey(bio, Cipher.DES_EDE3_CBC, MDP);
            }

            //Idem clé public
            using (var bio = BIO.File(PATH + "CA public key.key", "w"))
            {
                ca.Certificate.PublicKey.WritePrivateKey(bio, Cipher.DES_EDE3_CBC, MDP);
            }
        }

        /// <summary>
        /// Création des infos complémentaires
        /// </summary>
        /// <returns></returns>
        public X509V3ExtensionList GetCertificateAuthorityExtensions()
        {
            var extensions = new X509V3ExtensionList
            {
                new X509V3ExtensionValue("basicConstraints", true, "CA:TRUE"),
                new X509V3ExtensionValue("subjectKeyIdentifier", false, "hash"),
                new X509V3ExtensionValue("authorityKeyIdentifier", false, "keyid:always,issuer:always")
            };

            return extensions;
        }

        /// <summary>
        /// Informations concernant l'autorité de certification (faudra les récupérer du site web)
        /// </summary>
        /// <returns></returns>
        public X509Name GetCertificateAuthoritySubject()
        {
            //Un peu plus d'infos que le minimum syndical
            var subject = new X509Name
            {
                Common = "Flop Self Signing Certificate",
                Country = "FR",
                StateOrProvince = "WeshCountry",
                Organization = "FlopCorporation",
                OrganizationUnit = "BatmanR&D"
            };

            return subject;
        }

        /// <summary>
        /// Création d'une clé privée
        /// </summary>
        /// <param name="numberOfBits">Nombre de bits pour le cryptage</param>
        /// <returns></returns>
        public CryptoKey CreateNewRSAKey(int numberOfBits)
        {
            using (var rsa = new RSA())
            {
                //Doit etre un nombre premier
                BigNumber exponent = 0x10001; 
                rsa.GenerateKeys(numberOfBits, exponent, null, null);

                return new CryptoKey(rsa);
            }
        }

        #endregion

        #region Ajout de requete (CSR) de certificat et acceptation

        /// <summary>
        /// Fonction qui va créer la requete de certification et signer le certificat
        /// </summary>
        /// <param name="version"></param>
        public void GenerateSignedCertificate(string version)
        {
            //création de la requete
            var x509Request = CreateCertificateSigningRequest();

            //la suite ne se fait que si on a une autorité de certification
            if (MetaCertificateAuthority != null)
            {
                //Signature de la reqeuete pour un an
                var signedCert = MetaCertificateAuthority.ProcessRequest(x509Request, DateTime.UtcNow,
                    DateTime.UtcNow.AddYears(1), MessageDigest.SHA512);
                //Plusieurs versions d'enregistrement possibles
                switch (version.ToUpper().Trim())
                {
                    case "DER":
                        using (var bio = BIO.File(PATH + signedCert.Subject.Common + "-cert.cer", "w"))
                        {
                            signedCert.Write_DER(bio);
                        }
                        break;

                    case "PEM":
                        using (var bio = BIO.File(PATH + signedCert.Subject.Common + "-cert.cer", "w"))
                        {
                            signedCert.Write(bio);
                        }
                        break;

                    case "PKCS":
                        using (var bio = BIO.File(PATH + signedCert.Subject.Common + "-cert.cer", "w"))
                        using (var caStack = new Stack<X509Certificate>())
                        using (var pfx = new PKCS12(MDP, signedCert.PrivateKey, signedCert, caStack))
                        {
                            pfx.Write(bio);
                        }
                        break;
                    default:
                        Console.WriteLine("BAD PARAMETER");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Certification authority cant be null");
            }
        }

        /// <summary>
        /// Fonction de création de CSR
        /// </summary>
        /// <returns></returns>
        public X509Request CreateCertificateSigningRequest()
        {
            //Ajout des info complémentaires
            using (var requestDetails = GetCertificateSigningRequestSubject())
            //Création de sa clé et de la requete
            using (var key = CreateNewRSAKey(4096))
            {
                // Version 2 = X.509 Version 3
                int version = 2; 
                return new X509Request(version, requestDetails, key);
            }
        }

        /// <summary>
        /// Fonction d'ajout des infos du CSR (faudra les récupérer du site web)
        /// </summary>
        /// <returns></returns>
        public X509Name GetCertificateSigningRequestSubject()
        {
            var requestDetails = new X509Name();

            requestDetails.Common = "Cahir Mawr Dyffryn aep Ceallach";
            requestDetails.Country = "FR";
            requestDetails.StateOrProvince = "WeshCountry";
            requestDetails.Organization = "FlopCorporation";
            requestDetails.OrganizationUnit = "BatmanR&D";

            return requestDetails;
        }

        #endregion

        #region Revocation de certificats dans la base



        #endregion
    }
}