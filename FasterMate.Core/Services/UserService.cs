namespace FasterMate.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels.Common;
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

        public List<ApiViewModel> GetAPIData()
            => repo
                .AllAsNoTracking()
                .Include(x => x.Profile)
                .Select(x => new ApiViewModel()
                {
                    Username = x.UserName,
                    FirstName = x.Profile.FirstName,
                    LastName = x.Profile.LastName,
                    Email = x.Email,
                    Gender = (x.Profile.Gender).ToString()
                })
                .ToList();

        public async Task<ApplicationUser> GetOnlyUserByIdAsync(string id)
            => await repo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
            => await repo
                .AllAsNoTracking()
                .Include(x => x.Profile)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<UserEditViewModel> GetUserForEditAsync(string id)
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

        public async Task<IEnumerable<UserListViewModel>> GetUsersAsync()
        {
            return await repo
                    .AllAsNoTracking()
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

        public async Task<bool> UpdateUserAsync(UserEditViewModel model)
        {
            bool result = false;

            var user = await profileRepo
                .All()
                .FirstOrDefaultAsync(x => x.User.Id == model.Id);

            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                profileRepo.Update(user);
                await profileRepo.SaveChangesAsync();
                result = true;
            }

            return result;
        }
    }
}
