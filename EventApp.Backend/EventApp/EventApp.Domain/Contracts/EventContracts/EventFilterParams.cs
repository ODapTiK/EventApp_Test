namespace EventApp
{
    public class EventFilterParams
    {
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Venue { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int Page { get; set; }
    }
}
