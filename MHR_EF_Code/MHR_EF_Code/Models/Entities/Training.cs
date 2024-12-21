﻿using System.ComponentModel.DataAnnotations;

namespace MHR_EF_Code.Models.Entities
{
    public class Training
    {
        [Key]
        public Guid TrainingID { get; set; }  // Đảm bảo kiểu dữ liệu là Guid
        public string TrainingName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<EmployeeTraining> EmployeeTrainings { get; set; }
    }
}