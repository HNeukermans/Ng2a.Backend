using MediatR;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ng2Aa_demo.Domain.Avatar
{
    public class GetAvatarByUserHandler : IRequestHandler<GetAvatarByUser, Tuple<string, Stream>>
    {
        private ActiveDirectoryClientProvider _adClientProvider;

        public GetAvatarByUserHandler(ActiveDirectoryClientProvider adClientProvider) {
            _adClientProvider = adClientProvider;
        }

        public Tuple<string, Stream> Handle(GetAvatarByUser request)
        {
            var client = _adClientProvider.Get();
            return null;

        }
    }
}
