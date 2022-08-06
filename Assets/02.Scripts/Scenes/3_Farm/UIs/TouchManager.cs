using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchManager : MonoBehaviour
{
#region // 터치 시 UI 객체 on/off
    [Header("UI On Off")]
    public GameObject[] UIObj;
    private bool isOn = false;
    private Vector2 touchStartPosition, touchEndPosition;
    private Vector2 touchCurPosition;
#endregion

    [Space(10f)]    
#region // 줌인/줌아웃
    [Header("Zoom")]
    Camera cam;
    private float slideSpeed = 0.1f;

    private readonly float zoomSpeed = 1.8f;
    private readonly float zoomInMax = 5f;
    private readonly float zoomOutMax = 16f;
    // public float factor = 0.65f;

    private float scroll;
#endregion

    private void Awake() 
    {
        cam = FindObjectOfType<Camera>();
    }

    void Update () {
        // UIOnOff();
        ScreenPanning();
        ZoomInOut();
    }

    // void UIOnOff() {  
    //     // 1. 좌우로 빠지기
    //     // 2. 아래로 사라지기
    //         // off: 불투명도 내려가면서 y값 낮아지기
    //         // on:  불투명도 올라가면서 y값 높아지기
    //     if (Input.GetMouseButtonDown(0)) {
    //         touchStartPosition = Input.mousePosition;   // 탭 초기위치
    //         touchCurPosition = Input.mousePosition;
    //     }  
    //     else if (Input.GetMouseButtonUp(0)) {  
    //         touchEndPosition = Input.mousePosition;     // 탭 나중 위치. 초기 위치와 나중위치가 같다면 탭으로 인정
            
    //         float x = touchEndPosition.x - touchStartPosition.x;
    //         float y = touchEndPosition.y - touchStartPosition.y;
    //         if (Mathf.Abs((int)x) <= 1f && Mathf.Abs((int)y) <= 1f) {
    //             if (EventSystem.current.IsPointerOverGameObject()) {
    //                 return;
    //             }
    //             if (UIObj == null)
    //                 return;
    //             UIObjActiveManage();
    //         }
    //     }
    // }

    public void UIObjActiveManage(bool isActive=true)
    {
        for (int i = 0; i < UIObj.Length; i++) {
            UIObj[i].SetActive(isOn & isActive);
        }
        isOn = !(isOn & isActive);
    }

    public void ScreenPanning() 
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        if (results.Count > 0 && results[0].gameObject.layer == 5) // UI 레이어 외 오브젝트 클릭 시에도 패닝 가능
            return;

#if UNITY_ANDROID
        if (Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            var deltaPos = Input.GetTouch(0).deltaPosition;
            cam.transform.position -= (Vector3)(deltaPos) * slideSpeed * Time.deltaTime * cam.orthographicSize;

            // /* clamp */
            var clampX = (zoomOutMax - cam.orthographicSize) * cam.aspect;
            // var clampY = zoomOutMax - cam.orthographicSize + factor;
            var clampY = zoomOutMax - cam.orthographicSize + factor;
            var clampedPosX = Mathf.Clamp(cam.transform.position.x, -clampX, clampX);
            // var clampedPosY = Mathf.Clamp(cam.transform.position.y, -clampY, clampY + (2-(clampY-factor)/10));
            var clampedPosY = Mathf.Clamp(cam.transform.position.y, -clampY, clampY);
            cam.transform.position = new Vector3(clampedPosX, clampedPosY, cam.transform.position.z);
        }
#endif
#if !UNITY_ANDROID
        if (Input.GetMouseButtonDown(0)) touchCurPosition = Input.mousePosition;
        else if (Input.GetMouseButton(0))
        {
            var deltaPos = (Vector2)Input.mousePosition-touchCurPosition;
            cam.transform.position -= (Vector3)(deltaPos) * slideSpeed * Time.deltaTime * cam.orthographicSize;

            /* clamp */
            var clampX = (zoomOutMax - cam.orthographicSize) * cam.aspect;
            // var clampY = zoomOutMax - cam.orthographicSize + factor;
            var clampY = zoomOutMax - cam.orthographicSize;
            var clampedPosX = Mathf.Clamp(cam.transform.position.x, -clampX, clampX);
            // var clampedPosY = Mathf.Clamp(cam.transform.position.y, -clampY, clampY + (2-(clampY-factor)/10));
            var clampedPosY = Mathf.Clamp(cam.transform.position.y, -clampY, clampY);
            
            cam.transform.position = new Vector3(clampedPosX, clampedPosY, cam.transform.position.z);
            touchCurPosition = (Vector2)Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0)) touchCurPosition = Vector3.zero;
#endif
    }

    private void ZoomInOut()
    {
        if (Input.touchCount == 2) 
        { // 줌인 줌아웃
            /* get zoomAmount */
            var curTouchAPos = Input.GetTouch(0).position;                      // 현재 터치 중인 터치 1번 손가락
            var curTouchBPos = Input.GetTouch(1).position;                      // 현재 터치 중인 터치 2번 손가락
            var prevTouchAPos = curTouchAPos - Input.GetTouch(0).deltaPosition; // 이전 프레임 1번 손가락
            var prevTouchBPos = curTouchBPos - Input.GetTouch(1).deltaPosition; // 이전 프레임 2번 손가락
            var deltaDistance =
            Vector2.Distance(Normalize(curTouchAPos), Normalize(curTouchBPos))
            - Vector2.Distance(Normalize(prevTouchAPos), Normalize(prevTouchBPos));

            float curSize = cam.orthographicSize;   // 클수록 줌아웃
            var zoomAmount = deltaDistance * curSize * zoomSpeed;

            /* clamp & zoom */
            curSize -= zoomAmount;
            if (curSize < zoomInMax) {
                curSize = zoomInMax;
                zoomAmount = 0f;
            }
            if (zoomOutMax < curSize) {
                curSize = zoomOutMax;
                zoomAmount = 0f;
            }
            cam.orthographicSize = curSize;

            /* apply offset */
            // offset is a value against movement caused by scale up & down
            var pivotPos = cam.transform.position;
            var midX = (curTouchAPos.x + curTouchBPos.x) / 2;
            var midY = (curTouchAPos.y + curTouchBPos.y) / 2;
            var fromCenterToInputPos = new Vector3(                         // 터치 입력을 카메라 중심으로 보정
                midX - Screen.width * 0.5f, 
                midY - Screen.height * 0.5f, 0f);
            var fromPivotToInputPos = fromCenterToInputPos - pivotPos;
            var offsetX = (fromPivotToInputPos.x / curSize) * zoomAmount * 0.01f;
            var offsetY = (fromPivotToInputPos.y / curSize) * zoomAmount * 0.01f;
            cam.transform.position += new Vector3(offsetX, offsetY, 0f);

            /* clamp : prevent out of range */
            var clampX = (zoomOutMax * 2  * cam.aspect) / 2 - (cam.orthographicSize * 2 * cam.aspect) / 2;
            // var clampY = zoomOutMax - cam.orthographicSize + factor;
            var clampY = zoomOutMax - cam.orthographicSize;
            var clampedPosX = Mathf.Clamp(cam.transform.position.x, -clampX, clampX);
            // var clampedPosY = Mathf.Clamp(cam.transform.position.y, -clampY, clampY + (2-(clampY-factor)/10));
            var clampedPosY = Mathf.Clamp(cam.transform.position.y, -clampY, clampY);
            cam.transform.position = new Vector3(clampedPosX, clampedPosY, cam.transform.position.z);
        }
        if (0 !=(scroll = Input.GetAxis("Mouse ScrollWheel")*zoomSpeed))
        {
            float curSize = cam.orthographicSize;
            var zoomAmount = scroll* 0.5f * curSize * zoomSpeed;

            /* clamp & zoom */
            curSize -= zoomAmount;
            if (curSize < zoomInMax) {
                curSize = zoomInMax;
                zoomAmount = 0f;
            }
            if (zoomOutMax < curSize) {
                curSize = zoomOutMax;
                zoomAmount = 0f;
            }
            cam.orthographicSize = curSize;

            /* apply offset */
            // offset is a value against movement caused by scale up & down
            var pivotPos = cam.transform.position;
            var fromCenterToInputPos = new Vector3(                         // 터치 입력을 카메라 중심으로 보정
                Input.mousePosition.x - Screen.width * 0.5f, 
                Input.mousePosition.y - Screen.height * 0.5f, 0f);
            var fromPivotToInputPos = fromCenterToInputPos - pivotPos;
            var offsetX = (fromPivotToInputPos.x / curSize) * zoomAmount * 0.01f;
            var offsetY = (fromPivotToInputPos.y / curSize) * zoomAmount * 0.01f;
            cam.transform.position += new Vector3(offsetX, offsetY, 0f);

            /* clamp */
            var clampX = (zoomOutMax * 2  * cam.aspect) / 2 - (cam.orthographicSize * 2 * cam.aspect) / 2;
            // var clampY = zoomOutMax - cam.orthographicSize + factor;
            var clampY = zoomOutMax - cam.orthographicSize;
            var clampedPosX = Mathf.Clamp(cam.transform.position.x, -clampX, clampX);
            // var clampedPosY = Mathf.Clamp(cam.transform.position.y, -clampY, clampY + (2-(clampY-factor)/10));
            var clampedPosY = Mathf.Clamp(cam.transform.position.y, -clampY, clampY);
            cam.transform.position = new Vector3(clampedPosX, clampedPosY, cam.transform.position.z);
        }
    }

    private Vector2 Normalize(Vector2 position) { // 해상도 표준화
        var normalizedPos = new Vector2(
            (position.x - Screen.width * 0.5f) / (Screen.width * 0.5f),
            (position.y - Screen.height * 0.5f) / (Screen.height * 0.5f));
        return normalizedPos;
    }
}