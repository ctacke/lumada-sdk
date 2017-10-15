using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SolutionFamily.Lumada
{
    public class Server
    {
        public string Address { get; private set; }
        public int Port { get; private set; }

        private const string APIVersion = "v1";

        internal string APIRoot { get; private set; }

        internal RequestService m_requestService;

        public Server(string address)
        {
            if (!address.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                // lumada installs default to https - we should expect (and encourage) that
                address = "https://" + address;
            }

            var uri = new Uri(address);

            Address = uri.Host;
            Port = uri.Port;

            APIRoot = string.Format("{0}://{1}/{2}/",
                uri.Scheme, 
                uri.Authority,
                APIVersion);

            m_requestService = new RequestService(APIRoot, APIVersion);
        }

        /// <summary>
        /// This is unfriendly, and likely unwise, in a production environment.  It's meant to enable test environments where an SSL certificate has not been installed on the Lumada server.
        /// </summary>
        public bool IgnoreCertificateErrors
        {
            get { return m_requestService.IgnoreCertificateErrors; }
            set { m_requestService.IgnoreCertificateErrors = value; }
        }

        public async Task<ServerInfo> GetServerInfoAsync()
        {
            var path = string.Format("{0}security/about", APIRoot, APIVersion);

            return await m_requestService.GetAsync<ServerInfo>(path);
        }

        public async Task<Session> CreateSessionAsync(string username, string password, string clientID = "lumada-ui")
        {
            try
            {
                return await m_requestService.CreateSessionAsync(username, password, clientID);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is HttpRequestException)
                {
                    var he = ex.InnerException as HttpRequestException;

                    throw new ServerTimeoutException(he.InnerException.Message);
                }
                return null;
            }
        }
    }
}
