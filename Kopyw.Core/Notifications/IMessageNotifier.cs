using Kopyw.Core.DTO;
using System.Threading.Tasks;

namespace Kopyw.Core.Notifications
{
    public interface IMessageNotifier
    {
        Task SendMessage(MessageDTO message);
    }
}
