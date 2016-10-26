using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ng2Aa_demo.Hubs
{
    using System.Collections.Generic;
    using System.Linq;

    namespace BasicChat
    {
        public class ConnectionsPerUser
        {
            private readonly Dictionary<string, HashSet<string>> _connections =
                new Dictionary<string, HashSet<string>>();

            public int Count
            {
                get
                {
                    return _connections.Count;
                }
            }

            public void Add(string key, string connectionId)
            {
                lock (_connections)
                {
                    HashSet<string> connections;
                    if (!_connections.TryGetValue(key, out connections))
                    {
                        connections = new HashSet<string>();
                        _connections.Add(key, connections);
                    }

                    lock (connections)
                    {
                        connections.Add(connectionId);
                    }
                }
            }

            public IEnumerable<string> GetConnections(string user)
            {
                HashSet<string> connections;
                if (_connections.TryGetValue(user, out connections))
                {
                    return connections;
                }

                return Enumerable.Empty<string>();
            }

            public void Remove(string key, string connectionId)
            {
                lock (_connections)
                {
                    HashSet<string> connections;
                    if (!_connections.TryGetValue(key, out connections))
                    {
                        return;
                    }

                    lock (connections)
                    {
                        connections.Remove(connectionId);

                        if (connections.Count == 0)
                        {
                            _connections.Remove(key);
                        }
                    }
                }
            }

            public List<string> GetUsers() {
                var names = new List<string>();
                foreach (KeyValuePair<string, HashSet<string>> entry in _connections)
                {
                    names.Add(entry.Key);
                }
                return names;
            }

            public bool HasUser(string user)
            {
                var users = GetUsers();
                return users.Find((u) => u == user) != null;
            }
        }
    }
}
