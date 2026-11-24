using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;

public class OrbitSpawner : MonoBehaviour
{
    [Header("Настройки спавна")]
    public int objectCount = 5000; // Количество объектов
    public GameObject prefab; // Префаб с мешем высокой полигональности
    public float radius = 50f; // Радиус орбиты

    // TransformAccessArray для доступа к трансформам внутри джоба
    private TransformAccessArray _transformAccessArray;

    void Awake()
    {
        // Создаем массив для хранения трансформов
        Transform[] transforms = new Transform[objectCount];
        
        for (int i = 0; i < objectCount; i++)
        {
            // Вычисляем начальную позицию на круге, чтобы объекты не появлялись в одной точке
            float angle = i * Mathf.PI * 2f / objectCount;
            Vector3 initialPos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            
            // Спавним объект
            GameObject obj = Instantiate(prefab, initialPos, Quaternion.identity, transform);
            obj.name = $"OrbitObject_{i}";
            transforms[i] = obj.transform;
        }
        
        // Инициализируем TransformAccessArray
        _transformAccessArray = new TransformAccessArray(transforms);
    }

    void Update()
    {
        // Создаем и настраиваем джоб
        var orbitJob = new OrbitJob
        {
            DeltaTime = Time.deltaTime,
            Speed = 1.0f, // Скорость вращения
            Radius = radius
        };

        // Планируем выполнение джоба
        JobHandle handle = orbitJob.Schedule(_transformAccessArray);
        
        // Завершаем джоб в этом же кадре, чтобы трансформации применились
        // Для максимальной производительности можно использовать handle.Complete() позже,
        // но для корректного отображения в кадре часто требуется синхронизация.
        handle.Complete();
    }

    void OnDestroy()
    {
        // Обязательно освобождаем нативную память
        if (_transformAccessArray.isCreated)
        {
            _transformAccessArray.Dispose();
        }
    }
}