namespace Prog_part_2.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }

        public string Region { get; set; }

        public int ContactNumber { get; set; }

        public string EmailAddress { get; set; }
    }
}
