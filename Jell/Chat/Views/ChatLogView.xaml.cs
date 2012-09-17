using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Awesomium.Core;
using Jell.Chat.ViewModels;

namespace Jell.Chat.Views
{
   public partial class ChatLogView : IChatLogView
   {
      public override void OnApplyTemplate()
      {
         base.OnApplyTemplate();

         var assembly = Assembly.GetExecutingAssembly();
         var stream = assembly.GetManifestResourceStream("Jell.Chat.Views.ChatLog.html");
         var reader = new StreamReader(stream);

         Browser.LoadHTML(reader.ReadToEnd());
         Browser.ShowContextMenu += Browser_ShowContextMenu;
         Browser.OpenExternalLink += Browser_OpenExternalLink;
         Browser.ContextMenu = null;
      }

      public void AddRow(MessageRow row)
      {
         Linkify(row.Body, newBody => 
            Dispatcher.BeginInvoke((Action)(() =>
            {
               Browser.ExecuteJavascript(string.Format(AddRowScript, row.From, newBody, row.Class));
               Browser.ExecuteJavascript("window.scrollTo(0, document.body.scrollHeight)");
            }))
         );
      }

      public void Linkify(string text, Action<string> onSuccess)
      {
         text = text.Replace("<", "&lt;");
         text = text.Replace(">", "&gt;");
         text = text.Replace("'", "&apos;");
         text = Regex.Replace(text, @"(http|https|ftp)\://[a-zA-Z0-9\-\.]+" +
                             @"\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?" +
                             @"([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*", match => {
            
            var path = new Uri(match.Value).AbsolutePath;

            return path.EndsWith("jpg") || path.EndsWith("gif") || path.EndsWith("png")
                  ? "<img src=\\'" + match.Value + "\\'></img>"
                  : "<a target=\\'blank\\' href=\\'" + match.Value + "\\'>" + match.Value + "</a>";
         });

         onSuccess(text);
      }

      private void Browser_ShowContextMenu(object sender, ContextMenuEventArgs e)
      {
         e.Handled = true;
      }

      private void Browser_OpenExternalLink(object sender, OpenExternalLinkEventArgs e)
      {
         Process.Start(e.Url);
      }

      private string AddRowScript = @"
document.getElementById('chatLog').innerHTML += '<tr class=\'{2}\'><th>{0}</th><td>{1}</td></tr>';
";
   }

   public interface IChatLogView
   {
      void AddRow(MessageRow createRowForMessage);
   }
}
