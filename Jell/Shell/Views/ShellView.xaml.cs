namespace Jell.Shell.Views
{
   public partial class ShellView : IShellView
   {
      public ShellView()
      {
         InitializeComponent();
      }

      public override void OnApplyTemplate()
      {
         base.OnApplyTemplate();

         Width = 600;
         Height = 300;
         ResizeMode = System.Windows.ResizeMode.NoResize;
      }

      public void Release()
      {
         Width = 800;
         Height = 600;
         ResizeMode = System.Windows.ResizeMode.CanResize;
      }
   }

   public interface IShellView
   {
      void Release();
   }
}
