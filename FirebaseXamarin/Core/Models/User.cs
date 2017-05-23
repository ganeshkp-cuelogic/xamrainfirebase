using System;
using SQLite;
using Foundation;
using System.Collections.Generic;
using System.Linq;
using SystemConfiguration;
using Newtonsoft.Json;

namespace FirebaseXamarin
{
    [Table("user")]
    public class User : ICloneable
    {
        public User()
        {

        }

        [PrimaryKey]
        [MaxLength(30)]
        public string uid { get; set; }
        public string name { get; set; }
        public string emailid { get; set; }
        public string profilePic { get; set; }
        public string firebaseToken { get; set; }

        [Ignore]
        public List<string> arrIndRoomId { get; set; }

        [Ignore]
        public List<string> arrGroupRoomId { get; set; }
        public bool isSelected { get; set; } //TODO - Find an alternative - this property probably is not required.

        public NSDictionary ToDictionary()
        {
            object[] keys = { "uid", "displayName", "email", "photoUrl", "firebaseToken", FirebaseConstants.FB_IND_ROOM_IDS, FirebaseConstants.FB_GRP_ROOM_IDS };
            object[] values = { this.uid, this.name, this.emailid, this.profilePic, this.firebaseToken, toIndRoomsIDsArray(), toGrpRoomsIDsArray() };
            var data = NSMutableDictionary.FromObjectsAndKeys(values, keys, keys.Length);
            return data;
        }

        public NSArray toIndRoomsIDsArray()
        {
            if (this.arrIndRoomId == null)
            {
                return new NSArray();
            }
            this.arrIndRoomId.RemoveAll(item => item == null);
            return NSArray.FromStrings(this.arrIndRoomId.ToArray());
        }

        public NSArray toGrpRoomsIDsArray()
        {
            if (this.arrGroupRoomId == null)
            {
                return new NSArray();
            }
            this.arrGroupRoomId.RemoveAll(item => item == null);
            return NSArray.FromStrings(this.arrGroupRoomId.ToArray());
        }

        public static User fromDictionary(NSDictionary userDictionary)
        {
            User user = new User();
            user.emailid = userDictionary["email"].ToString();
            user.uid = userDictionary["uid"].ToString();
            user.name = userDictionary["displayName"].ToString();
            user.profilePic = userDictionary["photoUrl"].ToString();
            user.firebaseToken = (userDictionary[FirebaseConstants.FB_TOKEN] == null) ? "" : userDictionary[FirebaseConstants.FB_TOKEN].ToString();

            if (userDictionary[FirebaseConstants.FB_IND_ROOM_IDS] != null)
            {
                user.arrIndRoomId = ConvertToList(userDictionary[FirebaseConstants.FB_IND_ROOM_IDS] as NSArray);
            }
            else
            {
                user.arrIndRoomId = new List<string>();
            }

            if (userDictionary[FirebaseConstants.FB_GRP_ROOM_IDS] != null)
            {
                user.arrGroupRoomId = ConvertToList(userDictionary[FirebaseConstants.FB_GRP_ROOM_IDS] as NSArray);
            }
            else
            {
                user.arrGroupRoomId = new List<string>();
            }

            return user;
        }

        public static List<string> ConvertToList(NSArray arrRooms)
        {
            List<string> roomIds = new List<string>();
            for (nuint i = 0; i < arrRooms.Count; i++)
            {
                if (!String.IsNullOrEmpty(arrRooms.GetItem<NSString>(i)))
                {
                    roomIds.Add((arrRooms.GetItem<NSString>(i)).ToString());
                }
            }
            return roomIds;
        }

        //public NSArray getRoomIdArray()
        //{
        //    return arrRoomId.Split(',');
        //}

        public static User getMyDummyUser()
        {
            User dummyUser = new User();
            dummyUser.uid = "YKigRRuykkYBnWXf4r3DmtFaOAH3";
            dummyUser.name = "Ganesh patro - The Happyboy";
            dummyUser.emailid = "happyboy.ganesh@gmail.com";
            dummyUser.firebaseToken = "asdfghjkl";
            dummyUser.profilePic = "https://lh4.googleusercontent.com/-uQxfaqJEYm8/AAAAAAAAAAI/AAAAAAAAAAs/cnWomrCN2LE/s96-c/photo.jpg";

            return dummyUser;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
