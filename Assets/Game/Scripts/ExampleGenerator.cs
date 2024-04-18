using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public static class ExampleGenerator
{
    static readonly Operation[] EASY = {Operation.Addition, Operation.Subtraction};
    static readonly Operation[] HARD = {Operation.Addition, Operation.Subtraction, Operation.Multiplication, Operation.Division};
    
    public static int AmountOfAnswers { private get; set; }
    
    public static Example Generate(in bool isEasy, in int minValue, in int maxValue)
    {
        var operation = isEasy ? EASY[Random.Range(0, EASY.Length)] : HARD[Random.Range(0, HARD.Length)];

        return operation switch
        {
            Operation.Addition => Addition(minValue, maxValue),
            Operation.Subtraction => Subtraction(minValue, maxValue),
            Operation.Multiplication => Multiplication(minValue, maxValue),
            Operation.Division => Division(minValue, maxValue),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    static Example Addition(in int minValue, in int maxValue)
    {
        var answer = Random.Range(2 + minValue, maxValue + 1); //min 1 + 1, if minValue 0
        var first = Random.Range(1, answer);
        var second = answer - first;
        
        return new Example
        {
            First = first,
            Second = second,
            Answer = answer,
            Operation = Operation.Addition,
            Answers = GetAnswers(answer)
        };
    }

    static Example Subtraction(in int minValue, in int maxValue)
    {
        var first = Random.Range(2 + minValue, maxValue + 1);
        var answer = Random.Range(1, first);
        var second = first - answer;
        
        return new Example
        {
            First = first,
            Second = second,
            Answer = answer,
            Operation = Operation.Subtraction,
            Answers = GetAnswers(answer)
        };
    }

    static Example Multiplication(in int minValue, in int maxValue)
    {
        var answer = Random.Range(1 + minValue, maxValue + 1);
        var divisors = GetDivisors(answer);
        var first = divisors[Random.Range(0, divisors.Count)];
        var second = answer / first;
        
        return new Example
        {
            First = first,
            Second = second,
            Answer = answer,
            Operation = Operation.Multiplication,
            Answers = GetAnswers(answer)
        };
    }

    static Example Division(in int minValue, in int maxValue)
    {
        var first = Random.Range(1 + minValue, maxValue + 1);
        var divisors = GetDivisors(first);
        var second = divisors[Random.Range(0, divisors.Count)];
        var answer = first / second;
        
        return new Example
        {
            First = first,
            Second = second,
            Answer = answer,
            Operation = Operation.Division,
            Answers = GetAnswers(answer)
        };
    }

    static int[] GetAnswers(in int answer)
    {
        var answers = new int[AmountOfAnswers];
        var random = Random.Range(0, answers.Length);
        answers[random] = answer;
        
        for (int i = 0; i < answers.Length; i++)
        {
            if (i != random)
                answers[i] = RandomNear(answer, answers);
        }

        return answers;
    }
    
    static int RandomNear(in int value, int[] values)
    {
        var random = Random.Range(value - AmountOfAnswers, value + AmountOfAnswers + 1);
        return values.Contains(random) ? RandomNear(value, values) : random;
    }
    
    static List<int> GetDivisors(in int n)
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