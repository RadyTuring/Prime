using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using MySignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCall
{
    public interface IAppService
    {
        public Task FtpUpload(IFormFile file, string connectionId, IHubContext<UploadProgressHub> hubContext);
        public object Get2(string apiName, System.Net.Http.HttpContent o);
        public IEnumerable<T>  Get<T>(string apiName);
        public Task<bool> GetValAsync(string apiName);
        public T GetById<T>(string apiName);
        public Task<(string, T)> Post_Async<T>(string apiName, object o) where T : class;
        public Task<T> Execute<T>(string apiName, object o);
        public   Task<T> Login<T>(string apiName, object o) where T : class;
        public Task<bool> PostAsync<T>(string apiName, T o) where T : class;
        public bool Delete(string apiName, string id=null);
        public bool Update<T>(string apiName, T o);
        public Task<bool> UpdateAsync<T>(string apiName, T o);
        public bool Create(string apiName, object o);
        public IEnumerable<T> Create<T>(string apiName, object o, bool withresult = true);
        public bool CreateWithFile(string apiName, object o, bool isUpdate = false);
        public T Create2<T>(string apiName, object o, bool isUpdate = false);
        public string GetImage(string apiName);
        public SelectList GetList<T>(string apiName) where T : class;
        public  Task<TResponse> PostDt<TRequest, TResponse>(string url, TRequest data);
    }
}
