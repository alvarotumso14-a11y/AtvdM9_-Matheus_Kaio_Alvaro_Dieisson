using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoTarefas.Data;
using ProjetoTarefas.Models;

namespace ProjetoTarefas.Controllers
{
   
    [Authorize]
    public class TarefasController : Controller
    {
        
        private readonly ITarefaRepositorio _tarefaRepositorio;

        public TarefasController(ITarefaRepositorio tarefaRepositorio)
        {
            _tarefaRepositorio = tarefaRepositorio;
        }

        
        private int ObterUsuarioIdLogado()
        {
            var idTexto = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(idTexto!);
        }

        
        public async Task<IActionResult> Index(bool? status)
        {
            int usuarioId = ObterUsuarioIdLogado();
            var tarefas = await _tarefaRepositorio.ObterTodasAsync(usuarioId, status);

            
            ViewBag.StatusFiltro = status;

            return View(tarefas);
        }

        
        public async Task<IActionResult> Details(int id)
        {
            int usuarioId = ObterUsuarioIdLogado();
            var tarefa = await _tarefaRepositorio.ObterPorIdAsync(id, usuarioId);

            if (tarefa == null)
            {
                return NotFound();
            }

            return View(tarefa);
        }

        
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo,Descricao,Data,Concluida")] Tarefa tarefa)
        {
           
            if (!ModelState.IsValid)
            {
                return View(tarefa);
            }

            
            tarefa.UsuarioId = ObterUsuarioIdLogado();

            await _tarefaRepositorio.AdicionarAsync(tarefa);
            return RedirectToAction(nameof(Index));
        }

       
        public async Task<IActionResult> Edit(int id)
        {
            int usuarioId = ObterUsuarioIdLogado();
            var tarefa = await _tarefaRepositorio.ObterPorIdAsync(id, usuarioId);

            if (tarefa == null)
            {
                return NotFound();
            }

            return View(tarefa);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,Data,Concluida")] Tarefa tarefaEditada)
        {
            if (id != tarefaEditada.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(tarefaEditada);
            }

            int usuarioId = ObterUsuarioIdLogado();

       
            var tarefaOriginal = await _tarefaRepositorio.ObterPorIdAsync(id, usuarioId);
            if (tarefaOriginal == null)
            {
                return NotFound();
            }

            
            tarefaOriginal.Titulo = tarefaEditada.Titulo;
            tarefaOriginal.Descricao = tarefaEditada.Descricao;
            tarefaOriginal.Data = tarefaEditada.Data;
            tarefaOriginal.Concluida = tarefaEditada.Concluida;

            await _tarefaRepositorio.AtualizarAsync(tarefaOriginal);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            int usuarioId = ObterUsuarioIdLogado();
            var tarefa = await _tarefaRepositorio.ObterPorIdAsync(id, usuarioId);

            if (tarefa == null)
            {
                return NotFound();
            }

            return View(tarefa);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmado(int id)
        {
            int usuarioId = ObterUsuarioIdLogado();
            var tarefa = await _tarefaRepositorio.ObterPorIdAsync(id, usuarioId);

            if (tarefa == null)
            {
                return NotFound();
            }

            await _tarefaRepositorio.RemoverAsync(tarefa);
            return RedirectToAction(nameof(Index));
        }
    }
}
