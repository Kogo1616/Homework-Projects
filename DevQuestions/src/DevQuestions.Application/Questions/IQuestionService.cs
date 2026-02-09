using CSharpFunctionalExtensions;
using DevQuestions.Contracts.Questions;

namespace DevQuestions.Application.Questions;

public interface IQuestionService
{
    Task Create(CreateQuestionRequest questionRequest,
        CancellationToken cancellationToken);

    Task Get(GetQuestionRequest questionRequest,
        CancellationToken cancellationToken);

    Task GetById(Guid questionId, CancellationToken cancellationToken);

    Task Update(Guid questionId,
        UpdateQuestionRequest questionRequest, CancellationToken cancellationToken);

    Task Delete(Guid questionId,
        CancellationToken cancellationToken);

    Task SelectSolution(Guid questionId, Guid answerId,
        CancellationToken cancellationToken);

    Task AddAnswer(Guid questionId, AddAnswerRequest answerRequest,
        CancellationToken cancellationToken);
}