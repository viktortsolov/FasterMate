namespace FasterMate.Core.Contracts
{
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels.Profile;

    public interface IProfileService
    {
        Profile GetById(string id);

        Profile GetByUserId(string id);

        string GetId(string userId);

        Task<string> CreateAsync(RegisterViewModel input);

        RenderProfileViewModel RenderProfile(string id);

        EditProfileViewModel GetEditViewModel(Profile profile);

        Task UpdateAsync(string id, EditProfileViewModel input, string path);

    }
}
