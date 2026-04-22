namespace Prog_part_2.Models
{
    public class ContractFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long  FileSize { get; set; }

        public DateTime UploadedDate { get; set; }
       
        // foreign key
        public int ContractId { get; set; }

        public bool IsValid(string v1, int v2)
        {
            throw new NotImplementedException();
        }
    }
}
