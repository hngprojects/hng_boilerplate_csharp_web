﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hng.Application.Features.Notifications.Dtos
{
    public class CreateNotificationDto
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
