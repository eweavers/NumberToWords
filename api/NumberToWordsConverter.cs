using System.Text;

// Turns monetary numbers into the required format, e.g. 123.45 becomes "ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS".
// Handles negatives and up to 2 decimal places as dollars and cents.
//
// Only tested/supported up to about 10^18 (the API rejects anything bigger before it
// gets here), so plain long math is enough - no need for BigInteger.
public static class NumberToWordsConverter
{
    private static readonly string[] Ones =
    {
        "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine",
        "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
        "Seventeen", "Eighteen", "Nineteen"
    };

    private static readonly string[] Tens =
    {
        "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"
    };

    // index 0 is deliberately empty - the ones/hundreds group doesn't get a scale word
    private static readonly string[] Scales =
    {
        "", "Thousand", "Million", "Billion", "Trillion", "Quadrillion", "Quintillion"
    };

    public static string Convert(long number)
    {
        if (number == 0)
            return Ones[0];

        var isNegative = number < 0;
        var n = Math.Abs(number);

        var groups = new List<int>();
        while (n > 0)
        {
            groups.Add((int)(n % 1000));
            n /= 1000;
        }

        var parts = new List<string>();
        for (var i = groups.Count - 1; i >= 0; i--)
        {
            if (groups[i] == 0)
                continue; // e.g. 1,000,000 has no "thousands" or "ones" group to say

            var chunk = ConvertUnderThousand(groups[i]);
            if (i > 0)
                chunk += " " + Scales[i];

            parts.Add(chunk);
        }

        var result = string.Join(", ", parts);
        return isNegative ? "Negative " + result : result;
    }

    public static string ConvertWithDecimals(decimal number)
    {
        var isNegative = number < 0;
        var abs = Math.Abs(number);

        var whole = (long)Math.Truncate(abs);
        var cents = (int)Math.Round((abs - whole) * 100, MidpointRounding.AwayFromZero);

        // 0.996 rounds up to 100 cents, which should really carry into the whole number
        if (cents == 100)
        {
            whole++;
            cents = 0;
        }

        var dollarWord = whole == 1 ? "DOLLAR" : "DOLLARS";
        var centWord = cents == 1 ? "CENT" : "CENTS";

        var words = $"{Convert(whole).ToUpperInvariant()} {dollarWord}";

        if (cents > 0)
            words += $" AND {ConvertUnderThousand(cents).ToUpperInvariant()} {centWord}";

        var result = isNegative && (whole != 0 || cents != 0)
            ? "NEGATIVE " + words
            : words;

        return result;
    }

    // handles anything from 0-999
    private static string ConvertUnderThousand(int number)
    {
        var sb = new StringBuilder();

        if (number >= 100)
        {
            sb.Append(Ones[number / 100]).Append(" Hundred");
            number %= 100;
            if (number > 0)
                sb.Append(" AND ");
        }

        if (number >= 20)
        {
            sb.Append(Tens[number / 10]);
            if (number % 10 > 0)
                sb.Append('-').Append(Ones[number % 10]);
        }
        else if (number > 0)
        {
            sb.Append(Ones[number]);
        }

        return sb.ToString();
    }
}