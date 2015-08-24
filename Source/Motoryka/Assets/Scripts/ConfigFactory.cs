using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using System;

public class ConfigFactory : MonoBehaviour {

	public static Config CreateEasyLevel()
	{
		Config config = new Config { 
			ConfigNr = Random.Range(0, 1000), 
			UserId = Random.Range(1, 100), 
			Username = "Nojas", 
			TeacherName = "Magda Koks", 
			CreateDate = DateTime.Now, 
			NrOfLevels = 1 
		};
		return config;
	}

	public static Config CreateMediumLevel()
	{
		Config config = new Config { 
			ConfigNr = Random.Range(0, 1000), 
			UserId = Random.Range(1, 100), 
			Username = "Lysy", 
			TeacherName = "Magda Kroks", 
			CreateDate = DateTime.Now, 
			NrOfLevels = 2
		};
		return config;
	}

	public static Config CreateHardLevel()
	{
		Config config = new Config { 
			ConfigNr = Random.Range(0, 1000), 
			UserId = Random.Range(1, 100), 
			Username = "Jezyna", 
			TeacherName = "Magda Mops", 
			CreateDate = DateTime.Now, 
			NrOfLevels = 3 
		};
		return config;
	}
}
