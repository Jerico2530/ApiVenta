using System.Net;

namespace Api.Models
{
    public class APIResponse
    {
            public HttpStatusCode StatusCode { get; set; }
            public bool IsExitoso { get; set; } = true;
            public List<string> ErroresMessages { get; set; }
            public object Resultado { get; set; }
    }
}
