namespace Jell.ChatClient
{
   public class PresenceMessage
   {
      public PresenceMessage(string nickname, string @from, bool isRemoved, bool isAway)
      {
         Nickname = nickname;
         From = from;
         IsRemove = isRemoved;
         IsAway = isAway;
      }

      public string Nickname { get; private set; }
      public string From { get; private set; }
      public bool IsRemove { get; private set; }
      public bool IsAway { get; private set; }
   }
}