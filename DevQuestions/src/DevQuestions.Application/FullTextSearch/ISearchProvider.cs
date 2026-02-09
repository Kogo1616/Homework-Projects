using CSharpFunctionalExtensions;
using DevQuestions.Domain.Questions;

namespace DevQuestions.Application.FullTextSearch;

public interface ISearchProvider
{
    Task<List<Guid>> SearchAsync(string query, CancellationToken cancellationToken);
    Task IndexQuestionAsync(Question question, CancellationToken cancellationToken);
}