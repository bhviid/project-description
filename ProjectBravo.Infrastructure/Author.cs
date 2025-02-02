using System.ComponentModel.DataAnnotations;

namespace ProjectBravo.Infrastructure;

public class Author{
    public int Id {get; set;}

    public string Name { get; set; }

    public Author(string name)
    {
        Name = name;
    }

    public Author() { }
}