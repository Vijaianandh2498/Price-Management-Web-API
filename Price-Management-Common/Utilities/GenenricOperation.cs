using System.Numerics;

namespace Price_Management_Common.Utilities
{
    public static class GenenricOperation
    {
        public static long GetBase36fromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Empty value.");
            value = value.ToUpper();
            bool negative = false;
            if (value[0] == '-')
            {
                negative = true;
                value = value[1..];
            }
            if (value.Any(c => !Constants.Digits.Contains(c)))
                throw new ArgumentException("Invalid value: \"" + value + "\".");
            var decoded = 0L;
            for (var i = 0; i < value.Length; ++i)
                decoded += Constants.Digits.IndexOf(value[i]) *
                    (long)BigInteger.Pow(Constants.Digits.Length, value.Length - i - 1);
            return negative ? decoded * -1 : decoded;
        }

        public static int ToCeiling(decimal value)
        {
            return (int)Math.Ceiling(value);
        }

        public static int GetLargeIntegerValue()
        {
            return Constants.LargeIntegerValue;
        }

        public static int GetRoundedPercentageValue(int value, int percentage)
        {
            var result = Convert.ToDecimal((decimal)value * percentage / 100);
            return (int)Math.Ceiling(result);
        }

        public static T GetEnumValueFromString<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string Readstaticfile(string filename, string filepath = "")
        {
            if (string.IsNullOrEmpty(filepath))
                filepath = Path.Combine(AppContext.BaseDirectory, @"Staticfiles/" + filename);
            else
                filepath += @"Staticfiles/" + filename;

            return File.ReadAllText(filepath);
        }


    }
}