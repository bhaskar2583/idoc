namespace API.Models.DAL
{
    using API.Models.EDM;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    public class ManageUserRoles
    {
        private PFPEntities db = new PFPEntities();

        public List<UserRoleVM> GetUserRoles()
        {
            var userRolesList = new List<UserRoleVM>();
            var userRoles = db.UserRoles.ToList();
            userRolesList = userRoles.Select(x => new UserRoleVM()
            {
                Id = x.URS_Id,
                UserId = x.URS_UserId,
                RoleId = x.URS_RoleId,
                RoleName = this.GetRolesMaster(x.URS_RoleId).Name,
                UserName = this.GetUserById(x.URS_UserId).USR_FirstName + ' ' + this.GetUserById(x.URS_UserId).USR_LastName,
                OrganizationName = this.GetUserById(x.URS_UserId).USR_OrganizationName
            }).ToList();
            return userRolesList.Where(w => !string.IsNullOrEmpty(w.RoleName) && !string.IsNullOrEmpty(w.UserName)).ToList();

        }

        public UserRoleVM GetUserRole(int id)
        {
            UserRole userRolesMaster = db.UserRoles.Find(id);
            UserRoleVM userRole = new UserRoleVM()
            {
                Id = userRolesMaster.URS_Id,
                UserId = userRolesMaster.URS_UserId,
                RoleId = userRolesMaster.URS_RoleId,
                RoleName = this.GetRolesMaster(userRolesMaster.URS_RoleId).Name,
                UserName = this.GetUserById(userRolesMaster.URS_UserId).USR_FirstName + ' ' + this.GetUserById(userRolesMaster.URS_UserId).USR_LastName
            };
            return userRole;
        }

        public async Task PutUserRole(UserRole userRole)
        {
            try
            {
                UserRole userRolesMaster = db.UserRoles.Find(userRole.URS_Id);
                userRolesMaster.URS_RoleId = userRole.URS_RoleId;
                userRolesMaster.URS_UserId = userRole.URS_UserId;

                db.Entry(userRolesMaster).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task PostuserRole(UserRole userRole)
        {
            try
            {
                db.UserRoles.Add(userRole);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteUserRole(UserRole userRole)
        {
            try
            {
                UserRole userRolesMaster = db.UserRoles.Find(userRole.URS_Id);

                db.Entry(userRolesMaster).State = EntityState.Deleted;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsUserRoleExists(int id)
        {
            return db.UserRoles.Count(e => e.URS_Id == id) > 0;
        }

        public bool IsUserRoleExists(int userID, int roleId)
        {
            return db.UserRoles.Count(e => e.URS_UserId == userID && e.URS_RoleId == roleId) > 0;
        }
        public UserVM GetUserById(int id)
        {
            User userMaster = db.Users.Find(id);

            if (!Convert.ToBoolean(userMaster.USR_Active))
                return new UserVM();

            UserVM userVM = new UserVM()
            {
                USR_Id = userMaster.USR_Id,
                USR_FirstName = userMaster.USR_FirstName,
                USR_LastName = userMaster.USR_LastName,
                USR_Phone = userMaster.USR_Phone,
                USR_Email = userMaster.USR_Email,
                USR_Active = userMaster.USR_Active,
                USR_OrganizationName = userMaster.USR_OrganizationName
            };
            return userVM;
        }

        public Roles GetRolesMaster(int id)
        {
            RolesMaster rolesMaster = db.RolesMasters.Find(id);

            if (!rolesMaster.ROM_Active)
                return new Roles();

            Roles role = new Roles()
            {
                Id = rolesMaster.ROM_Id,
                Name = rolesMaster.ROM_Name,
                Description = rolesMaster.ROM_Description,
                Active = rolesMaster.ROM_Active

            };
            return role;
        }
    }
}