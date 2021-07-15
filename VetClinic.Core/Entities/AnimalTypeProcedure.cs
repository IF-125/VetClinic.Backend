namespace VetClinic.Core.Entities
{
    public class AnimalTypeProcedure
    {
        public int AnimalTypeId { get; set; }
        public AnimalType AnimalType { get; set; }
        public int ProcedureId { get; set; }
        public Procedure Procedure { get; set; }
    }
}
