using UnityEngine;
using System;
using DG.Tweening;

public class BuildingAttrib : MonoBehaviour
{
    [SerializeField] private Ease buildingAnimationEase;
    public BuildingData data;
    protected int buildingId;
    protected DateTime m_AppQuitTime;

    private void OnApplicationFocus(bool focusStatus) 
    {
        if (focusStatus == true)
        {
            data = BuildingManager.Instance.GetData(buildingId);
            LoadAppQuitTime();
            SetRechargeScheduler();
        }
    }

    virtual public void SetRechargeScheduler(bool newCycle=false) {}

    public bool LoadAppQuitTime() 
    { 
        bool result = false; 
        try 
        { 
            if (PlayerPrefs.HasKey("AppQuitTime")) 
            { 
                var appQuitTime = PlayerPrefs.GetString("AppQuitTime"); 
                m_AppQuitTime = DateTime.FromBinary(Convert.ToInt64(appQuitTime)); 
            }
            else 
                m_AppQuitTime = DateTime.FromBinary(Convert.ToInt64(DateTime.Now.ToLocalTime().ToBinary().ToString())); 
            result = true; 
        } 
        catch (System.Exception e) 
        {
            Debug.LogError("LoadAppQuitTime Failed (" + e.Message + ")"); 
        } 
        return result; 
    }

    protected async void ExpansionAnimation()
    {
        var data = BuildingManager.Instance.GetData(buildingId);
        
        while (data.cycleRemainTime > 0)
        {
            try
            {
                await transform?.DOScaleY(1.1f, 0.5f).SetEase(buildingAnimationEase).AsyncWaitForCompletion();
                await transform?.DOScaleY(1, 0.5f).SetEase(buildingAnimationEase).AsyncWaitForCompletion();
            }
            catch
            {
                return;
            }
        }
    }
}
