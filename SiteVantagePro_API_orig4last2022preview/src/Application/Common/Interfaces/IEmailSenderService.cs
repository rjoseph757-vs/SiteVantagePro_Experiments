﻿using Application.Notifications.Models;

namespace Application.Common.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendAsync(MessageDto message);
    }
}
