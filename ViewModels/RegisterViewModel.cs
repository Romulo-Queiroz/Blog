using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Este campo é obrigatório")]
    [MaxLength(20, ErrorMessage = "Este campo deve conter entre 3 e 20 caracteres")]
    [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 20 caracteres")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Este campo é obrigatório")]
    [EmailAddress(ErrorMessage = "Este campo deve ser um e-mail válido")]
    public string Email { get; set; }

    // [Required(ErrorMessage = "Este campo é obrigatório")]
    // [MaxLength(20, ErrorMessage = "Este campo deve conter entre 3 e 20 caracteres")]
    // [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 20 caracteres")]
    // public string Password { get; set; }

    // [Required(ErrorMessage = "Este campo é obrigatório")]
    // [Compare("Password", ErrorMessage = "As senhas não conferem")]
    // public string ConfirmPassword { get; set; }
}