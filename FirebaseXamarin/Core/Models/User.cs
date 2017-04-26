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


        public NSDictionary ToDictionary()
        {
            object[] keys = { "uid", "name", "emailid" };
            object[] values = { this.uid, this.name, this.emailid };
            var data = NSDictionary.FromObjectsAndKeys(values, keys, keys.Length);
            return data;
        }

        public static User fromDictionary(NSDictionary userDictionary)
        {
            User user = new User();
            user.emailid = userDictionary["emailid"].ToString();
            user.uid = userDictionary["uid"].ToString();
            user.name = userDictionary["name"].ToString();

            return user;
        }

    }
}
