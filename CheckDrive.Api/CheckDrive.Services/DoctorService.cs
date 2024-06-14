using AutoMapper;
using CheckDrive.ApiContracts.Doctor;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Auth;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Services;

public class DoctorService : IDoctorService
{
    private readonly IMapper _mapper;
    private readonly CheckDriveDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public DoctorService(IMapper mapper, CheckDriveDbContext context, IPasswordHasher passwordHasher)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }

    public async Task<GetBaseResponse<DoctorDto>> GetDoctorsAsync(DoctorResourceParameters resourceParameters)
    {
        var query = GetQueryDoctorResParameters(resourceParameters);

        var doctors = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

        var doctorDtos = _mapper.Map<List<DoctorDto>>(doctors);

        var paginatedResult = new PaginatedList<DoctorDto>(doctorDtos, doctors.TotalCount, doctors.CurrentPage, doctors.PageSize);

        return paginatedResult.ToResponse();
    }

    public async Task<DoctorDto?> GetDoctorByIdAsync(int id)
    {
        var doctor = await _context.Doctors.Include(x => x.Account).FirstOrDefaultAsync(x => x.Id == id);

        var doctorDto = _mapper.Map<DoctorDto>(doctor);

        return doctorDto;
    }

    public async Task<DoctorDto> CreateDoctorAsync(DoctorForCreateDto doctorForCreate)
    {
        doctorForCreate.Password = _passwordHasher.Generate(doctorForCreate.Password);
        var accountEntity = _mapper.Map<Account>(doctorForCreate);
        await _context.Accounts.AddAsync(accountEntity);
        await _context.SaveChangesAsync();

        var doctor = new Doctor() { AccountId = accountEntity.Id };
        await _context.Doctors.AddAsync(doctor);
        await _context.SaveChangesAsync();

        var accountDto = _mapper.Map<DoctorDto>(accountEntity);

        return accountDto;
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

    private IQueryable<Doctor> GetQueryDoctorResParameters(
           DoctorResourceParameters resourceParameters)
    {
        var query = _context.Doctors.Include(x => x.Account).AsQueryable();

        if (resourceParameters.AccountId != 0 && resourceParameters.AccountId is not null)
        {
            query = query.Where(x => x.AccountId == resourceParameters.AccountId);
        }

        return query;
    }
}

