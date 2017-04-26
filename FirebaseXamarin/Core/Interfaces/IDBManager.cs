using System;
namespace FirebaseXamarin
{
    public interface IDBManager
    {
        void createTables();
        void saveUserInfo(User user);
        User getLoggedInUserInfo();
        void deleteUserInfo();
    }
}
