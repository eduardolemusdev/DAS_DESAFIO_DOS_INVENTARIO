using DAS_DESAFIO_DOS_INVENTARIO.Repositories;

namespace DAS_DESAFIO_DOS_INVENTARIO.Servicios
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Signin(string username, string password)
        {
            var user = _userRepository.GetByEmail(username);
            if (user == null)
            {
                throw new Exception("Invalid email or password");
            }
            return user;
        }
    }
}
