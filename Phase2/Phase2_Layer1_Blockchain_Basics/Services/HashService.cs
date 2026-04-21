using System;
using System.Security.Cryptography;
using System.Text;
using Phase2_Layer1_Blockchain_Basics.Models;

namespace Phase2_Layer1_Blockchain_Basics.Services

public static class HashService
{
    public static string ComputeEventHash(ConsciousEvent ev)
    {
        string raw =
            $"{ev.ActionType}|{ev.Data}|{ev.TimeUtc:0}|{ev.PreviousHash}|{ev.PublicKey}";

        using SHA256 sha = SHA256.Create();
        byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));

        return Convert.ToHexString(bytes);
    }
}