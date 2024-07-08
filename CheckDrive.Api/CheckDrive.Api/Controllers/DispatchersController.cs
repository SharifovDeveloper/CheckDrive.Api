using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.Dispatcher;
using CheckDrive.ApiContracts.DispatcherReview;
using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/dispatchers")]
public class DispatchersController : Controller
{
    private readonly IDispatcherService _dispatcherService;
    private readonly IDispatcherReviewService _reviewService;
    private readonly IAccountService _accountService;

    public DispatchersController(IDispatcherService dispatcherService, IAccountService accountService, IDispatcherReviewService reviewService)
    {
        _dispatcherService = dispatcherService;
        _reviewService = reviewService;
        _accountService = accountService;
    }

    [Authorize(Policy = "AdminOrDispatcher")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DispatcherDto>>> GetDispatchersAsync(
        [FromQuery] DispatcherResourceParameters dispatcherResource)
    {
        var dispatchers = await _dispatcherService.GetDispatchersAsync(dispatcherResource);

        return Ok(dispatchers);
    }

    [Authorize(Policy = "AdminOrDispatcher")]
    [HttpGet("{id}", Name = "GetDispatcherById")]
    public async Task<ActionResult<DispatcherDto>> GetDispatcherByIdAsync(int id)
    {
        var dispatcher = await _dispatcherService.GetDispatcherByIdAsync(id);

        if (dispatcher is null)
            return NotFound($"Dispatcher with id: {id} does not exist.");

        return Ok(dispatcher);
    }

    [Authorize(Policy = "AdminOrDispatcher")]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] DispatcherForCreateDto dispatcherforCreateDto)
    {
        var createdDispatcher = await _dispatcherService.CreateDispatcherAsync(dispatcherforCreateDto);

        return CreatedAtAction("GetDispatcherById", new { id = createdDispatcher.Id }, createdDispatcher);
    }

    [Authorize(Policy = "AdminOrDispatcher")]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] AccountForUpdateDto dispatcherforUpdateDto)
    {
        if (id != dispatcherforUpdateDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {dispatcherforUpdateDto.Id}.");
        }

        var updateDispatcher = await _accountService.UpdateAccountAsync(dispatcherforUpdateDto);

        return Ok(updateDispatcher);
    }

    [Authorize(Policy = "AdminOrDispatcher")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _dispatcherService.DeleteDispatcherAsync(id);

        return NoContent();
    }

    [Authorize]
    [HttpGet("reviews")]
    public async Task<ActionResult<IEnumerable<DispatcherReviewDto>>> GetDispatcherReviewsAsync(
        [FromQuery] DispatcherReviewResourceParameters dispatcherReviewResource)
    {
        var dispatcherReviews = new GetBaseResponse<DispatcherReviewDto>();

        if (dispatcherReviewResource.RoleId == 5)
        {
            dispatcherReviews = await _reviewService.GetDispatcherReviewsForDispatcherAsync(dispatcherReviewResource);
        }
        else
        {
            dispatcherReviews = await _reviewService.GetDispatcherReviewsAsync(dispatcherReviewResource);
        }
        return Ok(dispatcherReviews);
    }

    [Authorize(Policy = "AdminOrDispatcher")]
    [HttpGet("review/{id}", Name = "GetDispatcherReviewById")]
    public async Task<ActionResult<DispatcherReviewDto>> GetDispatcherReviewByIdAsync(int id)
    {
        var dispatcherReview = await _reviewService.GetDispatcherReviewByIdAsync(id);

        if (dispatcherReview is null)
            return NotFound($"DispatcherReview with id: {id} does not exist.");

        return Ok(dispatcherReview);
    }

    [Authorize(Policy = "AdminOrDispatcher")]
    [HttpPost("review")]
    public async Task<ActionResult> PostAsync([FromBody] DispatcherReviewForCreateDto dispatcherReviewforCreateDto)
    {
        var createdDispatcherReview = await _reviewService.CreateDispatcherReviewAsync(dispatcherReviewforCreateDto);

        return CreatedAtAction("GetDispatcherReviewById", new { id = createdDispatcherReview.Id }, createdDispatcherReview);
    }

    [Authorize(Policy = "AdminOrDispatcher")]
    [HttpPut("review/{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] DispatcherReviewForUpdateDto dispatcherReviewforUpdateDto)
    {
        if (id != dispatcherReviewforUpdateDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {dispatcherReviewforUpdateDto.Id}.");
        }

        var updateDispatcherReview = await _reviewService.UpdateDispatcherReviewAsync(dispatcherReviewforUpdateDto);

        return Ok(updateDispatcherReview);
    }

    [Authorize(Policy = "AdminOrDispatcher")]
    [HttpDelete("review/{id}")]
    public async Task<ActionResult> DeleteAsyncReview(int id)
    {
        await _reviewService.DeleteDispatcherReviewAsync(id);

        return NoContent();
    }
}

