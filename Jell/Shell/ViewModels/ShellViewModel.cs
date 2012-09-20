using System;
using System.Windows.Threading;
using Jell.ChatClient;
using Jell.Shell.Views;

namespace Jell.Shell.ViewModels
{
   public class ShellViewModel : Caliburn.Micro.Conductor<Caliburn.Micro.Screen>, ILoginListener
   {
      private readonly LoginViewModel m_loginViewModel;
      private readonly Dispatcher m_dispatcher;

      public ShellViewModel()
      {
         m_dispatcher = Dispatcher.CurrentDispatcher;
         m_loginViewModel = new LoginViewModel(this, new XmppChatClientFactory());
      }

      public override string DisplayName
      {
         get { return "Jell Chat"; }
         set { }
      }

      protected override void OnInitialize()
      {
         ActivateItem(m_loginViewModel);
      }

      public void LoginSuccess(IChatClient client)
      {
         m_dispatcher.BeginInvoke((Action)(() => {
            ActivateItem(new ApplicationViewModel(client));
            ((IShellView)GetView()).Release();
         }));
      }
   }
}
