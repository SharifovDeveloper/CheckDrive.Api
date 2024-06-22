using CheckDrive.ApiContracts.Account;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

//[Authorize(Policy = "Admin")]
[ApiController]
[Route("api/accounts")]
public class AccountsController : Controller
{
    private readonly IAccountService _accountService;
    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccountsAsync(
        [FromQuery] AccountResourceParameters accountResource)
    {
        var accounts = await _accountService.GetAccountsAsync(accountResource);

        return Ok(accounts);
    }
    [HttpGet("{id}", Name = "GetAccountById")]
    public async Task<ActionResult<AccountDto>> GetAccountByIdAsync(int id)
    {
        var account = await _accountService.GetAccountByIdAsync(id);

        if (account is null)
            return NotFound($"Account with id: {id} does not exist.");

        return Ok(account);
    }
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] AccountForCreateDto forCreateDto)
    {
        var createdAccount = await _accountService.CreateAccountAsync(forCreateDto);

        return CreatedAtAction("GetAccountById", new { id = createdAccount.Id }, createdAccount);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] AccountForUpdateDto forUpdateDto)
    {
        if (id != forUpdateDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {forUpdateDto.Id}.");
        }

        var updateAccount = await _accountService.UpdateAccountAsync(forUpdateDto);

        return Ok(updateAccount);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _accountService.DeleteAccountAsync(id);

        return NoContent();
    }
}

