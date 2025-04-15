namespace Wego.API.Models.DTOS.Bookings
{
    public class RoomBookingDto
    {
        public int Id { get; set; }
        public DateTime Checkin { get; set; }
        public DateTime Checkout { get; set; }
        public int Guests { get; set; }
        public int Children { get; set; }
        public int RoomId { get; set; }
        public string RoomTitle { get; set; }
        public string Status { get; set; }
        public int RoomOptionId { get; set; }

        public decimal TotalPrice { get; set; }
        public string UserId { get; set; }
    }

    public class RoomBookingPostDto
    {
        public DateTime Checkin { get; set; }
        public DateTime Checkout { get; set; }
        public int Guests { get; set; }
        public int Children { get; set; }
        public int RoomId { get; set; }
        public int RoomOptionId { get; set; }

    }

    public class RoomBookingPutDto
    {
        public int Id { get; set; }
        public DateTime Checkin { get; set; }
        public DateTime Checkout { get; set; }
        public int Guests { get; set; }
        public int Children { get; set; }
        public int RoomId { get; set; }
        public string Status { get; set; }
        public int RoomOptionId { get; set; }

        //public string UserId { get; set; }
    }

}
