using Microsoft.AspNetCore.Mvc;
using Utility.Security;

namespace Coindesk.Controllers
{
    /// <summary>用來產生Token</summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class TokenController : ControllerBase
    {
        /// <summary>產生Token，限期一小時</summary>
        /// <param name="identification">識別項</param>
        [HttpPost]
        public string GenerateToken(string identification)
            => new JwtAuth(SecurityDefine.AuthKey).GenerateToken(identification, 3600);
    }
}
