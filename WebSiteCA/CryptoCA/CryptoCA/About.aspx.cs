using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CryptoCA.Core;
using Org.BouncyCastle.Crypto.Generators;

namespace CryptoCA
{
    public partial class About : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var certificationAutority = new CertificationAutority();
            certificationAutority.GenerateCACertificate();

        }
    }
}
