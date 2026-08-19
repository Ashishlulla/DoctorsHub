using DoctorsHub.Application.Interfaces.ServiceContracts;
using DoctorsHub.Application.Models.Authentication;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace DoctorsHub.Application.Services
{
    public class OtpService : IOtpService
    {
        //Private Feilds
        private readonly ConcurrentDictionary<string, OtpData> _otpStore = new();

        public Task<OtpData> GenerateOtpAsync(string userId)
        {
            string otp = Convert.ToString(RandomNumberGenerator.GetInt32(100000, 1000000));

            var otpData = new OtpData
            {
                UserId = userId,
                Otp = otp,
                ExpiresAt = DateTime.Now.AddSeconds(90),
                IsUsed = false
            };

            _otpStore[userId] = otpData;

            return Task.FromResult(otpData);

        }

        public Task RemoveOtpAsync(string userId)
        {
            _otpStore.TryRemove(userId, out _);

            return Task.CompletedTask;
        }

        public Task<bool> ValidateOtpAsync(string userId, string otp)
        {
            if (!_otpStore.TryGetValue(userId, out var otpData))
            {
                return Task.FromResult(false);
            }

            if (otpData.IsUsed)
            {
                _otpStore.TryRemove(userId, out _);
                return Task.FromResult(false);
            }

            if (otpData.ExpiresAt < DateTime.Now)
            {
                _otpStore.TryRemove(userId, out _);
                return Task.FromResult(false);
            }

            if (otpData.Otp != otp)
            {
                return Task.FromResult(false);
            }

            otpData.IsUsed = true;

            return Task.FromResult(true);
        }
    }
}
