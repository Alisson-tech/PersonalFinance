﻿namespace FinanceSimplify.Repositories;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> GetIqueryble();
    Task<T> GetById(int id);
    Task<T> Create(T entity);
    Task<T> Update(int id, T entity);
    Task SoftDelete(int id);
    Task HardDelete(int id);
}
