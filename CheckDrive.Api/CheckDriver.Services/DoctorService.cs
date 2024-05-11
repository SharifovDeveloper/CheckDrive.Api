using AutoMapper;
using CheckDricer.Infrastructure.Persistence;
using CheckDrive.Domain.DTOs.Doctor;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDriver.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Services;

public class DoctorService : IDoctorService
{
    private readonly IMapper _mapper;
    private readonly CheckDriveDbContext _context;

    public DoctorService(IMapper mapper, CheckDriveDbContext context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetBaseResponse<DoctorDto>> GetDoctorsAsync(DoctorResourceParameters resourceParameters)
    {
        var query = _context.Doctors.AsQueryable();

        if (resourceParameters.AccountId != 0 && resourceParameters.AccountId is not null)
        {
            query = query.Where(x => x.AccountId == resourceParameters.AccountId);
        }

        if (!string.IsNullOrEmpty(resourceParameters.OrderBy))
        {
            query = resourceParameters.OrderBy.ToLowerInvariant() switch
            {
                _ => query.OrderBy(x => x.Id),
            };
        }

        var doctors = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

        var doctorDtos = _mapper.Map<List<DoctorDto>>(doctors);

        var paginatedResult = new PaginatedList<DoctorDto>(doctorDtos, doctors.TotalCount, doctors.CurrentPage, doctors.PageSize);

        return paginatedResult.ToResponse();
    }

    public async Task<DoctorDto?> GetDoctorByIdAsync(int id)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(x => x.Id == id);

        var doctorDto = _mapper.Map<DoctorDto>(doctor);

        return doctorDto;
    }

    public async Task<DoctorDto> CreateDoctorAsync(DoctorForCreateDto doctorForCreate)
    {
        var doctorEntity = _mapper.Map<Doctor>(doctorForCreate);

        await _context.Doctors.AddAsync(doctorEntity);
        await _context.SaveChangesAsync();

        var doctorDto = _mapper.Map<DoctorDto>(doctorEntity);

        return doctorDto;
    }

    public async Task<DoctorDto> UpdateDoctorAsync(DoctorForUpdateDto doctorForUpdate)
    {
        var doctorEntity = _mapper.Map<Doctor>(doctorForUpdate);

        _context.Doctors.Update(doctorEntity);
        await _context.SaveChangesAsync();

        var doctorDto = _mapper.Map<DoctorDto>(doctorEntity);

        return doctorDto;
    }

    public async Task DeleteDoctorAsync(int id)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(x => x.Id == id);

        if (doctor is not null)
        {
            _context.Doctors.Remove(doctor);
        }

        await _context.SaveChangesAsync();
    }
}

