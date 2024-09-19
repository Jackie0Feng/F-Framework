namespace FFramework.Ideas.Tool.Database
{
	public interface IDatabase
	{
		uint GetTypeID();
		string GetDataPath();
		void LoadResources();
	}
}

