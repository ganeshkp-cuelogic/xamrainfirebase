using System;
using SQLite;
using Foundation;

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


        public NSDictionary ToDictionary()
        {
            object[] keys = { "uid", "displayName", "email", "photoUrl", "firebaseToken" };
			object[] values = { this.uid, this.name, this.emailid, this.profilePic, this.firebaseToken };
            var data = NSDictionary.FromObjectsAndKeys(values, keys, keys.Length);
            return data;
        }

        public static User fromDictionary(NSDictionary userDictionary)
        {
            User user = new User();
            user.emailid = userDictionary["email"].ToString();
            user.uid = userDictionary["uid"].ToString();
            user.name = userDictionary["displayName"].ToString();
			user.profilePic = userDictionary["photoUrl"].ToString();
			user.firebaseToken = userDictionary["firebaseToken"].ToString();

            return user;
        }

    }
}
