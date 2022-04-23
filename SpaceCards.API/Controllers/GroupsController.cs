using Microsoft.AspNetCore.Mvc;
using SpaceCards.API.Contracts;
using SpaceCards.DataAccess.Postgre;
using SpaceCards.Domain;
using System.Net.Mime;

namespace SpaceCards.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class GroupsController : ControllerBase
    {
        private readonly ILogger<GroupsController> _logger;
        private readonly IGroupsService _service;

        public GroupsController(ILogger<GroupsController> logger, IGroupsService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Creates new group for cards.
        /// </summary>
        /// <remarks>Test message.</remarks>
        /// <param name="request">New group.</param>
        /// <returns>Created group id.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateGroupRequest request)
        {
            var (groupId, errors) = await _service.Create(request.Name);

            if (errors.Any() || groupId == default)
            {
                _logger.LogError("{errors}", errors);
                return BadRequest(errors);
            }

            return Ok(groupId);
        }

        /// <summary>
        /// Add card in group.
        /// </summary>
        /// <param name="cardId">Card id.</param>
        /// <param name="groupId">Group id.</param>
        /// <returns>Successful added card.</returns>
        [HttpPost("{groupId:int}/cards")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCard(int cardId, int groupId)
        {
            var (result, errors) = await _service.AddCard(cardId, groupId);

            if (errors.Any() || !result)
            {
                _logger.LogError("{errors}", errors);
                return BadRequest(errors);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get all groups.
        /// </summary>
        /// <returns>Groups.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Group[]))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            var groups = await _service.Get();
            return Ok(groups);
        }

        /// <summary>
        /// Update group.
        /// </summary>
        /// <param name="groupId">Group id.</param>
        /// <param name="request">Group with new parametrs.</param>
        /// <returns>Successful updat group.</returns>
        [HttpPut("{groupId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int groupId, [FromBody] UpdateGroupRequest request)
        {
            var (result, errors) = await _service.Update(groupId, request.Name);

            if (errors.Any() || !result)
            {
                _logger.LogError("{errors}", errors);
                return BadRequest(errors);
            }

            return Ok(result);
        }

        /// <summary>
        /// Delete group.
        /// </summary>
        /// <param name="groupId">Group id.</param>
        /// <returns>Successful delete group.</returns>
        [HttpDelete("{groupId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int groupId)
        {
            var (result, errors) = await _service.Delete(groupId);

            if (errors.Any() || !result)
            {
                _logger.LogError("{errors}", errors);
                return BadRequest(errors);
            }

            return Ok(result);
        }
    }
}
