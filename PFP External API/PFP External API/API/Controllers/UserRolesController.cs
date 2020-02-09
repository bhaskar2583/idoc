namespace API.Controllers
{
    using API.Models;
    using API.Models.DAL;
    using API.Models.EDM;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    public class UserRolesController : ApiController
    {
        private ManageUserRoles userRoles = new ManageUserRoles();

        // GET: api/userRoles
        public List<UserRoleVM> GetUserRoles()
        {
            return userRoles.GetUserRoles();
        }

        // GET: api/userRoles/5
        [ResponseType(typeof(UserRoleVM))]
        public async Task<IHttpActionResult> GetUserRole(int id)
        {
            UserRoleVM userRole = userRoles.GetUserRole(id);
            if (userRole == null)
            {
                return NotFound();
            }

            return Ok(userRole);
        }

        // GET: api/userRoles?id={id}&roleId={roleId}
        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> GetUserRole(int id, int roleId)
        {
            bool userroleExists = userRoles.IsUserRoleExists(id, roleId);

            return Ok(userroleExists ? "1" : "0");
        }

      

        [ResponseType(typeof(UserRole))]
        public async Task<IHttpActionResult> PutUserRole(UserRoleVM userRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userRoleData = new UserRole()
            {
                URS_Id = userRole.Id,
                URS_UserId = userRole.UserId,
                URS_RoleId = userRole.RoleId,
                URS_CreatedBy = userRole.CreatedBy,
                URS_CreatedOn = DateTime.Now,
                URS_UpdatedBy = userRole.UpdatedBy,
                URS_UpdatedOn = DateTime.Now
            };
            await userRoles.PutUserRole(userRoleData);

            return CreatedAtRoute("DefaultApi", new { id = userRoleData.URS_Id }, userRole);
        }


        // POST: api/userRoles
        [ResponseType(typeof(UserRole))]
        public async Task<IHttpActionResult> PostUserRole(UserRoleVM userRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userRoleData = new UserRole()
            {
                URS_Id = userRole.Id,
                URS_UserId = userRole.UserId,
                URS_RoleId = userRole.RoleId,
                URS_CreatedBy = userRole.CreatedBy,
                URS_CreatedOn = DateTime.Now,
                URS_UpdatedBy = userRole.UpdatedBy,
                URS_UpdatedOn = DateTime.Now
            };
            await userRoles.PostuserRole(userRoleData);

            return CreatedAtRoute("DefaultApi", new { id = userRoleData.URS_Id }, userRole);
        }

        //DELETE: api/userRoles/5
        [ResponseType(typeof(UserRole))]
        public async Task<IHttpActionResult> DeleteUserRole(int id)
        {
            UserRoleVM userRole = userRoles.GetUserRole(id);
                       
            if (userRole == null)
            {
                return NotFound();
            }

            UserRole objUserRole = new UserRole()
            {
                URS_Id = userRole.Id,
                URS_RoleId = userRole.RoleId,
                URS_UserId = userRole.UserId
            };

            await userRoles.DeleteUserRole(objUserRole);
            return Ok(userRole);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                userRoles = null;
                GC.Collect();
                GC.SuppressFinalize(this);
            }
            base.Dispose(disposing);
        }
    }
}