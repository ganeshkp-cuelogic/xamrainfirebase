using System;
using System.Collections.Generic;

namespace FirebaseXamarin
{
    public interface IFirebaseManager
    {
        void getAllUser(Action<List<User>> callback);
    }
}
