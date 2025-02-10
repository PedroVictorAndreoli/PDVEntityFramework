namespace ServerPDV.Interfaces
{
    public interface ICrud<T>
    {
        Task<IEnumerable<T>> SelecionarTodos();

        Task<T> Selecionar(int id);

        Task<T> Salvar(T entity);

        Task Delete(T entity);
    }
}
