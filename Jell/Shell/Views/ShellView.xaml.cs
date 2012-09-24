using System.Windows;
using System.Windows.Shell;

namespace Jell.Shell.Views
{
   public partial class ShellView : IShellView
   {
      public static readonly DependencyProperty HasUnreadMessagesProperty = DependencyProperty.Register(
         "HasUnreadMessages",
         typeof(bool), 
         typeof(ShellView), 
         new UIPropertyMetadata(false, HasUnreadMessagesChanged));

      private static void HasUnreadMessagesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var shell = d as ShellView;

         if (shell == null)
            return;

         shell.TaskbarItemInfo.ProgressState = shell.HasUnreadMessages
            ? TaskbarItemProgressState.Paused
            : TaskbarItemProgressState.None;

      }

      public ShellView()
      {
         InitializeComponent();
      }

      public override void OnApplyTemplate()
      {
         base.OnApplyTemplate();

         Width = 600;
         Height = 300;
         ResizeMode = ResizeMode.NoResize;
      }

      public void Release()
      {
         Width = 800;
         Height = 600;
         ResizeMode = ResizeMode.CanResize;
      }

      public bool HasUnreadMessages
      {
         get { return (bool)GetValue(HasUnreadMessagesProperty); }
         set { SetValue(HasUnreadMessagesProperty, value); }
      }
   }

   public interface IShellView
   {
      void Release();
   }
}
