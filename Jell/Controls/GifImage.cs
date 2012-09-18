using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Jell.Controls
{
   class GifImage : Image
   {
      public static readonly DependencyProperty FrameIndexProperty =
          DependencyProperty.Register("FrameIndex", typeof(int), typeof(GifImage), new UIPropertyMetadata(0, new PropertyChangedCallback(ChangingFrameIndex)));

      private GifBitmapDecoder m_gf;
      private Int32Animation m_anim;
      bool m_animationIsWorking = false;

      static void ChangingFrameIndex(DependencyObject obj, DependencyPropertyChangedEventArgs ev)
      {
         GifImage ob = obj as GifImage;
         ob.Source = ob.m_gf.Frames[(int)ev.NewValue];
         ob.InvalidateVisual();
      }

      public int FrameIndex
      {
         get { return (int)GetValue(FrameIndexProperty); }
         set { SetValue(FrameIndexProperty, value); }
      }

      protected override void OnInitialized(EventArgs e)
      {
         base.OnInitialized(e);

         var stream = Application.GetResourceStream(new Uri(Source.ToString()));
         m_gf = new GifBitmapDecoder(stream.Stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
         m_anim = new Int32Animation(0, m_gf.Frames.Count - 1, new Duration(new TimeSpan(0, 0, 0, m_gf.Frames.Count / 10, (int)((m_gf.Frames.Count / 10.0 - m_gf.Frames.Count / 10) * 1000))));
         m_anim.RepeatBehavior = RepeatBehavior.Forever;
         Source = m_gf.Frames[0];
      }

      protected override void OnRender(DrawingContext dc)
      {
         base.OnRender(dc);
         if (!m_animationIsWorking)
         {
            BeginAnimation(FrameIndexProperty, m_anim);
            m_animationIsWorking = true;
         }
      }
   }
}
