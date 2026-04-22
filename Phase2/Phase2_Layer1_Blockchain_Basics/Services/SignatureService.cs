using Phase2_Layer1_Blockchain_Basics.Models;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Phase2_Layer1_Blockchain_Basics.Services
{
    public static class SignatureService
    {
        public static IdentityRecord GenerateIdentity()                                                    // Generate a new RSA key pair and return the public and private keys as Base64 strings          
        {
            using (RSA rsa = RSA.Create(2048))
            {
                string publicKey = Convert.ToBase64String(
                     rsa.ExportSubjectPublicKeyInfo()
                    );
                string privateKey = Convert.ToBase64String(
                rsa.ExportPkcs8PrivateKey());

                return new IdentityRecord
                {
                    PublicKey = publicKey,
                    PrivateKey = privateKey
                };
            }
        }


        public static string Sign(string privateKeyBase64, string message)                                  // Sign a message using the private key and return the signature as a Base64 string
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportPkcs8PrivateKey(
                Convert.FromBase64String(privateKeyBase64), out _);

                byte[] data = Encoding.UTF8.GetBytes(message);
                byte[] signature = rsa.SignData(
                    data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                return Convert.ToBase64String(signature);

            }

        }

        public static bool Verify(string publicKeyBase64, string message, string signatureBase64)           // Verify the signature of a message using the public key
        {
            try
            {
                using (RSA rsa = RSA.Create())
                {
                    rsa.ImportSubjectPublicKeyInfo(
                        Convert.FromBase64String(publicKeyBase64), out _);

                    byte[] data = Encoding.UTF8.GetBytes(message);
                    byte[] signature = Convert.FromBase64String(signatureBase64);

                    return rsa.VerifyData(
                    data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                }
            }
            catch
            {
                return false;

            }
        }
    }
}

