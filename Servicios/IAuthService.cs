using DAS_DESAFIO_DOS_INVENTARIO.Repositories;

namespace DAS_DESAFIO_DOS_INVENTARIO.Servicios
{
    public interface IAuthService
    {
        public User Signin(string username, string password);
    }
}
