using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.disco;
using agsXMPP.protocol.x.muc;

namespace Jell.Chat.ViewModels
{
   public class ChatRoomViewModel : Caliburn.Micro.Screen
   {
      private readonly XmppClientConnection m_client;
      private readonly DiscoItem m_discoItem;
      private readonly Jid m_room;
      private readonly Dispatcher m_dispatcher;
      private readonly MucManager m_chat;

      private string m_message;

      public ChatRoomViewModel(XmppClientConnection client, DiscoItem discoItem)
      {
         m_dispatcher = Dispatcher.CurrentDispatcher;
         
         m_client = client;
         m_discoItem = discoItem;
         m_room = discoItem.Jid;
         m_chat = new MucManager(m_client);
         ChatLog = new ChatLogViewModel(m_client.Username);

         Members = new ObservableCollection<string>();
      }

      public override string DisplayName
      {
         get { return m_discoItem.Name; }
         set { }
      }

      public ChatLogViewModel ChatLog { get; private set; }

      public string Message
      {
         get { return m_message; }
         set
         {
            m_message = value;
            NotifyOfPropertyChange("Message");
         }
      }

      public ObservableCollection<string> Members { get; private set; }

      public void SendMessage()
      {
         m_client.Send(new Message(m_room, MessageType.groupchat, Message));
         Message = string.Empty;
      }

      public void Leave()
      {
         m_chat.LeaveRoom(m_room, m_client.Username);
         m_client.OnMessage -= OnMessage;
         TryClose();
      }

      protected override void OnInitialize()
      {
         m_chat.AcceptDefaultConfiguration(m_room);
         m_chat.JoinRoom(m_room, m_client.Username);
         m_client.OnMessage += OnMessage;
         m_chat.RequestList(Role.participant, m_room, (a, b, c) => m_dispatcher.BeginInvoke((Action)(() => {
            Members.Clear();

            foreach (Item member in b.FirstChild.ChildNodes.Cast<object>().Where(x => x is Item))
            {
               Members.Add(member.Nickname);
            }
         })), null);
      }

      private void OnMessage(object sender, Message msg)
      {
         if (msg.From.Bare != m_room.Bare)
            return;

         m_dispatcher.BeginInvoke((Action) (() => ChatLog.AddMessage(msg)));
      }
   }
}