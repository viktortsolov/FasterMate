namespace FasterMate.Core.Contracts
{
    using FasterMate.ViewModels.Profile;

    public interface IProfileService
    {
        Guid GetId(string userId);

        T GetById<T>(int id);

        Task<Guid> CreateAsync(RegisterViewModel input);

        RenderProfileViewModel RenderProfile(Guid id);
    }
}
