using ToDoList.App.Models;

namespace ToDoList.App.Repository.Interfaces
{
	public interface IRepositoryBase<TEntity> where TEntity : Entity
	{
		Task<TEntity> Create(TEntity entity);
		Task<IEnumerable<TEntity>> GetAll();
		Task<TEntity> GetById(int id);
		Task Update(TEntity entity);
		Task Delete(TEntity entity);
	}
}