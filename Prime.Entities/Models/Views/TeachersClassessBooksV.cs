namespace Entities;

public class TeachersClassessBooksV
    {
        public int Id { get; set; }
        public int? StudentId { get; set; }
        public int? TeacherId { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public int? AdminId { get; set; }
        public int? BookId { get; set; }
        public string? BookName { get; set; }
        public DateTime? AssignDate { get; set; }
        public string? ClassCode { get; set; }
    }
 
