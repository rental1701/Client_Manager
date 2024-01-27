namespace Client_Manager.Models
{
    internal class Client
    {
        public int Id { get; set; }

        public string? NameOfCompany { get; set; }

        public string? Address { get; set; }

        public string? ContactPerson { get; set; }



        public override string ToString()
        {
            return $"Организация:\"{NameOfCompany}\" - Адрес: {Address}\n" +
                $"Контактное лицо: {ContactPerson}";
        }
    }
}
