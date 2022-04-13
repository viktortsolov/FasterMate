namespace FasterMate.Core.Services
{
    using System.Collections.Generic;

    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;

    public class CountryService : ICountryService
    {
        private readonly IRepository<Country> repo;

        public CountryService(
            IRepository<Country> _repo)
        {
            repo = _repo;
        }

        public ICollection<KeyValuePair<string, string>> GetAllAsKvp()
        => repo
            .AllAsNoTracking()
            .Select(x => new { x.Id, x.Name })
            .OrderBy(x => x.Name)
            .Select(x => new KeyValuePair<string, string>(x.Id, x.Name))
            .ToList();
    }
}
