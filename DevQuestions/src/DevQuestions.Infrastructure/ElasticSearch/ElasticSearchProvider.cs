using CSharpFunctionalExtensions;
using DevQuestions.Application.FullTextSearch;
using DevQuestions.Domain.Questions;

namespace DevQuestions.Infrastructure.ElasticSearch;

public class ElasticSearchProvider : ISearchProvider
{
    public async Task<List<Guid>> SearchAsync(string query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task IndexQuestionAsync(Question question, CancellationToken cancellationToken)
    {
       
    }
}