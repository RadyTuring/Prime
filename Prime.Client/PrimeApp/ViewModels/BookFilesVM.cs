using System.ComponentModel.DataAnnotations;

namespace ViewModels;
public class BookFilesVM
{
   
    public int BookId { get; set; }
    public string  BookName { get; set; }
    [ Display(Name = "Book PPk")]
    public IFormFile? PPKFile { get; set; }
    [ Display(Name = "Book Image")]
    public IFormFile? DefaultImage { get; set; }
    [ Display(Name = "Games File")]
    public IFormFile? GamesFileName { get; set; }
    [ Display(Name = "Games Image")]
    public IFormFile? GamesImage { get; set; }

    
}
