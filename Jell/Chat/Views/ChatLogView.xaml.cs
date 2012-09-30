using System;
using System.Diagnostics;
using System.IO;
using System.Net;
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
         text = Regex.Replace(text, @"(http\://|https\://|www.)\S*", match => {
            if (match.Index != 0)
            {
               var previousChar = text[match.Index - 1];

               if (!char.IsWhiteSpace(previousChar))
                  return match.Value;
            }

            var value = match.Value;
            var path = new UriBuilder(value).Uri.AbsolutePath;

            if (match.Groups[1].Value == "www.")
               value = "http://" + value;

            return path.EndsWith("jpg") || path.EndsWith("gif") || path.EndsWith("png")
                  ? "![Inline Image](" + value + ")"
                  : "[" + value + "](" + value + ")";
         });
         text = new MarkdownDeep.Markdown {
            ExtraMode = true,
            NewWindowForExternalLinks = true,
            NewWindowForLocalLinks = true,
         }.Transform(text);
         text = text.Replace("\n", "");
         text = text.Replace(@"\", @"\\");

         onSuccess(text);
      }

      private void Browser_ShowContextMenu(object sender, ContextMenuEventArgs e)
      {
         e.Handled = true;
      }

      private void Browser_OpenExternalLink(object sender, OpenExternalLinkEventArgs e)
      {
         if (string.IsNullOrWhiteSpace(e.Url))
            return;

         Process.Start(e.Url);
      }

      private string AddRowScript = @"
var table = $('#chatLog');
var lasthead = table.find('th:last:parent');

if (lasthead.html() == ""{0}"")
{{
   lasthead.attr('rowspan', parseInt(lasthead.attr('rowspan') || 1) + 1);
   table.append('<tr class=\'{2}\'><td>{1}</td></tr>');
}}
else
{{
   table.append('<tr class=\'{2}\'><th>{0}</th><td>{1}</td></tr>');
}}
";
   }

   public interface IChatLogView
   {
      void AddRow(MessageRow createRowForMessage);
   }
}
