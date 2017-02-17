using SQLite4Unity3d;

public class MazeEntity {

	public string xSize { get; set; }
	public string ySize { get; set; }

	public override string ToString ()
	{
		return string.Format ("xSize={0},  ySize={1}", xSize, ySize);
	}
}