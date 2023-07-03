public class CropTime : CropSaveLoad      
{
    void OnEnable()
    {
        LoadCrop();
    }

    private void OnDestroy() 
    {
        SaveManager.TrySaveAppQuitTime(); 
    }

    public void OnApplicationFocus(bool value)
    {
        if (value)
        {  
            LoadCrop();      
        }
        else
        {
            SaveCrop();
            SaveManager.TrySaveAppQuitTime();
        }
    }

    public void OnApplicationQuit()
    {
        SaveCrop();
        SaveManager.TrySaveAppQuitTime();
    }
}
