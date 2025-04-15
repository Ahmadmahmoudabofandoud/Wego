namespace Wego.API.Models.DTOS.Hotels.Dtos
{
    public class RoomOptionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Price { get; set; }
        public bool IsRefundable { get; set; }
        public bool IncludesBreakfast { get; set; }
        public int Capacity { get; set; }
    }

    public class RoomOptionCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public int Price { get; set; }
        public bool IsRefundable { get; set; }
        public bool IncludesBreakfast { get; set; }
        public int Capacity { get; set; }
    }
}
