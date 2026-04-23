using System;
using System.Collections.Generic;


namespace Phase2_Layer1_Blockchain_Basics.Models
{
    public class  ChainRecord
    {
        public string ChainId { get; set; } = Guid.NewGuid().ToString();

        public string OwnerPublicKey { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<ConsciousEvent> Events { get; set; } = new List<ConsciousEvent>();

    }
}