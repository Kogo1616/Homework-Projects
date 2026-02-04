namespace DevQuestions.Contracts;

public record UpdateQuestionRequest(string Title, string Body, Guid[] TagsId);