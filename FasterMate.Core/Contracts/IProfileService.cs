namespace FasterMate.Core.Contracts
{
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels.Home;
    using FasterMate.ViewModels.Profile;

    public interface IProfileService
    {
        Profile GetById(string id);

        Profile GetByUserId(string id);

        string GetId(string userId);

        EditProfileViewModel GetEditViewModel(Profile profile);

        RenderProfileViewModel RenderProfile(string id);

        Task UpdateAsync(string id, EditProfileViewModel input, string path);

        Task<string> CreateAsync(RegisterViewModel input);

        Task FollowProfileAsync(string currentProfileId, string askingProfileId);

        IEnumerable<ProfileSearchViewModel> SearchProfiles(string[] searchTokens);
    }
}
