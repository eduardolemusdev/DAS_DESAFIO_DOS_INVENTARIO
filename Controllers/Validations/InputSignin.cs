using System.ComponentModel.DataAnnotations;


namespace DAS_DESAFIO_DOS_INVENTARIO.Controllers.Validations
{

    public class InputSignin
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }

}
