using DevQuestions.Contracts.Questions;
using DevQuestions.Domain.Questions;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DevQuestions.Application.Questions;

public class QuestionsService(IQuestionsRepository questionsRepository, IValidator<CreateQuestionRequest> validator, ILogger<QuestionsService> logger) : IQuestionService
{
    private readonly IQuestionsRepository _questionsRepository = questionsRepository;
    private readonly IValidator<CreateQuestionRequest> _validator = validator;
    private readonly ILogger<QuestionsService> _logger = logger;

    public async Task<Guid> Create(CreateQuestionRequest questionRequest,
        CancellationToken cancellationToken)
    {
        //validation request model
        var validatorResult = await _validator.ValidateAsync(questionRequest, cancellationToken);

        if (!validatorResult.IsValid)
        {
            throw new ValidationException(validatorResult.Errors);
        }

        //validation business logic
        var openUserQuestionsCount =
            await _questionsRepository.GetOpenUserQuestionsAsync(questionRequest.UserId, cancellationToken);

        if (openUserQuestionsCount > 3)
        {
            throw new Exception("User can not open more three questions");
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

        _logger.LogInformation("Question created with id {questionId}", questionId);

        return questionId;
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