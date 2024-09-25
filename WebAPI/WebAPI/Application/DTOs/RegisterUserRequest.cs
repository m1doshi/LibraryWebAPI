﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Application.DTOs
{
    public class RegisterUserRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
