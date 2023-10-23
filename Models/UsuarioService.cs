using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Models
{
    public class UsuarioService
    {
        public List<Usuario> Listar()
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Usuarios.ToList();
            }
        }

        public Usuario Listar(int id)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Usuarios.Find(id);
            }
        }

        public void Incluir(Usuario u)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                bc.Add(u);
                
                bc.SaveChanges();
            }
        }

        public static bool loginRepetido(Usuario uL)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                IQueryable<Usuario> loginEncontrado = bc.Usuarios.Where(u => u.Login == uL.Login);

                List<Usuario> listLoginEncontrado = loginEncontrado.ToList();

                if (listLoginEncontrado.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public void Editar(Usuario u)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                Usuario userEd = bc.Usuarios.Find(u.Id);
                userEd.Login = u.Login;
                userEd.Nome = u.Nome;

                if (userEd.Senha != u.Senha)
                {
                    userEd.Senha = Criptografo.TextoCriptografado(u.Senha);
                }
                else
                {
                    userEd.Senha = u.Senha;
                }
                
                userEd.Tipo = u.Tipo;

                bc.SaveChanges();
            }
        }

        public void Excluir(int id)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                Usuario userEx = bc.Usuarios.Find(id);
                bc.Usuarios.Remove(userEx);
                
                bc.SaveChanges();
            }
        }
    }
}