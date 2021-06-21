using System.Collections.Generic;

namespace VetClinic.Core.Entities
{
    public class Position
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<EmployeePosition> EmployeePositions { get; set; }
    }
}
