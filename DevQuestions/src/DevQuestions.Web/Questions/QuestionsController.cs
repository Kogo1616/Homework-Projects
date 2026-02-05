using DevQuestions.Application.Questions;
using DevQuestions.Contracts.Questions;
using Microsoft.AspNetCore.Mvc;

namespace DevQuestions.Web.Questions
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionsController(IQuestionService questionService) : ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateQuestionRequest questionRequest,
            CancellationToken cancellationToken)
        {
            var questionId = await _questionService.Create(questionRequest, cancellationToken);
            return Ok(questionId);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetQuestionRequest questionRequest,
            CancellationToken cancellationToken)
        {
            return Ok("Questions Get");
        }

        [HttpGet("{quetionId:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid questionId, CancellationToken cancellationToken)
        {
            return Ok("Questions Get");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromRoute] Guid questionId,
            [FromBody] UpdateQuestionRequest questionRequest, CancellationToken cancellationToken)
        {
            return Ok("Questions Update");
        }

        [HttpDelete("{quetionId:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid questionId,
            CancellationToken cancellationToken)
        {
            return Ok("Questions Delete");
        }

        [HttpPut("{questionId:guid}/solution")]
        public async Task<IActionResult> SelectSolution([FromRoute] Guid questionId, [FromQuery] Guid answerId,
            CancellationToken cancellationToken)
        {
            return Ok("Select Solution");

        }

        [HttpPost("{questionId:guid}/answers")]
        public async Task<IActionResult> AddAnswer([FromRoute] Guid questionId, [FromBody] AddAnswerRequest answerRequest,
            CancellationToken cancellationToken)
        {
            return Ok("Questions Create");
        }
    }
}
