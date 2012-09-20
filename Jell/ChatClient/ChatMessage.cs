namespace Jell.ChatClient
{
   public class ChatMessage
   {
      public ChatMessage(string from, string body)
      {
         From = from;
         Body = body;
      }

      public string From { get; private set; }
      public string Body { get; private set; }
   }
}