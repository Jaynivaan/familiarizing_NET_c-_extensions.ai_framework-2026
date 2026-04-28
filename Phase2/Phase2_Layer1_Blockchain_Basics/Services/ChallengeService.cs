using System;
using System.Security.Cryptography;
using Phase2_Layer1_Blockchain_Basics.Models;

namespace Phase2_Layer1_Blockchain_Basics.Services

{
    public static class  ChallengeService
    {
        public static ChallengeRecord CreateChallenge()
        {
            byte[] bytes = RandomNumberGenerator.GetBytes(32);

            return new ChallengeRecord
            {
                Nonce = Convert.ToHexString(bytes),
                CreatedAtUtc = DateTime.UtcNow,
                ExpiresAtUtc = DateTime.UtcNow.AddMinutes(2),
                Used = false
            };
        }

        public static bool VerifyChallenge(ChallengeRecord challenge)
        {
            //
            if (challenge == null)
                return false;
            if (challenge.Used)
                return false;
            if (DateTime.UtcNow > challenge.ExpiresAtUtc)
                return false;
            challenge.Used = true;
            return true;
        }
    }
}

