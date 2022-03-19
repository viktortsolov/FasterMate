namespace FasterMate.Core.Contracts
{
    using FasterMate.ViewModels.Profile;

    public interface IProfileService
    {
        Guid GetId(string userId);

        Task<Guid> CreateAsync(RegisterViewModel input);
    }
}
