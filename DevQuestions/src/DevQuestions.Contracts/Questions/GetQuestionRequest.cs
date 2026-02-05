namespace DevQuestions.Contracts.Questions;

public record GetQuestionRequest(string Search, Guid[] TagIds, int Page);