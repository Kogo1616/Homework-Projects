using DevQuestions.Application.FullTextSearch;
using DevQuestions.Contracts.Questions;
using DevQuestions.Domain.Questions;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DevQuestions.Application.Questions;

public class QuestionsService(
    IQuestionsRepository questionsRepository,
    IValidator<CreateQuestionRequest> validator,
    ILogger<QuestionsService> logger,
    ISearchProvider searchProvider) : IQuestionService
{
    private readonly IQuestionsRepository _questionsRepository = questionsRepository;
    private readonly IValidator<CreateQuestionRequest> _validator = validator;
    private readonly ILogger<QuestionsService> _logger = logger;
    private readonly ISearchProvider _searchProvider = searchProvider;

    public async Task Create(CreateQuestionRequest questionRequest,
        CancellationToken cancellationToken)
    {
        var validatorResult = await _validator.ValidateAsync(questionRequest, cancellationToken);

        if (!validatorResult.IsValid)
        {
        }

        //validation business logic
        var openUserQuestionsCount =
            await _questionsRepository.GetOpenUserQuestionsAsync(questionRequest.UserId, cancellationToken);

        if (openUserQuestionsCount > 3)
        {
            //return Errors.Questions.ToManyQuestions().ToFailure();
        }

        var questionId = Guid.NewGuid();


        var question = new Question(
            questionId,
            questionRequest.Title,
            questionRequest.Text,
            questionRequest.UserId,
            null,
            questionRequest.TagsIds);

        await _questionsRepository.AddAsync(question, cancellationToken);

        await _searchProvider.IndexQuestionAsync(question, cancellationToken);

        _logger.LogInformation("Question created with id {questionId}", questionId);

       // return questionId;
    }

    public async Task Get(GetQuestionRequest questionRequest,
        CancellationToken cancellationToken)
    {
    }

    public async Task GetById(Guid questionId, CancellationToken cancellationToken)
    {
    }

    public async Task Update(Guid questionId,
        UpdateQuestionRequest questionRequest, CancellationToken cancellationToken)
    {
    }

    public async Task Delete(Guid questionId,
        CancellationToken cancellationToken)
    {
    }

    public async Task SelectSolution(Guid questionId, Guid answerId,
        CancellationToken cancellationToken)
    {

    }

    public async Task AddAnswer(Guid questionId, AddAnswerRequest answerRequest,
        CancellationToken cancellationToken)
    {
    }
}