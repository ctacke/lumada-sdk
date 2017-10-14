using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionFamily.Lumada
{
    public class Session
    {
        internal RequestService RequestService { get; private set; }

        public string ClientID { get; private set; }
        public string SessionID { get; private set; }
        internal string AccessToken { get; private set; }
        private string RefreshToken { get; set; }
        public DateTime Expires { get; private set; }

        public SessionAssetTypes AssetTypes { get; private set; }
        public SessionAssets Assets { get; private set; }
        public SessionData Data { get; private set; }

        internal Session(
            RequestService service,
            string clientID,
            string sessionID, 
            string accessToken,
            string refreshToken,
            int expiresInMinutes)
        {
            AssetTypes = new SessionAssetTypes(this);
            Assets = new SessionAssets(this);
            Data = new SessionData(this);

            RequestService = service;
            ClientID = clientID;
            SessionID = sessionID;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Expires = DateTime.Now.AddSeconds(expiresInMinutes);
        }

        public async Task RefreshAsync()
        {
            var response = await RequestService.RefreshSessionAsync(this.RefreshToken, this.ClientID);
            AccessToken = response.AccessToken;
            RefreshToken = response.RefreshToken;
            Expires = DateTime.Now.AddSeconds(response.Expiry);
        }

    }
}
