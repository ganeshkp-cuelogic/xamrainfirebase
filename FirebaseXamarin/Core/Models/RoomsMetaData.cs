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
        public int type { get; set; }
        public string displayName { get; set; }
        public string createdTime { get; set; }
        public string createdBy { get; set; }
        public string lastUpdatedTime { get; set; }
        public List<string> users { get; set; }

        public NSDictionary toDictionary()
        {
            object[] keys = { FirebaseConstants.FB_ROOM_ID,
                FirebaseConstants.FB_ROOM_TYPE,
                FirebaseConstants.FB_DISPLAY_NAME,
                FirebaseConstants.FB_CREATED_TIME,
                FirebaseConstants.FB_CREATED_BY,
                FirebaseConstants.FB_LAST_UPDATED_TIME,
                FirebaseConstants.FB_USERS
            };

            object[] values = { this.roomId, this.type, this.displayName, Convert.ToDouble(this.createdTime), this.createdBy, Convert.ToDouble(this.lastUpdatedTime), getUsersArray() };
            var data = NSMutableDictionary.FromObjectsAndKeys(values, keys, keys.Length);
            return data;
        }

        public static RoomsMetaData fromDictionary(NSDictionary roomDict)
        {
            RoomsMetaData roomData = new RoomsMetaData();
            roomData.roomId = roomDict[FirebaseConstants.FB_ROOM_ID].ToString();
            roomData.type = Convert.ToInt16(roomDict[FirebaseConstants.FB_ROOM_TYPE].ToString());
            roomData.displayName = roomDict[FirebaseConstants.FB_DISPLAY_NAME] == null ? "" : roomDict[FirebaseConstants.FB_DISPLAY_NAME].ToString();
            roomData.createdTime = roomDict[FirebaseConstants.FB_CREATED_TIME].ToString();
            roomData.createdBy = roomDict[FirebaseConstants.FB_CREATED_BY].ToString();
            roomData.lastUpdatedTime = roomDict[FirebaseConstants.FB_LAST_UPDATED_TIME].ToString();

            NSArray arrUsers = (roomDict[FirebaseConstants.FB_USERS] as NSArray);
            List<string> users = new List<string>();
            for (nuint i = 0; i < arrUsers.Count; i++)
            {
                if (!String.IsNullOrEmpty(arrUsers.GetItem<NSString>(i)))
                {
                    users.Add((arrUsers.GetItem<NSString>(i)).ToString());
                }
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
