namespace BlogAPI.Models;

public class PostRequest
{
    public int AuthorId { get; set;}
    public PostDto? Post { get; set; }
}