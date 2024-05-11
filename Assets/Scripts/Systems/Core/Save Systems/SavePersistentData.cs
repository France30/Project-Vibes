using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public enum ObjectWithPersistentData
{
	enemy,
	musicSheetDrop,
	chordSetDrop,
	levelHealthDrop
}

public static class SavePersistentData
{
	private static readonly BinaryFormatter formatter = new BinaryFormatter();


	public static void SavePersistentFlag(ObjectWithPersistentData objectWithPersistentData, string id, bool flag)
	{
		string path = Application.persistentDataPath + "/" + objectWithPersistentData.ToString() + id + ".data";
		FileStream stream = new FileStream(path, FileMode.Create);

		PersistentFlagData persistentFlagData = new PersistentFlagData(id, flag);
		formatter.Serialize(stream, persistentFlagData);
		stream.Close();
	}

	public static bool LoadPersistentFlag(ObjectWithPersistentData objectWithPersistentData, string id)
	{
		string path = Application.persistentDataPath + "/" + objectWithPersistentData.ToString() + id + ".data";
		if (File.Exists(path))
		{
			FileStream stream = new FileStream(path, FileMode.Open);

			PersistentFlagData persistentFlagData = formatter.Deserialize(stream) as PersistentFlagData;
			stream.Close();

			return persistentFlagData.isTrue;
		}

		//Set true as the default value
		return true;
	}

	public static void ClearPersistentFlagData(ObjectWithPersistentData objectWithPersistentData, string id)
	{
		string path = Application.persistentDataPath + "/" + objectWithPersistentData.ToString() + id + ".data";
		if (!File.Exists(path)) return;

		File.Delete(path);
	}
}
