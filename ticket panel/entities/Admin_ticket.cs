namespace ticket_panel.entities
{
    public class Admin_ticket
    {
        public int Id { get; set; }
        public int UserTicketId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
