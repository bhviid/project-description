using System.ComponentModel.DataAnnotations;

namespace ProjectBravo.Infrastructure;

public class Author{
    public int Id {get; set;}

    [Required]
    public string Name { get; set; }

    [EmailAddress]   
    public string Email { get; set; }

    public Author(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public Author() { }
}