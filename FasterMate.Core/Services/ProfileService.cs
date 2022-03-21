namespace FasterMate.Core.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.Infrastructure.Data.Enums;
    using FasterMate.ViewModels.Profile;

    public class ProfileService : IProfileService
    {
        private readonly IRepository<Profile> profileRepo;
        private readonly IRepository<Country> countryRepo;

        public ProfileService(
            IRepository<Profile> _profileRepo,
            IRepository<Country> _countryRepo)
        {
            this.profileRepo = _profileRepo;
            this.countryRepo = _countryRepo;
        }

        public async Task<Guid> CreateAsync(RegisterViewModel input)
        {
            if (CountryValidation(input.CountryId))
            {
                throw new ArgumentException("The selected country must be valid!");
            }

            var gender = GetGender(input.Gender);

            if (gender == 0)
            {
                throw new ArgumentException("The selected gender must be valid!");
            }

            var profile = new Profile()
            {
                BirthDate = DateTime.Parse(input.BirthDate),
                FirstName = input.FirstName,
                LastName = input.LastName,
                Gender = gender,
                CountryId = input.CountryId
            };

            await profileRepo.AddAsync(profile);
            await profileRepo.SaveChangesAsync();

            return profile.Id;
        }

        public Guid GetId(string userId)
         => profileRepo.AllAsNoTracking()
            .Where(x => x.User.Id == userId)
            .Select(x => x.Id)
            .FirstOrDefault();


        private Gender GetGender(string gender)
        {
            Enum.TryParse<Gender>(gender, out Gender genderValue);

            return genderValue;
        }
        
        public RenderProfileViewModel RenderProfile(Guid id)
        {
            //TODO:
            var profile = profileRepo
                .AllAsNoTracking();

            var count = profile
                .Where(x=>x.Id==id);

            Profile countA = count.FirstOrDefault();

            //var profileViewModel = new RenderProfileViewModel()
            //{
            //    Id = profile.Id,
            //    FirstName = profile.FirstName,
            //    LastName = profile.LastName,
            //    Gender = profile.Gender.ToString(),
            //    Birthdate = profile.BirthDate,
            //    Bio = profile.Bio,
            //    Country = profile.Country.Name,
            //    FollowersCount = 0,
            //    FollowingCount = 0,
            //    Posts = null
            //};

            return null;
        }

        private bool CountryValidation(Guid id)
            => countryRepo.AllAsNoTracking().FirstOrDefault(x => x.Id == id) == null;

        public T GetById<T>(int id)
        {
            throw new NotImplementedException();
        }
    }
}
