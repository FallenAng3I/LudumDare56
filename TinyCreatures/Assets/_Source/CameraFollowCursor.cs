using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class CameraFollowCursor : MonoBehaviour
{
    public Transform player;             // Игрок, за которым следует камера
    public CinemachineVirtualCamera cam; // Virtual Camera от Cinemachine
    public float followRadius = 2f;      // Радиус, в пределах которого камера может смещаться к курсору

    private CinemachineFramingTransposer transposer;
    private Vector3 mouseWorldPos;
    private Vector3 playerPos;
    private Vector3 offset;

    void Start()
    {
        // Получаем компонент FramingTransposer
        transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
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

        // Устанавливаем смещение камеры в сторону курсора
        transposer.m_TrackedObjectOffset = new Vector3(offset.x, offset.y, 0);
    }
}
