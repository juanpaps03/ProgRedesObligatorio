﻿using System;

namespace Domain
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Admin { get; set;  }

        public User()
        {
        }
    }
}