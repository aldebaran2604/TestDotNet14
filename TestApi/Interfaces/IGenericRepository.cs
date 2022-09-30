namespace TestApi.Interfaces;

public interface IGenericRepository<T> where T : class
{
    IEnumerable<T> GetAll();

    T GetById(int id);

    void Insert(T genericObject);

    void Update(T genericObject);

    void Delete(int id);

    void Save();
}