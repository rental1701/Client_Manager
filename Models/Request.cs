namespace Client_Manager.Models
{
    internal class Request
    {
        public int Id { get; set; }

        public int IdProduct { get; set; }

        public int IdClient { get; set; }

        public int NumRequest { get; set; }

        public int RequiredQuantity { get; set; }

        public DateTime PostingDate { get; set; }
    }
}
