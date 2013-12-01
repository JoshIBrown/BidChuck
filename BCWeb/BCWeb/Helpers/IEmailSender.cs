using System;
namespace BCWeb
{
    public interface IEmailSender
    {
        void SendConfirmationMail(string FirstName, string Email, string ConfirmationToken);
        void SendNewDelegateEmail(string Inviter, string FirstName, string Email, string ConfirmAccoutToken);
        void SendPasswordResetMail(string FirstName, string Email, string PasswordResetToken);

        
        void InviteArchitect(string email, string name, string company, string invitingTagLine, string token);
    }
}
