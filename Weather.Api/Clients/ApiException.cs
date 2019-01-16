﻿using System;

namespace Weather.Api.Clients
{
    public class ApiException : Exception
    {
        public int ErrorCode { get; set; }

        public ApiException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
