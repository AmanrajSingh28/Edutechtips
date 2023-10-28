using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace ProgrammingZone.Models
{
    public class EncryptionManager
    {
        //code for encrypt data.
        public string encrypt(string encrpt)
        {
            byte[] b;
            string enc;
            b = ASCIIEncoding.ASCII.GetBytes(encrpt);
            enc = Convert.ToBase64String(b);
            return enc;
        }
        //code for decrypt data.
        public string Decrypt(string decrpt)
        {
            byte[] b;
            string dec;
            b = Convert.FromBase64String(decrpt);
            dec = ASCIIEncoding.ASCII.GetString(b);
            return dec;
        }
    }
}