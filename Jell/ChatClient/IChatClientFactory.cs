namespace Jell.ChatClient
{
   public interface IChatClientFactory
   {
      IChatClient Connect(string server);
   }
}