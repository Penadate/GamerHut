using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Nickname { get; set; }
        public string Password { get; set; }
        public string Contact { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}