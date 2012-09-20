using System;
using System.Collections.Generic;
using System.Linq;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.disco;
using agsXMPP.protocol.iq.register;
using agsXMPP.sasl;

namespace Jell.ChatClient
{
   public class XmppChatClient : IChatClient
   {
      private readonly XmppClientConnection m_client;

      public XmppChatClient(XmppClientConnection client)
      {
         m_client = client;
      }

      public event ObjectHandler OnLogin
      {
         add { m_client.OnLogin += value; }
         remove { m_client.OnLogin -= value; }
      }

      public string Username
      {
         get { return m_client.Username; }
      }

      public void ListRooms(string jid, Action<IEnumerable<IChatRoom>> callback)
      {
         DiscoItemsIq discoIq = new DiscoItemsIq(IqType.get);
         discoIq.To = new Jid(jid);
         m_client.IqGrabber.SendIq(discoIq, (sender, iq, data) => OnGetChatRooms(iq, callback), null);
      }

      private void OnGetChatRooms(IQ iq, Action<IEnumerable<XmppChatRoom>> callback)
      {
         callback(iq.FirstChild.ChildNodes
            .OfType<DiscoItem>()
            .Select(x => new XmppChatRoom(x.Name, x.Jid, m_client)));
      }

      public void Open(string username, string password)
      {
         m_client.Open(username, password);
      }

      public void Send(string xml)
      {
         m_client.Send(xml);
      }

      public void Open(string xml)
      {
         m_client.Open(xml);
      }

      public void Close()
      {
         m_client.Close();
      }

      public event XmppConnectionStateHandler OnConnectionStateChanged
      {
         add { m_client.OnXmppConnectionStateChanged += value; }
         remove { m_client.OnXmppConnectionStateChanged -= value; }
      }

      public event ErrorHandler OnError
      {
         add { m_client.OnError += value; }
         remove { m_client.OnError -= value; }
      }

      public event ObjectHandler OnBinded
      {
         add { m_client.OnBinded += value; }
         remove { m_client.OnBinded -= value; }
      }

      public event RegisterEventHandler OnRegisterInformation
      {
         add { m_client.OnRegisterInformation += value; }
         remove { m_client.OnRegisterInformation -= value; }
      }

      public event ObjectHandler OnRegistered
      {
         add { m_client.OnRegistered += value; }
         remove { m_client.OnRegistered -= value; }
      }

      public event ObjectHandler OnPasswordChanged
      {
         add { m_client.OnPasswordChanged += value; }
         remove { m_client.OnPasswordChanged -= value; }
      }

      public event XmppElementHandler OnRegisterError
      {
         add { m_client.OnRegisterError += value; }
         remove { m_client.OnRegisterError -= value; }
      }

      public event XmppElementHandler OnStreamError
      {
         add { m_client.OnStreamError += value; }
         remove { m_client.OnStreamError -= value; }
      }

      public event XmppElementHandler OnAuthError
      {
         add { m_client.OnAuthError += value; }
         remove { m_client.OnAuthError -= value; }
      }

      public event ErrorHandler OnSocketError
      {
         add { m_client.OnSocketError += value; }
         remove { m_client.OnSocketError -= value; }
      }

      public event ObjectHandler OnClose
      {
         add { m_client.OnClose += value; }
         remove { m_client.OnClose -= value; }
      }

      public event ObjectHandler OnRosterStart
      {
         add { m_client.OnRosterStart += value; }
         remove { m_client.OnRosterStart -= value; }
      }

      public event ObjectHandler OnRosterEnd
      {
         add { m_client.OnRosterEnd += value; }
         remove { m_client.OnRosterEnd -= value; }
      }

      public event XmppClientConnection.RosterHandler OnRosterItem
      {
         add { m_client.OnRosterItem += value; }
         remove { m_client.OnRosterItem -= value; }
      }

      public event ObjectHandler OnAgentStart
      {
         add { m_client.OnAgentStart += value; }
         remove { m_client.OnAgentStart -= value; }
      }

      public event ObjectHandler OnAgentEnd
      {
         add { m_client.OnAgentEnd += value; }
         remove { m_client.OnAgentEnd -= value; }
      }

      public event IqHandler OnIq
      {
         add { m_client.OnIq += value; }
         remove { m_client.OnIq -= value; }
      }

      public event XmppClientConnection.AgentHandler OnAgentItem
      {
         add { m_client.OnAgentItem += value; }
         remove { m_client.OnAgentItem -= value; }
      }

      public event MessageHandler OnMessage
      {
         add { m_client.OnMessage += value; }
         remove { m_client.OnMessage -= value; }
      }

      public event PresenceHandler OnPresence
      {
         add { m_client.OnPresence += value; }
         remove { m_client.OnPresence -= value; }
      }

      public event SaslEventHandler OnSaslStart
      {
         add { m_client.OnSaslStart += value; }
         remove { m_client.OnSaslStart -= value; }
      }

      public event ObjectHandler OnSaslEnd
      {
         add { m_client.OnSaslEnd += value; }
         remove { m_client.OnSaslEnd -= value; }
      }
   }

   public class DiscoItemsIq : agsXMPP.protocol.component.IQ
   {
      private readonly DiscoItems m_discoItems = new DiscoItems();

      public DiscoItemsIq()
      {
         base.Query = m_discoItems;
         GenerateId();
      }

      public DiscoItemsIq(IqType type)
         : this()
      {
         Type = type;
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