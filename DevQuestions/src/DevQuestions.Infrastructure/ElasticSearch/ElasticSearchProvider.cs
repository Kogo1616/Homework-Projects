using CSharpFunctionalExtensions;
using DevQuestions.Application.FullTextSearch;
using DevQuestions.Domain.Questions;
using Shared;

namespace DevQuestions.Infrastructure.ElasticSearch;

public class ElasticSearchProvider : ISearchProvider
{
    public async Task<List<Guid>> SearchAsync(string query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<UnitResult<Failure>> IndexQuestionAsync(Question question, CancellationToken cancellationToken)
    {
        try
        {
            //_elastic.Search()
        }
        catch (Exception e)
        {
            return Error.Failure("search.error", e.Message).ToFailure();
        }

        return UnitResult.Success<Failure>();
    }
}