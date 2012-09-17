using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.disco;
using Jell.Shell.ViewModels;

namespace Jell.Chat.ViewModels
{
   public class LobbyViewModel : Caliburn.Micro.Screen
   {
      private readonly XmppClientConnection m_client;
      private readonly ApplicationViewModel m_applicationViewModel;
      private readonly Dispatcher m_dispatcher;

      public LobbyViewModel(XmppClientConnection client, ApplicationViewModel applicationViewModel)
      {
         m_client = client;
         m_applicationViewModel = applicationViewModel;
         m_dispatcher = Dispatcher.CurrentDispatcher;
         Rooms = new ObservableCollection<DiscoItem>();
      }

      public override string DisplayName
      {
         get { return "Lobby"; }
         set { }
      }
      
      public ObservableCollection<DiscoItem> Rooms { get; private set; }

      public void JoinRoom(DiscoItem item)
      {
         m_applicationViewModel.ActivateItem(new ChatRoomViewModel(m_client, item));
      }

      protected override void OnActivate()
      {
         FindChatRooms();
      }

      private void FindChatRooms()
      {
         DiscoItemsIq discoIq = new DiscoItemsIq(IqType.get);
         discoIq.To = new Jid("conference.softekinc.com");
         m_client.IqGrabber.SendIq(discoIq, OnGetChatRooms, null);
      }

      private void OnGetChatRooms(object sender, IQ iq, object data)
      {
         m_dispatcher.BeginInvoke((Action)(() => {
            Rooms.Clear();

            foreach (DiscoItem room in iq.FirstChild.ChildNodes.Cast<object>().Where(x => x is DiscoItem))
            {
               Rooms.Add(room);
            }
         }), DispatcherPriority.Normal);
      }
   }

   public class DiscoItemsIq : agsXMPP.protocol.component.IQ
   {
      private readonly DiscoItems m_discoItems = new DiscoItems();

      public DiscoItemsIq()
      {
         base.Query = m_discoItems;
         this.GenerateId();
      }

      public DiscoItemsIq(IqType type)
         : this()
      {
         this.Type = type;
      }

      public new DiscoItems Query
      {
         get
         {
            return m_discoItems;
         }
      }
   }
}