﻿using System;

namespace CheckDrive.ApiContracts.Operator
{
    public class OperatorForCreateDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
    }
}