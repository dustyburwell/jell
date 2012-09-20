using agsXMPP;
using Jell.ChatClient;

namespace Jell.Shell.ViewModels
{
   public interface ILoginListener
   {
      void LoginSuccess(IChatClient client);
   }
}