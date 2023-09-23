using System;

namespace HospitalSystem
{
    // since every user shares logout and exit behaviour, creating an interface for them
    // seemed appropriate
    public interface IUser
    { 
        void Logout();
        void Exit();
    }
}
