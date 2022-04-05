namespace FasterMate.Core.Contracts
{
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels.User;

    public interface IUserService
    {
        Task<IEnumerable<UserListViewModel>> GetUsers();

        Task<UserEditViewModel> GetUserForEdit(string id);

        Task<bool> UpdateUser(UserEditViewModel model);

        Task<ApplicationUser> GetUserById(string id);

        Task<ApplicationUser> GetOnlyUserById(string id);
    }
}
