using System;
[Serializable]
public class DduduData
{
	public int id;
	public DduduInfo info;
	public int satiety;
	public int like;
	public int interest = -1;
	public float x, y, z;
	public bool isGemIconActive;
	public bool isWork;

    public DduduData(DduduInfo info)
    {
		var random = new Random();
		this.id = random.Next();
		this.info = info;
		this.satiety = info.maxFull;
		(x, y, z) = (0, 0, 0);
    }

	public DduduData DduduDataSave(Ddudu ddudu)
	{
		this.x = ddudu.transform.position.x;
		this.y = ddudu.transform.position.y;
		this.z = ddudu.transform.position.z;
		this.isGemIconActive = ddudu.IconGem.activeSelf;
		this.isWork = ddudu.data.isWork;
		return this;
	}
}
