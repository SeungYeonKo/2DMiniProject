using UnityEngine;
using Cinemachine;

public class FixedCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;  // 드래그 앤 드롭을 통해 할당
    private float fixedYPosition = 0.66f; // 고정할 Y축 위치
    private float fixedYOffset = 3f; // 고정할 Y축 위치

    void Start()
    {
        // 카메라의 초기 Y 위치를 현재 위치로 설정

        // CinemachineFramingTransposer 구성 요소가 있다면 해당 설정 조정
     
    }

    void LateUpdate()
    {
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y = fixedYOffset;
        virtualCamera.transform.position= new Vector3(virtualCamera.transform.position.x, fixedYOffset, virtualCamera.transform.position.z);
     }
}