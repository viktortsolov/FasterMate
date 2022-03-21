namespace FasterMate.Core.Contracts
{
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels.Profile;

    public interface IProfileService
    {
        string GetId(string userId);

        T GetById<T>(string id);

        Task<string> CreateAsync(RegisterViewModel input);

        RenderProfileViewModel RenderProfile(string id);
    }
}
