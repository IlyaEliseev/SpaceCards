using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceCards.API.Contracts;
using SpaceCards.Domain.Interfaces;
using SpaceCards.Domain.Model;

namespace SpaceCards.API.Controllers
{
    [Authorize]
    public class StatisticsController : BaseApiController
    {
        private readonly ILogger _logger;
        private readonly IStatisticsService _service;
        private readonly IMapper _mapper;

        public StatisticsController(
            ILogger<StatisticsController> logger,
            IStatisticsService service,
            IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Collecting card guessing statistics.
        /// </summary>
        /// <param name="cardId">Card id.</param>
        /// <param name="successValue">Result of guessing - 0 or 1.</param>
        /// <returns>Successful result.</returns>
        [HttpPost("{cardId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CollectCardStatistics(
            [FromRoute] int cardId,
            [FromQuery] int successValue)
        {
            var userId = UserId.Value;
            var (result, errors) = await _service.CollectCardStatistics(cardId, successValue, userId);

            if (errors.Any() && result == false)
            {
                _logger.LogError("{errors}", errors);
                return BadRequest(errors);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get cards guessing statistics.
        /// </summary>
        /// <returns>Cards guessing statistics.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCardGusessingStatisticsResponse[]))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStatistics()
        {
            var userId = UserId.Value;
            var cardGuessingStatistics = await _service.GetGuessingCardStatistics(userId);

            var cardGuessingStatisticsContract = _mapper.Map<CardGuessingStatistics[],
                Contracts.GetCardGusessingStatisticsResponse[]>(cardGuessingStatistics);

            return Ok(cardGuessingStatisticsContract);
        }
    }
}
