  namespace ProjectBravo.Infrastructure;

public class Commit{
    public int Id { get; set; }

    public string BelongsTo {get; set;}

    public Author Author {get; set;}

    public DateTime Date {get; set;}

    public string Message{get; set;}


}
