using System.ComponentModel.DataAnnotations;

namespace Journal.Models;
    public enum NoteType
    {
        Blog,
        Article
    }
public class Note
{
    public int Id { get; set; }
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;
    [MaxLength(300)]
    [Display(Name = "Einführung")]
    public string Introduce { get; set; } = string.Empty;
    [Display(Name = "Inhalt")]
    public string Content { get; set; } = string.Empty;
    [Display(Name = "Typ des Notizes")]
    public NoteType Type { get; set; }
    [Display(Name = "Veröffentlicht")]
    public bool IsPublished { get; set; }
    [Display(Name = "Erscheinungsdatum")]
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    public DateTime? ReleaseDate { get; set; }
    [Display(Name = "Vorschaubild")]
    public string ThumbnailUrl {get;set;} = string.Empty;
    public void NewDate()
    {
        ReleaseDate = DateTime.UtcNow;
    }


}