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

namespace API.Controllers
{
    public class UserController : ApiController
    {
        private ManageUsers users = new ManageUsers();

        // GET: api/Users
        public List<UserVM> GetUserMasters()
        {
            return users.GetUserMasters();
        }

        // GET: api/Users/5
        [ResponseType(typeof(UserVM))]
        public async Task<IHttpActionResult> GetUserById(int id)
        {
            UserVM userMaster = (UserVM)await users.GetUserById(id);
            if (userMaster == null)
            {
                return NotFound();
            }

            return Ok(userMaster);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserMaster(User userMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await users.PutUserMaster(userMaster);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUserMaster(UserVM UserVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userMaster = new User()
            {
                USR_Id = UserVM.USR_Id,
                USR_FirstName = UserVM.USR_FirstName,
                USR_LastName = UserVM.USR_LastName,
                USR_Email = UserVM.USR_Email,
                USR_OrganizationName = UserVM.USR_OrganizationName,
                USR_Active = UserVM.USR_Active,
                USR_Phone = UserVM.USR_Phone,
                USR_CreatedOn = DateTime.Now
            };
            
            await users.PostUserMaster(userMaster, UserVM.USR_RoleId);

            return CreatedAtRoute("DefaultApi", new { id = userMaster.USR_Id }, userMaster);
        }

        // DELETE: api/User/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUserMaster(int id)
        {
            
            await users.DeleteUserMaster(id);
            return Ok();
        }

        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> GetUserByEmail(int id, string email)
        {
            bool userExists =  users.IsUserExists(new UserVM() {USR_Id = id, USR_Email = email });

            return Ok(userExists ? "1" : "0");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                users = null;
                GC.Collect();
                GC.SuppressFinalize(this);
            }
            base.Dispose(disposing);
        }
    }
}
