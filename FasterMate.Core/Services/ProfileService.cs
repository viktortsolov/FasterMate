﻿namespace FasterMate.Core.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.Infrastructure.Data.Enums;
    using FasterMate.ViewModels.Profile;
    using Microsoft.EntityFrameworkCore;

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

        public async Task<string> CreateAsync(RegisterViewModel input)
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
        public T GetById<T>(string id)
        {
            throw new NotImplementedException();
        }

        public string GetId(string profileId)
         => profileRepo.AllAsNoTracking()
            .Where(x => x.User.ProfileId == profileId)
            .Select(x => x.Id)
            .FirstOrDefault();


        public RenderProfileViewModel RenderProfile(string id)
        {
            var profile =
                profileRepo
                .AllAsNoTracking()
                .Include(x => x.Country)
                .Where(x => x.User.ProfileId == id)
                .FirstOrDefault();


            var profileViewModel = new RenderProfileViewModel()
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Gender = profile.Gender.ToString(),
                Birthdate = profile.BirthDate,
                Bio = profile.Bio,
                Country = profile.Country.Name,
                FollowersCount = 0,
                FollowingCount = 0
            };

            return profileViewModel;
        }

        private bool CountryValidation(string id)
            => countryRepo.AllAsNoTracking().FirstOrDefault(x => x.Id == id) == null;

        private Gender GetGender(string gender)
        {
            Enum.TryParse<Gender>(gender, out Gender genderValue);

            return genderValue;
        }
    }
}
