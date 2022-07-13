[System.Serializable]
public class DduduInfo
{
    public int code;
	public string name;
	public string note;
	public string imgPath;
	public int maxFull;
	public int maxLike;
	public int gem1Id;
	public int gem2Id;

    public DduduInfo(int id, string name, string note, string imgPath, int maxFull,
					int maxLike, int gem1Id, int gem2Id)
    {
		this.code = id;
		this.name = name;
		this.note = note;
		this.imgPath = imgPath;
		this.maxFull = maxFull;
		this.maxLike = maxLike;
		this.gem1Id = gem1Id;
		this.gem2Id = gem2Id;
    }
}

