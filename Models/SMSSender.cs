using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ProgrammingZone.Models
{
    public class SMSSender
    {
        string user, password, sender, priorty, stype;
        public SMSSender()
        {
            user = "MTECHL";
            password = "erdirector";
            sender = "MTECHL";
            priorty = "ndnd";
            stype = "normal";
        }
        public bool SendSms(string mobile, string msg)
        {
            string url = "http://trans.smsfresh.co/api/sendmsg.php?user=" + user + "&pass=" + password + "&sender=" + sender + "&phone=" + mobile + "&text=" + msg + "&priority=" + priorty + "&stype=" + stype + "";
            WebClient client = new WebClient();
            string s = client.DownloadString(url);
            char ch = s[0];
            if (ch == 'S')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}