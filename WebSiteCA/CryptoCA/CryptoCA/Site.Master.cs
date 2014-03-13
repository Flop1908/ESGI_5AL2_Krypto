using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CryptoCA.Core.Annotations;
using OpenSSL.Core;
using OpenSSL.Crypto;
using OpenSSL.X509;


namespace CryptoCA
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        public static X509CertificateAuthority CertifAuthority;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
