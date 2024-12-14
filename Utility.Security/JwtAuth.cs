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
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool AuthorizationToken(string key, string token)
        {
            if (key == null || token == null) return false;

            var jwtObject = JWT.Decode<Dictionary<string, string>>(token, Encoding.UTF8.GetBytes(key), JwsAlgorithm.HS512);
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
