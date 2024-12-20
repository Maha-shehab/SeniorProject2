﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

using PadelChampAPI.Data;
using PadelChampAPI.Interfaces;

namespace PadelChampAPI.Repositories;

public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : class
{
    public async Task Create(T entity) => await context.Set<T>().AddAsync(entity);

    public void Delete(T entity) => context.Set<T>().Remove(entity);

    public async Task<IEnumerable<T>> GetAll() => await context.Set<T>().ToListAsync();

    public async Task<T?> GetOne(int id) => await context.Set<T>().FindAsync(id);

    public async Task SaveChangesAsync()
    => await context.SaveChangesAsync();

    public void Update(T entity)
    => context.Set<T>().Update(entity);

    public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate) =>
    await context.Set<T>().Where(predicate).ToListAsync();

    public async Task<T?> FindOne(Expression<Func<T, bool>> predicate)
    => await context.Set<T>().FirstOrDefaultAsync(predicate);

    public async Task<IEnumerable<T>> GetAllAndPopulateAsync(Expression<Func<T, object>> includeExpression) =>
    await context.Set<T>().Include(includeExpression).ToListAsync();

    public async Task<T?> FindOneAndPopulateAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> includeExpression)
    => await context.Set<T>().Include(includeExpression).FirstOrDefaultAsync(predicate);
    public async Task<IEnumerable<T?>> FindAndPopulateAsync(Expression<Func<T,bool>>predicate, Expression<Func<T, object>> includeExpression)
    => await context.Set<T>().Include(includeExpression).Where(predicate).ToListAsync();
}
