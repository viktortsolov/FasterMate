namespace FasterMate.Core.Services
{
    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels.Profile;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class UserService : IUserService
    {
        private readonly IRepository<ApplicationUser> repo;

        public UserService(IRepository<ApplicationUser> _repo)
        {
            repo = _repo;
        }

        public async Task<IEnumerable<UserListViewModel>> GetUsers()
        {
            return await repo
                    .All()
                    .Include(x => x.ProfileId)
                    .Select(x => new UserListViewModel()
                    {
                        Id = x.Id,
                        Email = x.Email,
                        Name = $"{x.Profile.FirstName} {x.Profile.LastName}"
                    })
                    .ToListAsync();
        }
    }
}
