using Microsoft.AspNetCore.Mvc;
using Utility.Security;

namespace Coindesk.Controllers
{
    /// <summary>�ΨӲ���Token</summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class TokenController : ControllerBase
    {
        /// <summary>����Token�A�����@�p��</summary>
        /// <param name="identification">�ѧO��</param>
        [HttpPost]
        public string GenerateToken(string identification)
            => new JwtAuth(SecurityDefine.AuthKey).GenerateToken(identification, 3600);
    }
}
