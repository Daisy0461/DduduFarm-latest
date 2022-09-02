using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public partial class Ddudu : DduduMovement, IPointerDownHandler, IPointerUpHandler, IDragHandler
{    
    public DduduData data;
    public float satietyTime = 0f;

    [Header("Children")]
    public GameObject IconGem;      // 자식 오브젝트로 있는 보석 표시
    public GameObject IconFeed;     // 먹이 말풍선

    [Header("Pointing")]
    Vector2 pos;
    CircleCollider2D circleCollider2D;
    SpriteRenderer render;

    [Header("Attribute")]
    public Craft interest; // 관심 건물             // 관심 건물에 해당하면 능률 증가

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] normalSounds;
    public AudioClip eatingSound;

    private bool isEnter = false;
    
#region Unity Method

	protected override void Start() 
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        render = GetComponent<SpriteRenderer>();
        InvokeRepeating("ChoseDir", 0f, 3f); 
    }

    protected override void Update() 
    {
        base.Update();
        render.sortingOrder = (int)(this.transform.position.y * -10) + (int)(transform.position.x * -10);

        if (IconGem.activeSelf == false && data.satiety > 0)  // 돌아다니고 있을 때
        {
            satietyTime += Time.deltaTime;
            if (satietyTime >= 1) // 분당 감소되는 포만도
            {
                satietyTime = 0;
                data.satiety -= 1;
            }
        }
        if (data.satiety <= 0)
        {
            data.satiety = 0;
            ActiveIconFeed();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (isEnter == false)   // for 1
        {
            isEnter = true;
            dir = Vector2.zero;
            CancelInvoke("ChoseDir");
            float ran = Random.Range(0.5f, 4f);
            InvokeRepeating("ChoseDir", ran, 3f); 
            isEnter = false;
        }
    }

#endregion

#region  Touch Method

    public void OnDrag(PointerEventData eventData)
    {  
        this.transform.position = Camera.main.ScreenToWorldPoint(eventData.position);
        this.transform.position -= new Vector3(0,0,-10);
    }


    public void OnPointerDown(PointerEventData eventData) 
    {    
        pos = dir;  // 잡으면 일단 뚜두 가만히 있기
        dir = Vector2.zero;
        CancelInvoke("ChoseDir");

        circleCollider2D.enabled = false;
        int ran = Random.Range(0,3);
        audioSource.clip = normalSounds[ran];
        audioSource.Play();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        circleCollider2D.enabled = true;
        dir = pos;
        InvokeRepeating("ChoseDir", 0f, 3f);
    }

#endregion

    public void ActiveIconFeed() // -> ActiveIconFeed
    {
        if (!IconFeed.activeSelf & !IconGem.activeSelf)
        {
            IconFeed.SetActive(true);
        }
    }    
}
