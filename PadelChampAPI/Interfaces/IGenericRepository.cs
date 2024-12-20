﻿using System.Linq.Expressions;


namespace PadelChampAPI.Interfaces;
public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Gets a list of all entities in the database.
    /// </summary>
    Task<IEnumerable<T>> GetAll();

    /// <summary>
    /// Gets a single entity by its ID. Returns null if not found.
    /// </summary>
    Task<T?> GetOne(int id);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    void Update(T entity);

    /// <summary>
    /// Adds a new entity to the database.
    /// </summary>
    Task Create(T entity);

    /// <summary>
    /// Removes an entity from the database.
    /// </summary>
    void Delete(T entity);

    /// <summary>
    /// Saves all changes made to the database.
    /// </summary>
    Task SaveChangesAsync();

    /// <summary>
    /// Gets a list of entities that match a specific condition.
    /// </summary>
    Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Gets a single entity that matches a specific condition. Returns null if not found.
    /// </summary>
    Task<T?> FindOne(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Gets all entities and includes related data based on the provided expression.
    /// </summary>
    Task<IEnumerable<T>> GetAllAndPopulateAsync(Expression<Func<T, object>> includeExpression);

    /// <summary>
    /// Finds a single entity that matches a specific condition and includes related data based on the provided expression.
    /// </summary>
    Task<T?> FindOneAndPopulateAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> includeExpression);
    Task<IEnumerable<T?>> FindAndPopulateAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> includeExpression);
}