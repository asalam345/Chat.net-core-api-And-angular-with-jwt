using System.Threading.Tasks;

namespace chat_server.Entity.interfaces
{
    public interface IChatHub
    {
        Task MessageReceivedFromHub(ChatMessage message);
        Task NewUserConnected(string message);
    }
}
