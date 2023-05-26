using Microsoft.EntityFrameworkCore;
using ToDoList.App.Data.Context;
using ToDoList.App.Models;
using ToDoList.App.Repository.Interfaces;

namespace ToDoList.App.Repository.Base
{
	public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : Entity
	{
		public readonly DbSet<TEntity> DbSet;
		public readonly TaskContext Context;

		public RepositoryBase(TaskContext context)
		{
			DbSet = context.Set<TEntity>();
			Context = context;
		}

		public async Task<TEntity> Create(TEntity entity)
		{
			await Context.AddAsync(entity);
			await Context.SaveChangesAsync();

			return entity;
		}

		public async Task<IEnumerable<TEntity>> GetAll() => await DbSet.ToListAsync();

		public async Task<TEntity> GetById(int id) => await DbSet.FindAsync(id);

		public async Task Update(TEntity entity)
		{
			DbSet.Update(entity);
			await Context.SaveChangesAsync();
		}

		public async Task Delete(TEntity entity)
		{
			DbSet.Remove(entity);
			await Context.SaveChangesAsync();
		}

	}
}