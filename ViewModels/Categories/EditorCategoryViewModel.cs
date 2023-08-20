using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Categories;

public class EditorCategoryViewModel
{
    [Required(ErrorMessage = "O Nome é obrigatório")]
    [StringLength(40, ErrorMessage = "Este campo deve conter entre 3 e 40 caracteres", MinimumLength = 3)]
    public string Name { get; set; }
    [Required(ErrorMessage = "o slug é obrigatório")]
    public string Slug { get; set; }
}