using Models;
using System.Threading.Tasks;

namespace Models.interfaces
{
    public interface IChatHub
    {
        Task MessageReceivedFromHub(ChatMessage message);
        Task NewUserConnected(string message);
    }
}
