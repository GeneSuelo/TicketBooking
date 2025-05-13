using System;
using TicketBookingCore.Validation;  // hämta den nya validatorn

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
            /// Validation error path:
            if (!EmailValidator.IsValid(request.Email)) // Ersätt det inline helper call
            {
                return new TicketBookingResponse
                {
                    Success = false,
                    ErrorMessage = "Invalid email address."
                };
            }


            //----------Om giltigt, fortsätt som tidigare----------//
            // Spara bokning
            _ticketBookingRepository.Save(Create<TicketBooking>(request));

            // Skapa och returnera ett lyckat svar
            var response = Create<TicketBookingResponse>(request);
            response.Success = true;
            return response;
        }

        // NOTE: private IsValidEmail har tagits bort

  
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