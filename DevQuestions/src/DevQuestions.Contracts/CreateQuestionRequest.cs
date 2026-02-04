namespace DevQuestions.Contracts;

public record CreateQuestionRequest(string Title, string Body, Guid UserId, Guid[] TagsId);