using System.Collections.Generic;

namespace Generator
{
    public static class MathUtil
    {
        public static List<int> GetDivisors(in int n)
        {
            var divisors = new List<int> {1};

            if (n <= 1 || IsPrime(n))
                return divisors;

            for (int i = 2; i * i <= n; i++)
            {
                if (n % i == 0)
                    divisors.Add(i);
            }

            divisors.Sort();

            return divisors;
        }

        static bool IsPrime(in int number)
        {
            if (number <= 1)
                return false;

            if (number % 2 == 0)
                return number == 2;

            if (number % 3 == 0)
                return number == 3;

            for (int i = 5; i * i <= number; i += 6)
            {
                if (number % i == 0 || number % (i + 2) == 0)
                    return false;
            }

            return true;
        }
    }
}