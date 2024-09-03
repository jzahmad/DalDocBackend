public class Comment
{
    public int CommentID { get; set; }
    public required string Department { get; set; }
    public required string Message { get; set; }
    public DateTime Timestamp { get; set; }
}
