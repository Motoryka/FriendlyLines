using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;

public class ConfigLoader : MonoBehaviour {

	// serialize config and save to data dir
	public static void SerializeConfig(Config details, string filename)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(Config));

		using (TextWriter writer = new StreamWriter (Application.persistentDataPath + "/" + filename + ".xml"))
		{
			serializer.Serialize(writer, details);
		}
	}

	//deserialize config from data dir
	public static Config DeserializeConfig(string filename)
	{
		Config config = null;
		XmlSerializer serializer = new XmlSerializer(typeof(Config));
		StreamReader reader = new StreamReader (Application.persistentDataPath + "/" + filename);
		config = (Config)serializer.Deserialize (reader);
		reader.Close ();

		return config;
	}
}
