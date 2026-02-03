namespace DevQuestions.Domain.Questions;

public class Answer
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Text { get; set; } = string.Empty;
    public required Answer Parent { get; set; }
    public required Question Question { get; set; }
    public List<Guid> Comments { get; set; } = [];
}