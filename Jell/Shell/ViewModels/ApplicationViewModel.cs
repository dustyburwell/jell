using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Jell.Chat.ViewModels;
using Jell.ChatClient;

namespace Jell.Shell.ViewModels
{
   public class ApplicationViewModel : Conductor<Screen>.Collection.OneActive
   {
      private readonly IChatClient m_client;
      private readonly IList<ChatRoomViewModel> m_chatRooms;

      public ApplicationViewModel(IChatClient client)
      {
         m_client = client;
         m_chatRooms = new List<ChatRoomViewModel>();
      }

      public bool HasUnreadMessages
      {
         get { return m_chatRooms.Any(room => room.HasUnreadMessages); }
      }

      protected override void OnInitialize()
      {
         ActivateItem(new LobbyViewModel(m_client, this));
      }

      public void OpenRoom(IChatRoom item)
      {
         ChatRoomViewModel roomViewModel;

         if ((roomViewModel = m_chatRooms.FirstOrDefault(r => r.DisplayName == item.Name)) != null)
         {
            ActivateItem(roomViewModel);
            return;
         }

         roomViewModel = new ChatRoomViewModel(m_client, item);

         roomViewModel.HasUnreadMessagesChanged += RoomHasUnreadMessagesChanged;
         m_chatRooms.Add(roomViewModel);

         ActivateItem(roomViewModel);
      }

      private void RoomHasUnreadMessagesChanged(object sender, EventArgs e)
      {
         NotifyOfPropertyChange("HasUnreadMessages");
      }
   }
}