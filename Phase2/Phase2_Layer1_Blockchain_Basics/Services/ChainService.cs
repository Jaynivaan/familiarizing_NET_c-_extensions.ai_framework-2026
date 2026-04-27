using Phase2_Layer1_Blockchain_Basics.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Phase2_Layer1_Blockchain_Basics.Services
{
    public class ChainService
    {
       
        private readonly ChainRecord _chain;
        //constructor 1
        public ChainService(string ownerPublicKey)
        {
            _chain = new ChainRecord
            {
                OwnerPublicKey = ownerPublicKey
            };
        }
        //constructor 2
        public ChainService (ChainRecord existingChain)
        {
            _chain = existingChain;
        }

        public string ChainId => _chain.ChainId;
        public string OwnerPublicKey => _chain.OwnerPublicKey;
        public DateTime CreatedAt => _chain.CreatedAt;
        public IReadOnlyList<ConsciousEvent> Events => _chain.Events;

        

        public void AddEvent(ConsciousEvent ev)
        {
            
        
            ev.PreviousHash = _chain.Events.Count == 0 
                ? "GENESIS" 
                : _chain.Events[_chain.Events.Count - 1].Hash;
            ev.Hash = HashService.ComputeEventHash(ev);
            _chain.Events.Add(ev);
        }

        public bool ValidateChain()
        {
            for (int i = 0; i < _chain.Events.Count; i++)
            {
                ConsciousEvent current = _chain.Events[i];

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

                    if (current.PreviousHash != _chain.Events[i - 1].Hash)
                        return false;
                }
            }
            return true;
        }
        public bool HasEvent (string actionType, string data)
        {
            return _chain.Events.Any(ev =>
            ev.ActionType == actionType &&
            ev.Data == data
            );

        }
        public ChainRecord GetChain()
            { return _chain; }

    }
}




