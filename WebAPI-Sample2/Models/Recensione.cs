namespace WebAPI_Sample2.Models
{
    public class Recensione
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? UserName { get; set; }

        public string? Testo { get; set; }

        public DateTime? DataRec { get; set; }

        public int? Stelle { get; set; }

    }
}
