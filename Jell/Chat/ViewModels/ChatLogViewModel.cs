using Jell.Chat.Views;
using Jell.ChatClient;

namespace Jell.Chat.ViewModels
{
   public class ChatLogViewModel : Caliburn.Micro.Screen
   {
      private readonly string m_nick;

      public ChatLogViewModel(string nick)
      {
         m_nick = nick;
      }

      public void AddMessage(ChatMessage message)
      {
         ((IChatLogView)GetView()).AddRow(CreateRowForMessage(message));
      }

      private MessageRow CreateRowForMessage(ChatMessage message)
      {
         return new MessageRow {
            From = message.From,
            Body = message.Body ?? string.Empty,
            Class = message.From == m_nick ? "self" : string.Empty
         };
      }
   }

   public class MessageRow
   {
      public string From { get; set; }
      public string Body { get; set; }
      public string Class { get; set; }
   }
}