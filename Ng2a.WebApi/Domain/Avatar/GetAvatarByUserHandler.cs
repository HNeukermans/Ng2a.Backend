using MediatR;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ng2Aa_demo.Domain.Avatar
{
    public class GetAvatarByUserHandler : IRequestHandler<GetAvatarByUser, Task<Tuple<string, Stream>>>
    {
        private InMemoryAvatarCache _cache;

        public GetAvatarByUserHandler(InMemoryAvatarCache cache) {
            _cache = cache;
        }

        public async Task<Tuple<string, Stream>> Handle(GetAvatarByUser request)
        {
            var avatars = await _cache.Load();

            return avatars.Find(i => i.Item1 == request.UserName);
        }        
    }
}
