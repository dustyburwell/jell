using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using Jell.ChatClient;
using Jell.Shell.ViewModels;

namespace Jell.Chat.ViewModels
{
   public class LobbyViewModel : Caliburn.Micro.Screen
   {
      private readonly IChatClient m_client;
      private readonly ApplicationViewModel m_applicationViewModel;
      private readonly Dispatcher m_dispatcher;

      private bool m_searching;

      public LobbyViewModel(IChatClient client, ApplicationViewModel applicationViewModel)
      {
         m_client = client;
         m_applicationViewModel = applicationViewModel;
         m_dispatcher = Dispatcher.CurrentDispatcher;
         Rooms = new ObservableCollection<IChatRoom>();
      }

      public override string DisplayName
      {
         get { return "Lobby"; }
         set { }
      }
      
      public ObservableCollection<IChatRoom> Rooms { get; private set; }
      
      public bool Searching
      {
         get { return m_searching; }
         private set
         {
            m_searching = value;
            NotifyOfPropertyChange("Searching");
         }
      }

      public void JoinRoom(XmppChatRoom item)
      {
         m_applicationViewModel.ActivateItem(new ChatRoomViewModel(m_client, item));
      }

      protected override void OnActivate()
      {
         FindChatRooms();
      }

      private void FindChatRooms()
      {
         Searching = true;
         m_client.ListRooms("conference.softekinc.com", OnGetChatRooms);
      }

      private void OnGetChatRooms(IEnumerable<IChatRoom> rooms)
      {
         m_dispatcher.BeginInvoke((Action)(() => {
            Rooms.Clear();

            foreach (var room in rooms)
            {
               Rooms.Add(room);
            }

            Searching = false;
         }));
      }
   }
}