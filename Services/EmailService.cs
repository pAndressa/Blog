using System.Net;
using System.Net.Mail;

namespace Blog.Services
{
    public class EmailService
    {
        public bool Send(
            string toName,
            string toEmail,
            string subject,
            string body,
            string fromName = "Teste Curso Envio Email",
            string fromEmail = "email@hotmail.com"
        )
        {
            var smtpClient = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port);
            smtpClient.Credentials = new NetworkCredential(Configuration.Smtp.UserName, Configuration.Smtp.Password);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true; 

            var email =  new  MailMessage();
            email.From = new MailAddress(fromEmail, fromName);
            email.To.Add(new MailAddress(toEmail, toName));//pode repetir
            email.Subject = subject;
            email.Body = body;
            email.IsBodyHtml = true;

            try
            {
                smtpClient.Send(email);
                return true;
            }
            catch (System.Exception)
            {
                
                return false;
            }
        }
    }
}