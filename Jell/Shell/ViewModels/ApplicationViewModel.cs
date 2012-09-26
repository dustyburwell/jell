using System;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using Jell.Chat.ViewModels;
using Jell.ChatClient;

namespace Jell.Shell.ViewModels
{
   public class ApplicationViewModel : Conductor<Screen>.Collection.OneActive
   {
      private readonly IChatClient m_client;

      public ApplicationViewModel(IChatClient client)
      {
         m_client = client;
         ChatRooms = new ObservableCollection<ChatRoomViewModel>();
      }

      public bool HasUnreadMessages
      {
         get { return ChatRooms.Any(room => room.HasUnreadMessages); }
      }

      public ObservableCollection<ChatRoomViewModel> ChatRooms { get; private set; }
      public LobbyViewModel Lobby { get; private set; }

      protected override void OnInitialize()
      {
         ActivateItem(Lobby = new LobbyViewModel(m_client, this));
      }

      public void OpenRoom(IChatRoom item)
      {
         ChatRoomViewModel roomViewModel;

         if ((roomViewModel = ChatRooms.FirstOrDefault(r => r.DisplayName == item.Name)) != null)
         {
            ActivateItem(roomViewModel);
            return;
         }

         roomViewModel = new ChatRoomViewModel(m_client, item);

         roomViewModel.HasUnreadMessagesChanged += RoomHasUnreadMessagesChanged;
         ChatRooms.Add(roomViewModel);

         ActivateItem(roomViewModel);
      }

      public void LeaveRoom(ChatRoomViewModel room)
      {
         room.TryClose();
         ChatRooms.Remove(room);
      }

      private void RoomHasUnreadMessagesChanged(object sender, EventArgs e)
      {
         NotifyOfPropertyChange("HasUnreadMessages");
      }
   }
}