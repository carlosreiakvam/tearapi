using System.ComponentModel.DataAnnotations.Schema;

namespace TearApi
{
    /// <summary>
    /// This class represents a model for a User.
    /// </summary>
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public int OppositeId { get; set; } // should be a foreign key
        public bool IsAgreeing { get; set; }
        public Opposite? Opposite { get; set; } // Navigation property
    }
}
