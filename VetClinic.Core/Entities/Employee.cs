﻿namespace VetClinic.Core.Entities
{
    public class Employee : User
    {
        public string Address { get; set; }
        public int? EmployeePositionId { get; set; }
        public EmployeePosition EmployeePosition { get; set; }
    }
}
