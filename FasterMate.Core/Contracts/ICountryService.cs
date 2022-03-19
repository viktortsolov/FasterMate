namespace FasterMate.Core.Contracts
{
    public interface ICountryService
    {
        IEnumerable<KeyValuePair<Guid, string>> GetAllAsKvp();
    }
}
