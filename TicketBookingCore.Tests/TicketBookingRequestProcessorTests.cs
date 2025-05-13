using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Moq;

namespace TicketBookingCore.Tests
{
    public class TicketBookingRequestProcessorTests
    {
        private readonly TicketBookingRequest _request;
        private readonly Mock<ITicketBookingRepository> _ticketBookingRepositoryMock;
        private readonly TicketBookingRequestProcessor _processor;
        public TicketBookingRequestProcessorTests()
        {
            _request = new TicketBookingRequest
            {
                FirstName = "Nevena",
                LastName = "Kicanovic",
                Email = "nevena@gmail.com"
            };
            _ticketBookingRepositoryMock = new Mock<ITicketBookingRepository>();
            _processor = new TicketBookingRequestProcessor(_ticketBookingRepositoryMock.Object);
        }

        [Fact]
        public void ShouldReturnTicketBookningResultWithRequestValues()
        {
            // Arrange

            var request = new TicketBookingRequest
            {
                FirstName = "Nevena",
                LastName = "Kicanovic",
                Email = "nevena@gmail.com"
            };

            // Act
            TicketBookingResponse response = _processor.Book(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(request.FirstName, response.FirstName);
            Assert.Equal(request.LastName, response.LastName);
            Assert.Equal(request.Email, response.Email);
        }
        [Fact]
        public void ShouldThrowExceptionIfRequestIsNull()
        {
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => _processor.Book(null));

            //Assert
            Assert.Equal("request", exception.ParamName);
        }

        /// <summary>
        /// This test will fail because the method is not implemented yet.
        /// </summary>
        [Fact]
        public void ShouldSaveToDatabase()
        {
            // Arrange
            TicketBooking savedTicketBooking = null;

            // Setup the Save method to capture the saved ticket booking
            _ticketBookingRepositoryMock.Setup(x => x.Save(It.IsAny<TicketBooking>()))
            .Callback<TicketBooking>((ticketBooking) =>
            {
                savedTicketBooking = ticketBooking;
            });


            // Act
            _processor.Book(_request);

            // Assert

            /// Verify that the Save method was called once
            _ticketBookingRepositoryMock.Verify(x => x.Save(It.IsAny<TicketBooking>()), Times.Once);

            Assert.NotNull(savedTicketBooking);
            Assert.Equal(_request.FirstName, savedTicketBooking.FirstName);
            Assert.Equal(_request.LastName, savedTicketBooking.LastName);
            Assert.Equal(_request.Email, savedTicketBooking.Email);
        }


        /// <summary>
        /// Testar att Book säger nej till en felaktig e-post, visar felmeddelandet "Invalid email address." och inte sparar något.
        /// </summary>
        [Fact]
        public void ShouldReturnErrorForInvalidEmail()
        {
            //----------Arrange----------//
            // Skapa ett förfrågningsobjekt med exempel på för-/efternamn och ett ogiltigt e-postformat
            var invalidRequest = new TicketBookingRequest
            {
                FirstName = "Gene",
                LastName = "Suelo",
                Email = "not-an-email"    //avsiktligt ogiltig
            };

            // Skapa en simulerad version av repositoriet så att vi senare kan verifiera att Save() aldrig anropas
            var repoMock = new Mock<ITicketBookingRepository>();

            // Instansiera processorn och injicera vårt mock-arkiv
            var processor = new TicketBookingRequestProcessor(repoMock.Object);


            //----------Act----------//
            // Anropa Book-metoden med den ogiltiga begäran
            var response = processor.Book(invalidRequest);


            //----------Assert----------//
            // Verifiera att processorn indikerar fel
            Assert.False(response.Success);

            //Verifiera att rätt felmeddelande returneras för en ogiltig e-postadress 
            Assert.Equal("Invalid email address.", response.ErrorMessage);

            // Se till att inget Save()-anrop gjordes på repository (bokningen ska inte sparas)
            repoMock.Verify(x => x.Save(It.IsAny<TicketBooking>()), Times.Never);
        }
    }
}