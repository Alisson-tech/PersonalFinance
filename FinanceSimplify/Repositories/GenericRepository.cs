using FinanceSimplify.Context;
using Microsoft.EntityFrameworkCore;

namespace FinanceSimplify.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ContextFinance _context;

    public GenericRepository(ContextFinance context)
    {
        _context = context;
    }

    public IQueryable<T> GetAll()
    {
        return _context.Set<T>().AsQueryable();
    }

    public async Task<T?> GetById(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<T> Create(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task SoftDelete(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);

        if (entity == null)
        {
            throw new ArgumentException("Entity not found.");
        }

        _context.Entry(entity).Property("DateDeleted").CurrentValue = DateTime.UtcNow;
        _context.Entry(entity).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    public async Task HardDelete(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);

        if (entity == null)
        {
            throw new ArgumentException("Entity not found.");
        }

        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}
