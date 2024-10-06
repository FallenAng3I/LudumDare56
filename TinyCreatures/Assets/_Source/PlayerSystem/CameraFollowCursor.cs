using Cinemachine;
using UnityEngine;

sealed public class CameraFollowCursor : MonoBehaviour
{
    public Transform player;
    public CinemachineVirtualCamera cam;
    public float followRadius = 2f;
    public float smoothSpeed = 0.1f;
    public float maxCameraMoveSpeed = 5f;

    private CinemachineFramingTransposer transposer;
    private Vector3 mouseWorldPos;
    private Vector3 playerPos;
    private Vector3 offset;
    private Vector3 smoothedOffset;

    void Start()
    {
        transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
        smoothedOffset = Vector3.zero;
    }

    void Update()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        
        playerPos = player.position;
        offset = mouseWorldPos - playerPos;
        
        if (offset.magnitude > followRadius)
        {
            offset = offset.normalized * followRadius;
        }
        
        smoothedOffset = Vector3.Lerp(smoothedOffset, offset, smoothSpeed);
        
        Vector3 currentOffset = transposer.m_TrackedObjectOffset;
        Vector3 desiredOffset = new Vector3(smoothedOffset.x, smoothedOffset.y, 0);
        
        Vector3 limitedOffset = Vector3.MoveTowards(currentOffset, desiredOffset, maxCameraMoveSpeed * Time.deltaTime);
        
        transposer.m_TrackedObjectOffset = limitedOffset;
    }
}
