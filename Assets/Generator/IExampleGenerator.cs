namespace Generator
{
    public interface IExampleGenerator
    {
        void Init(in int amountOfAnswers);
        Example Generate(in bool isEasy, in int minValue, in int maxValue);
    }
}