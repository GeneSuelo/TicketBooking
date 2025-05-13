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
        public string ErrorMessage { get; set; }

    }
}