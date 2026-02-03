namespace DevQuestions.Domain.Reports;

public class Report
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ReportedUserId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public Status Status { get; set; } = Status.Open;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public Guid ResolvedByUserId { get; set; }
}