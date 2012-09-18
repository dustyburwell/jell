using System;
using agsXMPP;
using agsXMPP.Xml.Dom;
using Caliburn.Micro;
using Jell.Shell.Views;

namespace Jell.Shell.ViewModels
{
   public class LoginViewModel : Screen
   {
      private readonly ILoginListener m_loginListener;

      private XmppClientConnection m_client;
      private string m_status;
      private bool m_isLoading;

      public LoginViewModel(ILoginListener loginListener)
      {
         m_loginListener = loginListener;
      }

      public string Jid { get; set; }

      public string Status
      {
         get { return m_status; }
         set
         {
            m_status = value;
            NotifyOfPropertyChange("Status");
         }
      }

      public bool IsLoading
      {
         get { return m_isLoading; }
         set
         {
            m_isLoading = value;
            NotifyOfPropertyChange("IsLoading");
            NotifyOfPropertyChange("IsFormEnabled");
         }
      }

      public bool IsFormEnabled
      {
         get { return !m_isLoading; }
      }

      public void Login()
      {
         if (string.IsNullOrEmpty(Jid))
            return;

         var parts = Jid.Split('@');

         if (parts.Length != 2)
            return;

         var user = parts[0];
         var server = parts[1];
         var password = ((LoginView) GetView()).Password.Password;

         IsLoading = true;

         m_client = new XmppClientConnection(server);
         m_client.OnLogin += OnLogin;
         m_client.OnError += OnError;
         m_client.OnAuthError += OnAuthError;
         m_client.OnSocketError += OnSocketError;
         m_client.OnXmppConnectionStateChanged += OnStatusChange;
         m_client.Open(user, password);
      }

      private void OnStatusChange(object sender, XmppConnectionState state)
      {
         Status = state.ToString();
      }

      private void OnAuthError(object sender, Element e)
      {
         m_client.Close();
         
         IsLoading = false;
      }

      private void OnError(object sender, Exception ex)
      {
         m_client.Close();
         
         IsLoading = false;
      }

      private void OnSocketError(object sender, Exception ex)
      {
         m_client.Close();
         
         IsLoading = false;
      }

      private void OnLogin(object sender)
      {
         Status = "Awesome!";

         m_loginListener.LoginSuccess(m_client);
      }
   }
}