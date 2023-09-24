using System;

namespace HospitalSystem
{
    // since every user shares some similar behaviours, creating an interface for them seemed appropriate
    public interface IUser
    {
        string[] ToStringArray();
        void Logout();
        void Exit();
    }
}
