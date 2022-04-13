namespace FasterMate.Core.Contracts
{
    public interface ICountryService
    {
        ICollection<KeyValuePair<string, string>> GetAllAsKvp();
    }
}
