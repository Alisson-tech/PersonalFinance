using FinanceSimplify.Context;
using FinanceSimplify.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FinanceSimplify.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ContextFinance _context;

    public GenericRepository(ContextFinance context)
    {
        _context = context;
    }

    public IQueryable<T> GetIqueryble()
    {
        return _context.Set<T>().AsQueryable();
    }

    public async Task<T> GetById(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);

        if (entity == null)
            throw new FinanceNotFoundException("Entity not found");

        return entity;
    }

    public async Task<T> Create(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> Update(int id, T entity)
    {
        entity.GetType().GetProperty("Id")?.SetValue(entity, id);
        var existingEntity = await GetById(id);

        _context.Entry(existingEntity).CurrentValues.SetValues(entity);

        await _context.SaveChangesAsync();

        return existingEntity;
    }

    public async Task SoftDelete(int id)
    {
        var entity = await GetById(id);

        _context.Entry(entity).Property("DateDeleted").CurrentValue = DateTime.UtcNow;
        _context.Entry(entity).State = EntityState.Modified;

        await _context.SaveChangesAsync();
    }

    public async Task HardDelete(int id)
    {
        var entity = await GetById(id);

        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}
