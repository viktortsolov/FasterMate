﻿namespace FasterMate.Core.Services
{
    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels.Group;
    using FasterMate.ViewModels.Message;

    using Microsoft.EntityFrameworkCore;

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
                Name = input.Name,
                ProfileId = profileId
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

        public async Task DeleteGroupAsync(string groupId)
        {
            var group = groupRepo
                .All()
                .Where(x => x.Id == groupId)
                .FirstOrDefault();

            if (group != null)
            {
                foreach (var groupMember in groupMemberRepo.All().Where(x => x.GroupId == groupId))
                {
                    groupMemberRepo.Delete(groupMember);
                }

                groupRepo.Delete(group);
                await groupMemberRepo.SaveChangesAsync();
            }
        }

        public GroupViewModel GetGroupById(string groupId)
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
                        .ToList()
                })
                .FirstOrDefault();

        public EditGroupViewModel GetGroupForEdit(string groupId)
            => groupRepo
                .AllAsNoTracking()
                .Where(x => x.Id == groupId)
                .Select(x => new EditGroupViewModel()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .FirstOrDefault();

        public IEnumerable<GroupMemberViewModel> GetMembers(string groupId, string profileId)
            => groupMemberRepo
                .AllAsNoTracking()
                .Include(x => x.Profile)
                .Include(x => x.Profile.Image)
                .Where(x => x.GroupId == groupId)
                .Select(x => new GroupMemberViewModel
                {
                    ProfileId = x.ProfileId,
                    Name = $"{x.Profile.FirstName} {x.Profile.LastName}",
                    ImagePath = x.Profile.Image != null ? $"{x.Profile.Image.Id} {x.Profile.Image.Extension}" : "",
                    IsOwner = x.ProfileId == profileId
                })
                .ToList();

        public IEnumerable<ProfileGroupsViewModel> GetProfileGroups(string profileId)
            => groupRepo
                .AllAsNoTracking()
                .Include(x => x.Image)
                .Where(x => x.GroupMembers
                    .Any(y => y.ProfileId == profileId))
                .Select(x => new ProfileGroupsViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ImagePath = x.Image != null ? $"{x.Image.Id}.{x.Image.Extension}" : "",
                    GroupMembersCount = x.GroupMembers.Count()
                })
                .ToList();

        public bool IsMemberOfTheGroup(string groupId, string profileId)
            => groupMemberRepo
                .AllAsNoTracking()
                .Where(x => x.GroupId == groupId && x.ProfileId == profileId)
                .FirstOrDefault() != null;

        public bool IsOwnerOfTheGroup(string groupId, string profileId)
            => groupRepo
                .AllAsNoTracking()
                .Where(x => x.Id == groupId && x.ProfileId == profileId)
                .Any();

        public async Task LeaveAsync(string groupId, string profileId)
        {
            var groupMember = groupMemberRepo
                .All()
                .Where(x => x.ProfileId == profileId && x.GroupId == groupId)
                .FirstOrDefault();

            if (groupMember != null)
            {
                groupMemberRepo.Delete(groupMember);
                await groupMemberRepo.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(EditGroupViewModel input, string path)
        {
            var group = groupRepo
                .All()
                .Include(x => x.Image)
                .Where(x => x.Id == input.Id)
                .FirstOrDefault();

            if (group != null)
            {
                group.Name = input.Name;

                if (input.Image?.Length > 0)
                {
                    group.ImageId = await imgService.CreateAsync(input.Image, path);
                }

                groupRepo.Update(group);
                await groupRepo.SaveChangesAsync();
            }
        }
    }
}
