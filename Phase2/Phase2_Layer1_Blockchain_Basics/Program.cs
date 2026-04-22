using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Win32.SafeHandles;
using Phase2_Layer1_Blockchain_Basics.Models;
using Phase2_Layer1_Blockchain_Basics.Services;

Console.WriteLine( "==Blockchain learning default recepie project==");

//1.generate identity
IdentityRecord identity = SignatureService.GenerateIdentity();
Console.WriteLine("Identity generated:");
Console.WriteLine($"Public Key ( short ): {identity.PublicKey.Substring(0, 40)}...");

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
ChainService chainService = new();


//5.record  first event
ConsciousEvent ev1 = new()
{
    ActionType = "AffirmationCompleted",
    Data = "I observe the world with curiosity and openness. ",
    TimeUtc = DateTime.UtcNow,
    PublicKey = identity.PublicKey
};

chainService.AddEvent(ev1);
ev1.Signature = SignatureService.Sign(identity.PrivateKey, ev1.Hash);

//6. Record second event
ConsciousEvent ev2 = new()
{
    ActionType = "affirmationCompleted",
    Data = "I am grateful for the opportunities to learn and grow. ",
    TimeUtc = DateTime.UtcNow,
    PublicKey = identity.PublicKey
};
chainService .AddEvent(ev2);
ev2.Signature = SignatureService.Sign(identity.PrivateKey, ev2.Hash);

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
Console.WriteLine("\n----------------------------------");
Console.WriteLine($"Chain Integrity Valid: {chainService.ValidateChain()}");

//8. tamper test
Console.WriteLine("\nTampering with the first event data...");

Console.WriteLine($"Original Data: {chainService.Events[0].Data}");
chainService.Events[0].Data = "I am tampering with the data.";
chainService.Events[1].Signature = "FAKE_SIGNATURE"; // Invalidate the signature of the second event since it depends on the hash of the first event
Console.WriteLine($"Changed Data: {chainService.Events[0].Data}");
Console.WriteLine($"\nChain Integrity Valid after tampering: {chainService.ValidateChain()}");
