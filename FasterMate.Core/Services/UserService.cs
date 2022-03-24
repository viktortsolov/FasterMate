namespace FasterMate.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels.User;

    using Microsoft.EntityFrameworkCore;

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
    }
}
