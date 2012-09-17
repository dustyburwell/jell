using agsXMPP;

namespace Jell.Shell.ViewModels
{
   public interface ILoginListener
   {
      void LoginSuccess(XmppClientConnection client);
   }
}