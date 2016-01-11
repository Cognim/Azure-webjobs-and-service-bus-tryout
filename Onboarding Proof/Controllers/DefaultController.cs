using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Onboarding_Proof.Controllers
{
    public class DefaultController : ApiController
    {
		[Route("")]
		[HttpGet]
		public async Task<IHttpActionResult> Get()
		{
			var result = new
			{
				Text = "New Release"
			};

			return Ok(result);
		}
	}
}
