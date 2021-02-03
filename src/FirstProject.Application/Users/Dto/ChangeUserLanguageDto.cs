using System.ComponentModel.DataAnnotations;

namespace FirstProject.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}