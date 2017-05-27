using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TexasHoldemShared.Security;

namespace TexasHoldemTests.shared
{
    [TestFixture]
    class SecurityHandlerTest
    {
        private readonly ISecurity _security = new SecurityHandler();

        [TestCase]
        public void EncryptTest()
        {
            var ptBytes = Encoding.UTF8.GetBytes("This is a plaintext msg");
            var result = _security.Encrypt(ptBytes);
            Assert.False(CompareArrays(result, ptBytes));
            Console.WriteLine("plainText: " + Encoding.UTF8.GetString(ptBytes));
            Console.WriteLine("result: " + Encoding.UTF8.GetString(result));
        }

        [TestCase]
        public void DecryptionTest()
        {
            var ptBytes = Encoding.UTF8.GetBytes("This is a plaintext msg");
            var encrypted = _security.Encrypt(ptBytes);
            var s = Encoding.UTF8.GetString(encrypted);
            s = _security.Decrypt(encrypted);
            Console.WriteLine(s);
        }

        private bool CompareArrays<T>(IReadOnlyList<T> arr1, IReadOnlyList<T> arr2)
        {
            bool ans = true;
            if (arr1.Count == arr2.Count)
            {
                for (int i = 0; i < arr1.Count; i++)
                {
                    if (!Equals(arr1[i], arr2[i]))
                    {
                        ans = false;
                        break;
                    }
                }
            }
            else
            {
                ans = false;
            }
            return ans;
        }
    }
}
