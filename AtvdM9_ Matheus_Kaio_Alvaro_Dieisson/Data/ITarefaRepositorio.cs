using ProjetoTarefas.Models;

namespace ProjetoTarefas.Data
{

    public interface ITarefaRepositorio
    {
    
        Task<List<Tarefa>> ObterTodasAsync(int usuarioId, bool? status);

        Task<Tarefa?> ObterPorIdAsync(int id, int usuarioId);

        Task AdicionarAsync(Tarefa tarefa);

        Task AtualizarAsync(Tarefa tarefa);

        Task RemoverAsync(Tarefa tarefa);
    }
}
