using FasterMate.ViewModels.Profile;

namespace FasterMate.Core.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserListViewModel>> GetUsers();
    }
}
