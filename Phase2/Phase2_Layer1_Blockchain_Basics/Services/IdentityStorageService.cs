using System.Text.Json;
using Phase2_Layer1_Blockchain_Basics.Models;

namespace Phase2_Layer1_Blockchain_Basics.Services
{
    public static class IdentityStorageService
    {
        private static string GetFileName(string presenceId)
        {
            return $"identity_{presenceId}.json";
        }

        public static void Save ( string presenceId, IdentityRecord identity)
        {
            string fileName = GetFileName(presenceId);
            
            string json = JsonSerializer.Serialize(identity, new JsonSerializerOptions
            {
                WriteIndented =true
            });
            File.WriteAllText(fileName, json);

        }

        public static IdentityRecord? Load( string presenceId )
        {
            string fileName = GetFileName(presenceId);

            if (!File.Exists(fileName))
                return null;
            string json = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<IdentityRecord>(json);
        }
    }
}