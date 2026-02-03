namespace DevQuestions.Domain.Comments;

public class Comment
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Comment? Parent { get; set; }
    public required Guid EntityId { get; set; }
    public List<Comment> Children { get; set; } = [];
}