using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ng2Aa_demo.Hubs
{
    public class IncomingMessage
    {
        public string content;
        public string user;

        public IncomingMessage(string user, string message)
        {
            this.user = user;
            this.content = message;
        }
    }
}
