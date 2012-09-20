namespace Jell.ChatClient
{
   public class Participant
   {
      public Participant(string nickname)
      {
         Nickname = nickname;
      }

      public string Nickname { get; private set; }
   }
}