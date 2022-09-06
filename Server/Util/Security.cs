using System;
using System.Security.Cryptography;
using System.Text;

namespace e_CommerceContinental.Server.Util;

class Security
{
    public static string Encrypt(string password)
    {
        var provider = SHA512.Create();
        string salt = "ThisSaltIsUncr@ble@-2300&^%$#@!";
        byte[] bytes = provider.ComputeHash(Encoding.UTF32.GetBytes(salt + password));
        var pass = BitConverter.ToString(bytes).Replace("-", "").ToLower();
        return pass;
    }
}
