using Caliburn.Micro;
using Jell.Chat.ViewModels;
using Jell.ChatClient;

namespace Jell.Shell.ViewModels
{
   public class ApplicationViewModel : Conductor<Screen>.Collection.OneActive
   {
      private readonly IChatClient m_client;

      public ApplicationViewModel(IChatClient client)
      {
         m_client = client;
      }

      protected override void OnInitialize()
      {
         ActivateItem(new LobbyViewModel(m_client, this));
      }
   }
}