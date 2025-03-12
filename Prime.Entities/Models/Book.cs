using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class Book
{
    [Key]
    public int BookId { get; set; }

    [Required(ErrorMessage = "Please Enter the Book Name"), StringLength(60), Display(Name = "Book Name")]
    public string? BookName { get; set; }

    [StringLength(100), Display(Name = "PPK File")]
    public string? PPKFileName { get; set; }
    [StringLength(160),  Display(Name = "Book Image")]
    public string? DefaultImage { get; set; }

    [StringLength(100), Display(Name = "Games File Name")]
    public string? GamesFileName { get; set; }
    [StringLength(160), Display(Name = "Games Image")]
    public string? GamesImage { get; set; }
    [ Required(ErrorMessage = "Please Enter the Book Prefix"), StringLength(3, ErrorMessage = "The Prefix should be 3 Characters"), MinLength(3, ErrorMessage = "The Prefix should be 3 Characters"), Display(Name = "Book Prefix")]
    public string?  Prefix { get; set; }
    [Required(ErrorMessage = "Please enter the version"), Display(Name = "PPK Version")]
    public int PPkVersion { get; set; }
    [Required(ErrorMessage = "Please Choose the Update Level"), Display(Name = "Update Level")]
    public int? UpdateLevelId { get; set; }
    public UpdateLevel? UpdateLevel { get; set; }
    [StringLength(200), DataType(DataType.Url, ErrorMessage = "Invalid Url"), Column(TypeName = "varchar(200)")]
    public string? DownloadLink { get; set; }
    [StringLength(200),DataType(DataType.Url,ErrorMessage ="Invalid Url"), Column(TypeName ="varchar(200)")]
    public string? LinkUrl { get; set; }
    [StringLength(100)]
    public string? Notes { get; set; }
}
