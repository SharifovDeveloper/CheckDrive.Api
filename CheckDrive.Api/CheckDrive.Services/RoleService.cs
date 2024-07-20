using AutoMapper;
using CheckDrive.ApiContracts.Role;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Services;

public class RoleService : IRoleService
{
    private readonly IMapper _mapper;
    private readonly CheckDriveDbContext _context;

    public RoleService(IMapper mapper, CheckDriveDbContext context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetBaseResponse<RoleDto>> GetRolesAsync(RoleResourceParameters resourceParameters)
    {
        var query = GetQueryRoleResParameters(resourceParameters);

        var roles = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

        var roleDtos = _mapper.Map<List<RoleDto>>(roles);

        var paginatedResult = new PaginatedList<RoleDto>(roleDtos, roles.TotalCount, roles.CurrentPage, roles.PageSize);

        return paginatedResult.ToResponse();
    }

    public async Task<RoleDto?> GetRoleByIdAsync(int id)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);

        var roleDto = _mapper.Map<RoleDto>(role);

        return roleDto;
    }

    public async Task<RoleDto> CreateRoleAsync(RoleForCreateDto roleForCreate)
    {
        var roleEntity = _mapper.Map<Role>(roleForCreate);

        await _context.Roles.AddAsync(roleEntity);
        await _context.SaveChangesAsync();

        var roleDto = _mapper.Map<RoleDto>(roleEntity);

        return roleDto;
    }

    public async Task<RoleDto> UpdateRoleAsync(RoleForUpdateDto roleForUpdate)
    {
        var roleEntity = _mapper.Map<Role>(roleForUpdate);

        _context.Roles.Update(roleEntity);
        await _context.SaveChangesAsync();

        var roleDto = _mapper.Map<RoleDto>(roleEntity);

        return roleDto;
    }

    public async Task DeleteRoleAsync(int id)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);

        if (role is not null)
        {
            _context.Roles.Remove(role);
        }

        await _context.SaveChangesAsync();
    }

    private IQueryable<Role> GetQueryRoleResParameters(
   RoleResourceParameters resourceParameters)
    {
        var query = _context.Roles.AsQueryable();

        if (!string.IsNullOrEmpty(resourceParameters.OrderBy))
        {
            query = resourceParameters.OrderBy.ToLowerInvariant() switch
            {
                _ => query.OrderBy(x => x.Name),
            };
        }

        return query;
    }
}

