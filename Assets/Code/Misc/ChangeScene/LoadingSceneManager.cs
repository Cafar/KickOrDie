// ************************************************************************ 
// File Name:   ScreenManager.cs 
// Purpose:    	Transfers between scenes
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2015 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;


// ************************************************************************ 
// Class: LoadingSceneManager
// ************************************************************************
using UnityEngine.SceneManagement;


public class LoadingSceneManager : Singleton<LoadingSceneManager> 
{

	// ********************************************************************
	// Function:	UnloadLoadingScene()
	// Purpose:		Destroys the loading scene
	// ********************************************************************
	public static void UnloadLoadingScene()
	{
		//SceneManager.UnloadScene("LoadingScreen");
		GameObject.Destroy(instance.gameObject);
	}
}
