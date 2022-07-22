using UnityEngine;

[System.Serializable]
public class BuildingData
{
    public int id;
    public BuildingInfo info;
    public bool isBuilded;
    public int cycleRemainTime;
    public int workerId;
    public bool isDone;
    public float x, y, z;

    public BuildingData(BuildingInfo info)
    {
        var ran = new System.Random();
        this.id = ran.Next();
        this.info = info;
        this.isBuilded = false;
        this.isDone = false;
        (x, y, z) = (0, 0, 0);
    }

    public void SetPos(Vector3 pos)
    {
        (this.x, this.y, this.z) = (pos.x, pos.y, pos.z);
    }
}