using System;

namespace Phase2_Layer1_Blockchain_Basics.Models
{
    public class ChallengeRecord
    {
        public string ChallengeId { get; set; } = Guid.NewGuid().ToString();
        public string Nonce { get; set; } = "";
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAtUtc { get; set; } = DateTime.UtcNow.AddMinutes(2);
        public bool Used { get; set; } = false;
    }
}

