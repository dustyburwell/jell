using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.x.muc;

namespace Jell.ChatClient
{
   public class XmppChatRoom : IChatRoom
   {
      private readonly XmppClientConnection m_client;
      private readonly MucManager m_chat;

      private IChatRoomListener m_listener;

      public XmppChatRoom(string name, string jid, XmppClientConnection client)
      {
         // todo: There's a memory leak here. Need to design around it.
         m_client = client;
         m_client.OnMessage += OnMessage;
         m_client.OnPresence += OnPresence;

         m_chat = new MucManager(m_client);
         
         Name = name;
         Jid = jid;
      }

      public string Name { get; private set; }
      public string Jid { get; private set; }

      public void Join(string nick, IChatRoomListener listener)
      {
         m_listener = listener;

         m_chat.AcceptDefaultConfiguration(Jid);
         m_chat.JoinRoom(Jid, nick);
      }

      public void Leave(string nick)
      {
         m_listener = null;

         m_chat.LeaveRoom(Jid, nick);
      }

      private void OnMessage(object sender, Message msg)
      {
         if (m_listener == null)
            return;

         if (msg.From.Bare != Jid)
            return;

         m_listener.OnMessage(new ChatMessage(msg.From.Resource, msg.Body));
      }

      private void OnPresence(object sender, Presence pres)
      {
         if (m_listener == null)
            return;

         if (pres.From.Bare != Jid)
            return;

         m_listener.OnPresence(
            new PresenceMessage(
               pres.From.Resource,
               pres.From,
               pres.Type == PresenceType.unavailable,
               pres.Show == ShowType.away));
      }

      public void Send(string message)
      {
         m_client.Send(new Message(Jid, MessageType.groupchat, message));
      }
   }
}