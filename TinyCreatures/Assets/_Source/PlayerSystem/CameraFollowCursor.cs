using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class CameraFollowCursor : MonoBehaviour
{
    public Transform player;             // Игрок, за которым следует камера
    public CinemachineVirtualCamera cam; // Virtual Camera от Cinemachine
    public float followRadius = 2f;      // Радиус, в пределах которого камера может смещаться к курсору
    public float smoothSpeed = 0.1f;     // Скорость сглаживания
    public float maxCameraMoveSpeed = 5f; // Максимальная скорость смещения камеры

    private CinemachineFramingTransposer transposer;
    private Vector3 mouseWorldPos;
    private Vector3 playerPos;
    private Vector3 offset;
    private Vector3 smoothedOffset;      // Переменная для плавного перехода

    void Start()
    {
        // Получаем компонент FramingTransposer
        transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
        smoothedOffset = Vector3.zero;   // Инициализация сглаженного смещения
    }

    void Update()
    {
        // Получаем мировую позицию курсора
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;  // Обнуляем Z координату для 2D

        // Вычисляем смещение курсора относительно игрока
        playerPos = player.position;
        offset = mouseWorldPos - playerPos;

        // Ограничиваем смещение радиусом
        if (offset.magnitude > followRadius)
        {
            offset = offset.normalized * followRadius;
        }

        // Плавно интерполируем смещение
        smoothedOffset = Vector3.Lerp(smoothedOffset, offset, smoothSpeed);

        // Ограничиваем скорость движения камеры
        Vector3 currentOffset = transposer.m_TrackedObjectOffset;
        Vector3 desiredOffset = new Vector3(smoothedOffset.x, smoothedOffset.y, 0);

        // Ограничиваем скорость смещения
        Vector3 limitedOffset = Vector3.MoveTowards(currentOffset, desiredOffset, maxCameraMoveSpeed * Time.deltaTime);

        // Устанавливаем ограниченное и сглаженное смещение камеры
        transposer.m_TrackedObjectOffset = limitedOffset;
    }
}
