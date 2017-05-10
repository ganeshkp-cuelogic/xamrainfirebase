using System;
namespace FirebaseXamarin
{
    public class RoomsMetaData
    {
        public RoomsMetaData()
        {
        }

        public string roomId { get; set; }
        public string displayName { get; set; }
        public string createdTime { get; set; }
        public string createdBy { get; set; }
        public string lastUpdatedTime { get; set; }
        public string users { get; set; }
    }
}
