using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceCards.API.Contracts;
using SpaceCards.Domain.Interfaces;
using SpaceCards.Domain.Model;

namespace SpaceCards.API.Controllers
{
    [Authorize]
    public class GroupsController : BaseApiController
    {
        private readonly ILogger<GroupsController> _logger;
        private readonly IGroupsService _service;
        private readonly IMapper _mapper;

        public GroupsController(ILogger<GroupsController> logger, IGroupsService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
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
            var userId = UserId.Value;
            var result = await _service.Create(request.Name, userId);
            if (result.IsFailure)
            {
                _logger.LogError("{errors}", result.Error);
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
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
        public async Task<IActionResult> AddCard([FromQuery] int cardId, [FromRoute] int groupId)
        {
            var result = await _service.AddCard(cardId, groupId);
            if (result.IsFailure)
            {
                _logger.LogError("{errors}", result.Error);
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Get all groups.
        /// </summary>
        /// <returns>Groups.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Contracts.GetGroupResponse[]))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            var userId = UserId.Value;
            var groups = await _service.Get(userId);
            var groupsContract = _mapper.Map<Group[], Contracts.GetGroupResponse[]>(groups);
            return Ok(groupsContract);
        }

        /// <summary>
        /// Update group.
        /// </summary>
        /// <param name="groupId">Group id.</param>
        /// <param name="request">Group with new parameters.</param>
        /// <returns>Successful update group.</returns>
        [HttpPut("{groupId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] int groupId, [FromBody] UpdateGroupRequest request)
        {
            var result = await _service.Update(groupId, request.Name);
            if (result.IsFailure)
            {
                _logger.LogError("{errors}", result.Error);
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Delete group.
        /// </summary>
        /// <param name="groupId">Group id.</param>
        /// <returns>Successful delete group.</returns>
        [HttpDelete("{groupId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] int groupId)
        {
            var result = await _service.Delete(groupId);
            if (result.IsFailure)
            {
                _logger.LogError("{errors}", result.Error);
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Get group by id.
        /// </summary>
        /// <param name="groupId">Group id.</param>
        /// <returns>Group.</returns>
        [HttpGet("{groupId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Contracts.GetGroupResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetGroupById([FromRoute] int groupId)
        {
            var result = await _service.GetById(groupId);
            if (result.IsFailure)
            {
                _logger.LogError("{errors}", result.Error);
                return BadRequest(result.Error);
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Get random cards from all groups.
        /// </summary>
        /// <param name="countCards">Count cards.</param>
        /// <returns>Random cards.</returns>
        [HttpGet("randomCards")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Contracts.GetCardResponse[]))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRandomCards([FromQuery] int countCards)
        {
            var userId = UserId.Value;
            var result = await _service.GetRandomCards(countCards, userId);
            if (result.IsFailure)
            {
                _logger.LogError("{errors}", result.Error);
                return BadRequest(result.Error);
            }

            return Ok(_mapper.Map<Card[], Contracts.GetCardResponse[]>(result.Value));
        }
    }
}
