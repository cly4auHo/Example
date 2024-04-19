using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generator
{
    public class ExampleGenerator : IExampleGenerator
    {
        private const int MIN_ANSWERS_AMOUNT = 2;
        private const int MAX_ANSWERS_AMOUNT = 10;
        
        private static readonly Operation[] EASY = {Operation.Addition, Operation.Subtraction};
        private static readonly Operation[] HARD = {Operation.Addition, Operation.Subtraction, Operation.Multiplication, Operation.Division};

        private int _amountOfAnswers;

        public void Init(in int amountOfAnswers)
        {
            _amountOfAnswers = Mathf.Clamp(amountOfAnswers, MIN_ANSWERS_AMOUNT, MAX_ANSWERS_AMOUNT);
        }
        
        public Example Generate(in bool isEasy, in int minValue, in int maxValue)
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

        private Example Addition(in int minValue, in int maxValue)
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

        private Example Subtraction(in int minValue, in int maxValue)
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

        private Example Multiplication(in int minValue, in int maxValue)
        {
            var answer = Random.Range(1 + minValue, maxValue + 1);
            var divisors = MathUtil.GetDivisors(answer);
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

        private Example Division(in int minValue, in int maxValue)
        {
            var first = Random.Range(1 + minValue, maxValue + 1);
            var divisors = MathUtil.GetDivisors(first);
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

        private int[] GetAnswers(in int answer)
        {
            var answers = new int[_amountOfAnswers];
            var random = Random.Range(0, answers.Length);
            answers[random] = answer;

            for (int i = 0; i < answers.Length; i++)
            {
                if (i != random)
                    answers[i] = RandomNear(answer, answers);
            }

            return answers;
        }

        private int RandomNear(in int value, int[] values)
        {
            var random = Random.Range(value - _amountOfAnswers, value + _amountOfAnswers + 1);
            return values.Contains(random) ? RandomNear(value, values) : random;
        }
    }
}