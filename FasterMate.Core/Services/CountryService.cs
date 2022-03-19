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
            this.repo = _repo;
        }

        public IEnumerable<KeyValuePair<Guid, string>> GetAllAsKvp()
        => this.repo
            .AllAsNoTracking()
            .Select(x => new
            {
                x.Id,
                x.Name
            })
            .OrderBy(x => x.Name)
            .Select(x => new KeyValuePair<Guid, string>(x.Id, x.Name));
    }
}
