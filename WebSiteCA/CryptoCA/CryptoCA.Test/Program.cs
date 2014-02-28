using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryptoCA.Core;

namespace CryptoCA.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var ca = new CertificationAutority();
            ca.GenerateCACertificateV2();

        }
    }
}
