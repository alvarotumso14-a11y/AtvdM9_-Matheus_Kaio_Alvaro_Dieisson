using Microsoft.EntityFrameworkCore;
using ProjetoTarefas.Models;

namespace ProjetoTarefas.Data
{
    // Essa é a implementação de verdade do contrato ITarefaRepositorio.
    // É o ÚNICO lugar do projeto (fora o DbContext) que fala com a tabela Tarefas.
    public class TarefaRepositorio : ITarefaRepositorio
    {
        private readonly ApplicationDbContext _context;

        // Injeção de Dependência: o repositório recebe o DbContext pronto
        // pelo construtor. Ele nunca faz "new ApplicationDbContext()".
        public TarefaRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tarefa>> ObterTodasAsync(int usuarioId, bool? status)
        {
            
            var query = _context.Tarefas.Where(t => t.UsuarioId == usuarioId);

            
            if (status.HasValue)
            {
                query = query.Where(t => t.Concluida == status.Value);
            }

            return await query.OrderBy(t => t.Data).ToListAsync();
        }

        public async Task<Tarefa?> ObterPorIdAsync(int id, int usuarioId)
        {

            return await _context.Tarefas
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == usuarioId);
        }

        public async Task AdicionarAsync(Tarefa tarefa)
        {
            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Tarefa tarefa)
        {
            _context.Tarefas.Update(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(Tarefa tarefa)
        {
            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();
        }
    }
}
