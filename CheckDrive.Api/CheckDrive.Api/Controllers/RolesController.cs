using CheckDrive.Domain.DTOs.Role;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RolesController : Controller
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetRolesAsync(
        [FromQuery] RoleResourceParameters roleResource)
    {
        var roles = await _roleService.GetRolesAsync(roleResource);

        return Ok(roles);
    }

    [HttpGet("{id}", Name = "GetRoleByIdAsync")]
    public async Task<ActionResult<RoleDto>> GetRoleByIdAsync(int id)
    {
        var role = await _roleService.GetRoleByIdAsync(id);

        if (role is null)
            return NotFound($"Role with id: {id} does not exist.");

        return Ok(role);
    }
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] RoleForCreateDto forCreateDto)
    {
        var createdRole = await _roleService.CreateRoleAsync(forCreateDto);

        return CreatedAtAction(nameof(GetRoleByIdAsync), new { createdRole.Id }, createdRole);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] RoleForUpdateDto forUpdateDto)
    {
        if (id != forUpdateDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {forUpdateDto.Id}.");
        }

        var updateRole = await _roleService.UpdateRoleAsync(forUpdateDto);

        return Ok(updateRole);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _roleService.DeleteRoleAsync(id);

        return NoContent();
    }
}
