using System;
using System.Security.AccessControl;

namespace Phase2_Layer1_Blockchain_Basics.Models
{
    public class ConsciousEvent
    {
        public string ActionType { get; set; } = "";
        public string Data { get; set; } = "";
        public DateTime TimeUtc { get; set; }
        public string PreviousHash { get; set; } = "";
        public string Hash { get; set; } = "";
        public string Signature { get; set; } = "";
        public string PublicKey { get; set; } = "";

        public string TargetPresenceId { get; set; } = "";


    }

}
