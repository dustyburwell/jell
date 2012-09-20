using Jell.Chat.ViewModels;

namespace Jell.ChatClient
{
   public interface IChatRoom
   {
      string Name { get; }
      string Jid { get; }

      void Join(string nick, IChatRoomListener listener);
      void Leave(string nick);

      void Send(string message);
   }
}