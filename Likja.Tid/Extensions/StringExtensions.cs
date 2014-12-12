using System;
using System.Collections.Generic;
using System.Linq;

namespace Likja.Tid.Extensions
{
    internal static class StringExtensions
    {
        public static string ToBase64(this string content)
        {
            // convert to base64
            var byteArr = new List<byte>();
            content.ToArray().ToList().ForEach(x => byteArr.Add(Convert.ToByte(x)));
            return Convert.ToBase64String(byteArr.ToArray());
        }

        public static string FromBase64(this string content)
        {
            var byteArr2 = Convert.FromBase64String(content);
            var charArr = new List<char>();
            byteArr2.ToList().ForEach(x => charArr.Add(Convert.ToChar(x)));
            return string.Join("", charArr);
        }

    }
}
