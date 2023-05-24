using UnityEngine;

public class FishFarmColor : MonoBehaviour
{
    private FishArea[] fishAreaList;

    public void Start()
    {
        fishAreaList = gameObject.GetComponentsInChildren<FishArea>();
    }

    public void ReturnColor()
    {
        for(int i=0; i<fishAreaList.Length; i++){
            fishAreaList[i].TurnToOrigin();
        }
    }
}
