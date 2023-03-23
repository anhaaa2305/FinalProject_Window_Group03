namespace WpfApp.Data.DAOs;

public interface IDAO<T> where T : class
{
	Task<int> AddAsync(T model);
	Task<IReadOnlyCollection<T>> GetAllAsync();
	Task<int> UpdateAsync(T model);
}
