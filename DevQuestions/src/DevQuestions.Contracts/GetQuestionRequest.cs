namespace DevQuestions.Contracts;

public record GetQuestionRequest(string Search, Guid[] TagIds, int Page);