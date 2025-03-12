using Entities;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Services;
using System.Net.NetworkInformation;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using HelperModels;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Net;

using System.Net.Http;
using System.Security.Cryptography;
using Dto;

namespace Services
{
    public class AppService : IAppService
    {
        private IWebHostEnvironment _host;
        private string  _ftpServer;
        private string _ftpUser;
        private string _ftpPassword;
        private readonly Random random = new Random();
        private readonly IUnitOfWork _unitOfWork;
        
        public AppService(IUnitOfWork unitOfWork, IWebHostEnvironment host, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _host = host;
            _ftpServer = configuration.GetSection("ftp")["host"];
            _ftpUser = configuration.GetSection("ftp")["user"];
            _ftpPassword = configuration.GetSection("ftp")["password"];
        }
       
        public string Uploade(IFormFile file, string path)
        {
            var _file = Guid.NewGuid() + file.FileName;
            var _filwithpath = Path.Combine(Directory.GetCurrentDirectory(), path, _file);

            using (var stream = new FileStream(_filwithpath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return _file;
        }
        public void Delete(string fileNameWithPath)
        {
            File.Delete(Path.Combine(Directory.GetCurrentDirectory(), fileNameWithPath));
        }
        public string Uploade(List<IFormFile>? files, string path)
        {
            StringBuilder _filesNames = new StringBuilder(); ;
            foreach (var file in files)
            {
                var _file = Guid.NewGuid() + file.FileName;
                _filesNames.Append(_file);
                _filesNames.Append("|");
                var _filwithpath = Path.Combine(Directory.GetCurrentDirectory(), path, _file);

                using (var stream = new FileStream(_filwithpath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            return _filesNames.ToString().Substring(0, _filesNames.Length - 1);
        }
        public async Task<string> UploadeAsync(List<IFormFile>? files, string path)
        {
            StringBuilder _filesNames = new StringBuilder(); ;
            foreach (var file in files)
            {
                var _file = Guid.NewGuid() + file.FileName;
                _filesNames.Append(_file);
                _filesNames.Append("|");
                var _filwithpath = Path.Combine(Directory.GetCurrentDirectory(), path, _file);

                using (var stream = new FileStream(_filwithpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            return _filesNames.ToString().Substring(0, _filesNames.Length - 1);
        }

        public AppSetting GetAppSetting(int id)
        {
            return _unitOfWork.AppSettings.Find(m => m.Id == id);
        }

        public DateTime GetDTM(DateTime dt, int userId)
        {
            var _timeDiff = _unitOfWork.Users.Find(m => m.UserId == userId, new string[] { "Country" }).Country.TimeDif;
            var _timeDifFactToUtc = _unitOfWork.AppSettings.Find(m => m.Id == 1).TimeFactor;
            var newDt = dt.AddHours( _timeDiff+ _timeDifFactToUtc);
            return newDt;
        }
        public async Task<string> UploadFile(IFormFile _IFormFile)
        {
            string FileName = "";
            try
            {
                FileInfo _FileInfo = new FileInfo(_IFormFile.FileName);
                FileName = Path.GetFileNameWithoutExtension(_IFormFile.FileName) + "_" + DateTime.Now.Ticks.ToString() + _FileInfo.Extension;
                var _GetFilePath = Common.GetFilePath(FileName);
                using (var _FileStream = new FileStream(_GetFilePath, FileMode.Create))
                {
                    await _IFormFile.CopyToAsync(_FileStream);
                }
                return FileName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<(byte[], string, string)> DownloadFile(string FileName)
        {
            try
            {
                var _GetFilePath = Common.GetFilePath(FileName);
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(_GetFilePath, out var _ContentType))
                {
                    _ContentType = "application/octet-stream";
                }
                var _ReadAllBytesAsync = await File.ReadAllBytesAsync(_GetFilePath);
                return (_ReadAllBytesAsync, _ContentType, Path.GetFileName(_GetFilePath));
            }
            catch (Exception ex)
            {
                return (new byte[1] ,"notfound", "notfound");
            }
        }
        public (byte[], string, string) DownloadFile2(string fileName)
        {
            try
            {
                var filePath = Common.GetFilePath(fileName);
                var provider = new FileExtensionContentTypeProvider();

                if (!provider.TryGetContentType(filePath, out var contentType))
                {
                    contentType = "application/octet-stream"; // Default to binary if unknown
                }

                var fileBytes = File.ReadAllBytes(filePath);
                return (fileBytes, contentType, Path.GetFileName(filePath)); // Return with original name
            }
            catch (Exception ex)
            {
                // Handle or log exception if needed
                return (new byte[1], "notfound", "notfound");
            }
        }

        //public string GenerateCode(int length)
        //{
        //    const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        //    StringBuilder code = new StringBuilder();
        //    Random random = new Random();
        //    // Add random characters
        //    for (int i = 0; i < length - 5; i++)
        //    {
        //        int index = random.Next(characters.Length);
        //        code.Append(characters[index]);
        //    }
        //    long timestampPart = DateTime.Now.Ticks % 1000000000;
        //    string timestampPartString = timestampPart.ToString().PadLeft(5, '0').Substring(0, 5);
        //    code.Append(timestampPartString);
        //    return code.ToString();
        //}

        public string GenerateCode(int length)
        {
            // Get current timestamp (in seconds)
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Generate a random UUID
            Guid randomGuid = Guid.NewGuid();

            // Combine timestamp and random UUID
            string combinedData = $"{timestamp}-{randomGuid}";

            // Hash the combined data using SHA-256
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(combinedData));

                // Convert the hashed bytes to a hexadecimal string
                string hashedData = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                // Take the first 6 characters of the hashed data
                string _code = hashedData.Substring(0, length);

                return _code;
            }
        }
        public void UploadFile(string localFilePath, string remoteFileName)
        {
            string ftpUrl = $"{_ftpServer}/{remoteFileName}";

            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(_ftpUser, _ftpPassword);

                using (FileStream fileStream = File.OpenRead(localFilePath))
                using (Stream requestStream = request.GetRequestStream())
                {
                    fileStream.CopyTo(requestStream);
                }

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
            }
            catch (WebException ex)
            {
                Console.WriteLine($"Error: {ex.Status} - {ex.Message}");
            }
        }

        #region Notifications
        public async void SendNote(int fromUserId, NoteDto noteDto)
        {
            Notification note = new Notification()
            {
                Message = noteDto.Message,
                Category = noteDto.Category,
                FromUserId = fromUserId,
                ToUserId = noteDto.ToUserId
            };
            await _unitOfWork.Notifications.AddAsync(note);
            _unitOfWork.Save();
        }

        public void UpdateNoteToRead(int noteId)
        {
            var note = _unitOfWork.Notifications.Find(m => m.Id == noteId);
            note.IsRead = true;
            _unitOfWork.Notifications.Update(note);
            _unitOfWork.Save();
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotes(int userId)
        {
            IEnumerable<Notification> notes = await _unitOfWork.Notifications.FindAllAsync(m => m.ToUserId == userId && m.IsRead == false, new string[] { "User" });
            return notes.OrderByDescending(m => m.NoteDate);
        }
        public async Task<IEnumerable<Notification>> GetReadNotes(int userId)
        {
            IEnumerable<Notification> notes = await _unitOfWork.Notifications.FindAllAsync(m => m.ToUserId == userId && m.IsRead == true, new string[] { "User" });
            return notes.OrderByDescending(m => m.NoteDate);
        }
        public async Task<IEnumerable<Notification>> GetAllNotes(int userId)
        {
            IEnumerable<Notification> notes = await _unitOfWork.Notifications.FindAllAsync(m => m.ToUserId == userId, new string[] { "User" });
            return notes.OrderByDescending(m=>m.NoteDate);
        } 
        #endregion


    }
}
