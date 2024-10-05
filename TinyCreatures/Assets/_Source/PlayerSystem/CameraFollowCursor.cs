using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class CameraFollowCursor : MonoBehaviour
{
    public Transform player;             // �����, �� ������� ������� ������
    public CinemachineVirtualCamera cam; // Virtual Camera �� Cinemachine
    public float followRadius = 2f;      // ������, � �������� �������� ������ ����� ��������� � �������

    private CinemachineFramingTransposer transposer;
    private Vector3 mouseWorldPos;
    private Vector3 playerPos;
    private Vector3 offset;

    void Start()
    {
        // �������� ��������� FramingTransposer
        transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    void Update()
    {
        // �������� ������� ������� �������
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;  // �������� Z ���������� ��� 2D

        // ��������� �������� ������� ������������ ������
        playerPos = player.position;
        offset = mouseWorldPos - playerPos;

        // ������������ �������� ��������
        if (offset.magnitude > followRadius)
        {
            offset = offset.normalized * followRadius;
        }

        // ������������� �������� ������ � ������� �������
        transposer.m_TrackedObjectOffset = new Vector3(offset.x, offset.y, 0);
    }
}
