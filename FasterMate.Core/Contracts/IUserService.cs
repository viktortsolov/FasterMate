namespace FasterMate.Core.Contracts
{
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels.Common;
    using FasterMate.ViewModels.User;

    public interface IUserService
    {
        Task<IEnumerable<UserListViewModel>> GetUsersAsync();

        Task<UserEditViewModel> GetUserForEditAsync(string id);

        Task<bool> UpdateUserAsync(UserEditViewModel model);

        Task<ApplicationUser> GetUserByIdAsync(string id);

        Task<ApplicationUser> GetOnlyUserByIdAsync(string id);

        List<ApiViewModel> GetAPIData();
    }
}
