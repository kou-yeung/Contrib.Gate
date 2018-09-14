using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;

class AesGen
{
    static void Main()
    {
        var managed = new AesManaged();
        List<string> s = new List<string>
        {
            string.Format("IV:{0}", Convert.ToBase64String(managed.IV)),
            string.Format("Key:{0}", Convert.ToBase64String(managed.Key))
        };
        File.WriteAllLines("aes.txt", s.ToArray());
    }
}
