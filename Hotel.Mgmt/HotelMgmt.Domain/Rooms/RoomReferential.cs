using System.Collections.Generic;

namespace HotelMgmt.Domain.Rooms
{
    public class RoomReferential
    {
        private IDictionary<int, Room> _rooms => new Dictionary<int, Room>()
        {
            { 1 , new Room("670f7e7c-033e-48d6-a94c-19b5a6c6b0a8")},
            { 2 , new Room("5ce7d868-d2dd-401b-9840-3f9e9b94b76e")},
            { 3 , new Room("07259a1d-9e16-4888-b6f1-cbc2432c9931")},
            { 4 , new Room("5b78cc30-afc1-471a-9c28-60673e6b3914")},
            { 5 , new Room("72272ed2-5d16-4ed3-9fa5-98d347891c32")},
            { 6 , new Room("3e3775f6-441f-4cdb-98bb-ee7fcee476e6")},
            { 7 , new Room("2440b9c2-39d0-4757-809f-c30c7e5f0214")},
            { 8 , new Room("d600433a-7e20-4ee5-b70b-06301d3f3856")},
            { 9 , new Room("9a416545-f775-48ac-93db-3bb8a109b305")},
            { 10, new Room("ec9354d8-4e0a-4baf-b73c-a233cf1f80be")},
            { 11, new Room("689e4132-0541-410e-bfbf-2b59dcb15292")},
            { 12, new Room("9c91878b-9e8d-4657-868c-55bb33efd342")},
            { 13, new Room("78f7992b-10cf-49a4-9115-78f4006d4b2e")},
            { 14, new Room("90edc429-85e9-4a48-ae6a-94d3c5c315a8")},
            { 15, new Room("e899947d-be89-4d97-bcad-cf3a4d4a4ef1")},
            { 16, new Room("08b6ce30-70c8-40ea-b77f-af661956a128")},
            { 17, new Room("238d8958-34ea-42b6-9b3d-948247949d94")},
            { 18, new Room("19ed207f-e671-4c9a-acad-b9b278c88753")},
            { 19, new Room("4d387a89-5519-41f8-af0f-d713cc0e7318")},
            { 20, new Room("3ac784bc-c7fc-4cc5-9769-60becd6a9819")}
        };

        public Room GetByNumber(int roomNumber) => _rooms[roomNumber];
        public int Count() => _rooms.Count;
    }
}