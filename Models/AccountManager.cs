using System.ComponentModel.DataAnnotations;


namespace Journal.Models;
public class AccountManager
{
    /*public string Id;
    [Required(ErrorMessage = "Login-Feld ist")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Пароль обязателен")]
    [DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "{0} должен быть минимум {2} символов.", MinimumLength = 8)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
    public string ConfirmPassword { get; set; }*/

    public string Id;
    [Required]
    [Display(Name = "Nutzername")]
    public string UserName {get;set;}
    [Required]
    [Display(Name = "Passwort")]
    public string Password {get;set;}
    [Compare("Password")]
    [Display(Name = "Geben das Passwort noch Mal ein")]
    public string ConfirmPassword {get;set;}
}