namespace ChatkaReservation.Models
{
    public class CalendarModel
    {
        public int CottageID { get; set; }
        public List<CalendarEvent> Events { get; set; }
    }

    public class CalendarEvent
    {
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

}
