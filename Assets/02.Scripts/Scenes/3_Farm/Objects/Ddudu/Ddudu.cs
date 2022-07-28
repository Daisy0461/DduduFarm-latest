using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public partial class Ddudu : DduduMovement, IPointerDownHandler, IPointerUpHandler, IDragHandler
{    
    [HideInInspector] public DduduManager DM;
    public DduduData data;
    public float satietyTime = 0f;

    [Header("Children")]
    public GameObject IconGem;      // 자식 오브젝트로 있는 보석 표시
    public GameObject IconFeed;     // 먹이 말풍선

    [Header("Pointing")]
    Vector2 pos;
    BoxCollider2D boxCollider2D;
    GameObject touchProtectPanel;

    [Header("Attribute")]
    public Craft interest; // 관심 건물             // 관심 건물에 해당하면 능률 증가

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] normalSounds;
    public AudioClip eatingSound;

    private bool isEnter = false;
    private WaitForSeconds sec1_5 = new WaitForSeconds(1.5f);
    

	protected override void Start() 
    {
        DM = DduduManager.Instance;
        if (transform.GetChild(0).childCount >= 2) touchProtectPanel = transform.GetChild(0).GetChild(1).gameObject;
        audioSource = GetComponent<AudioSource>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        base.Start();
        InvokeRepeating("ChoseDir", 0f, 3f); 
    }

    protected override void Update() 
    {
        this.GetComponent<SpriteRenderer>().sortingOrder = 
                    (int)(this.transform.position.y * -10) + (int)(transform.position.x * -5);
        base.Update();

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

    public void OnDrag(PointerEventData eventData)
    {  
        boxCollider2D.enabled = false;
        this.transform.position = Camera.main.ScreenToWorldPoint(eventData.position);
        this.transform.position -= new Vector3(0,0,-10);
    }

    public void OnPointerDown(PointerEventData eventData) 
    {    
        pos = dir;  // 잡으면 일단 뚜두 가만히 있기
        dir = Vector2.zero;
        CancelInvoke("ChoseDir");

        if (touchProtectPanel != null) touchProtectPanel.SetActive(true);
        int ran = Random.Range(0,3);
        audioSource.clip = normalSounds[ran];
        audioSource.Play();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (SceneManager.GetActiveScene().name == "Farm")
        {
            if (touchProtectPanel != null) touchProtectPanel.SetActive(false);
            boxCollider2D.enabled = true;
            dir = pos;
            InvokeRepeating("ChoseDir", 0f, 3f);
        }
    }

    public void ActiveIconFeed() // -> ActiveIconFeed
    {
        if (!IconFeed.activeSelf & !IconGem.activeSelf)
        {
            IconFeed.SetActive(true);
        }
    }

    
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (isEnter == false)
        {
            isEnter = true;
            dir = Vector2.zero;
            CancelInvoke("ChoseDir");
            float ran = Random.Range(0.5f, 4f);
            InvokeRepeating("ChoseDir", ran, 3f); 
            isEnter = false;
        }
    }

    public void ChoseDir() 
    { // 랜덤한 방향으로 움직인다.
        float h = Random.Range(-1f, 1f);
        float v = Random.Range(-1f, 1f);
        Vector2 moveDir = new Vector2(h, v).normalized;

        dir = moveDir;
    }
}
