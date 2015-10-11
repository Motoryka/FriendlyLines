using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;

public class ConfigLoader : MonoBehaviour {

	public static string SaveConfig()
	{
		StreamWriter fileWriter = null;

		string fileName = Application.persistentDataPath + "/" + "GraMotoryka" + ".txt"; 
		fileWriter = File.CreateText(fileName); 
		fileWriter.WriteLine("Hello world"); 
		fileWriter.Close();
		return Application.persistentDataPath;
	}

	public static void SerializeConfig(Config details)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(Config));
		using (TextWriter writer = new StreamWriter(Application.persistentDataPath + "/" + "config" + ".txt"))
		{
			serializer.Serialize(writer, details);
		}
	}
}
