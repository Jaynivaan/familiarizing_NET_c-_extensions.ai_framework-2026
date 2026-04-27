//using System.Security.Authentication;
//using System.Security.Cryptography.X509Certificates;
//using Microsoft.Win32.SafeHandles;
using System;
using System.Security.Principal;
using Phase2_Layer1_Blockchain_Basics.Models;
using Phase2_Layer1_Blockchain_Basics.Services;

Console.WriteLine( "==Blockchain learning default recepie project==");

//1.generate identity
Console.WriteLine("Enter PresenceId: ");
string presenceId = Console.ReadLine() ?? "presence1";

Console.WriteLine($"Active Presence: {presenceId}");



IdentityRecord? loadedIdentity = IdentityStorageService.Load(presenceId);

IdentityRecord identity;
if (loadedIdentity != null)
{
    Console.WriteLine("Using existing identity...");
    identity = loadedIdentity;
}
else
{
    Console.WriteLine("Creating new Identity...");
    identity = SignatureService.GenerateIdentity();
    
    IdentityStorageService.Save(presenceId, identity);
}

Console.WriteLine("Identity generated:");
Console.WriteLine($"Public Key ( short ): {identity.PublicKey.Substring(0, 40)}...");


Console.WriteLine("Send event to (target presence ): ");
string targetPresenceId = Console.ReadLine() ?? presenceId;

//load target user chain
var targetChainRecord = ChainStorageService.Load(targetPresenceId);
ChainService targetChainService;
if (targetChainRecord != null)
{
    Console.WriteLine("Using existing target chain...");
    targetChainService = new ChainService(targetChainRecord);
}
else
{
    Console.WriteLine("Target chain not found.Creating new .. ");
    targetChainService = new ChainService(identity.PublicKey);
    //
    //targetChainService = new ChainService(identity.PublicKey);
}


//2.Create challenge
ChallengeRecord challenge = ChallengeService.CreateChallenge();
Console.WriteLine($"Challenge nonce: {challenge.Nonce}\n");

//3.sign challenge
string challengeSignature = SignatureService.Sign(identity.PrivateKey, challenge.Nonce);
//string challengeSignature = "FAKE_SIGNATURE";           //For testing invalid signature scenario
bool identityValid = SignatureService.Verify(identity.PublicKey, challenge.Nonce, challengeSignature);

if (!identityValid)
{
    Console.WriteLine("Identity verification failed. Event Rejected. Exiting.");
    return;
}

Console.WriteLine($"Identity Verified: {identityValid}\n");
Console.WriteLine("congrats!");


//4.create chain
var loadedChain = ChainStorageService.Load(presenceId);

ChainService chainService;
if (loadedChain != null)
{
    Console.WriteLine("Using Existing chain..");
    chainService = new ChainService(loadedChain);
}
else
{
    Console.WriteLine("Creating new chain...");
    chainService = new ChainService(identity.PublicKey);
}    

Console.WriteLine($"Chain Id: {chainService.ChainId}");
Console.WriteLine($"Chain Owner (short): {chainService.OwnerPublicKey.Substring(0, 40)}...");
Console.WriteLine($"Chain Created At: {chainService.CreatedAt}");
Console.WriteLine();




//5.record  first event
ConsciousEvent ev1 = new()
{
    ActionType = "AffirmationCompleted",
    Data = "I observe the world with curiosity and openness. ",
    TimeUtc = DateTime.UtcNow,
    PublicKey = identity.PublicKey
};

if (!chainService.HasEvent(ev1.ActionType, ev1.Data))
{
    ev1.TargetPresenceId = targetPresenceId;
    targetChainService.AddEvent(ev1);
   //hainService.AddEvent(ev1);
    ev1.Signature = SignatureService.Sign(identity.PrivateKey, ev1.Hash);
}
else
{
    Console.WriteLine("Event already exists. skipping.");
}
//ainService.AddEvent(ev1);
//1.Signature = SignatureService.Sign(identity.PrivateKey, ev1.Hash);

//6. Record second event
ConsciousEvent ev2 = new()
{
    ActionType = "affirmationCompleted",
    Data = "I am grateful for the opportunities to learn and grow. ",
    TimeUtc = DateTime.UtcNow,
    PublicKey = identity.PublicKey
};
if (!chainService.HasEvent(ev2.ActionType, ev2.Data))
{
    chainService.AddEvent(ev2);
    ev2.Signature = SignatureService.Sign(identity.PrivateKey, ev2.Hash);
}
else
{
    Console.WriteLine("Event already exists. skipping.");
}
//7. display
foreach (var ev in chainService .Events)
{

   Console.WriteLine("-------------------------------------");
   Console.WriteLine($"Action Type : {ev.ActionType}");     
   Console.WriteLine($"Data        : {ev.Data}");
   Console.WriteLine($"TimeUtc     : {ev.TimeUtc}");
   Console.WriteLine($"PreviousHash: {ev.PreviousHash}");
   Console.WriteLine($"Hash        : {ev.Hash}");
   Console.WriteLine($"Signature Ok: {SignatureService.Verify(ev.PublicKey, ev.Hash, ev.Signature)}");

}
Console.WriteLine("\n---------------------------------------");
Console.WriteLine($"Chain Integrity Valid: {chainService.ValidateChain()}");
ChainStorageService.Save(targetPresenceId, targetChainService.GetChain());
Console.WriteLine($"\nChain saved to chain_{targetPresenceId}.json");

Console.WriteLine("\n----Loading chain from file----");

var reloadedChain = ChainStorageService.Load(targetPresenceId);

if (reloadedChain != null)
{
    Console.WriteLine($"Loaded Chain Id: {reloadedChain.ChainId}");
    Console.WriteLine($"Loaded Events Counts: {reloadedChain.Events.Count}");

}
else
{
    Console.WriteLine("No chain found.");

}

//8. tamper test
Console.WriteLine("\nTampering with the first event data...");

Console.WriteLine($"Original Data: {chainService.Events[0].Data}");
chainService.Events[0].Data = "I am tampering with the data.";
//chainService.Events[1].Signature = "FAKE_SIGNATURE"; // Invalidate the signature of the second event since it depends on the hash of the first event
Console.WriteLine($"Changed Data: {chainService.Events[0].Data}");
Console.WriteLine($"\nChain Integrity Valid after tampering: {chainService.ValidateChain()}");

