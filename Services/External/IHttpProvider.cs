using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.External
{
    public interface IHttpProvider
    {
        Task<string> Get(string url);

        Task<string> Post(string url, string json);
    }
}
