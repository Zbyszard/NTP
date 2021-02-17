using System.Threading.Tasks;

namespace Kopyw.Core.Notifications
{
    public interface IPostNotifier
    {
        Task SendUpdate(long postId);
    }
}
