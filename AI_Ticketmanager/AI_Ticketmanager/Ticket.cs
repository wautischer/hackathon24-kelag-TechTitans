public class Ticket
{
    public int id { get; set; }
    public string title { get; set; }
    public string tags { get; set; }
    public string description { get; set; }

    public DateTime created_at { get; set; }

    public string priority { get; set; }

    public string descriptionLong { get; set; }
}
