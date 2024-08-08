using Hng.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Notifications.Dtos
{
    public class NotificationResult
    {
        public NotificationDto Notification { get; set; }
        public FailureResponseDto<object> FailureResponse { get; set; }
        public SuccessResponseDto<object> SuccessResponse { get; set; }
        public bool IsSuccess { get; set; }
    }
}
