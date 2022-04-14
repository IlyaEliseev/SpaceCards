using Microsoft.AspNetCore.Mvc;
using SpaceCards.API.Contracts;
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

        public GroupsController(ILogger<GroupsController> logger)
        {
            _logger = logger;
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
            var group = Group.Create(request.Name);

            if (group.Result == null)
            {
                return Ok(group.Errors);
            }

            return Ok(group.Result);
        }

        /// <summary>
        /// Add card in group.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        /// <param name="cardWord">Word in card.</param>
        /// <returns>Word added card.</returns>
        [HttpPost("{groupName}/{cardWord}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCard(string groupName, string cardWord)
        {
            var result = Group.AddCard(groupName, cardWord);

            if (result.Word == null)
            {
                return Ok(result.Errors);
            }

            return Ok(result.Word);
        }

        /// <summary>
        /// Get all groups.
        /// </summary>
        /// <returns>Groups.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Group[]))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var groups = Group.GetAll();

            if (groups.Result == Array.Empty<Group>())
            {
                return Ok(groups.Errors);
            }

            return Ok(groups.Result);
        }

        /// <summary>
        /// Update group.
        /// </summary>
        /// <param name="groupName">Group search parametr.</param>
        /// <param name="request">Group with new parametrs.</param>
        /// <returns>Successful updat group.</returns>
        [HttpPost("Update/{groupName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Group[]))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string groupName, [FromBody] UpdateGroupRequest request)
        {
            var updateGroup = Group.Update(groupName, request.Name);

            if (updateGroup.Result == null)
            {
                return Ok(updateGroup.Errors);
            }

            return Ok(updateGroup.Result);
        }

        /// <summary>
        /// Delete group.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        /// <returns>Successful delete group.</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string groupName)
        {
            var deletedGroup = Group.Delete(groupName);

            if (!deletedGroup.Result)
            {
                return Ok(deletedGroup.Errors);
            }

            return Ok(deletedGroup.Result);
        }
    }
}
