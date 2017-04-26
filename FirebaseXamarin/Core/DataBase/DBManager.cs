using System;
using System.IO;
using SQLite;

namespace FirebaseXamarin
{
    public class DBManager : IDBManager
    {
        public static IDBManager sharedManager = new DBManager();

        private string dbPath;
        private SQLiteConnection dataBaseConnection;

        public DBManager()
        {
            var sqliteFilename = "XamarinFireBaseDatabase.db3";
#if __ANDROID__
            // Just use whatever directory SpecialFolder.Personal returns
            string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
#else
            // we need to put in /Library/ on iOS5.1+ to meet Apple's iCloud terms
            // (they don't want non-user-generated data in Documents)
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder instead
#endif
            dbPath = Path.Combine(libraryPath, sqliteFilename);
            dataBaseConnection = new SQLiteConnection(dbPath);
        }

        public void createTables()
        {
            dataBaseConnection.CreateTable<User>();
        }
        #region User Info         
        public void saveUserInfo(User user)
        {
            dataBaseConnection.Insert(user);
        }

        public User getLoggedInUserInfo()
        {
            User userInfoModel = null;
            foreach (var userInfo in dataBaseConnection.Table<User>())
            {
                userInfoModel = userInfo;
                break;
            }
            return userInfoModel;
        }

        public void deleteUserInfo()
        {
            dataBaseConnection.Delete<User>(getLoggedInUserInfo().uid);
        }
        #endregion
    }
}
