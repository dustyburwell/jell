namespace Jell.ChatClient
{
   public interface IChatRoomListener
   {
      void OnMessage(ChatMessage message);
      void OnPresence(PresenceMessage presenceMessage);
   }
}