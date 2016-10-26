using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Ng2Aa_demo.Hubs.BasicChat;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Ng2Aa_demo.Hubs
{
    [HubName("ChatAppHub")]
    public class ChatAppHub : Hub
    {
        private readonly static ConnectionsPerUser _connectionsPerUser =
           new ConnectionsPerUser();

        public override Task OnConnected() {

            var user = Context.QueryString["user"];
            
            Debug.WriteLine("OnConnected. User: {0}", user);
            var hasUser = _connectionsPerUser.HasUser(user);
            _connectionsPerUser.Add(user, Context.ConnectionId);

            if (!hasUser) {
                Clients.Others.onNewUserSessionReceived(user);
            }

            return base.OnConnected();
        }

        public void SendMessage(string content)
        {
            var user = GetAuthenticatedUser();
            Clients.Others.onMessageReceived(new IncomingMessage(user, content));
        }

        public List<string> GetOtherUsers()
        {
            var myself = GetAuthenticatedUser();
            var allOthers = _connectionsPerUser.GetUsers();
            allOthers.Remove(myself);
            return allOthers;
        }

        public void SendBeginNewMessage()
        {
            var username = GetAuthenticatedUser();
            Clients.Others.onBeginNewMessageReceived(username);
        }
       
        public override Task OnReconnected()
        {
            var name = Context.QueryString["user"];
            Debug.WriteLine("OnConnected. Name: {0}", name);
            if (!_connectionsPerUser.GetConnections(name).Contains(Context.ConnectionId))
            {
                _connectionsPerUser.Add(name, Context.ConnectionId);
            }

            return base.OnReconnected();
        }

        private string GetAuthenticatedUser() {
            var username = Context.QueryString["user"];
            if (string.IsNullOrWhiteSpace(username))
                throw new System.Exception("Failed to authenticate user.");
            return username;
        }

       
    }
}
