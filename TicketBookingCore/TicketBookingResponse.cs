namespace TicketBookingCore
{
    public class TicketBookingResponse : TicketBookingBase
    {
        /// <summary>
        /// Sant om bokningen lyckades; falskt om det uppstod ett validerings- eller bearbetningsfel.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// När "Lyckades" är falskt, visas felmeddelandet (e.g. "Invalid email address.").
        /// </summary>

        // Förhindra CS8618: inga nullvärden i felmeddelandet
        public string ErrorMessage { get; set; } = string.Empty;

    }
}




