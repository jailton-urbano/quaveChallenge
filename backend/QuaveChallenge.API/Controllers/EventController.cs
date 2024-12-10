using Microsoft.AspNetCore.Mvc;
using QuaveChallenge.API.Models;
using QuaveChallenge.API.Services;

namespace QuaveChallenge.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("communities")]
        public async Task<IActionResult> GetCommunitiesAsync()
        {
            var communities = await _eventService.GetCommunitiesAsync();
            // Verifica se encontrou comunidades
            if (communities == null || !communities.Any())
            {
                return NotFound("No communities found.");
            }

            // Retorna as comunidades como resposta
            return Ok(communities);
        }

        [HttpGet("people/{communityId}")]
        public async Task<IActionResult> GetPeopleAsync(int communityId)
        {
            // TODO: Implement get people by community
            var people = await _eventService.GetPeopleByEventAsync(communityId);
            return Ok(people);
        }

        [HttpPost("check-in/{personId}")]
        public async Task<IActionResult> CheckInAsync(int personId)
        {
            // TODO: Implement check-in
            var checkin = await _eventService.CheckInPersonAsync(personId);
            return Ok(checkin);
        }

        [HttpPost("check-out/{personId}")]
        public async Task<IActionResult> CheckOutAsync(int personId)
        {
            // TODO: Implement check-out
            var checkout = await _eventService.CheckOutPersonAsync(personId);
            return Ok(checkout);
        }

        [HttpGet("summary/{communityId}")]
        public async Task<IActionResult> GetSummaryAsync(int communityId)
        {
            // TODO: Implement get summary
            var summary = await _eventService.GetEventSummaryAsync(communityId);
            return Ok(summary);
        }
    }
} 