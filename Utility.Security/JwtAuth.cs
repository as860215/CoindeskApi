using Jose;
using System.Text;

namespace Utility.Security
{
    /// <summary>JWT授權機制</summary>
    public class JwtAuth
    {
        /// <summary>到期時間</summary>
        private const string expirationTimeTag = "Exp";
        /// <summary>識別項</summary>
        private const string identificationTag = "Identification";

        /// <summary>產生Token</summary>
        /// <param name="key">鍵值</param>
        /// <param name="identification">識別項</param>
        /// <param name="timeout">到期時間（NULL代表無限期）</param>
        public string GenerateToken(string key, string identification, int? timeout = null)
        {
            ArgumentNullException.ThrowIfNull(key);
            ArgumentNullException.ThrowIfNull(identification);

            var payload = new Dictionary<string, string>
            {
                { identificationTag, identification }
            };
            if(timeout != null)
                payload.Add(expirationTimeTag, DateTimeOffset.Now.AddSeconds(timeout.Value).ToString());

            var token = JWT.Encode(payload, Encoding.UTF8.GetBytes(key), JwsAlgorithm.HS512);
            return token;
        }

        /// <summary>驗證Token</summary>
        /// <param name="key">鍵值</param>
        /// <param name="token">Token</param>
        /// <returns>是否驗證成功</returns>
        public bool AuthorizationToken(string key, string token)
        {
            if (key == null || token == null) return false;

            var jwtObject = new Dictionary<string, string>();

            try
            {
                jwtObject = JWT.Decode<Dictionary<string, string>>(token, Encoding.UTF8.GetBytes(key), JwsAlgorithm.HS512);
            }
            catch
            {
                // 不合規的token亦視為驗證失敗
                return false;
            }

            if (!jwtObject.ContainsKey(identificationTag))
                return false;

            if (jwtObject.ContainsKey(expirationTimeTag))
            {
                if (DateTimeOffset.TryParse(jwtObject[expirationTimeTag], out var time))
                    return time > DateTimeOffset.Now;
                else
                    return false;
            }

            return true;
        }
    }
}
