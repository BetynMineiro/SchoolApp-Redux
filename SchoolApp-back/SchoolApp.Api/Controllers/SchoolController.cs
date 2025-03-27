using System.ComponentModel.DataAnnotations;
using System.Net;
using SchoolApp.Application.UserCases.School.Create.Dto;
using SchoolApp.Application.UserCases.School.Delete.Dto;
using SchoolApp.Application.UserCases.School.Update.Dto;
using SchoolApp.CrossCutting.Bus;
using SchoolApp.CrossCutting.RequestObjects;
using SchoolApp.CrossCutting.ResultObjects;
using SchoolApp.Domain.Messages.Queries.School;
using SchoolApp.Domain.Messages.Queries.School.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Api.Middleware;

namespace SchoolApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SchoolController(IBus bus) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(Result<Pagination<SchoolQueryListResult>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] PagedRequest pagedRequest,
            [FromQuery] string? filter, CancellationToken cancellationToken)
        {
            var result = await bus.SendQueryAsync(
                new GetFilteredSchoolQuery(filter, pagedRequest), cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Result<SchoolQueryResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get([Required] string id,
            CancellationToken cancellationToken)
        {
            var result = await bus.SendQueryAsync(new GetSchoolByIdQuery { Id = id }, cancellationToken);

            return Ok(result);
        }
        

        [HttpPost]
        [ProducesResponseType(typeof(Result<CreateSchoolOutput>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CreateSchoolInput input,
            CancellationToken cancellationToken)
        {
            var result = await bus.SendCommandAsync(input, cancellationToken);

            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Result<UpdateSchoolOutput>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Put([Required] string id,
            [FromBody] UpdateSchoolInput input,
            CancellationToken cancellationToken)
        {
            input.Id = id;
            var result = await bus.SendCommandAsync(input, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result<DeleteSchoolOutput>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete([Required] string id,
            CancellationToken cancellationToken)
        {
            var result = await bus.SendCommandAsync(new DeleteSchoolInput() { Id = id }, cancellationToken);

            return Ok(result);
        }
    }
}