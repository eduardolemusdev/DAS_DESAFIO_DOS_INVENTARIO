namespace DAS_DESAFIO_DOS_INVENTARIO.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly InventoryContext _inventoryContext;
        public UserRepository(InventoryContext inventoryContext)
        {
            _inventoryContext = inventoryContext;
        }
        public User GetByEmail(string email)
        {
            return _inventoryContext.Users.SingleOrDefault(u => u.Email == email);
        }
    }
}
