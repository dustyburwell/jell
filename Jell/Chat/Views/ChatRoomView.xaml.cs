using System.Windows.Input;
using Jell.Chat.ViewModels;

namespace Jell.Chat.Views
{
   public partial class ChatRoomView
   {
      public override void OnApplyTemplate()
      {
         base.OnApplyTemplate();

         Message.KeyUp += MessageKeyUp;
      }

      private void MessageKeyUp(object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Enter)
         {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
               Message.SelectedText = "\r\n";
               Message.SelectionStart = Message.SelectionStart + Message.SelectionLength;
               Message.SelectionLength = 0;
            }
            else
            {
               ((ChatRoomViewModel)DataContext).SendMessage();
            }
         }
      }
   }
}
