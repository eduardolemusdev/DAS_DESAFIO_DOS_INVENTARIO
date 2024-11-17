namespace DAS_DESAFIO_DOS_INVENTARIO.Repositories
{
    public interface IUserRepository
    {
        public User GetByEmail(string email);
    }
}
