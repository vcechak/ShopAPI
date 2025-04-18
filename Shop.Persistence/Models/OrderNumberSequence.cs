using System.ComponentModel.DataAnnotations;

namespace Shop.DataAccess.Models
{
    public class OrderNumberSequence
    {
        [Key]
        public DateTime Date { get; set; }

        public int Counter { get; set; }
    }
}
