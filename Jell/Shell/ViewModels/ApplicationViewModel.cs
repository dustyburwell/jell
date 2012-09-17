using agsXMPP;
using Caliburn.Micro;
using Jell.Chat.ViewModels;

namespace Jell.Shell.ViewModels
{
   public class ApplicationViewModel : Conductor<Screen>.Collection.OneActive
   {
      private readonly XmppClientConnection m_client;

      public ApplicationViewModel(XmppClientConnection client)
      {
         m_client = client;
      }

      protected override void OnInitialize()
      {
         ActivateItem(new LobbyViewModel(m_client, this));
      }
   }
}