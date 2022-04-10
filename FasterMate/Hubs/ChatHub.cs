namespace FasterMate.Hubs
{
    using FasterMate.Core.Contracts;

    using Ganss.XSS;
    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub
    {
        private readonly IProfileService profilesService;
        private readonly IGroupService groupService;
        private readonly IMessageService messagesService;

        public ChatHub(
            IProfileService _profilesService,
            IGroupService _groupService,
            IMessageService _messagesService)
        {
            profilesService = _profilesService;
            groupService = _groupService;
            messagesService = _messagesService;
        }

        public async Task ConnectToChat(string groupId)
        {
            var connection = Context.ConnectionId;
            await Groups.AddToGroupAsync(connection, groupId);
        }

        public async Task Send(string groupId, string message)
        {
            var userId = Context.UserIdentifier;
            var profileId = profilesService.GetId(userId);
            var isMember = groupService.IsMemberOfTheGroup(groupId, profileId);

            if (isMember)
            {
                var sanitizer = new HtmlSanitizer();
                message = sanitizer.Sanitize(message);

                var messageId = await messagesService.AddMessageAsync(groupId, profileId, message);
                var messageModel = messagesService.GetMessageById(messageId);
                await Clients.Group(groupId.ToString()).SendAsync("RecieveMessage", messageModel);
            }
        }
    }
}
