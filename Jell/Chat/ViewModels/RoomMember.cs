namespace Jell.Chat.ViewModels
{
   public class RoomMember
   {
      public RoomMember(string name, bool isAway)
      {
         Name = name;
         IsAway = isAway;
      }

      public string Name { get; private set; }
      public bool IsAway { get; set; }
   }
}