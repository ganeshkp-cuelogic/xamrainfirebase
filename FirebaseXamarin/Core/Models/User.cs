using System;
using SQLite;
using Foundation;
using System.Collections.Generic;
using System.Linq;
using SystemConfiguration;

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
        public string arrRoomId { get; set; }

        public NSDictionary ToDictionary()
        {
            object[] keys = { "uid", "displayName", "email", "photoUrl", "firebaseToken" };
            object[] values = { this.uid, this.name, this.emailid, this.profilePic, this.firebaseToken };
            var data = NSMutableDictionary.FromObjectsAndKeys(values, keys, keys.Length);
            // data.SetValueForKey(getRoomIdArray(), new NSString(FirebaseConstants.FB_ROOMS));
            return data;
        }

        public static User fromDictionary(NSDictionary userDictionary)
        {
            User user = new User();
            user.emailid = userDictionary["email"].ToString();
            user.uid = userDictionary["uid"].ToString();
            user.name = userDictionary["displayName"].ToString();
            user.profilePic = userDictionary["photoUrl"].ToString();
            user.firebaseToken = (userDictionary[FirebaseConstants.FB_TOKEN] == null) ? "" : userDictionary[FirebaseConstants.FB_TOKEN].ToString();
            user.arrRoomId = (userDictionary[FirebaseConstants.FB_ROOMS] == null) ? "" : userDictionary[FirebaseConstants.FB_ROOMS].ToString();

            return user;
        }

        public NSArray getRoomIdArray()
        {
            NSMutableArray arrIds = new NSMutableArray();
            foreach (string id in arrRoomId.Split(','))
            {
                arrIds.Add(new NSString(id));
            }

            return NSArray.FromObjects(arrRoomId.Split(','));

            //return arrIds;
        }

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
