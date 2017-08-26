using System;
using System.Collections.Generic;
using System.Text;

namespace ExmoApi.Extensions
{
    internal static class DateTimeExtensions
    {
        public static long ToUnixTimestamp(this DateTime dateTime) =>
            (long)(dateTime - new DateTime(1970, 1, 1)).TotalMilliseconds;
    }
}
