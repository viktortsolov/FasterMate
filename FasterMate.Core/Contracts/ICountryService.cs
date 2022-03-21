namespace FasterMate.Core.Contracts
{
    public interface ICountryService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKvp();
    }
}
