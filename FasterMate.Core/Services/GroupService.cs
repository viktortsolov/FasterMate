using FasterMate.Core.Contracts;
using FasterMate.Infrastructure.Common;
using FasterMate.Infrastructure.Data;
using FasterMate.ViewModels.Group;
using FasterMate.ViewModels.Message;
using Microsoft.EntityFrameworkCore;

namespace FasterMate.Core.Services
{
    public class GroupService : IGroupService
    {
        private readonly IRepository<GroupMember> groupMemberRepo;
        private readonly IRepository<Group> groupRepo;
        private readonly IRepository<Message> messageRepo;

        private readonly IImageService imgService;

        public GroupService(
            IRepository<GroupMember> _groupMemberRepo,
            IRepository<Group> _groupRepo,
            IRepository<Message> _messageRepo,
            IImageService _imgService)
        {
            groupMemberRepo = _groupMemberRepo;
            groupRepo = _groupRepo;
            messageRepo = _messageRepo;

            imgService = _imgService;
        }

        public async Task<string> CreateAsync(string profileId, CreateGroupViewModel input, string path)
        {
            var group = new Group()
            {
                Name = input.Name
            };

            if (input.Image?.Length > 0)
            {
                group.ImageId = await imgService.CreateAsync(input.Image, path);
            }

            await groupRepo.AddAsync(group);
            await groupRepo.SaveChangesAsync();

            var groupMember = new GroupMember()
            {
                ProfileId = profileId,
                GroupId = group.Id
            };

            await groupMemberRepo.AddAsync(groupMember);
            await groupMemberRepo.SaveChangesAsync();

            return group.Id;
        }

        public GroupViewModel GetById(string groupId)
            => groupRepo
                .AllAsNoTracking()
                .Where(x => x.Id == groupId)
                .Select(x => new GroupViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Messages = messageRepo
                        .AllAsNoTracking()
                        .Include(x => x.Profile)
                        .Where(x => x.GroupId == groupId)
                        .Select(x => new MessageViewModel()
                        {
                            Id = x.Id,
                            Text = x.Text,
                            ProfileId = x.ProfileId,
                            ProfileName = $"{x.Profile.FirstName} {x.Profile.LastName}",
                            CreatedOn = x.CreateOn.ToString("dd/MM/yyyy HH:mm")
                        })
                })
                .FirstOrDefault();

        public bool IsMemberOfTheGroup(string groupId, string profileId)
            => groupMemberRepo
                .AllAsNoTracking()
                .Where(x => x.GroupId == groupId && x.ProfileId == profileId)
                .FirstOrDefault() != null;
    }
}
