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
    public class User
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
        public List<string> arrRoomId { get; set; }

        public NSDictionary ToDictionary()
        {
            object[] keys = { "uid", "displayName", "email", "photoUrl", "firebaseToken", FirebaseConstants.FB_ROOMS };
            object[] values = { this.uid, this.name, this.emailid, this.profilePic, this.firebaseToken, toRoomsIDsArray() };
            var data = NSMutableDictionary.FromObjectsAndKeys(values, keys, keys.Length);
            // data.SetValueForKey(getRoomIdArray(), new NSString(FirebaseConstants.FB_ROOMS));
            return data;
        }

        public NSArray toRoomsIDsArray()
        {
            return NSArray.FromObjects(arrRoomId);
        }

        public static User fromDictionary(NSDictionary userDictionary)
        {
            User user = new User();
            user.emailid = userDictionary["email"].ToString();
            user.uid = userDictionary["uid"].ToString();
            user.name = userDictionary["displayName"].ToString();
            user.profilePic = userDictionary["photoUrl"].ToString();
            user.firebaseToken = (userDictionary[FirebaseConstants.FB_TOKEN] == null) ? "" : userDictionary[FirebaseConstants.FB_TOKEN].ToString();
            if (userDictionary[FirebaseConstants.FB_ROOMS] != null)
            {
                user.arrRoomId = ConvertToList(userDictionary[FirebaseConstants.FB_ROOMS] as NSArray);
            }
            return user;
        }

        public static List<string> ConvertToList(NSArray arrRooms)
        {
            List<string> roomIds = new List<string>();
            for (nuint i = 0; i < arrRooms.Count; i++)
            {
                roomIds.Add((arrRooms.GetItem<NSString>(i)).ToString());
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

    }
}
