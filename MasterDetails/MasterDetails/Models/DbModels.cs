using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MasterDetails.Models
{
    public enum Gender { Male = 1, Female, Others }
    public class Patient
    {
        public int PatientId { get; set; }
        [Required, StringLength(50)]
        public string PatientName { get; set; } = default!;
        [Required]
        public int Age { get; set; }
        [Required, EnumDataType(typeof(Gender))]
        public Gender? Gender { get; set; } = default!;
        public virtual ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
    }
    public class Medicine
    {
        public int MedicineId { get; set; }
        [Required, StringLength(50)]
        public string MedicineName { get; set; } = default!;
        [Required, StringLength(100)]
        public string Quantity { get; set; } = default!;
        [Required, StringLength(150)]
        public string Advice { get; set; } = default!;
        [Required, ForeignKey("Patient")]
        public int PatientId { get; set; }
        public virtual Patient? Patient { get; set; }
    }
    public class PatientDbContext : DbContext
    {
        public PatientDbContext(DbContextOptions<PatientDbContext> options) :base (options){ }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
    }
}