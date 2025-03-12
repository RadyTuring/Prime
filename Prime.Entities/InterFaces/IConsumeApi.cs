using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCall
{
    public interface IConsumeApi
    {
        public IEnumerable<T> Get<T>(string apiName);
        public T GetById<T>(string apiName);
        public void SetToken(string token);
        public Task<(string, T)> Post_Async<T>(string apiName, object o) where T : class;
        public Task<bool> PostAsync<T>(string apiName, T o) where T : class;
        public bool Delete(string apiName, string id);
        public bool Update<T>(string apiName, T o);
        public bool Create(string apiName, object o);
        public HttpResponseMessage Create(string apiName, object o,bool withresult=true);
        public bool CreateWithFile(string apiName, object o, bool isUpdate = false);
        public Task<string> GetImage(string imageUrl);
       

    }
}
