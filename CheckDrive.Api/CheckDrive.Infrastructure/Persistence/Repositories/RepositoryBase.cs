using CheckDrive.Domain.Common;
using CheckDrive.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Infrastructure.Persistence.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
    {
        protected readonly CheckDriveDbContext _context;
        public RepositoryBase(CheckDriveDbContext context)
        {
            _context = context;
        }
        public async Task<T> CreateAsync(T entity)
        {
            var createdEntity = await _context.Set<T>().AddAsync(entity);

            return createdEntity.Entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await FindByIdAsync(id);

            _context.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>> FindAllAsync()
        {
            var entities = await _context.Set<T>()
                .AsNoTracking()
                .ToListAsync();

            return entities;
        }

        public async Task<T> FindByIdAsync(int id)
        {
            var entity = await _context.Set<T>()
                .FindAsync(id);

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
