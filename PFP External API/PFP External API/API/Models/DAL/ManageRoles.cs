namespace API.Models.DAL
{
    using API.Models.EDM;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    public class ManageRoles
    {
        private PFPEntities db = new PFPEntities();
        private readonly string SuperAdminRoleName = "HANYS Admin";

        public IQueryable<Roles> GetRoles()
        {
            try
            {
                var saRole = db.RolesMasters.Where(a => a.ROM_Name.Replace(" ", string.Empty) == SuperAdminRoleName.Replace(" ", string.Empty)).FirstOrDefault();
                if (saRole != null && (saRole.ROM_Active == false || saRole.ROM_IsDeleted == true))
                {
                    saRole.ROM_Active = true;
                    saRole.ROM_IsDeleted = false;
                    saRole.ROM_UpdatedBy = "HANYSNT\\Ksuriyak";
                    saRole.ROM_UpdatedOn = DateTime.Now;
                    db.Entry(saRole).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else if (saRole == null)
                {
                    saRole = new RolesMaster() { ROM_Id = 0, ROM_Name = SuperAdminRoleName, ROM_Description = "HANYS admin/project manager role to manage app", ROM_Active = true, ROM_CreatedBy = "HANYSNT\\Ksuriyak", ROM_CreatedOn = DateTime.Now, ROM_UpdatedBy = "HANYSNT\\Ksuriyak", ROM_UpdatedOn = DateTime.Now, ROM_IsDeleted = false };
                    db.Entry(saRole).State = EntityState.Added;
                    db.SaveChanges();
                }

                return db.RolesMasters.Where(r => r.ROM_IsDeleted != true).Select(x => new Roles() { Id = x.ROM_Id, Name = x.ROM_Name, Description = x.ROM_Description, Active = x.ROM_Active, CreatedBy = x.ROM_CreatedBy, CreatedOn = x.ROM_CreatedOn, UpdatedBy = x.ROM_UpdatedBy, UpdatedOn = x.ROM_UpdatedOn });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<object> GetRole(int id)
        {
            RolesMaster rolesMaster = await db.RolesMasters.FindAsync(id);
            return new Roles()
            {
                Id = rolesMaster.ROM_Id,
                Name = rolesMaster.ROM_Name,
                Description = rolesMaster.ROM_Description,
                Active = rolesMaster.ROM_Active,
                CreatedBy = rolesMaster.ROM_CreatedBy,
                CreatedOn = rolesMaster.ROM_CreatedOn,
                UpdatedBy = rolesMaster.ROM_UpdatedBy,
                UpdatedOn = rolesMaster.ROM_UpdatedOn,
                UserCount = db.UserRoles.Where(x => x.URS_RoleId == rolesMaster.ROM_Id).Count()
            };
        }

        private RolesMaster GetRoleMaster(string RoleName)
        {
            return db.RolesMasters.Where(a => a.ROM_Name.Replace(" ", string.Empty) == RoleName.Replace(" ", string.Empty)).FirstOrDefault();
        }

        public async Task PutRolesMaster(Roles role)
        {
            try
            {
                RolesMaster roleMaster;
                var saRole = db.RolesMasters.Where(a => a.ROM_Name.Replace(" ", string.Empty) == SuperAdminRoleName.Replace(" ", string.Empty)).FirstOrDefault();
                if (saRole != null && saRole.ROM_Id == role.Id)
                {
                    saRole.ROM_Description = role.Description;
                    saRole.ROM_UpdatedBy = role.UpdatedBy;
                    saRole.ROM_UpdatedOn = DateTime.Now;
                    roleMaster = saRole;
                }
                else
                {
                    roleMaster = GetRoleMaster(role.Name);

                    if (roleMaster != null && roleMaster.ROM_Id > 0)
                    {
                        roleMaster.ROM_Description = role.Description;
                        roleMaster.ROM_Active = true;
                        roleMaster.ROM_UpdatedBy = role.UpdatedBy;
                        roleMaster.ROM_UpdatedOn = DateTime.Now;
                        roleMaster.ROM_IsDeleted = false;
                    }
                    else
                    {
                        roleMaster = new RolesMaster()
                        {
                            ROM_Id = role.Id,
                            ROM_Name = role.Name,
                            ROM_Description = role.Description,
                            ROM_Active = (bool)role.Active,
                            ROM_CreatedBy = role.CreatedBy,
                            ROM_CreatedOn = DateTime.Now,
                            ROM_UpdatedBy = role.UpdatedBy,
                            ROM_UpdatedOn = DateTime.Now
                        };
                    }
                }
                db.Entry(roleMaster).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task PostRole(Roles role)
        {
            try
            {
                if (role.Name.Replace(" ", string.Empty) != SuperAdminRoleName.Replace(" ", string.Empty))
                {
                    RolesMaster roleMaster = GetRoleMaster(role.Name);
                    if (roleMaster != null && roleMaster.ROM_Id > 0)
                    {
                        roleMaster.ROM_Description = role.Description;
                        roleMaster.ROM_Active = true;
                        roleMaster.ROM_UpdatedBy = role.UpdatedBy;
                        roleMaster.ROM_UpdatedOn = DateTime.Now;
                        roleMaster.ROM_IsDeleted = false;

                        db.Entry(roleMaster).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        RolesMaster rolesMaster = new RolesMaster()
                        {
                            ROM_Id = role.Id,
                            ROM_Name = role.Name,
                            ROM_Description = role.Description,
                            ROM_Active = (bool)role.Active,
                            ROM_CreatedBy = role.CreatedBy,
                            ROM_CreatedOn = DateTime.Now,
                            ROM_UpdatedBy = role.UpdatedBy,
                            ROM_UpdatedOn = DateTime.Now,
                            ROM_IsDeleted = false
                        };

                        db.RolesMasters.Add(rolesMaster);
                        await db.SaveChangesAsync();
                    }                    
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<object> GetRoleMaster(int id)
        {
            return await db.RolesMasters.FindAsync(id);
        }

        public async Task DeleteRolesMaster(RolesMaster rolesMaster)
        {
            try
            {
                if (rolesMaster.ROM_Name.Replace(" ", string.Empty) != SuperAdminRoleName.Replace(" ", string.Empty))
                {
                    rolesMaster.ROM_IsDeleted = true;
                    rolesMaster.ROM_Active = false;
                    db.Entry(rolesMaster).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    var userRoles = db.UserRoles.Where(x => x.URS_RoleId == rolesMaster.ROM_Id).ToList();
                    foreach (var userRole in userRoles)
                    {
                        db.Entry(userRole).State = EntityState.Deleted;
                        await db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool RoleExists(int id)
        {
            return db.RolesMasters.Count(e => e.ROM_Id == id) > 0;
        }

        public async Task<bool> IsSuperAdmin(string UserEmail)
        {
            var user = await db.Users.Where(u => u.USR_Email.ToLower().Replace(" ", string.Empty) == UserEmail.ToLower().Replace(" ", string.Empty)).FirstOrDefaultAsync();
            var rolesMaster = await db.RolesMasters.Where(r => r.ROM_IsDeleted != true && r.ROM_Name.Replace(" ", string.Empty) == SuperAdminRoleName.Replace(" ", string.Empty)).FirstOrDefaultAsync();            
            return user != null && rolesMaster != null && user.USR_Id > 0 && rolesMaster.ROM_Id > 0 ? db.UserRoles.Count(e => e.URS_UserId == user.USR_Id && e.URS_RoleId == rolesMaster.ROM_Id) > 0 : false;
        }
    }
}