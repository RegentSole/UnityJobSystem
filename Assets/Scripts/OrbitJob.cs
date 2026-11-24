using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

public struct OrbitJob : IJobParallelForTransform
{
    public float DeltaTime;
    public float Speed;
    public float Radius;

    public void Execute(int index, TransformAccess transform)
    {
        // Получаем текущую позицию
        Vector3 currentPos = transform.position;
        
        // Вычисляем текущий угол на основе позиции (предполагая, что движение начинается с оси X)
        // Для стабильного вращения лучше использовать angle += speed * deltaTime,
        // но в данном случае мы просто движемся по кругу, сохраняя радиус.
        // Простой способ - поворот вектора позиции вокруг оси Y.
        
        // Вычисляем угол текущего положения
        float currentAngle = Mathf.Atan2(currentPos.z, currentPos.x);
        
        // Увеличиваем угол
        float newAngle = currentAngle + Speed * DeltaTime;
        
        // Вычисляем новую позицию на круге
        Vector3 newPos = new Vector3(Mathf.Cos(newAngle), 0, Mathf.Sin(newAngle)) * Radius;
        
        // Применяем позицию
        transform.position = newPos;
        
        // Опционально: можно добавить вращение самого объекта
        // transform.rotation = Quaternion.Euler(0, newAngle * Mathf.Rad2Deg, 0);
    }
}