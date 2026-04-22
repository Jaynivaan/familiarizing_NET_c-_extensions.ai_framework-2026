using Phase2_Layer1_Blockchain_Basics.Models;
using System.Collections.Generic;

namespace Phase2_Layer1_Blockchain_Basics.Services
{
    public class ChainService
    {
        private readonly List<ConsciousEvent> _events = new List<ConsciousEvent>();
        public IReadOnlyList<ConsciousEvent> Events => _events;

        public void AddEvent(ConsciousEvent ev)
        {
            ev.PreviousHash = _events.Count == 0 ? "GENESIS" : _events[_events.Count - 1].Hash;
            ev.Hash = HashService.ComputeEventHash(ev);
            _events.Add(ev);
        }

        public bool ValidateChain()
        {
            for (int i = 0; i < _events.Count; i++)
            {
                ConsciousEvent current = _events[i];

                string recalculatedHash = HashService.ComputeEventHash(current);

                if (current.Hash != recalculatedHash)
                    return false;
                if (!SignatureService.Verify(current.PublicKey,current.Hash, current.Signature))
                    return false;

                if (i == 0)
                {
                    if (current.PreviousHash != "GENESIS")
                        return false;

                }
                else
                {

                    if (current.PreviousHash != _events[i - 1].Hash)
                        return false;
                }
            }
            return true;
        }

    }
}




