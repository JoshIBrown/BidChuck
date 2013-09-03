using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace BCWeb.Models
{
    public class WebSecurityWrapper : IWebSecurityWrapper
    {
        public int CurrentUserId { get { return WebSecurity.CurrentUserId; } }
        public string CurrentUserName { get { return "admin_user"; } } // WebSecurity.CurrentUserName;
        public bool HasUserId { get { return WebSecurity.HasUserId; } }
        public bool Initialized { get { return WebSecurity.Initialized; } }
        public bool IsAuthenticated { get { return WebSecurity.IsAuthenticated; } }
        public bool ChangePassword(string userName, string currentPassword, string newPassword) { return WebSecurity.ChangePassword(userName, currentPassword, newPassword); }
        public bool ConfirmAccount(string accountConfirmationToken) { return WebSecurity.ConfirmAccount(accountConfirmationToken); }
        public bool ConfirmAccount(string userName, string accountConfirmationToken) { return WebSecurity.ConfirmAccount(userName, accountConfirmationToken); }
        public string CreateAccount(string userName, string password, bool requireConfirmationToken = false) { return WebSecurity.CreateAccount(userName, password, requireConfirmationToken = false); }
        public string CreateUserAndAccount(string userName, string password, object propertyValues = null, bool requireConfirmationToken = false) { return WebSecurity.CreateUserAndAccount(userName, password, propertyValues = null, requireConfirmationToken = false); }
        public string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow = 1440) { return WebSecurity.GeneratePasswordResetToken(userName, tokenExpirationInMinutesFromNow = 1440); }
        public DateTime GetCreateDate(string userName) { return WebSecurity.GetCreateDate(userName); }
        public DateTime GetLastPasswordFailureDate(string userName) { return WebSecurity.GetLastPasswordFailureDate(userName); }
        public DateTime GetPasswordChangedDate(string userName) { return WebSecurity.GetPasswordChangedDate(userName); }
        public int GetPasswordFailuresSinceLastSuccess(string userName) { return WebSecurity.GetPasswordFailuresSinceLastSuccess(userName); }
        public int GetUserId(string userName) { return WebSecurity.GetUserId(userName); }
        public int GetUserIdFromPasswordResetToken(string token) { return WebSecurity.GetUserIdFromPasswordResetToken(token); }
        public void InitializeDatabaseConnection(string connectionStringName, string userTableName, string userIdColumn, string userNameColumn, bool autoCreateTables) { WebSecurity.InitializeDatabaseConnection(connectionStringName, userTableName, userIdColumn, userNameColumn, autoCreateTables); }
        public void InitializeDatabaseConnection(string connectionString, string providerName, string userTableName, string userIdColumn, string userNameColumn, bool autoCreateTables) { WebSecurity.InitializeDatabaseConnection(connectionString, providerName, userTableName, userIdColumn, userNameColumn, autoCreateTables); }
        public bool IsAccountLockedOut(string userName, int allowedPasswordAttempts, int intervalInSeconds) { return WebSecurity.IsAccountLockedOut(userName, allowedPasswordAttempts, intervalInSeconds); }
        public bool IsAccountLockedOut(string userName, int allowedPasswordAttempts, TimeSpan interval) { return WebSecurity.IsAccountLockedOut(userName, allowedPasswordAttempts, interval); }
        public bool IsConfirmed(string userName) { return WebSecurity.IsConfirmed(userName); }
        public bool IsCurrentUser(string userName) { return WebSecurity.IsCurrentUser(userName); }
        public bool Login(string userName, string password, bool persistCookie = false) { return WebSecurity.Login(userName, password, persistCookie = false); }
        public void Logout() { WebSecurity.Logout(); }
        public void RequireAuthenticatedUser() { WebSecurity.RequireAuthenticatedUser(); }
        public void RequireRoles(params string[] roles) { WebSecurity.RequireRoles(roles); }
        public void RequireUser(int userId) { WebSecurity.RequireUser(userId); }
        public void RequireUser(string userName) { WebSecurity.RequireUser(userName); }
        public bool ResetPassword(string passwordResetToken, string newPassword) { return WebSecurity.ResetPassword(passwordResetToken, newPassword); }
        public bool UserExists(string userName) { return WebSecurity.UserExists(userName); }

        // role provider
        public string ApplicationName { get { return Roles.ApplicationName; } set { Roles.ApplicationName = value; } }
        public bool CacheRolesInCookie { get { return Roles.CacheRolesInCookie; } }
        public string CookieName { get { return Roles.CookieName; } }
        public string CookiePath { get { return Roles.CookiePath; } }
        public System.Web.Security.CookieProtection CookieProtectionValue { get { return Roles.CookieProtectionValue; } }
        public bool CookieRequireSSL { get { return Roles.CookieRequireSSL; } }
        public bool CookieSlidingExpiration { get { return Roles.CookieSlidingExpiration; } }
        public int CookieTimeout { get { return Roles.CookieTimeout; } }
        public bool CreatePersistentCookie { get { return Roles.CreatePersistentCookie; } }
        public string Domain { get { return Roles.Domain; } }
        public bool Enabled { get { return Roles.Enabled; } set { Roles.Enabled = value; } }
        public int MaxCachedResults { get { return Roles.MaxCachedResults; } }
        public System.Web.Security.RoleProvider Provider { get { return Roles.Provider; } }
        public System.Web.Security.RoleProviderCollection Providers { get { return Roles.Providers; } }
        public void AddUsersToRole(string[] usernames, string roleName) { Roles.AddUsersToRole(usernames, roleName); }
        public void AddUsersToRoles(string[] usernames, string[] roleNames) { Roles.AddUsersToRoles(usernames, roleNames); }
        public void AddUserToRole(string username, string roleName) { Roles.AddUserToRole(username, roleName); }
        public void AddUserToRoles(string username, string[] roleNames) { Roles.AddUserToRoles(username, roleNames); }
        public void CreateRole(string roleName) { Roles.CreateRole(roleName); }
        public void DeleteCookie() { Roles.DeleteCookie(); }
        public bool DeleteRole(string roleName) { return Roles.DeleteRole(roleName); }
        public bool DeleteRole(string roleName, bool throwOnPopulatedRole) { return Roles.DeleteRole(roleName, throwOnPopulatedRole); }
        public string[] FindUsersInRole(string roleName, string usernameToMatch) { return Roles.FindUsersInRole(roleName, usernameToMatch); }
        public string[] GetAllRoles() { return Roles.GetAllRoles(); }
        public string[] GetRolesForUser() { return GetRolesForUser(); }
        public string[] GetRolesForUser(string username) { return Roles.GetRolesForUser(username); }
        public string[] GetUsersInRole(string roleName) { return Roles.GetUsersInRole(roleName); }
        public bool IsUserInRole(string roleName) { return Roles.IsUserInRole(roleName); }
        public bool IsUserInRole(string username, string roleName) { return Roles.IsUserInRole(username, roleName); }
        public void RemoveUserFromRole(string username, string roleName) { Roles.RemoveUserFromRole(username, roleName); }
        public void RemoveUserFromRoles(string username, string[] roleNames) { Roles.RemoveUserFromRoles(username, roleNames); }
        public void RemoveUsersFromRole(string[] usernames, string roleName) { Roles.RemoveUsersFromRole(usernames, roleName); }
        public void RemoveUsersFromRoles(string[] usernames, string[] roleNames) { Roles.RemoveUsersFromRoles(usernames, roleNames); }
        public bool RoleExists(string roleName) { return Roles.RoleExists(roleName); }
    }
}