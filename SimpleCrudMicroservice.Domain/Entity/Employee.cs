using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleCrudMicroservice.Domain.Entity
{
    [Table("tbl_employee")]
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_employee")]
        public long Id { get; set; }
        
        [Column("txt_name")]
        public string Name { get; set; }
    }
}