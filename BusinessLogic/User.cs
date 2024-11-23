﻿using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class User
    {
        protected UserData _userData;
        protected UserRole _role;

        public UserData Data => _userData;
        public UserRole Role => _role;


        public User(UserData userData)
        {
            _userData = userData;

            if (userData.Role == "Admin")
            {
                _role = UserRole.Admin;
            }
            else if (userData.Role == "User")
            {
                _role = UserRole.User;
            }
            else
            {
                throw new InvalidOperationException("Unknown role");
            }
        }
    }
}
