using System.Text.Json;
using Phase2_Layer1_Blockchain_Basics.Models;

namespace Phase2_Layer1_Blockchain_Basics.Services
{
    public static class ChainStorageService
    {
        private const string FileName = "chain.json";

        public static void Save(ChainRecord chain)
        {
            string json = JsonSerializer.Serialize(chain, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(FileName, json);

        }

        public static ChainRecord? Load()
        {
            if (!File.Exists(FileName))
                return null;

            string json = File.ReadAllText(FileName);
            return JsonSerializer.Deserialize<ChainRecord>(json);

        }
    }
}