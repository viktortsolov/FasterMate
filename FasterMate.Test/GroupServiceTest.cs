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

            await groupService.AddMemberAsync("test1234-test-test-test-test1234test", "test1234-test-test-test-test1234test");

            Assert.AreEqual(1, groupMemberRepo.All().Count());
        }

        [Test]
        public async Task AddMemberUnSuccessfully()
        {
            var groupMemberRepo = serviceProvider.GetService<IRepository<GroupMember>>();
            var groupService = serviceProvider.GetService<IGroupService>();

            await groupService.AddMemberAsync("test1234-test-test-test-test1234test", "test1234-test-test-test-test1234test");
            await groupService.AddMemberAsync("test1234-test-test-test-test1234test", "test1234-test-test-test-test1234test");

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

            await groupService.DeleteGroupAsync("test1234-test-test-test-test1234test");

            Assert.AreEqual(0, groupMemberRepo.All().Count());
        }

        [Test]
        public async Task DeleteGroupWithMembersSuccessfully()
        {
            var groupMemberRepo = serviceProvider.GetService<IRepository<GroupMember>>();
            var groupService = serviceProvider.GetService<IGroupService>();

            await groupService.AddMemberAsync("test1234-test-test-test-test1234test", "test1234-test-test-test-test1234test");

            await groupService.DeleteGroupAsync("test1234-test-test-test-test1234test");

            Assert.AreEqual(0, groupMemberRepo.All().Count());
        }

        [Test]
        public async Task DeleteGroupWithMessagesSuccessfully()
        {
            var msgRepo = serviceProvider.GetService<IRepository<Message>>();
            var groupService = serviceProvider.GetService<IGroupService>();

            await groupService.DeleteGroupAsync("test1234-test-test-test-test1234test");

            Assert.AreEqual(0, msgRepo.All().Count());
        }

        //TODO: Work on this test
        [Test]
        public void GetByIdSuccessfully()
        {
            var groupService = serviceProvider.GetService<IGroupService>();

            var expected = new GroupViewModel()
            {
                Id = "test1234-test-test-test-test1234test",
                Name = "Test Group Name",
                Messages = null
            };

            var actual = groupService.GetGroupById("test1234-test-test-test-test1234test");

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public void GetGroupForEditSuccessfully()
        {
            var groupService = serviceProvider.GetService<IGroupService>();
            var expected = new EditGroupViewModel()
            {
                Id = "test1234-test-test-test-test1234test",
                Name = "Test Group Name"
            };

            var actual = groupService.GetGroupForEdit("test1234-test-test-test-test1234test");

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
                ProfileId = "test1234-test-test-test-test1234test",
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

                await groupService.CreateAsync("test1234-test-test-test-test1234test", input, $"{Directory.GetCurrentDirectory()}");
            }

            var groupId = groupRepo.All().Skip(1).FirstOrDefault().Id;

            var actual = groupService.GetMembers(groupId, "test1234-test-test-test-test1234test");

            Assert.AreEqual(expected.IsOwner, actual.FirstOrDefault().IsOwner);
        }

        [Test]
        public void IsOwnerOfTheGroupSuccessfully()
        {
            var groupService = serviceProvider.GetService<IGroupService>();

            var expected = true;
            var actual = groupService.IsOwnerOfTheGroup("test1234-test-test-test-test1234test", "c996abfe-1850-48dd-bfcd-b61f18ec3358");

            Assert.AreNotEqual(expected, actual);
        }

        [Test]
        public void IsMemberOfTheGroupSuccessfully()
        {
            var groupService = serviceProvider.GetService<IGroupService>();

            var expected = true;
            var actual = groupService.IsMemberOfTheGroup("test1234-test-test-test-test1234test", "c996abfe-1850-48dd-bfcd-b61f18ec3358");

            Assert.AreNotEqual(expected, actual);
        }

        [Test]
        public async Task RemoveSuccessfully()
        {
            var groupService = serviceProvider.GetService<IGroupService>();
            var groupMemberRepo = serviceProvider.GetService<IRepository<GroupMember>>();

            await groupService.AddMemberAsync("c996abfe-1850-48dd-bfcd-b61f18ec3358", "test1234-test-test-test-test1234test");
            await groupService.RemoveAsync("test1234-test-test-test-test1234test", "c996abfe-1850-48dd-bfcd-b61f18ec3358");

            Assert.AreEqual(0, groupMemberRepo.All().Count());
        }

        [Test]
        public async Task UpdateWithoutPhotoSuccessfully()
        {
            var groupService = serviceProvider.GetService<IGroupService>();
            var groupRepo = serviceProvider.GetService<IRepository<Group>>();

            var input = new EditGroupViewModel()
            {
                Id = "test1234-test-test-test-test1234test",
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
                    Id = "test1234-test-test-test-test1234test",
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
                Id = "test1234-test-test-test-test1234test",
                Name = "Test Group Name",
                ImagePath = "test1234-test-test-test-test1234test.test",
                GroupMembersCount = 1
            };
            await groupService.AddMemberAsync("test1234-test-test-test-test1234test", "test1234-test-test-test-test1234test");

            var actual = groupService.GetProfileGroups("test1234-test-test-test-test1234test");

            Assert.AreEqual(expected.Id, actual.FirstOrDefault().Id);
        }

        [Test]
        public void GetFollowersSuccessfully()
        {
            var groupService = serviceProvider.GetService<IGroupService>();

            var expected = 1;
            var actual = groupService.GetFollowersOfProfile("test1234-test-test-test-test1234test", "test1234-test-test-test-test1234test");

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

            await countryRepo.AddAsync(new Country() { Id = "test1234-test-test-test-test1234test", Name = "test" });
            await countryRepo.SaveChangesAsync();

            await imgRepo.AddAsync(new Image() { Id = "test1234-test-test-test-test1234test", Extension = "test" });
            await imgRepo.SaveChangesAsync();

            await profileRepo.AddAsync(new Profile()
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
                ImageId = imgRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
            });

            await profileRepo.AddAsync(new Profile()
            {
                Id = "test1234-test-test-test-test1234test",
                FirstName = "test",
                LastName = "test",
                CountryId = countryRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
                BirthDate = DateTime.UtcNow,
                Gender = Gender.Male,
                User = new ApplicationUser()
                {
                    Id = "test1234-test-test-test-test1234test"
                },
                ImageId = imgRepo.AllAsNoTracking().Select(x => x.Id).FirstOrDefault(),
            });
            await profileRepo.SaveChangesAsync();

            await groupRepo.AddAsync(new Group()
            {
                Id = "test1234-test-test-test-test1234test",
                Name = "Test Group Name",
                ImageId = "test1234-test-test-test-test1234test",
                ProfileId = "test1234-test-test-test-test1234test"
            });
            await groupRepo.SaveChangesAsync();

            await msgRepo.AddAsync(new Message()
            {
                Id = "test1234-test-test-test-test1234test",
                Text = "some test text",
                GroupId = "test1234-test-test-test-test1234test",
                ProfileId = "test1234-test-test-test-test1234test",
                CreatedOn = DateTime.UtcNow
            });
            await msgRepo.SaveChangesAsync();

            await profileFollowerRepo.AddAsync(new ProfileFollower()
            {
                ProfileId = "test1234-test-test-test-test1234test",
                FollowerId = "c996abfe-1850-48dd-bfcd-b61f18ec3358"
            });
            await profileFollowerRepo.SaveChangesAsync();
        }
    }
}
