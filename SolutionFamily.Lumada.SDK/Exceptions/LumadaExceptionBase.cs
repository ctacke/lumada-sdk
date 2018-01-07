using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SolutionFamily.Lumada
{
    public abstract class LumadaExceptionBase : Exception
    {
        public LumadaExceptionBase(string message)
            : base(message)
        {
        }

        public LumadaExceptionBase()
        {
        }
    }

    public class CommunicationException : LumadaExceptionBase
    {
        public HttpStatusCode StatusCode { get; private set; }

        internal CommunicationException(HttpResponseMessage response)
            : base(response.ReasonPhrase)
        {
            StatusCode = response.StatusCode;
        }
    }

    public class ServerException : LumadaExceptionBase
    {
        public int Code { get; private set; }

        internal ServerException(ErrorResponse response)
            : base(response.Message)
        {
            Code = response.Code;
        }
    }

    public class ServerUnavailableException : LumadaExceptionBase
    {
    }

    public class ServerTimeoutException : LumadaExceptionBase
    {
        public ServerTimeoutException(string message)
            : base(message)
        {
        }
    }
}
