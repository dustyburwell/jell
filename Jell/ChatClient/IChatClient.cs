using System;
using System.Collections.Generic;
using agsXMPP;

namespace Jell.ChatClient
{
   public interface IChatClient
   {
      event ObjectHandler OnLogin;
      event ErrorHandler OnError;
      event XmppElementHandler OnAuthError;
      event ErrorHandler OnSocketError;
      event XmppConnectionStateHandler OnConnectionStateChanged;
      
      string Username { get; }

      void ListRooms(string jid, Action<IEnumerable<IChatRoom>> callback);

      void Open(string user, string password);
      void Close();
   }
}