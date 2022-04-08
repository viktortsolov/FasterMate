namespace FasterMate.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.Infrastructure.Data.Enums;
    using FasterMate.ViewModels.Home;
    using FasterMate.ViewModels.Profile;

    using Microsoft.EntityFrameworkCore;

    public class ProfileService : IProfileService
    {
        private readonly IRepository<Profile> profileRepo;
        private readonly IRepository<Country> countryRepo;
        private readonly IRepository<ProfileFollower> followersRepo;

        private readonly IPostService postService;
        private readonly IImageService imageService;

        public ProfileService(
            IRepository<Profile> _profileRepo,
            IRepository<Country> _countryRepo,
            IRepository<ProfileFollower> _followersRepo,
            IPostService _postService,
            IImageService _imageService)
        {
            profileRepo = _profileRepo;
            countryRepo = _countryRepo;
            followersRepo = _followersRepo;

            postService = _postService;
            imageService = _imageService;
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

        public string GetId(string userId)
            => profileRepo
                .AllAsNoTracking()
                .Where(x => x.User.Id == userId)
                .Select(x => x.Id)
                .FirstOrDefault();

        public Profile GetById(string id)
            => profileRepo
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == id);

        public Profile GetByUserId(string id)
            => profileRepo
                 .AllAsNoTracking()
                 .FirstOrDefault(x => x.User.Id == id);

        public EditProfileViewModel GetEditViewModel(Profile profile)
        {
            var editProfileViewModel = new EditProfileViewModel()
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Bio = profile.Bio
            };

            return editProfileViewModel;
        }

        public RenderProfileViewModel RenderProfile(string id)
        {
            var profile = profileRepo
                .AllAsNoTracking()
                .Include(x => x.Country)
                .Include(x => x.Image)
                .Include(x => x.Followers)
                .Include(x => x.Following)
                .Where(x => x.User.ProfileId == id)
                .FirstOrDefault();

            var profileViewModel = new RenderProfileViewModel()
            {
                Id = profile.Id,
                IsFollowing = profile.Following.Any(x => x.ProfileId == id),
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Gender = profile.Gender.ToString(),
                Birthdate = profile.BirthDate,
                Bio = profile.Bio,
                Country = profile.Country.Name,
                FollowingCount = profile.Followers.Count,
                FollowersCount = profile.Following.Count,
                Posts = postService.RenderPostsForProfile(id)
            };

            if (profile.Image != null)
            {
                var imgId = profile.Image.Id;
                var imgExtension = profile.Image.Extension;

                profileViewModel.ImagePath = $"{imgId}.{imgExtension}";
            }

            return profileViewModel;
        }

        public async Task UpdateAsync(string id, EditProfileViewModel input, string path)
        {
            var profile = profileRepo
                .All()
                .FirstOrDefault(x => x.User.Id == id);

            if (profile != null)
            {
                profile.FirstName = input.FirstName;
                profile.LastName = input.LastName;
                profile.Bio = input.Bio;

                if (input.Image?.Length > 0)
                {
                    profile.ImageId = await imageService.CreateAsync(input.Image, path);
                }

                profileRepo.Update(profile);
                await profileRepo.SaveChangesAsync();
            }
        }

        public async Task FollowProfileAsync(string currentProfileId, string askingProfileId)
        {
            if (askingProfileId != currentProfileId)
            {
                var followingRelation = followersRepo
                    .All()
                    .FirstOrDefault
                        (x => x.FollowerId == askingProfileId && x.ProfileId == currentProfileId);

                if (followingRelation == null)
                {
                    await followersRepo.AddAsync(new ProfileFollower
                    {
                        ProfileId = currentProfileId,
                        FollowerId = askingProfileId,
                    });
                }
                else
                {
                    followersRepo.Delete(followingRelation);
                }

                await followersRepo.SaveChangesAsync();
            }
        }

        public IEnumerable<ProfileSearchViewModel> SearchProfiles(string[] searchTokens)
        {
            var results = new List<ProfileSearchViewModel>();

            foreach (var search in searchTokens)
            {
                results.AddRange(profileRepo
                    .AllAsNoTracking()
                    .Include(x => x.User)
                    .Include(x => x.Image)
                    .Where(x => x.FirstName.ToLower().Contains(search) ||
                                x.LastName.ToLower().Contains(search) ||
                                x.User.UserName.ToLower().Contains(search))
                    .OrderByDescending(x => x.User.UserName)
                    .Select(x => new ProfileSearchViewModel()
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Username = x.User.UserName,
                        ImagePath = x.Image != null ? $"{x.Image.Id}.{x.Image.Extension}" : "",
                        Followers = x.Followers.Count(),
                        Following = x.Following.Count()
                    })
                    .ToList());
            }

            return results;
        }

        private bool CountryValidation(string id)
            => countryRepo.AllAsNoTracking().FirstOrDefault(x => x.Id == id) == null;

        private static Gender GetGender(string gender)
        {
            _ = Enum.TryParse<Gender>(gender, out Gender genderValue);

            return genderValue;
        }
    }
}