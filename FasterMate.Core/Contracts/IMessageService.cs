namespace FasterMate.Core.Contracts
{
    using FasterMate.ViewModels.Message;

    public interface IMessageService
    {
        Task<string> AddMessageAsync(string groupId, string profileId, string text);

        MessageViewModel GetMessageById(string id);
    }
}
