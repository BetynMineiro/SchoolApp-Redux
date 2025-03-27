using System.ComponentModel.DataAnnotations;
using System.Net;
using SchoolApp.Application.UserCases.Authentication.UpdatePassword.Dto;
using SchoolApp.Application.UserCases.User.Create.Dto;
using SchoolApp.Application.UserCases.User.Delete.Dto;
using SchoolApp.Application.UserCases.User.Update.Dto;
using SchoolApp.CrossCutting.Bus;
using SchoolApp.CrossCutting.RequestObjects;
using SchoolApp.CrossCutting.ResultObjects;
using SchoolApp.Domain.Enum.Management;
using SchoolApp.Domain.Messages.Queries.User;
using SchoolApp.Domain.Messages.Queries.User.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Api.Middleware;

namespace SchoolApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(IBus bus) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(Result<Pagination<UserQueryResultForList>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] ProfileType profileType, [FromQuery] string? filter, [FromQuery] PagedRequest pagedRequest,
            CancellationToken cancellationToken)
        {
            var result = await bus.SendQueryAsync(
                new GetFilteredUsersQuery(filter, profileType, pagedRequest),
                cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Result<UserQueryResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get([Required] string id,
            CancellationToken cancellationToken)
        {
            var result = await bus.SendQueryAsync(new GetUserByIdQuery { Id = id }, cancellationToken);

            return Ok(result);
        }
        [HttpGet("profile")]
        [ProducesResponseType(typeof(Result<UserByAuthIdQueryResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetUserProfile(CancellationToken cancellationToken)
        {
            var result = await bus.SendQueryAsync(new GetUserByUserAuthIdQuery(), cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result<CreateUserOutput>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CreateUserInput input,
            CancellationToken cancellationToken)
        {
            var result = await bus.SendCommandAsync(input, cancellationToken);

            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Result<UpdateUserOutput>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Put([Required] string id,
            [FromBody] UpdateUserInput input,
            CancellationToken cancellationToken)
        {
            input.Id = id;
            var result = await bus.SendCommandAsync(input, cancellationToken);

            return Ok(result);
        }
        
        [HttpPut("update-password")]
        [ProducesResponseType(typeof(Result<UpdatePasswordOutput>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> PutUpdatePassword([FromBody] UpdatePasswordInput input,
            CancellationToken cancellationToken)
        {
            var result = await bus.SendCommandAsync(input, cancellationToken);
            return Ok(result);
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result<DeleteUserOutput>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete([Required] string id,
            CancellationToken cancellationToken)
        {
            var result = await bus.SendCommandAsync(new DeleteUserInput() { Id = id }, cancellationToken);

            return Ok(result);
        }

    }
}