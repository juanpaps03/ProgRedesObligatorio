﻿using System;

namespace Domain
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Admin { get; set;  }

        public User()
        {
        }
    }
}