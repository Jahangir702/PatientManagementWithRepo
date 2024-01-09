using MasterDetails.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MasterDetails.ViewModels
{
    public class MedicineInputModel
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
    }
}

