using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using Jell.ChatClient;

namespace Jell.Chat.ViewModels
{
   public class ChatRoomViewModel : Caliburn.Micro.Screen, IChatRoomListener
   {
      public event EventHandler HasUnreadMessagesChanged = delegate { }; 

      private readonly IChatClient m_client;
      private readonly IChatRoom m_chatRoom;
      private readonly Dispatcher m_dispatcher;

      private string m_message;
      private bool m_hasUnreadMessages;

      public ChatRoomViewModel(IChatClient client, IChatRoom chatRoom)
      {
         m_dispatcher = Dispatcher.CurrentDispatcher;
         
         m_client = client;
         m_chatRoom = chatRoom;
         ChatLog = new ChatLogViewModel(m_client.Username);

         Members = new ObservableCollection<RoomMember>();
      }

      public override string DisplayName
      {
         get { return m_chatRoom.Name; }
         set { }
      }

      public ChatLogViewModel ChatLog { get; private set; }
      public ObservableCollection<RoomMember> Members { get; private set; }
      
      public bool HasUnreadMessages
      {
         get { return m_hasUnreadMessages; }
         private set
         {
            m_hasUnreadMessages = value;
            NotifyOfPropertyChange("HasUnreadMessages");
            HasUnreadMessagesChanged(this, EventArgs.Empty);
         }
      }

      public string Message
      {
         get { return m_message; }
         set
         {
            m_message = value;
            NotifyOfPropertyChange("Message");
         }
      }

      public void SendMessage()
      {
         m_chatRoom.Send(Message);
         Message = string.Empty;
      }

      public void Leave()
      {
         m_chatRoom.Leave(m_client.Username);
         TryClose();
      }

      protected override void OnInitialize()
      {
         m_chatRoom.Join(m_client.Username, this);
      }

      protected override void OnActivate()
      {
         HasUnreadMessages = false;
      }

      public void OnMessage(ChatMessage message)
      {
         m_dispatcher.BeginInvoke((Action) (() => ChatLog.AddMessage(message)));

         if (!IsActive)
            HasUnreadMessages = true;
      }

      public void OnPresence(PresenceMessage message)
      {
         m_dispatcher.BeginInvoke((Action) (() => {
            var member = Members.FirstOrDefault(rm => rm.Name == message.Nickname);

            if(member != null)
            {
               if (message.IsRemove)
               {
                  Members.Remove(member);
               }
               else
               {
                  member.IsAway = message.IsAway;
               }
            }
            else
            {
               Members.Add(new RoomMember(message.Nickname, message.IsAway));
            }
         }));
      }
   }
}