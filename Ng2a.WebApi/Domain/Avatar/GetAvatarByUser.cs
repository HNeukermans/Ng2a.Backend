using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ng2Aa_demo.Domain.Avatar
{
    public class GetAvatarByUser : IAsyncRequest<Tuple<string, Stream>> 
    {
        public GetAvatarByUser(string user) {
            UserName = user;
        }

        public string UserName { get; set; }

    }
}
