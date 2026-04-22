using System.Diagnostics.Contracts;

namespace Prog_part_2.Models
{
    public class ServiceRequests
    {
        public int Id { get; set; }
        public string? ServiceStatus { get; set; }
     public double ContractCost { get; set; }

        public string? RequestTypes { get; set; }

        public DateOnly RequestDate { get; set; }   
   
        public string? RequestDescription { get; set; }

       public int ContractId { get; set; }
        public Contracts? Contract { get; set; }
    }
}
