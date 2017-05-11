using System;
using Foundation;
using System.Collections.Generic;
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
        public List<string> users { get; set; }

        public NSDictionary toDictionary()
        {
            object[] keys = { FirebaseConstants.FB_ROOM_ID,
                FirebaseConstants.FB_DISPLAY_NAME,
                FirebaseConstants.FB_CREATED_TIME,
                FirebaseConstants.FB_CREATED_BY,
                FirebaseConstants.FB_LAST_UPDATED_TIME,
                FirebaseConstants.FB_USERS
            };

            object[] values = { this.roomId, this.displayName, this.createdTime, this.createdBy, this.lastUpdatedTime, getUsersArray() };
            var data = NSMutableDictionary.FromObjectsAndKeys(values, keys, keys.Length);
            return data;
        }

        public static RoomsMetaData fromDictionary(NSDictionary roomDict)
        {
            RoomsMetaData roomData = new RoomsMetaData();
            roomData.roomId = roomDict[FirebaseConstants.FB_ROOM_ID].ToString();
            roomData.displayName = roomDict[FirebaseConstants.FB_DISPLAY_NAME].ToString();
            roomData.createdTime = roomDict[FirebaseConstants.FB_CREATED_TIME].ToString();
            roomData.createdBy = roomDict[FirebaseConstants.FB_CREATED_BY].ToString();
            roomData.lastUpdatedTime = roomDict[FirebaseConstants.FB_LAST_UPDATED_TIME].ToString();

            List<string> users = new List<string>();
            foreach (string user in users)
            {
                users.Add(user);
            }
            roomData.users = users;
            return roomData;
        }

        private NSMutableArray getUsersArray()
        {
            NSMutableArray arrUsers = new NSMutableArray();
            foreach (string user in users)
            {
                arrUsers.Add(new NSString(user));
            }

            return arrUsers;
        }
    }
}
