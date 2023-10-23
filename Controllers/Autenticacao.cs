using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Biblioteca.Models;


namespace Biblioteca.Controllers
{
    public class Autenticacao
    {
        public static void CheckLogin(Controller controller)
        {   
            if(string.IsNullOrEmpty(controller.HttpContext.Session.GetString("Login")))
            {
                controller.Request.HttpContext.Response.Redirect("/Home/Login");
            }
        }
        
        public static void CheckIfUserAdmin(Controller controller)
        {
            if (controller.HttpContext.Session.GetInt32("Tipo") != Usuario.ADMIN)
            {
                controller.Request.HttpContext.Response.Redirect("/Usuario/Admin");
            }
        }

        public static bool CheckLoginSenha (string login, string senha, Controller controller)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                CheckUserAdminExists(bc);

                senha = Criptografo.TextoCriptografado(senha);
                IQueryable<Usuario> userEncontrado = bc.Usuarios.Where(u => u.Login == login && u.Senha == senha);

                List<Usuario> listUserEncontrado = userEncontrado.ToList();

                if (listUserEncontrado.Count == 0)
                {
                    return false;
                }
                else
                {
                    controller.HttpContext.Session.SetString("Login", listUserEncontrado[0].Login);
                    controller.HttpContext.Session.SetString("Nome", listUserEncontrado[0].Nome);
                    controller.HttpContext.Session.SetInt32("Tipo", listUserEncontrado[0].Tipo);

                    return true;
                }
            }
        }

        public static void CheckUserAdminExists(BibliotecaContext bc)
        {
            IQueryable<Usuario> userEncontrado = bc.Usuarios.Where(u => u.Login == "admin");

            List<Usuario> listUserEncontrado = userEncontrado.ToList();

            if (listUserEncontrado.Count==0)
            {
                Usuario admin = new Usuario();
                admin.Login = "Admin";
                admin.Senha = Criptografo.TextoCriptografado("123");
                admin.Nome = "Administrador";
                admin.Tipo = Usuario.ADMIN;

                bc.Add(admin);
                bc.SaveChanges();
            }
        }
    }
}