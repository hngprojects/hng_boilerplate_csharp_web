using FluentValidation;
using Hng.Application.Models.EmailModels;
using Hng.Application.Models.WaitlistModels;
using Hng.Application.Services;
using Hng.Domain.Entities;
using Hng.Domain.Repositories;
using Hng.Domain.Repository;
using System;
using System.Threading.Tasks;

namespace Hng.Application.Services
{
    public class WaitlistService : IWaitlistService
    {
        private readonly IWaitlistUserRepository _waitlistUserRepository;
        private readonly IValidator<WaitlistUserRequestModel> _validator;
        private readonly IEmailService _emailService;
        private readonly IRateLimitRepository _rateLimitRepository;

        public WaitlistService(IWaitlistUserRepository waitlistUserRepository, IValidator<WaitlistUserRequestModel> validator, IEmailService emailService, IRateLimitRepository rateLimitRepository)
        {
            _waitlistUserRepository = waitlistUserRepository;
            _validator = validator;
            _emailService = emailService;
            _rateLimitRepository = rateLimitRepository;
        }

        public async Task<WaitlistUserResponseModel> SignUpAsync(WaitlistUserRequestModel model)
        {
            var result = new WaitlistUserResponseModel();

            var validationResult = await _validator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                result.Success = false;
                result.StatusCode = 400; 
                result.Message = "Validation error";
                result.Error = string.Join(", ", validationResult.Errors);
                return result;
            }

            try
            {
                var userId = Guid.NewGuid(); 
                var rateLimit = await _rateLimitRepository.GetRateLimitByUserIdAsync(userId);

                if (rateLimit != null && rateLimit.RequestCount >= 5) 
                {
                    result.Success = false;
                    result.StatusCode = 429;
                    result.Message = "Rate limit exceeded";
                    result.Error = "Too Many Requests";
                    return result;
                }

                var waitlistUser = new WaitlistUser
                {
                    Email = model.Email,
                    FullName = model.FullName,
                    CreatedAt = DateTime.UtcNow
                };
                await _waitlistUserRepository.AddAsync(waitlistUser);

                var email = new Email
                {
                    To = model.Email,
                    Subject = "Welcome to the Waitlist!",
                    Body = $"Dear {model.FullName},\n\nYou are now on our waitlist. Thank you for signing up!"
                };
                var emailSent = await _emailService.SendEmail(email);

                if (!emailSent)
                {
                    result.Success = false;
                    result.StatusCode = 500;
                    result.Message = "Failed to send confirmation email";
                    result.Error = "Internal Server Error";
                    return result;
                }

                result.Success = true;
                result.StatusCode = 201;
                result.Message = "You are all signed up!";
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.StatusCode = 500;
                result.Message = "An error occurred while processing your request.";
                result.Error = ex.Message;
                return result;
            }
        }
    }
}
