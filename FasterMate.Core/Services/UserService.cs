namespace FasterMate.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels.User;

    using Microsoft.EntityFrameworkCore;

    public class UserService : IUserService
    {
        private readonly IRepository<ApplicationUser> repo;
        private readonly IRepository<Profile> profileRepo;

        public UserService(
            IRepository<ApplicationUser> _repo, 
            IRepository<Profile> _profileRepo)
        {
            repo = _repo;
            profileRepo = _profileRepo;
        }

        public async Task<UserEditViewModel> GetUserForEdit(string id)
        {
            var user = await repo
                .AllAsNoTracking()
                .Include(x => x.Profile)
                .FirstOrDefaultAsync(x => x.Id == id);

            return new UserEditViewModel()
            {
                Id = user.Id,
                FirstName = user.Profile.FirstName,
                LastName = user.Profile.LastName
            };
        }

        public async Task<IEnumerable<UserListViewModel>> GetUsers()
        {
            return await repo
                    .All()
                    .Include(x => x.Profile)
                    .Select(x => new UserListViewModel()
                    {
                        Id = x.Id,
                        Email = x.Email,
                        Name = $"{x.Profile.FirstName} {x.Profile.LastName}",
                        Username = x.UserName
                    })
                    .ToListAsync();
        }

        //TODO: Updating user does not work...
        public async Task<bool> UpdateUser(UserEditViewModel model)
        {
            bool result = false;

            var user = await profileRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.User.Id == model.Id);

            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                profileRepo.Update(user);
                profileRepo.SaveChangesAsync();
                result = true;
            }

            return result;
        }
    }
}
