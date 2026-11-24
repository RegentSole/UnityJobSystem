using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct LogCalculationJob : IJobParallelFor
{
    public NativeArray<float> Results;
    public float MinValue;
    public float MaxValue;
    public uint Seed;

    public void Execute(int index)
    {
        // Инициализируем генератор случайных чисел для каждого индекса,
        // чтобы избежать конфликтов в многопоточной среде
        var random = new Unity.Mathematics.Random(Seed + (uint)index);
        
        // Генерируем случайное число от 1 до 100
        float randomValue = random.NextFloat(MinValue, MaxValue);
        
        // Вычисляем натуральный логарифм
        // Логарифм от числа от 1 до 100 будет в диапазоне [0, 4.605]
        Results[index] = Mathf.Log(randomValue);
    }
}