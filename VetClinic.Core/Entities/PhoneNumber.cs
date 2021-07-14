namespace VetClinic.Core.Entities
{
    public class PhoneNumber
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string ClientId { get; set; }
        public Client Client { get; set; }
    }
}
