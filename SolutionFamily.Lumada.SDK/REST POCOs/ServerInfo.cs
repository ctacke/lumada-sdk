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
    public class ServerInfo
    {
        public string LumadaOSVersion { get; set; }
        public string LumadaVersion { get; set; }
    }
}
