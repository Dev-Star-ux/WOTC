using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using StaffZoneMaster.Controllers;
using StaffZoneMaster.Entities.SZEmpAppControl;
using StaffZoneMaster.Models;
using StaffZoneMaster.Resources;
using StaffZoneMaster.Services;

namespace StaffZoneMaster.Auth
{

    public class AuthService : BaseService
    {
        public async Task<AuthUser> Login(BaseController controller, string userName, string password)
        {
            Logout(controller);
            try
            {
                UserPrincipal up = null;
                Exception adEx = null;
                foreach (ConnectionStringSettings css in ConfigurationManager.ConnectionStrings)
                {
                    if (!css.Name.StartsWith("ADCS_"))
                    {
                        continue;
                    }
                    try
                    {
                        SqlConnectionStringBuilder adcsBuilder = new SqlConnectionStringBuilder(css.ConnectionString);
                        PrincipalContext pc = new PrincipalContext(ContextType.Domain, adcsBuilder.DataSource, adcsBuilder.InitialCatalog, userName, password);
                        up = UserPrincipal.FindByIdentity(pc, userName);
                        if (up != null)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        adEx = ex;
                    }
                }
                if (up == null)
                {
                    if (adEx == null)
                    {
                        throw new Exception(Resource.Auth_Error_UserNotFound);
                    }
                    throw adEx;
                }
                PrincipalSearchResult<Principal> userGroups = up.GetAuthorizationGroups();
                List<string> locationIds = new List<string>();
                List<IdValueModel> assignedBranches = new List<IdValueModel>();
                if (userGroups != null)
                {
                    foreach (Principal gp in userGroups)
                    {
                        if (gp.SamAccountName.StartsWith("Branch"))
                        {
                            string locationId = gp.SamAccountName.Substring(6);
                            if (gp.SamAccountName.StartsWith("Branch0"))
                            {
                                locationId = gp.SamAccountName.Substring(7);
                            }
                            if (!string.IsNullOrEmpty(locationId))
                            {
                                locationIds.Add(locationId);
                            }
                        }
                    }
                }
                if (locationIds.Count > 0)
                {
                    using SZEmpAppControlDbContext db = new SZEmpAppControlDbContext();
                    assignedBranches = await (from t in db.BranchReferences
                                              where t.Active == (bool?)true && t.Centralized == (bool?)true && locationIds.Contains(t.LocationID)
                                              orderby t.LocationID
                                              select new IdValueModel
                                              {
                                                  Id = "S" + t.LocationID,
                                                  Value = string.Concat(t.LocationID + "  ", t.Branchname)
                                              }).ToListAsync();
                }
                if (assignedBranches == null || assignedBranches.Count < 1)
                {
                    throw new Exception(Resource.Auth_Error_UserNotAuthorized);
                }
                AuthUser authUser = new AuthUser
                {
                    UserName = up.SamAccountName,
                    Roles = assignedBranches.Select((IdValueModel elem) => elem.Id).ToList(),
                    AssignedBranches = assignedBranches
                };
                AuthUser authUser2 = authUser;
                authUser2.AllBranches = await getAllBranches(userName, password);
                authUser.FirstName = up.GivenName;
                authUser.LastName = up.Surname;
                authUser.DisplayName = up.DisplayName;
                authUser.EmailAddress = up.EmailAddress;
                AuthUser user = authUser;
                controller.AuthenticationManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = false
                }, user.GenerateUserIdentity());
                return user;
            }
            catch (Exception ex2)
            {
                string errorMessage = ex2.Message;
                if (ex2.GetType() == typeof(PrincipalServerDownException))
                {
                    errorMessage = Resource.Auth_Error_ServerNotAvailable;
                }
                else if (ex2.GetType() == typeof(AuthenticationException))
                {
                    errorMessage = Resource.Auth_Error_IncorrectCredential;
                }
                throw new Exception(errorMessage);
            }
        }

        public void Logout(BaseController controller)
        {
            if (controller.AuthenticationManager.User.Identity.IsAuthenticated)
            {
                controller.AuthenticationManager.SignOut("ApplicationCookie");
                controller.TempData.Clear();
                controller.Session.Clear();
            }
        }

        public async Task<List<IdValueModel>> getAllBranches(string userName, string password)
        {
            List<string> locationIds = new List<string>();
            List<IdValueModel> result = new List<IdValueModel>();
            foreach (ConnectionStringSettings css in ConfigurationManager.ConnectionStrings)
            {
                if (!css.Name.StartsWith("ADCS_"))
                {
                    continue;
                }
                try
                {
                    SqlConnectionStringBuilder adcsBuilder = new SqlConnectionStringBuilder(css.ConnectionString);
                    PrincipalContext pc = new PrincipalContext(ContextType.Domain, adcsBuilder.DataSource, adcsBuilder.InitialCatalog, userName, password);
                    GroupPrincipal gpQuery = new GroupPrincipal(pc);
                    PrincipalSearcher ps = new PrincipalSearcher(gpQuery);
                    PrincipalSearchResult<Principal> psr = ps.FindAll();
                    foreach (Principal p in psr)
                    {
                        if (p is GroupPrincipal gp && gp.SamAccountName.StartsWith("Branch"))
                        {
                            string locationId = gp.SamAccountName.Substring(6);
                            if (gp.SamAccountName.StartsWith("Branch0"))
                            {
                                locationId = gp.SamAccountName.Substring(7);
                            }
                            if (!string.IsNullOrEmpty(locationId))
                            {
                                locationIds.Add(locationId);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            if (locationIds.Count > 0)
            {
                using SZEmpAppControlDbContext db = new SZEmpAppControlDbContext();
                result = await (from t in db.BranchReferences
                                where t.Active == (bool?)true && t.Centralized == (bool?)true && locationIds.Contains(t.LocationID)
                                orderby t.LocationID
                                select new IdValueModel
                                {
                                    Id = "S" + t.LocationID,
                                    Value = string.Concat(t.LocationID + "  ", t.Branchname)
                                }).ToListAsync();
            }
            return result;
        }
    }

}
