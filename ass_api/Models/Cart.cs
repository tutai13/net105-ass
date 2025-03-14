using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace ass_api.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }

       
        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }

        [Required]
        public int SoLuong { get; set; }


        [JsonIgnore]

        public  ApplicationUser User { get; set; }
        [JsonIgnore]
        public virtual Product Product { get; set; }
    }
}
