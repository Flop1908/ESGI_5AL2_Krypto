﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CryptoCA.Core;
using CryptoCA.Core.Annotations;
using OpenSSL.Core;
using OpenSSL.Crypto;
using OpenSSL.X509;

namespace CryptoCA
{
    
    public partial class _Default : System.Web.UI.Page
    {
        public static X509CertificateAuthority CertifAuthority;
        public static string tmppath = Path.GetTempPath();

        protected void Page_Load(object sender, EventArgs e)
        {
            GetDDLCountry();
            
        }

        protected void btn_valider_Click(object sender, EventArgs e)
        {
            var ca = new CertificationAutority();
            CertifAuthority = ca.GenerateCACertificateV2(tb_ca.Text, tb_ville.Text, ddl_pays.SelectedValue, tb_entreprise.Text, tb_service.Text);

            string filename = tmppath + "CA-Certificate.cer";
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filename);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileInfo.Name);
            Response.AppendHeader("Content-Length", fileInfo.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.WriteFile(fileInfo.FullName);
            Response.End();
        }

        protected void GetDDLCountry()
        {

            Dictionary<string, string> objDic = new Dictionary<string, string>();

            foreach (CultureInfo ObjCultureInfo in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                RegionInfo objRegionInfo = new RegionInfo(ObjCultureInfo.Name);
                if (!objDic.ContainsKey(objRegionInfo.EnglishName))
                {
                    objDic.Add(objRegionInfo.EnglishName, objRegionInfo.TwoLetterISORegionName.ToUpper());
                }
            }

            var obj = objDic.OrderBy(p => p.Key);
            foreach (KeyValuePair<string, string> val in obj)
            {
                ddl_pays.Items.Add(new ListItem(val.Key, val.Value));
            }
        }
    }
}
