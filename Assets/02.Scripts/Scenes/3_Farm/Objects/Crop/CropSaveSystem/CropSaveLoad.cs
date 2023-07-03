using UnityEngine;

[System.Serializable]       
public class CropDatas
{     
    public GameObject[] savedCropLevel = new GameObject[10];
}

public class CropSaveLoad : MonoBehaviour
{
    public CropDatas[] savedCropKind;

    public void SaveCrop()
    {
        CropSaveManager.Save(GameObject.FindObjectsOfType<CropGrowTime>());
    }

    public void LoadCrop()
    {
        AllCropsData save = CropSaveManager.Load();
        if (save == null) return;

        GameObject[] crops = GameObject.FindGameObjectsWithTag("Crop");

        var cropCount = save.cropSaveDatas.Length;
        for(int i = 0; i < cropCount; i++)
        {      //int i=save.farmSaveDatas.Length-1; i>=0; i--  //int i=0; i<=save.cropSaveDatas.Length-1; i++
            var saveData = save.cropSaveDatas[cropCount-1-i];
            GameObject genCrop = Instantiate(savedCropKind[saveData.cropKind - (int)DataTable.Crop - 1].savedCropLevel[saveData.cropLevel],     //종류 선택!
                                            new Vector3(saveData.x, saveData.y, saveData.z), 
                                            Quaternion.identity);

            CropGrowTime cropGrowTime = genCrop.GetComponent<CropGrowTime>();
            cropGrowTime.remainGrowTime = save.cropSaveDatas[save.cropSaveDatas.Length-1-i].remainGrowTimeSave;
        }

        for(int i = 0; i < crops.Length; i++)
        {
            Destroy(crops[i]);
        }
    }
}