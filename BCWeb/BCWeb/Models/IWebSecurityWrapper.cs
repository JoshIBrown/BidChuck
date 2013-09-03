using System;
using System.Web.Security;
namespace BCWeb.Models
{
    public interface IWebSecurityWrapper
    {
        // web security
        bool ChangePassword(string userName, string currentPassword, string newPassword);
        bool ConfirmAccount(string accountConfirmationToken);
        bool ConfirmAccount(string userName, string accountConfirmationToken);
        string CreateAccount(string userName, string password, bool requireConfirmationToken = false);
        string CreateUserAndAccount(string userName, string password, object propertyValues = null, bool requireConfirmationToken = false);
        int CurrentUserId { get; }
        string CurrentUserName { get; }
        string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow = 1440);
        DateTime GetCreateDate(string userName);
        DateTime GetLastPasswordFailureDate(string userName);
        DateTime GetPasswordChangedDate(string userName);
        int GetPasswordFailuresSinceLastSuccess(string userName);
        int GetUserId(string userName);
        int GetUserIdFromPasswordResetToken(string token);
        bool HasUserId { get; }
        bool Initialized { get; }
        void InitializeDatabaseConnection(string connectionString, string providerName, string userTableName, string userIdColumn, string userNameColumn, bool autoCreateTables);
        void InitializeDatabaseConnection(string connectionStringName, string userTableName, string userIdColumn, string userNameColumn, bool autoCreateTables);
        bool IsAccountLockedOut(string userName, int allowedPasswordAttempts, int intervalInSeconds);
        bool IsAccountLockedOut(string userName, int allowedPasswordAttempts, TimeSpan interval);
        bool IsAuthenticated { get; }
        bool IsConfirmed(string userName);
        bool IsCurrentUser(string userName);
        bool Login(string userName, string password, bool persistCookie = false);
        void Logout();
        void RequireAuthenticatedUser();
        void RequireRoles(params string[] roles);
        void RequireUser(int userId);
        void RequireUser(string userName);
        bool ResetPassword(string passwordResetToken, string newPassword);
        bool UserExists(string userName);


        // role provider
        string ApplicationName { get; set; }
        bool CacheRolesInCookie { get; }
        string CookieName { get; }
        string CookiePath { get; }
        CookieProtection CookieProtectionValue { get; }
        bool CookieRequireSSL { get; }
        bool CookieSlidingExpiration { get; }
        int CookieTimeout { get; }
        bool CreatePersistentCookie { get; }
        string Domain { get; }
        bool Enabled { get; set; }
        int MaxCachedResults { get; }
        RoleProvider Provider { get; }
        RoleProviderCollection Providers { get; }
        void AddUsersToRole(string[] usernames, string roleName);
        void AddUsersToRoles(string[] usernames, string[] roleNames);
        void AddUserToRole(string username, string roleName);
        void AddUserToRoles(string username, string[] roleNames);
        void CreateRole(string roleName);
        void DeleteCookie();
        bool DeleteRole(string roleName);
        bool DeleteRole(string roleName, bool throwOnPopulatedRole);
        string[] FindUsersInRole(string roleName, string usernameToMatch);
        string[] GetAllRoles();
        string[] GetRolesForUser();
        string[] GetRolesForUser(string username);
        string[] GetUsersInRole(string roleName);
        bool IsUserInRole(string roleName);
        bool IsUserInRole(string username, string roleName);
        void RemoveUserFromRole(string username, string roleName);
        void RemoveUserFromRoles(string username, string[] roleNames);
        void RemoveUsersFromRole(string[] usernames, string roleName);
        void RemoveUsersFromRoles(string[] usernames, string[] roleNames);
        bool RoleExists(string roleName);
    }
}
