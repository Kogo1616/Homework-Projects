namespace DevQuestions.Contracts.Questions;

public record UpdateQuestionRequest(string Title, string Body, Guid[] TagsId);