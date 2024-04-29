using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.web.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Villa> VillaList { get; set; }
        public DateOnly CheckInData { get; set; }
        public DateOnly? CheckOutDate { get; set; }
        public int Nights { get; set; }
    }
}
