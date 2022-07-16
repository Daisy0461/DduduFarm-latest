using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using E7.Native;

public class ShopItemSlotCreate : MonoBehaviour
{
    [HideInInspector] public ItemManager IM;
    [HideInInspector] public CropManager CM;
    [HideInInspector] public FishManager FM;
    public GameObject ShopItemPrefab;
    public GameObject parentContent;                    //  스크롤뷰 content

    public ShopItemInform selectedShopItem;
    public TalkTextScript talkTextScript;
    public OnClickBuyButton onClickBuyButton;
    public AudioSource audioSource;
    public AudioClip coinSound;
    // private NativeSource test;
    // private NativeAudioPointer sound;

    [Header("Info Popup - Cycle")]
    public GameObject popupCycle;
    public Text cycle_nameTxt;
    public Image cycle_iconImg;
    public Text cycle_noteTxt;
    public Text cycle_cycletimeTxt;
    public Image cycle_inputImg;
    public Text cycle_inputTxt;
    public Image cycle_outputImg;
    public Text cycle_outputTxt;

    [Header("Info Popup - user")]
    public GameObject popupUser;
    public Text user_nameTxt;
    public Image user_iconImg;
    public Text user_noteTxt;

    void Start()
    {  
        IM = ItemManager.Instance;

        CM = CropManager.Instance;
        FM = FishManager.Instance;
        ShopItemCreate();

        if(Application.platform == RuntimePlatform.Android){
            // sound = NativeAudio.Load(audioSource.clip);
        }
    }

    private void OnEnable() 
    {
        onClickBuyButton.OnClickBuyNo();    
    }

    public void ShopItemCreate()
    {   
        foreach (var info in IM.GetInfoDict())
        {
            if (info.Value.buyCost > 0)
            {
                var newItem = info.Value;
                var newObj = Instantiate(ShopItemPrefab, parentContent.transform);
                var shopItemInform = newObj.GetComponent<ShopItemInform>();

                shopItemInform.SIUI = this;
                shopItemInform.info = newItem;

                // 이미지 - 값 매칭
                Image newImg = newObj.transform.GetChild(0).GetComponent<Image>();
                newImg.sprite = Resources.Load<Sprite>(newItem.imgPath);
            
                Text[] newText = newObj.GetComponentsInChildren<Text>();
                newText[0].text = newItem.name;
                newText[1].text = string.Format("{0:#,##0}", newItem.buyCost);
                
                newObj.GetComponent<Button>().onClick.AddListener(() => OnclickCheckPurchasable());
            }
        }
    }

    public bool OnClickShopItemBuy()
    {
        if (IM.AddData(selectedShopItem.info.code, EncryptedPlayerPrefs.GetInt("BuyCount")) == false)
        {
            selectedShopItem = null;
            return false;
        }
        selectedShopItem = null;
        return true;
    }

    public void OnclickCheckPurchasable()
    {
        if (selectedShopItem == null) return;
        
        ItemInfo info = selectedShopItem.info;
        if (info.buyCost > IM.GetData((int)DataTable.Money).amount)
            talkTextScript.UnableToBuyText("그걸 구매하기엔 돈이 모자르군");
        else
            talkTextScript.GreetText();
    }

    public void PlayButtonSound(){
        #if UNITY_EDITOR
        audioSource.Play();
        #endif
        
        if(Application.platform == RuntimePlatform.Android){
            // test.Play(sound);
        }
    }
}
