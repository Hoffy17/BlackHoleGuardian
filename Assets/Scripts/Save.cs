//------------------------------------------------------------
// Simple save system by Richard Stern
// No copyright, you can use this for whatever you want.
//------------------------------------------------------------
// This system lets you save variables between scenes and also save them to a file
//
// Note that you cannot save a whole GameObject, you have to save the individual
// variables (health, position, etc) that make up the object and then use them
// to recreate the object in the new scene.
//
// To save a variable:
// Save.SetInt("Health", 100);
//
// To load a variable:
// int health = Save.GetInt("Health");
//
// You can reset the system (destroying all saved variables) like this:
// Save.Reset();
//
// You can save all variables to a file:
// Save.SaveToFile("myFileName.sav");
//
// You can load all variables from a file:
// Save.LoadFromFile("myFileName.sav");
//------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System;

//------------------------------------------------------------
//------------------------------------------------------------
public static class Save
{
	private static Dictionary<string, object> database = new Dictionary<string, object>();

	[XmlInclude(typeof(Vector3))]
	public class Entry
	{
		public string key;
		public object value;
		public Entry() { }
		public Entry(string key, object value) { this.key = key; this.value = value; }
	}

	//------------------------------------------------------------
	//------------------------------------------------------------
	public static void Reset()
	{
		database.Clear();
	}

	//------------------------------------------------------------
	//------------------------------------------------------------
	public static void SetValue(string name, object value)
	{
		if(value.GetType() == typeof(GameObject))
			Debug.LogError("Save System: GameObjects cannot be stored in the Save system. Store the individual variables instead.");

		if(database.ContainsKey(name))
			database[name] = value;
		else
			database.Add(name, value);
	}

	//------------------------------------------------------------
	//------------------------------------------------------------
	public static T GetValue<T>(string name)
	{
		try
		{
			if(database.ContainsKey(name))
				return (T)database[name];
		}
		catch(InvalidCastException e)
		{
			Debug.LogError("Save System: Attempting to get '" + name + "' but it's the wrong type. Did you use the correct function?");
			return default;
		}

		Debug.LogError("Save System: Cannot retrieve value '" + name + "'. Did you spell it wrong?");
		return default;
	}

	//------------------------------------------------------------
	//------------------------------------------------------------
	public static bool Contains(string name)
	{
		return database.ContainsKey(name);
	}

	//------------------------------------------------------------
	//------------------------------------------------------------
	public static void SaveToFile(string filename)
	{
		try
		{
			using(TextWriter textWriter = File.CreateText(filename))
			{
				List<Entry> entries = new List<Entry>(database.Count);
				foreach(var pair in database)
				{
					entries.Add(new Entry(pair.Key, pair.Value));
				}

				XmlSerializer serializer = new XmlSerializer(typeof(List<Entry>));
				serializer.Serialize(textWriter, entries);
			}
		}
		catch(Exception e)
		{
			Debug.LogError("Save System: Cannot save file. Error was: " + e.Message);
		}
	}

	//------------------------------------------------------------
	//------------------------------------------------------------
	public static void LoadFromFile(string filename)
	{
		Reset();

		try
		{
			using(TextReader textReader = File.OpenText(filename))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(List<Entry>));
				List<Entry> list = (List<Entry>)serializer.Deserialize(textReader);
				foreach(Entry entry in list)
				{
					database.Add(entry.key, entry.value);
				}
			}
		}
		catch(Exception e)
		{
			Debug.LogError("Save System: Cannot load file. Error was: " + e.Message);
		}
	}

	//------------------------------------------------------------
	//------------------------------------------------------------
	public static void SetInt(string name, int value)
	{
		SetValue(name, value);
	}

	//------------------------------------------------------------
	//------------------------------------------------------------
	public static void SetFloat(string name, float value)
	{
		SetValue(name, value);
	}

	//------------------------------------------------------------
	//------------------------------------------------------------
	public static void SetBool(string name, bool value)
	{
		SetValue(name, value);
	}

	//------------------------------------------------------------
	//------------------------------------------------------------
	public static void SetString(string name, string value)
	{
		SetValue(name, value);
	}

	//------------------------------------------------------------
	//------------------------------------------------------------
	public static void SetVector3(string name, Vector3 value)
	{
		SetValue(name, value);
	}

	//------------------------------------------------------------
	//------------------------------------------------------------
	public static int GetInt(string name)
	{
		return GetValue<int>(name);
	}

	//------------------------------------------------------------
	//------------------------------------------------------------
	public static float GetFloat(string name)
	{
		return GetValue<float>(name);
	}

	//------------------------------------------------------------
	//------------------------------------------------------------
	public static bool GetBool(string name)
	{
		return GetValue<bool>(name);
	}

	//------------------------------------------------------------
	//------------------------------------------------------------
	public static string GetString(string name)
	{
		return GetValue<string>(name);
	}

	//------------------------------------------------------------
	//------------------------------------------------------------
	public static Vector3 GetVector3(string name)
	{
		return GetValue<Vector3>(name);
	}
}
