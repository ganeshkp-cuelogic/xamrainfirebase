using System;
using Firebase.Database;
using System.Collections.Generic;
using Foundation;

namespace FirebaseXamarin
{
    public class FirebaseManager
    {
        public FirebaseManager() { }

        public static FirebaseManager sharedManager = new FirebaseManager();

        private readonly DatabaseReference rootNode = Database.DefaultInstance.GetRootReference();

        public void getAllUser(Action<List<User>> callback)
        {
            DatabaseReference userNode = rootNode.GetChild(FirebaseConstants.FB_USERS);
            userNode.ObserveEvent(DataEventType.Value, (snapshot) =>
                {
                    // Loop over the children
                    NSEnumerator children = snapshot.Children;
                    var child = children.NextObject() as DataSnapshot;

                    List<User> users = new List<User>();
                    while (child != null)
                    {
                        // Work with data...
                        var dictionaryData = child.GetValue<NSDictionary>();
                        if (dictionaryData["uid"].ToString() != DBManager.sharedManager.getLoggedInUserInfo().uid)
                        {
                            users.Add(User.fromDictionary(dictionaryData));
                        }

                        child = children.NextObject() as DataSnapshot;
                    }
                    callback(users);
                });
        }

        public void createGroup()
        {

        }
    }
}

