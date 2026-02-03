namespace DevQuestions.Domain.Questions;

public class Question
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid? ScreenshotId { get; set; }
    public List<Answer> Answers { get; set; } = [];
    public Answer? Solution { get; set; }
    public List<Guid> Tags { get; set; } = [];
}