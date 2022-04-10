namespace FasterMate.Core.Services
{
    using System.Threading.Tasks;

    using FasterMate.Core.Contracts;
    using FasterMate.Infrastructure.Common;
    using FasterMate.Infrastructure.Data;
    using FasterMate.ViewModels.Message;
    using Microsoft.EntityFrameworkCore;

    public class MessageService : IMessageService
    {
        private readonly IRepository<Message> messageRepo;

        public MessageService(IRepository<Message> _messageRepo)
        {
            messageRepo = _messageRepo;
        }

        public async Task<string> AddMessageAsync(string groupId, string profileId, string text)
        {
            var message = new Message
            {
                Text = text,
                GroupId = groupId,
                ProfileId = profileId,
            };

            await messageRepo.AddAsync(message);
            await messageRepo.SaveChangesAsync();

            return message.Id;
        }

        public MessageViewModel GetMessageById(string id)
            => messageRepo
                .AllAsNoTracking()
                .Include(x => x.Profile)
                .Where(x => x.Id == id)
                .Select(x => new MessageViewModel()
                {
                    Id = x.Id,
                    Text = x.Text,
                    ProfileId = x.ProfileId,
                    ProfileName = $"{x.Profile.FirstName} {x.Profile.LastName}",
                    CreatedOn = x.CreateOn.ToString("dd/MM/yyyy HH:mm")
                })
                .FirstOrDefault();

        public bool IsOwnerOfTheMessage(string messageId, string profileId)
            => messageRepo
                .AllAsNoTracking()
                .Any(x => x.Id == messageId && x.ProfileId == profileId);
    }
}
