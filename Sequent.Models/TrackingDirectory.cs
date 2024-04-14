using System.ComponentModel.DataAnnotations.Schema;

namespace Sequent.Models;

public class TrackingDirectory
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string Path { get; set; }
    public DateTime Added { get; set; }
}
