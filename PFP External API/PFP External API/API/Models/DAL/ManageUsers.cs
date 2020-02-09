using API.Models.EDM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.DAL
{
    public class ManageUsers
    {
        private PFPEntities db = new PFPEntities();

        public List<UserVM> GetUserMasters()
        {
            var userList = new List<UserVM>();
            var user = db.Users.Where(r => r.USR_Active == true).ToList();
            
            userList = user.Select(x => new UserVM()
            {
                USR_Id = x.USR_Id,
                USR_FirstName = x.USR_FirstName,
                USR_LastName = x.USR_LastName,
                USR_Phone = x.USR_Phone,
                USR_Email = x.USR_Email,
                USR_OrganizationName = x.USR_OrganizationName,
                USR_Active = x.USR_Active,
                USR_UserName = x.USR_FirstName + ' ' + x.USR_LastName
            }).ToList();
            return userList;


        }

        public async Task<object> GetUserById(int id)
        {
            User userMaster = await db.Users.FindAsync(id);
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

        public async Task PutUserMaster(User userMaster)
        {
            try
            {
                db.Entry(userMaster).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task PostUserMaster(User userMaster, int roleId)
        {
            try
            {
                db.Users.Add(userMaster);
                await db.SaveChangesAsync();

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsUserExists(UserVM user)
        {
            if (user.USR_Id == 0)
                return db.Users.Count(e => e.USR_Email == user.USR_Email && e.USR_Active == true) > 0;
            else
                return db.Users.Count(e => e.USR_Id != user.USR_Id && e.USR_Email == user.USR_Email && e.USR_Active == true) > 0;
        }

        public async Task DeleteUserMaster(int id)
        {
            try
            {
                User userMaster = await db.Users.FindAsync(id);
                userMaster.USR_Active = false;
                db.Entry(userMaster).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool userExists(int id)
        {
            return db.Users.Count(e => e.USR_Id == id) > 0;
        }
    }
}