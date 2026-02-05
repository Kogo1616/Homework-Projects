namespace DevQuestions.Contracts.Questions;

public record AddAnswerRequest(Guid UserId, string Title);