﻿using MasterDetails.Models;
using System.ComponentModel.DataAnnotations;

namespace MasterDetails.ViewModels
{
    public class PatientEditModel
    {
        public int PatientId { get; set; }
        [Required, StringLength(50)]
        public string PatientName { get; set; } = default!;
        [Required]
        public int Age { get; set; }
        [Required, EnumDataType(typeof(Gender))]
        public Gender? Gender { get; set; } = default!;
        public virtual List<Medicine> Medicines { get; set; } = new List<Medicine>();
    }
}
