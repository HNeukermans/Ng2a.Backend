using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Microsoft.Net.Http.Headers;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;
using Ng2Aa_demo.Domain.Avatar;
using MediatR;

namespace Ng2Aa_demo.Controllers
{
    [Route("api/[controller]")]
    public class AvatarsController : Controller
    {
        //private GetAvatarByUserHandler _handler;
        private readonly IMediator _mediator;

        //public AvatarsController(IMediator mediator) {
        //    _mediator = mediator;
        //}

        public AvatarsController(Mediator mediator)
        {
             _mediator = mediator;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> Get()
        {
            var userAvatar = await _mediator.SendAsync(new GetAvatarByUser("jef"));

            if (userAvatar == null) return new HttpResponseMessage(HttpStatusCode.NotFound);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(userAvatar.Item2);
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            return result;
        }
    }
}
