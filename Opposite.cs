using System.ComponentModel.DataAnnotations.Schema;

namespace TearApi
{
    public class Opposite
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OppositeId { get; set; }
        public string? Description { get; set; }
        public int Intensity { get; set; }
    }
}
