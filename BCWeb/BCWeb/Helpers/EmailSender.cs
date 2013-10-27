using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace BCWeb
{
    public  class EmailSender : BCWeb.IEmailSender
    {
        public  void SendConfirmationMail(string FirstName, string Email, string ConfirmationToken)
        {
            MailMessage message = new MailMessage();
            List<MailAddress> recipients = new List<MailAddress>();

            message.To.Add(new MailAddress(Email, FirstName));
            message.From = new MailAddress("admin@bidchuck.com", "BidChuck Admin");
            message.Subject = "BidChuck New Account Confirmation";

            string text = @"
                Hello " + FirstName + @",

                To complete your registration visit:

                http://bidchuck.com/Account/RegisterConfirmation/" + ConfirmationToken + @"

                Thanks!

                -The BidChuck Team
            ";

            string html = @"
                <html>
                  <body>
                    <p>Hello " + FirstName + @",</p>
                    <p>To complete your registration visit:</p>
                    <p></p>
                    <p><a href=""http://bidchuck.com/Account/RegisterConfirmation/" + ConfirmationToken + @""">http://bidchuck.com/Account/RegisterConfirmation/" + ConfirmationToken + @"</a></p>
                    <p></p>
                    <p>Thanks!</p>
                    <p></p>
                    <p>-The BidChuck Team</p>
                  </body>
                </html>
            ";

            SendMail(message, text, html);
        }

        public  void SendPasswordResetMail(string FirstName, string Email, string PasswordResetToken)
        {
            MailMessage message = new MailMessage();
            List<MailAddress> recipients = new List<MailAddress>();

            message.To.Add(new MailAddress(Email, FirstName));
            message.From = new MailAddress("admin@bidchuck.com", "BidChuck Admin");
            message.Subject = "BidChuck Password Reset";

            string text = @"
                Hello " + FirstName + @",

                To reset your password visit:
                
                http://bidchuck.com/Account/ResetPassword?user=" + Email + @"&token=" + PasswordResetToken + @"

                Thanks!

                -The BidChuck Team
            ";

            string html = @"
                <html>
                  <body>
                    <p>Hello " + FirstName + @",</p>
                    <p>To reset your password visit:</p>
                    <p></p>
                    <p><a href=""http://bidchuck.com/Account/ResetPassword?user=" + Email + @"&token=" + PasswordResetToken + @""">http://bidchuck.com/Account/ResetPassword?user=" + Email + @"&token=" + PasswordResetToken + @"</a></p>
                    <p></p>
                    <p>Thanks!</p>
                    <p></p>
                    <p>-The BidChuck Team</p>
                  </body>
                </html>
            ";

            SendMail(message, text, html);
        }

        public  void SendNewDelegateEmail(string Inviter, string FirstName, string Email, string ConfirmAccoutToken)
        {
            MailMessage message = new MailMessage();
            List<MailAddress> recipients = new List<MailAddress>();

            message.To.Add(new MailAddress(Email, FirstName));
            message.From = new MailAddress("admin@bidchuck.com", "BidChuck Admin");
            message.Subject = "BidChuck Invitation from " + Inviter;

            string text = @"
                Hello " + FirstName + @"," +

                Inviter + @"has added you to their bidCHUCK account.  To complete the process please go to: 
                
                http://bidchuck.com/Account/AcceptInvitation?user=" + Email + @"&token=" + ConfirmAccoutToken + @"

                Thanks!

                -The BidChuck Team
            ";

            string html = @"
                <html>
                  <body>
                    <p>Hello " + FirstName + @",</p>
                    <p>" + Inviter + @"has added you to their bidCHUCK account.  To complete the process please go to: </p>
                    <p></p>
                    <p><a href=""http://bidchuck.com/Account/AcceptInvitation?user=" + Email + @"&token=" + ConfirmAccoutToken + @""">http://bidchuck.com/Account/AcceptInvitation?user=" + Email + @"&token=" + ConfirmAccoutToken + @"</a></p>
                    <p></p>
                    <p>Thanks!</p>
                    <p></p>
                    <p>-The BidChuck Team</p>
                  </body>
                </html>
            ";

            SendMail(message, text, html);
        }

        private void SendMail(MailMessage message, string text, string html)
        {
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Send(message);
            }
        }




        public void InviteArchitect(string email, string name, string company, string invitingTagLine, string token)
        {
            MailMessage message = new MailMessage();
            List<MailAddress> recipients = new List<MailAddress>();

            message.To.Add(new MailAddress(email, name));
            message.From = new MailAddress("admin@bidchuck.com", "BidChuck Admin");
            message.Subject = "BidChuck Invitation from " + invitingTagLine;

            string text = @"
                Hello " + name + @"," +

                invitingTagLine + @"has invited you to join bidChuck.com.  You can accept this invitation, and register " + company + @" with bidChuck.com by following this link: 
                
                http://bidchuck.com/Account/AcceptInvitation?user=" + email + @"&token=" + token + @"

                Thanks!

                -The BidChuck Team
            ";

            string html = @"
                <html>
                  <body>
                    <p>Hello " + name + @",</p>
                    <p>" + invitingTagLine + @"has invited you to join bidChuck.com.  You can accept this invitation, and register " + company + @" with bidChuck.com by following this link:  </p>
                    <p></p>
                    <p><a href=""http://bidchuck.com/Account/AcceptInvitation?user=" + email + @"&token=" + token + @""">http://bidchuck.com/Account/AcceptInvitation?user=" + email + @"&token=" + token + @"</a></p>
                    <p></p>
                    <p>Thanks!</p>
                    <p></p>
                    <p>-The BidChuck Team</p>
                  </body>
                </html>
            ";

            SendMail(message, text, html);
        }
    }
}