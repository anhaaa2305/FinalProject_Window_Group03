using WpfApp1.Models;

namespace WpfApp1.DAOs;

public interface IBaseDAO<T> where T : class
{
	Task<bool> EnsureTableAsync();
	Task<int> AddAsync(T model);
	Task<T?> GetByIdAsync(T model);
	Task<IReadOnlyCollection<T>> GetAllAsync();
	Task<int> UpdateAsync(T model);
	Task<int> DeleteByIdAsync(T model);
}
