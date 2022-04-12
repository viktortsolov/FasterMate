﻿namespace FasterMate.Core.Contracts
{
    using System.Threading.Tasks;

    using FasterMate.ViewModels.Group;

    public interface IGroupService
    {
        bool IsMemberOfTheGroup(string groupId, string profileId);

        bool IsOwnerOfTheGroup(string groupId, string profileId);

        GroupViewModel GetGroupById(string groupId);

        Task<string> CreateAsync(string profileId, CreateGroupViewModel input, string path);

        IEnumerable<ProfileGroupsViewModel> GetProfileGroups(string profileId);

        IEnumerable<GroupMemberInfoViewModel> GetMembers(string groupId, string profileId);

        EditGroupViewModel GetGroupForEdit(string groupId);

        Task UpdateAsync(EditGroupViewModel input, string path);

        Task DeleteGroupAsync(string groupId);

        Task RemoveAsync(string groupId, string profileId);

        IEnumerable<GroupFollowersViewModel> GetFollowersOfProfile(string profileId, string groupId);

        Task AddMemberAsync(string profileId, string groupId);
    }
}