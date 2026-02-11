using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;
using StaffZoneMaster.Models;

namespace StaffZoneMaster.Auth
{

    public class AuthUser
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public string EmailAddress { get; set; }

        public List<string> Roles { get; set; }

        public List<IdValueModel> AssignedBranches { get; set; }

        public List<IdValueModel> AllBranches { get; set; }

        public ClaimsIdentity GenerateUserIdentity()
        {
            List<Claim> claims = new List<Claim>
        {
            new Claim("UserName", UserName),
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", UserName),
            new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "Active Directory")
        };
            claims.AddRange(Roles.Select((string role) => new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", role)));
            claims.Add(new Claim("AssignedBranches", JsonConvert.SerializeObject(AssignedBranches)));
            claims.Add(new Claim("AllBranches", JsonConvert.SerializeObject(AllBranches)));
            if (!string.IsNullOrEmpty(FirstName))
            {
                claims.Add(new Claim("FirstName", FirstName));
            }
            if (!string.IsNullOrEmpty(LastName))
            {
                claims.Add(new Claim("LastName", LastName));
            }
            if (!string.IsNullOrEmpty(DisplayName))
            {
                claims.Add(new Claim("DisplayName", DisplayName));
            }
            if (!string.IsNullOrEmpty(EmailAddress))
            {
                claims.Add(new Claim("EmailAddress", EmailAddress));
            }
            if (!string.IsNullOrEmpty(UserName))
            {
                return new ClaimsIdentity(claims, "ApplicationCookie");
            }
            return null;
        }

        public static AuthUser GetApplicationUser(ClaimsPrincipal cp)
        {
            string assignedBranches = GetClaimValue(cp, "AssignedBranches");
            string allBranches = GetClaimValue(cp, "AllBranches");
            AuthUser user = new AuthUser
            {
                UserName = GetClaimValue(cp, "UserName"),
                FirstName = GetClaimValue(cp, "FirstName"),
                LastName = GetClaimValue(cp, "LastName"),
                DisplayName = GetClaimValue(cp, "DisplayName"),
                EmailAddress = GetClaimValue(cp, "EmailAddress"),
                Roles = GetClaimValues(cp, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"),
                AssignedBranches = ((assignedBranches == null) ? new List<IdValueModel>() : JsonConvert.DeserializeObject<List<IdValueModel>>(assignedBranches)),
                AllBranches = ((allBranches == null) ? new List<IdValueModel>() : JsonConvert.DeserializeObject<List<IdValueModel>>(allBranches))
            };
            if (!string.IsNullOrEmpty(user.UserName))
            {
                return user;
            }
            return null;
        }

        private static string GetClaimValue(ClaimsPrincipal cp, string type)
        {
            return (from c in cp.Claims
                    where c.Type == type
                    select c.Value).FirstOrDefault();
        }

        private static List<string> GetClaimValues(ClaimsPrincipal cp, string type)
        {
            return (from c in cp.Claims
                    where c.Type == type
                    select c.Value).ToList();
        }
    }

}
