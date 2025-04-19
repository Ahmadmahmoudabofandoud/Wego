using System.Linq.Expressions;
using Wego.Core.Models.Hotels;

namespace Wego.Core.Specifications.RoomSpecification
{
    public class RoomByHotelIdsSpecification : BaseSpecifcation<Room>
    {
        public RoomByHotelIdsSpecification(List<int>? hotelIds)
            : base(R => R.HotelId.HasValue && hotelIds.Contains(R.HotelId.Value))
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(R => R.Images);
            Includes.Add(R => R.Hotel);
            Includes.Add(R => R.RoomBookings);
            Includes.Add(R => R.Amenities);
            Includes.Add(R => R.RoomOptions);

            ThenIncludes.Add((
                (Expression<Func<Room, object>>)(h => h.Hotel),
                (Expression<Func<object, object>>)(r => ((Hotel)r).Images)));
            ThenIncludes.Add((
                (Expression<Func<Room, object>>)(h => h.Hotel),
                (Expression<Func<object, object>>)(r => ((Hotel)r).Amenities)));
            ThenIncludes.Add((
                (Expression<Func<Room, object>>)(h => h.Hotel),
                (Expression<Func<object, object>>)(r => ((Hotel)r).Reviews)));
        }
    }


}
