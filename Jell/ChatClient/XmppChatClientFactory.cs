using agsXMPP;

namespace Jell.ChatClient
{
   public class XmppChatClientFactory : IChatClientFactory
   {
      public IChatClient Connect(string server)
      {
         return new XmppChatClient(new XmppClientConnection(server));
      }
   }
}