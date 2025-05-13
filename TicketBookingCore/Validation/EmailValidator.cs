using System.Net.Mail;

namespace TicketBookingCore.Validation
{
    /// <summary>
    /// E-mail format validation utility.
    /// </summary>
    public static class EmailValidator
    {
        /// <summary>
        /// Returns true if <paramref name="email"/> is in a valid format.
        /// </summary>
        public static bool IsValid(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
