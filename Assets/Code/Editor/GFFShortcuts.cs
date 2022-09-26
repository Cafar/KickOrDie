using UnityEngine;
using System.Collections;
using UnityEditor;

public class GFFShortcuts {

	[MenuItem ("Rafa/Delete ALL SaveData (including playerPrefs)")]
	static void DeleteSaveGames () 
	{
		PlayerPrefs.DeleteAll();
	}
}
