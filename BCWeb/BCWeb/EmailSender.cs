using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace BCWeb
{
    public static class EmailSender
    {
        public static void SendConfirmationMail(string FirstName, string Email, string ConfirmationToken)
        {
            MailMessage message = new MailMessage();
            List<MailAddress> recipients = new List<MailAddress>();

            message.To.Add(new MailAddress(Email, FirstName));
            message.From = new MailAddress("admin@bidchuck.com", "BidChuck Admin");
            message.Subject = "BidChuck New Account Confirmation";

            string text = @"
                Hello " + FirstName + @",

                To complete your registration visit:

                http://bcdev.azurewebsites.net/Account/RegisterConfirmation/" + ConfirmationToken + @"

                Thanks!

                -The BidChuck Team
            ";

            string html = @"
                <html>
                  <body>
                    <p>Hello " + FirstName + @",</p>
                    <p>To complete your registration visit:</p>
                    <p></p>
                    <p><a href=""http://bcdev.azurewebsites.net/Account/RegisterConfirmation/" + ConfirmationToken + @""">http://bcdev.azurewebsites.net/Account/RegisterConfirmation/" + ConfirmationToken + @"</a></p>
                    <p></p>
                    <p>Thanks!</p>
                    <p></p>
                    <p>-The BidChuck Team</p>
                  </body>
                </html>
            ";

            SendMail(message, text, html);
        }

        public static void SendPasswordResetMail(string FirstName, string Email, string PasswordResetToken)
        {
            MailMessage message = new MailMessage();
            List<MailAddress> recipients = new List<MailAddress>();

            message.To.Add(new MailAddress(Email, FirstName));
            message.From = new MailAddress("admin@bidchuck.com", "BidChuck Admin");
            message.Subject = "BidChuck Password Reset";

            string text = @"
                Hello " + FirstName + @",

                To reset your password visit:
                
                http://bcdev.azurewebsites.net/Account/ResetPassword?user=" + Email + @"&token=" + PasswordResetToken + @"

                Thanks!

                -The BidChuck Team
            ";

            string html = @"
                <html>
                  <body>
                    <p>Hello " + FirstName + @",</p>
                    <p>To reset your password visit:</p>
                    <p></p>
                    <p><a href=""http://bcdev.azurewebsites.net/Account/ResetPassword?user=" + Email + @"&token=" + PasswordResetToken + @""">http://bcdev.azurewebsites.net/Account/ResetPassword?user=" + Email + @"&token=" + PasswordResetToken + @"</a></p>
                    <p></p>
                    <p>Thanks!</p>
                    <p></p>
                    <p>-The BidChuck Team</p>
                  </body>
                </html>
            ";

            SendMail(message, text, html);
        }

        public static void SendNewDelegateEmail(string Inviter, string FirstName, string Email, string ConfirmAccoutToken)
        {
            MailMessage message = new MailMessage();
            List<MailAddress> recipients = new List<MailAddress>();

            message.To.Add(new MailAddress(Email, FirstName));
            message.From = new MailAddress("admin@bidchuck.com", "BidChuck Admin");
            message.Subject = "BidChuck Invitation from " + Inviter;

            string text = @"
                Hello " + FirstName + @"," +

                Inviter + @"has added you to their bidCHUCK account.  To complete the process please go to: 
                
                http://bcdev.azurewebsites.net/Account/AcceptInvitation?user=" + Email + @"&token=" + ConfirmAccoutToken + @"

                Thanks!

                -The BidChuck Team
            ";

            string html = @"
                <html>
                  <body>
                    <p>Hello " + FirstName + @",</p>
                    <p>" + Inviter + @"has added you to their bidCHUCK account.  To complete the process please go to: </p>
                    <p></p>
                    <p><a href=""http://bcdev.azurewebsites.net/Account/AcceptInvitation?user=" + Email + @"&token=" + ConfirmAccoutToken + @""">http://bcdev.azurewebsites.net/Account/AcceptInvitation?user=" + Email + @"&token=" + ConfirmAccoutToken + @"</a></p>
                    <p></p>
                    <p>Thanks!</p>
                    <p></p>
                    <p>-The BidChuck Team</p>
                  </body>
                </html>
            ";

            SendMail(message, text, html);
        }

        private static void SendMail(MailMessage message, string text, string html)
        {
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Send(message);
        }
    }
}