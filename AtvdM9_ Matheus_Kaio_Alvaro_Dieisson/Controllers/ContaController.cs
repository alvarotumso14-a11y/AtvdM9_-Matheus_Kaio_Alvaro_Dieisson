using AtvdM9__Matheus_Kaio_Alvaro_Dieisson.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoTarefas.Data;
using ProjetoTarefas.Models;
using System.Security.Claims;

namespace ProjetoTarefas.Controllers
{
    public class ContaController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly PasswordHasher<Usuario> _passwordHasher = new();

        public ContaController(ApplicationDbContext context)
        {
            _context = context;
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Cadastro()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastro(CadastroViewModel modelo)
        {

            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            bool jaExiste = await _context.Usuarios.AnyAsync(u => u.Email == modelo.Email);
            if (jaExiste)
            {
                ModelState.AddModelError(string.Empty, "Já existe uma conta com esse e-mail.");
                return View(modelo);
            }

            var novoUsuario = new Usuario
            {
                Nome = modelo.Nome,
                Email = modelo.Email
            };

            novoUsuario.SenhaHash = _passwordHasher.HashPassword(novoUsuario, modelo.Senha);

            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();

            TempData["Mensagem"] = "Cadastro realizado! Faça login para continuar.";
            return RedirectToAction("Login");
        }



        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == modelo.Email);

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
                return View(modelo);
            }


            var resultado = _passwordHasher.VerifyHashedPassword(
                usuario, usuario.SenhaHash, modelo.Senha);

            if (resultado == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
                return View(modelo);
            }

  
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);


            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Tarefas");
        }



        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
