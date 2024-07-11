using System.ComponentModel;

namespace Para.Api;

public class Book
{
    
    [DisplayName("Book id")]
    public int Id { get; set; }
        
    
    [DisplayName("Book name")]
    public string Name { get; set; }
    
    [DisplayName("Book author info")]
    public string Author { get; set; }
    
    [DisplayName("Book page count")]
    [PageCount]
    public int PageCount { get; set; }
    
    [DisplayName("Book year")]
    public int Year { get; set; }

}