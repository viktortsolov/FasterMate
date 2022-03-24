namespace FasterMate.Core.Contracts
{
    using FasterMate.ViewModels.User;

    public interface IUserService
    {
        Task<IEnumerable<UserListViewModel>> GetUsers();

        Task<UserEditViewModel> GetUserForEdit(string id);

        Task<bool> UpdateUser(UserEditViewModel model);
    }
}
