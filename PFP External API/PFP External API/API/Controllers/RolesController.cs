namespace API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using API.Models;
    using API.Models.DAL;
    using API.Models.EDM;

    public class RolesController : ApiController
    {
        private ManageRoles roles = new ManageRoles();

        // GET: api/Roles
        public IQueryable<Roles> GetRolesMasters()
        {
            return roles.GetRoles();
        }

        // GET: api/Roles/5
        [ResponseType(typeof(RolesMaster))]
        public async Task<IHttpActionResult> GetRolesMaster(int id)
        {
            Roles role = (Roles)await roles.GetRole(id);
            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        // GET: api/Roles/1?useremail=KSuriyak@HANYS.org
        [ResponseType(typeof(RolesMaster))]
        public async Task<IHttpActionResult> GetRolesMaster(int id, string useremail)
        {
            if (!string.IsNullOrEmpty(useremail) && !string.IsNullOrWhiteSpace(useremail))
            {
                return Ok(await roles.IsSuperAdmin(useremail));
            }
            return Ok(false);
        }

        // PUT: api/Roles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRolesMaster(int id, Roles role)
        {
            if (id != role.Id)
            {
                return BadRequest();
            }

            try
            {
                await roles.PutRolesMaster(role);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!roles.RoleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Roles
        [ResponseType(typeof(RolesMaster))]
        public async Task<IHttpActionResult> PostRolesMaster(Roles role)
        {
            await roles.PostRole(role);

            return CreatedAtRoute("DefaultApi", new { id = role.Id }, role);
        }

        // DELETE: api/Roles/5
        [ResponseType(typeof(RolesMaster))]
        public async Task<IHttpActionResult> DeleteRolesMaster(int id)
        {
            RolesMaster rolesMaster = (RolesMaster)await roles.GetRoleMaster(id);
            if (rolesMaster == null)
            {
                return NotFound();
            }

            await roles.DeleteRolesMaster(rolesMaster);
            return Ok(rolesMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                roles = null;
                GC.Collect();
                GC.SuppressFinalize(this);
            }
            base.Dispose(disposing);
        }
    }
}