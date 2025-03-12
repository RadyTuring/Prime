using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

using System.Reflection;
using NuGet.Configuration;
using static System.Net.WebRequestMethods;
using Dto;
using Humanizer;
using System.Text;
using NuGet.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataTablesHelper;
using Microsoft.AspNetCore.SignalR;
using MySignalR;
namespace ApiCall
{

    public class AppService : IAppService
    {
        #region DeclareConstruct
        private readonly string _ftpServer;
        private readonly string _ftpUsername;
        private readonly string _ftpPassword;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly string baseAddress;
        private HttpClient client;
        // Static property to track progress
        public static int Progress { get; private set; }
        public AppService(IConfiguration configuration, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {

            _ftpServer = configuration.GetSection("ftp")["host"];
            _ftpUsername = configuration.GetSection("ftp")["user"];
            _ftpPassword = configuration.GetSection("ftp")["password"];

            baseAddress = configuration.GetSection("api")["baseurl"];

            client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);

            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion
        #region All
        private void SetRefreshTokenCookie(string refreshToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            httpContext.Response.Cookies.Append("_ref_token", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                // Secure = true,
                Expires = DateTime.UtcNow.AddDays(30)
            });
        }

        public bool Create(string apiName, object o)
        {
            var postTask = client.PostAsJsonAsync(apiName, o);
            postTask.Wait();
            var result = postTask.Result;
            return result.IsSuccessStatusCode;
        }
        public IEnumerable<T> Create<T>(string apiName, object o, bool withresult = true)
        {
            var postTask = client.PostAsJsonAsync(apiName, o);
            postTask.Wait();
            return postTask.Result.Content.ReadAsAsync<IEnumerable<T>>().Result;

        }
        public async Task<T> Execute<T>(string apiName, object o)
        {
            RefreshToken();
            //var content = new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
            var postTask = await client.PostAsJsonAsync(apiName, o);
             
            return postTask.Content.ReadAsAsync<T>().Result;

        }
        public async Task<TResponse> PostDt<TRequest, TResponse>(string url, TRequest data)
        {
            RefreshToken();
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var jsonData = JsonConvert.DeserializeObject<TResponse>(jsonResponse);
                return jsonData;
            }
            else
            {
                throw new Exception("Error: Unable to complete the request");
            }
        }

        public IEnumerable<T> Get<T>(string apiName)
        {
            if (RefreshToken() == null)
                return null;
            var res = client.GetAsync(apiName).Result;
            var data = res.Content.ReadAsAsync<IEnumerable<T>>().Result;
            return data;
        }
        public object Get2(string apiName, System.Net.Http.HttpContent o)
        {
            if (RefreshToken() == null)
                return null;


            // Send POST request to the API endpoint with the JSON payload
            var res = client.PostAsync("CodeGen/GetPrimeCodes", o).Result;

            var data = res.Content.ReadAsStringAsync().Result;
            return data;
        }
        public T GetById<T>(string apiName)
        {
            RefreshToken();
            var res = client.GetAsync(apiName).Result;
            var data = res.Content.ReadAsAsync<T>().Result;
            return data;
        }
        public async Task<bool> GetValAsync(string apiName)
        {
            RefreshToken();
            HttpResponseMessage response = await client.GetAsync(apiName);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<bool>();
            }
            return false;
        }
        public async Task<T> Login<T>(string apiName, object o) where T : class
        {

            string refToken = "";
            var jsonContent = JsonConvert.SerializeObject(o);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiName, content);

            var res = await response.Content.ReadFromJsonAsync<T>();

            // Extract the refresh token from the response cookie
            if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
            {
                var refreshTokenCookie = cookies.FirstOrDefault(cookie => cookie.StartsWith("refreshToken="));
                if (!string.IsNullOrEmpty(refreshTokenCookie))
                {
                    refToken = refreshTokenCookie.Split("=")[1].Split(";")[0];
                    SetRefreshTokenCookie(refToken);
                }

            }
            return res;
        }
        public async Task<(string, T)> Post_Async<T>(string apiName, object o) where T : class
        {
            RefreshToken();

            T result = null;
            string s = null;
            var postTask = client.PostAsJsonAsync(apiName, o);
            postTask.Wait();
            var response = postTask.Result;
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                result = JsonConvert.DeserializeObject<T>(responseContent);
            }
            else
            {
                s = await response.Content.ReadAsStringAsync();
            }
            return (s, result);
        }
        public async Task<bool> PostAsync<T>(string apiName, T o) where T : class
        {
            bool flag = false;

            var postTask = client.PostAsJsonAsync(apiName, o);
            postTask.Wait();
            var response = postTask.Result;
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                flag = true;
            }

            return flag;
        }
        public bool Delete(string apiName, string id = null)
        {
            RefreshToken();
            Task<HttpResponseMessage> postTask = null;
            if (string.IsNullOrEmpty(id))
                postTask = client.DeleteAsync(apiName);
            else
                postTask = client.DeleteAsync(apiName + "?id=" + id);
            postTask.Wait();
            var result = postTask.Result;
            return result.IsSuccessStatusCode;
        }
        public AuthModel RefreshToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var _ref_token = httpContext.Request.Cookies["_ref_token"];
            AuthModel authModel = null;
            client.DefaultRequestHeaders.Add("Cookie", $"refreshToken={_ref_token}");

            var res = client.GetAsync("User/RefreshToken").Result;
            authModel = res.Content.ReadAsAsync<AuthModel>().Result;

            if (authModel.IsAuthenticated)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authModel.Token);
            return authModel;

        }
        public bool Update<T>(string apiName, T o)
        {
            RefreshToken();
            var postTask = client.PutAsJsonAsync(apiName, o);
            postTask.Wait();
            var result = postTask.Result;
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateAsync<T>(string apiName, T o)
        {
            RefreshToken();
            var postTask = client.PutAsJsonAsync(apiName, o);
            postTask.Wait();
            var result = postTask.Result;
            return result.IsSuccessStatusCode;
        }
        public bool CreateWithFile(string apiName, object o, bool isUpdate = false)
        {
            RefreshToken();
            var content = new MultipartFormDataContent();

            Type objectType = o.GetType();

            PropertyInfo[] properties = objectType.GetProperties();

            foreach (var property in properties)
            {
                if (typeof(IFormFile).IsAssignableFrom(property.PropertyType))
                {
                    var fileValue = (IFormFile)property.GetValue(o);
                    if (fileValue != null)
                    {
                        var fileContent = new StreamContent(fileValue.OpenReadStream());
                        content.Add(fileContent, property.Name, fileValue.FileName);
                    }
                }
                else
                {
                    var propertyValue = property.GetValue(o)?.ToString();
                    if (propertyValue != null)

                        content.Add(new StringContent(propertyValue), property.Name);
                }
            }
            if (!isUpdate)
            {
                var postTask = client.PostAsync(apiName, content);
                postTask.Wait();
                var result = postTask.Result;
                return result.IsSuccessStatusCode;
            }
            else
            {
                var postTask = client.PutAsync(apiName, content);
                postTask.Wait();
                var result = postTask.Result;
                return result.IsSuccessStatusCode;
            }

        }
        public T Create2<T>(string apiName, object o, bool isUpdate = false)
        {
            RefreshToken();
            var content = new MultipartFormDataContent();

            Type objectType = o.GetType();

            PropertyInfo[] properties = objectType.GetProperties();

            foreach (var property in properties)
            {
                if (typeof(IFormFile).IsAssignableFrom(property.PropertyType))
                {
                    var fileValue = (IFormFile)property.GetValue(o);
                    if (fileValue != null)
                    {
                        var fileContent = new StreamContent(fileValue.OpenReadStream());
                        content.Add(fileContent, property.Name, fileValue.FileName);
                    }
                }
                else
                {
                    var propertyValue = property.GetValue(o)?.ToString();
                    if (propertyValue != null)

                        content.Add(new StringContent(propertyValue), property.Name);
                }
            }
            if (!isUpdate)
            {
                var postTask = client.PostAsync(apiName, content);
                postTask.Wait();
                var result = postTask.Result;
                return postTask.Result.Content.ReadAsAsync<T>().Result;
            }
            else
            {
                var postTask = client.PutAsync(apiName, content);
                postTask.Wait();
                var result = postTask.Result;
                return postTask.Result.Content.ReadAsAsync<T>().Result;
            }

        }
        public string GetImage(string apiName)
        {
            if (RefreshToken() == null)
                return null;
            try
            {
                var response = client.GetAsync(apiName).Result;
                if (response.IsSuccessStatusCode)
                {
                    byte[] imageData = response.Content.ReadAsByteArrayAsync().Result;
                    string base64Image = Convert.ToBase64String(imageData);
                    return base64Image;
                }
                else
                {
                    return response.StatusCode.ToString();
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public SelectList GetList<T>(string apiName) where T : class
        {
            IList<T> lst = (IList<T>)Get<T>(apiName);
            var properties = typeof(T).GetProperties();
            var idProperty = properties.ElementAtOrDefault(0)?.Name;
            var nameProperty = properties.ElementAtOrDefault(1)?.Name;
            return new SelectList(lst, idProperty, nameProperty);
        }

        #endregion
        public async Task FtpUpload(IFormFile file, string connectionId, IHubContext<UploadProgressHub> hubContext)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file", nameof(file));

            var tempFilePath = Path.GetTempFileName();
            try
            {
                // Save the file temporarily
                using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Prepare the FTP request
                var requestUri = new Uri($"{_ftpServer}/{file.FileName}");
                var request = (FtpWebRequest)WebRequest.Create(requestUri);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(_ftpUsername, _ftpPassword);

                var fileInfo = new FileInfo(tempFilePath);
                request.ContentLength = fileInfo.Length;

                // Upload the file
                using (var requestStream = await request.GetRequestStreamAsync())
                using (var fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read))
                {
                    var buffer = new byte[4096];
                    int bytesRead;
                    long totalBytesSent = 0;

                    while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await requestStream.WriteAsync(buffer, 0, bytesRead);
                        totalBytesSent += bytesRead;

                        // Report progress to SignalR
                        int percentComplete = (int)((double)totalBytesSent / fileInfo.Length * 100);
                        await hubContext.Clients.Client(connectionId).SendAsync("ReceiveProgress", percentComplete);
                    }
                }

                // Confirm upload completion
                using (var response = (FtpWebResponse)await request.GetResponseAsync())
                {
                    Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
                }
            }
            finally
            {
                // Cleanup temporary file
                if (System.IO.File.Exists(tempFilePath))
                {
                    System.IO.File.Delete(tempFilePath);
                }
            }
        }
    }

}
