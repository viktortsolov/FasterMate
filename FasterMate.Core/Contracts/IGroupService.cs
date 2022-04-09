namespace FasterMate.Core.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FasterMate.ViewModels.Group;

    public interface IGroupService
    {
        bool IsMemberOfTheGroup(string groupId, string profileId);

        GroupViewModel GetById(string groupId);

        Task<string> CreateAsync(string profileId, CreateGroupViewModel input, string path);
    }
}