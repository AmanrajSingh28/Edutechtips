using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;

namespace ProgrammingZone.Models
{
    public class EmailSender
    {
        public string SendTo { get; set; }
        public string MessageBody { get; set; }
        public string Subject { get; set; }
        public string CC { get; set; }
        public string AttachmentFile { get; set; }
        public bool sendEmail()
        {
            SmtpClient smtp = new SmtpClient();
            MailMessage msg = new MailMessage();
            MailAddress from = new MailAddress("sa7496531@gmail.com");
            MailAddress to = new MailAddress(SendTo);

            //Networkcredetional Setting start here
            NetworkCredential nc = new NetworkCredential("sa7496531@gmail.com", "khanarslaankhan2");
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = nc;
            //Networkcredetional Setting close here

            //MailMessage Setting start here
            msg.Sender = from;
            msg.Subject = Subject;
            msg.To.Add(to);
            msg.From = from;
            msg.Body = MessageBody;
            if (CC != null)
            {
                Attachment att = new Attachment(HttpContext.Current.Server.MapPath(AttachmentFile));
                msg.Attachments.Add(att);
            }
            smtp.Send(msg);
            return true;
        }
    }
}