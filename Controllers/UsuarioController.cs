using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Logout() 
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult Listagem()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.CheckIfUserAdmin(this);

            return View(new UsuarioService().Listar());
        }
        
        public IActionResult Cadastro()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.CheckIfUserAdmin(this);

            return View();
        }

        public IActionResult Edicao(int Id)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.CheckIfUserAdmin(this);

            new UsuarioService().Listar(Id);

            return View(new UsuarioService().Listar(Id));
        }

        public IActionResult Exclusao(int Id)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.CheckIfUserAdmin(this);

            new UsuarioService().Excluir(Id);

            return RedirectToAction("Listagem");
        }

        [HttpPost]
        public IActionResult Edicao(Usuario u)
        {
            new UsuarioService().Editar(u);

            return RedirectToAction("Listagem");
        }

        [HttpPost]
        public IActionResult Cadastro(Usuario u)
        {
            u.Senha = Criptografo.TextoCriptografado(u.Senha);
            bool checkLogin = UsuarioService.loginRepetido(u);
            
            if (checkLogin == false)
            {
                ViewBag.LoginJaExiste = "Já existe um usuário com esse Login. Defina outro.";
                return View();
            }
            else
            {
                new UsuarioService().Incluir(u);
            }

            return RedirectToAction("Listagem");
        }
    }
}