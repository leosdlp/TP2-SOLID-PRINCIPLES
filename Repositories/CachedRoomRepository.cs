namespace HotelReservation.Repositories;

using HotelReservation.Models;

// LSP VIOLATION (Example 2): This implementation does not respect the contract
// of IRoomRepository.GetAvailableRooms. It returns cached data that may be stale
// and ignores the date parameters entirely. Substituting this for InMemoryRoomRepository
// produces semantically incorrect results.
public class CachedRoomRepository : IRoomRepository
{
    private readonly IRoomRepository _inner;
    private readonly Dictionary<string, Room> _cache = new();
    private readonly Dictionary<string, List<Room>> _availabilityCache = new();

    public CachedRoomRepository(IRoomRepository inner)
    {
        _inner = inner;
    }

    public Room? GetById(string roomId)
    {
        if (!_cache.ContainsKey(roomId))
        {
            var room = _inner.GetById(roomId);
            if (room != null)
                _cache[roomId] = room;
            return room;
        }
        return _cache[roomId];
    }

    public List<Room> GetAvailableRooms(DateTime from, DateTime to)
    {
        var key = BuildKey(from, to);
        if (_availabilityCache.TryGetValue(key, out var cachedRooms))
            return cachedRooms.Select(CloneRoom).ToList();

        var freshRooms = _inner.GetAvailableRooms(from, to);
        _availabilityCache[key] = freshRooms.Select(CloneRoom).ToList();
        return freshRooms;
    }

    public void Save(Room room)
    {
        _inner.Save(room);
        _cache.Remove(room.Id);
        _availabilityCache.Clear();
    }

    private static string BuildKey(DateTime from, DateTime to)
    {
        return $"{from:yyyyMMdd}-{to:yyyyMMdd}";
    }

    private static Room CloneRoom(Room room)
    {
        return new Room
        {
            Id = room.Id,
            Type = room.Type,
            MaxGuests = room.MaxGuests,
            PricePerNight = room.PricePerNight,
            IsAvailable = room.IsAvailable
        };
    }
}
