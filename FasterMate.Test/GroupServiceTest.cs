namespace FasterMate.Test
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using FasterMate.Core.Contracts;
    using FasterMate.Core.Services;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.Infrastructure.Data.Enums;
    using FasterMate.ViewModels.Group;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;

    public class GroupServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollecton = new ServiceCollection();

            serviceProvider = serviceCollecton
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton(typeof(IRepository<>), typeof(Repository<>))
                .AddSingleton<IImageService, ImageService>()
                .AddSingleton<IGroupService, GroupService>()
                .BuildServiceProvider();

            await SeedDb();
        }

        [Test]
        public async Task AddMemberSuccessfully()
        {
            var groupMemberRepo = serviceProvider.GetService<IRepository<GroupMember>>();
            var groupService = serviceProvider.GetService<IGroupService>();

            await groupService.AddMemberAsync("test", "test");

            Assert.AreEqual(1, groupMemberRepo.All().Count());
        }

        [Test]
        public async Task AddMemberUnSuccessfully()
        {
            var groupMemberRepo = serviceProvider.GetService<IRepository<GroupMember>>();
            var groupService = serviceProvider.GetService<IGroupService>();

            await groupService.AddMemberAsync("test", "test");
            await groupService.AddMemberAsync("test", "test");

            Assert.AreNotEqual(2, groupMemberRepo.All().Count());

        }

        [Test]
        public async Task CreateGroupSuccessfully()
        {
            var groupRepo = serviceProvider.GetService<IRepository<Group>>();
            var groupService = serviceProvider.GetService<IGroupService>();

            using (var stream = File.OpenRead("test.png"))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png"
                };

                var input = new CreateGroupViewModel()
                {
                    Name = "tets name group",
                    Image = file
                };

                await groupService.CreateAsync("c996abfe-1850-48dd-bfcd-b61f18ec3358", input, $"{Directory.GetCurrentDirectory()}");
            }

            Assert.AreEqual(2, groupRepo.All().Count());
        }

        [Test]
        public async Task DeleteGroupSuccessfully()
        {
            var groupMemberRepo = serviceProvider.GetService<IRepository<GroupMember>>();
            var groupService = serviceProvider.GetService<IGroupService>();

            await groupService.DeleteGroupAsync("test");

            Assert.AreEqual(0, groupMemberRepo.All().Count());
        }

        [Test]
        public async Task DeleteGroupWithMembersSuccessfully()
        {
            var groupMemberRepo = serviceProvider.GetService<IRepository<GroupMember>>();
            var groupService = serviceProvider.GetService<IGroupService>();

            await groupService.AddMemberAsync("test", "test");

            await groupService.DeleteGroupAsync("test");

            Assert.AreEqual(0, groupMemberRepo.All().Count());
        }

        [Test]
        public async Task DeleteGroupWithMessagesSuccessfully()
        {
            var msgRepo = serviceProvider.GetService<IRepository<Message>>();
            var groupService = serviceProvider.GetService<IGroupService>();

            await groupService.DeleteGroupAsync("test");

            Assert.AreEqual(0, msgRepo.All().Count());
        }

        [Test]
        public void GetByIdSuccessfully()
        {
            var groupService = serviceProvider.GetService<IGroupService>();

            var expected = new GroupViewModel()
            {
                Id = "test",
                Name = "Test Group Name",
                Messages = null
            };

            var actual = groupService.GetGroupById("test");

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public void GetGroupForEditSuccessfully()
        {
            var groupService = serviceProvider.GetService<IGroupService>();
            var expected = new EditGroupViewModel()
            {
                Id = "test",
                Name = "Test Group Name"
            };

            var actual = groupService.GetGroupForEdit("test");

            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public async Task GetMembersSuccessfully()
        {
            var groupService = serviceProvider.GetService<IGroupService>();
            var groupRepo = serviceProvider.GetService<IRepository<Group>>();

            var expected = new GroupMemberInfoViewModel()
            {
                ProfileId = "test",
                Name = "test test",
                Username = "test",
                ImagePath = "test.png",
                IsOwner = true
            };

            using (var stream = File.OpenRead("test.png"))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png"
                };

                var input = new CreateGroupViewModel()
                {
                    Name = "tets name group",
                    Image = file
                };

                await groupService.CreateAsync("test", input, $"{Directory.GetCurrentDirectory()}");
            }

            var groupId = groupRepo.All().Skip(1).FirstOrDefault().Id;

            var actual = groupService.GetMembers(groupId, "test");

            Assert.AreEqual(expected.IsOwner, actual.FirstOrDefault().IsOwner);
        }

        [Test]
        public void IsOwnerOfTheGroupSuccessfully()
        {
            var groupService = serviceProvider.GetService<IGroupService>();

            var expected = true;
            var actual = groupService.IsOwnerOfTheGroup("test", "c996abfe-1850-48dd-bfcd-b61f18ec3358");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsMemberOfTheGroupSuccessfully()
        {
            var groupService = serviceProvider.GetService<IGroupService>();

            var expected = true;
            var actual = groupService.IsMemberOfTheGroup("test", "c996abfe-1850-48dd-bfcd-b61f18ec3358");

            Assert.AreNotEqual(expected, actual);
        }

        [Test]
        public async Task RemoveSuccessfully()
        {
            var groupService = serviceProvider.GetService<IGroupService>();
            var groupMemberRepo = serviceProvider.GetService<IRepository<GroupMember>>();

            await groupService.AddMemberAsync("c996abfe-1850-48dd-bfcd-b61f18ec3358", "test");
            await groupService.RemoveAsync("test", "c996abfe-1850-48dd-bfcd-b61f18ec3358");

            Assert.AreEqual(0, groupMemberRepo.All().Count());
        }

        [Test]
        public async Task UpdateWithoutPhotoSuccessfully()
        {
            var groupService = serviceProvider.GetService<IGroupService>();
            var groupRepo = serviceProvider.GetService<IRepository<Group>>();

            var input = new EditGroupViewModel()
            {
                Id = "test",
                Name = "test name etc."
            };

            await groupService.UpdateAsync(input, "");

            Assert.AreEqual("test name etc.", groupRepo.All().FirstOrDefault().Name);
        }

        [Test]
        public async Task UpdateWithPhotoSuccessfully()
        {
            var groupService = serviceProvider.GetService<IGroupService>();
            var groupRepo = serviceProvider.GetService<IRepository<Group>>();

            using (var stream = File.OpenRead("test.png"))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png"
                };

                var input = new EditGroupViewModel()
                {
                    Id = "test",
                    Name = "test name etc.",
                    Image = file
                };

                await groupService.UpdateAsync(input, $"{Directory.GetCurrentDirectory()}");
            }

            Assert.AreEqual("test name etc.", groupRepo.All().FirstOrDefault().Name);
        }

        [Test]
        public async Task GetProfileGroupsSuccessfully()
        {
            var groupService = serviceProvider.GetService<IGroupService>();

            var expected = new ProfileGroupsViewModel()
            {
                Id = "test",
                Name = "Test Group Name",
                ImagePath = "test1234-test-test-test-test1234test.test",
                GroupMembersCount = 1
            };
            await groupService.AddMemberAsync("test", "test");

            var actual = groupService.GetProfileGroups("test");

            Assert.AreEqual(expected.Id, actual.FirstOrDefault().Id);
        }

        [Test]
        public void GetFollowersSuccessfully()
        {
            var groupService = serviceProvider.GetService<IGroupService>();

            var expected = 1;
            var actual = groupService.GetFollowersOfProfile("test", "test");

            Assert.AreEqual(expected, actual.Count());

        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDb()
        {
            var countryRepo = serviceProvider.GetService<IRepository<Country>>();
            var imgRepo = serviceProvider.GetService<IRepository<Image>>();
            var profileRepo = serviceProvider.GetService<IRepository<Profile>>();
            var groupRepo = serviceProvider.GetService<IRepository<Group>>();
            var profileFollowerRepo = serviceProvider.GetService<IRepository<ProfileFollower>>();
            var msgRepo = serviceProvider.GetService<IRepository<Message>>();

            await countryRepo.AddAsync(new Country() { Id = "test", Name = "test" });
            await countryRepo.SaveChangesAsync();

            var image = new Image() { Id = "test", Extension = "test" };
            await imgRepo.AddAsync(image);
            await imgRepo.SaveChangesAsync();

            var profile = new Profile()
            {
                Id = "c996abfe-1850-48dd-bfcd-b61f18ec3358",
                FirstName = "test",
                LastName = "test",
                CountryId = countryRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
                BirthDate = DateTime.UtcNow,
                Gender = Gender.Male,
                User = new ApplicationUser()
                {
                    Id = "310b8a7e-734d-44d6-b3f3-efa6e8f6259d"
                },
                Image = image,
            };
            await profileRepo.AddAsync(profile);

            await profileRepo.AddAsync(new Profile()
            {
                Id = "test",
                FirstName = "test",
                LastName = "test",
                CountryId = countryRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
                BirthDate = DateTime.UtcNow,
                Gender = Gender.Male,
                User = new ApplicationUser()
                {
                    Id = "test"
                },
                ImageId = imgRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
            });
            await profileRepo.SaveChangesAsync();

            var group = new Group()
            {
                Id = "test",
                Name = "Test Group Name",
                ProfileId = "c996abfe-1850-48dd-bfcd-b61f18ec3358",
                Image = image,
                Profile = profile
            };
            await groupRepo.AddAsync(group);
            await groupRepo.SaveChangesAsync();

            await msgRepo.AddAsync(new Message()
            {
                Id = "test",
                Text = "some test text",
                Group = group,
                GroupId = "test",
                Profile = profile,
                ProfileId = "c996abfe-1850-48dd-bfcd-b61f18ec3358",
                CreatedOn = DateTime.UtcNow
            });
            await msgRepo.SaveChangesAsync();

            await profileFollowerRepo.AddAsync(new ProfileFollower()
            {
                ProfileId = "test",
                FollowerId = "c996abfe-1850-48dd-bfcd-b61f18ec3358"
            });
            await profileFollowerRepo.SaveChangesAsync();
        }
    }
}
