using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DduduCanvas : MonoBehaviour
{   // 뚜두가 가진 캔버스 _ UI를 씬오브젝트에 갖다대는 용도
    // 자식: 보석(Gem)과 먹이(Feed) 이미지
    public GameObject ddudu;
    RectTransform canvas;
    public float height = 0.7f;

    private void Start() 
    {
        canvas = this.GetComponent<RectTransform>();    
    }

    private void Update() 
    {
        Vector3 _Pos = 
            Camera.main.WorldToScreenPoint(new Vector3(ddudu.transform.position.x, ddudu.transform.position.y + height, 0));
        canvas.position = _Pos;
    }
}
