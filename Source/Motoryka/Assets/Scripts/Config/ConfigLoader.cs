/**********************************************************************
Copyright (C) 2015  Mateusz Nojek

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/

using System.IO;
using System.Xml.Serialization;

using UnityEngine;

public class ConfigLoader 
{

	// serialize config and save to data dir
	public static void SerializeConfig(Config details, string filename)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(Config));

		using (TextWriter writer = new StreamWriter (Application.persistentDataPath + "/configs/" + filename + ".xml"))
		{
			serializer.Serialize(writer, details);
		}
	}

	//deserialize config from data dir
	public static Config DeserializeConfig(string filename)
	{
		Config config = null;
		XmlSerializer serializer = new XmlSerializer(typeof(Config));
        StreamReader reader;
        try
        {
			reader = new StreamReader(Application.persistentDataPath + "/configs/" + filename + ".xml");
        }
        catch (FileNotFoundException e)
        {
            Debug.LogError(e.Message);
            return null;
        }

		config = (Config)serializer.Deserialize (reader);
		reader.Close();

		return config;
	}
}
