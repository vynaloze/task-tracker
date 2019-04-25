using System;
using System.Net.Mail;

namespace Service.Users.Util
{
    public class Mailer
    {
        private static readonly SmtpClient _client;

        static Mailer()
        {
            _client = new SmtpClient();
            _client.Host = "localhost";
            _client.Port = 25;
            _client.DeliveryMethod = SmtpDeliveryMethod.Network;
            _client.UseDefaultCredentials = false;
        }

        public static void SendResetRequest(string targetEmail, string token)
        {
            var target = "http://localhost:3000/reset?token=" + token;
            var mail = new MailMessage("noreply@tasktracker.com", targetEmail);
            mail.Subject = "Task Tracker reset password request";
            mail.Body = "Someone requested to reset your password on Task Tracker." +
                        "<br/>Click <a href=\"" + target + "\"><button>here</button></a> to complete the procedure" +
                        "<br/><b>This link is valid 30 minutes.</b>";
            
            Console.WriteLine(mail.Body);
            mail.IsBodyHtml = true;
            _client.Send(mail);
        }
    }
}