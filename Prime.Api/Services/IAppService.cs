using CSV;
using Dto;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace  Services
{
    public interface IAppService
    {
        #region Files
        public string Uploade(IFormFile files, string path);
        public string Uploade(List<IFormFile>? files, string path);
        public Task<string> UploadeAsync(List<IFormFile>? files, string path);
        Task<string> UploadFile(IFormFile _IFormFile);
        Task<(byte[], string, string)> DownloadFile(string FileName);
        public (byte[], string, string) DownloadFile2(string FileName);
        public void Delete(string fileNameWithPath);
        #endregion

        #region Setting_GenCode
        public AppSetting GetAppSetting(int id);
        public DateTime GetDTM(DateTime dt, int userId);
        public string GenerateCode(int length);
        #endregion

        #region Notifications
        public void SendNote(int fromUserId, NoteDto noteDto);

        public void UpdateNoteToRead(int noteId);
        public Task<IEnumerable<Notification>> GetUnreadNotes(int userId);
        public Task<IEnumerable<Notification>> GetReadNotes(int userId);
        public  Task<IEnumerable<Notification>> GetAllNotes(int userId);
        #endregion
    }
}
