﻿namespace FasterMate.Core.Services
{
    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.Infrastructure.Data.Enums;
    using FasterMate.ViewModels.Profile;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProfileService : IProfileService
    {
        private readonly IRepository<Profile> profileRepo;
        private readonly IRepository<Country> countryRepo;


        public async Task<Guid> CreateAsync(RegisterViewModel input)
        {
            if (CountryValidation(input.CountryId))
            {
                throw new ArgumentException("The selected country must be valid!");
            }

            var gender = GetGender(input.Gender);

            if (gender == null)
            {
                throw new ArgumentException("The selected gender must be valid!");
            }

            var profile = new Profile()
            {
                BirthDate = input.BirthDate,
                FirstName = input.FirstName,
                LastName = input.LastName,
                Gender = gender,
                CountryId = input.CountryId,
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


        private bool CountryValidation(Guid id)
            => countryRepo.AllAsNoTracking().FirstOrDefault(x => x.Id == id) == null;
    }
}
