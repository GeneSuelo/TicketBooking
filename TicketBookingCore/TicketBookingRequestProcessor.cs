using System;

namespace TicketBookingCore
{
    public class TicketBookingRequestProcessor
    {
        //suggesting that _ticketBookingRepository will hold a reference to an object that implements this interface.
        private readonly ITicketBookingRepository _ticketBookingRepository;
        public TicketBookingRequestProcessor(ITicketBookingRepository ticketBookingRepository)
        {
            _ticketBookingRepository = ticketBookingRepository;
        }

        public TicketBookingResponse Book(TicketBookingRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }


            //-----------Steg 2: Validera e-postformatet-----------//
            /// Denna fanns i filen TicketBookingRequestProcessor.cs som kom med labbinstruktionerna.
            if (!IsValidEmail(request.Email))
            {
                return new TicketBookingResponse
                {
                    Success = false,
                    ErrorMessage = "Invalid email address."
                };
            }


            //----------Om giltigt, fortsätt som tidigare----------//
            _ticketBookingRepository.Save(Create<TicketBooking>(request));
            return Create<TicketBookingResponse>(request);
        }


        /// <summary>
        /// Denna hjälpare fanns i filen TicketBookingRequestProcessor.cs som kom med labbinstruktionerna.
        /// Returnerar true om strängen är en giltig e-postadress, annars false.
        /// </summary>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// This method creates a new instance of the specified type 
        /// and sets the properties from the request object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        private static T Create<T>(TicketBookingRequest request) where T : TicketBookingBase, new()
        {
            return new T
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };
        }
    }
}