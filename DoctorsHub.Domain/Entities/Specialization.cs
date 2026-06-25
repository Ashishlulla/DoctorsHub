using System;
using System.Collections.Generic;
using System.Text;

namespace DoctorsHub.Domain.Entities
{
    public class Specialization
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        //Nvigation Property
        public List<Doctor> Doctors { get; set; } = new();
    }
}
