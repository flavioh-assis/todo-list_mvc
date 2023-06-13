using Microsoft.EntityFrameworkCore;
using ToDoList.App.Data.Context;
using ToDoList.App.Models;
using ToDoList.App.Repository.Interfaces;

namespace ToDoList.App.Repository.Base
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : Entity
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly TaskContext _context;

        public RepositoryBase(TaskContext context)
        {
            _dbSet = context.Set<TEntity>();
            _context = context;
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task Update(TEntity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}