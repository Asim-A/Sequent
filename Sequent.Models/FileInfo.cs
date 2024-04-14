using System.ComponentModel.DataAnnotations.Schema;

namespace Sequent.Models;

public class FileInfo
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string Path { get; set; }
    public DateTime LastModified { get; set; }
    public long LastLineIndex { get; set; }
}
