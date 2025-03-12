using Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Dto;

public class AuthModel
{
    public string? Message { get; set; }
    public bool IsAuthenticated { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public bool? IsNewUser { get; set; } 
    public List<string>? Roles { get; set; }
    public string? Token { get; set; }
    public DateTime? ExpiresOn { get; set; }
    public int UserId { get; set; }
    public int? RoleId { get; set; }
    public Role? Role { get; set; }
    public int? LogOfflineTimes { get; set; }
    [StringLength(150)]
    public string? ImageFile { get; set; }
    [JsonIgnore]
    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiration { get; set; }
}