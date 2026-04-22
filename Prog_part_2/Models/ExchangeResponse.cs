namespace Prog_part_2.Models
{
    public class ExchangeResponse
    {
        public string result { get; set; }
        public string base_code { get; set; }
        public string target_code { get; set; }
        public double conversion_rate { get; set; }
        public double conversion_result { get; set; }
    }
}
