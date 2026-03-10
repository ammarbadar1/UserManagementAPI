using System.ComponentModel.DataAnnotations;

public class User
{
    [Required(ErrorMessage = "Id is required")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(20, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 20 characters")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must contain only alphabets with no leading or trailing spaces")]
    public string Name { get; set; }
}