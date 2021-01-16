using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.SignalR;
using Models;
using System;
using Models.interfaces;

namespace chat_server
{
    public class ChatHub : Hub<IChatHub>
    {
        private IGenericService<MessageVM> _genericService;
        public ChatHub(IGenericService<MessageVM> genericService)
        {
            _genericService = genericService;
        }
        public async Task BroadcastAsync(ChatMessage chat)
        {
			try
			{
                if (chat.ChatId == 0)
                {
                    MessageVM msg = new MessageVM();
                    msg.Message = chat.Text;
                    msg.ReceiverId = chat.ReceiverId;
                    msg.SenderId = chat.SenderId;
                    Result result = await _genericService.Entry(msg);
                    if (result.IsSuccess)
                    {
                        chat.ChatId = Convert.ToInt64(((string[])result.Data)[0]);
                        chat.Time = ((string[])result.Data)[1];
                        await Clients.All.MessageReceivedFromHub(chat);
                    }
                }
				else
				{
                    chat.IsChnaged = true;
                    await Clients.All.MessageReceivedFromHub(chat);
                }
            }
			catch (Exception ex)
			{

			}
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.NewUserConnected("a new user connectd");
        }
	}

   
}
