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
// Class: ScreenManager
// ************************************************************************
using UnityEngine.SceneManagement;


public class ChangeSceneManager : MonoBehaviour  {
	
	
	// ********************************************************************
	// Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	private FadeImage m_blackScreenCover;
	[SerializeField]
	private float m_minDuration = 1.5f;

	private Coroutine co;

	private static ChangeSceneManager instance;
	public static ChangeSceneManager Instance 
	{
		get
		{
			if(instance == null)
			{
				string resourcesPrefabPath = "LoadingScreen";
				// Search in resources folder for this GameObject
				ChangeSceneManager managerPrefab = Resources.Load<ChangeSceneManager>(resourcesPrefabPath);
				
				if(managerPrefab == null)
				{
					Debug.LogError("[ERROR] Prefab "+resourcesPrefabPath+" not found in Resources directory");
					return null;
				}
				
				Instance = Instantiate(managerPrefab) as ChangeSceneManager;
			}
			
			return instance;
		}
		
		private set{
			instance = value;
		}
	}

	private void Awake()
	{
		instance = this;
	}

	public void ChangeScene(string _nameScene)
	{
		if(co == null)
			co = StartCoroutine(LoadSceneAsync(_nameScene));
	}

	// ********************************************************************
	// Function:	LoadScene()
	// Purpose:		Loads the supplied scene
	// ********************************************************************
	public IEnumerator LoadSceneAsync(string sceneName)
	{
		// Fade to black
		yield return StartCoroutine(m_blackScreenCover.FadeIn());
		
		// Load loading screen
		yield return SceneManager.LoadSceneAsync("LoadingScreen");
		//yield return Application.LoadLevelAsync("LoadingScreen");
		
		// !!! unload old screen (automatic)
		
		// Fade to loading screen
		yield return StartCoroutine(m_blackScreenCover.FadeOut());
		
		float endTime = Time.time + m_minDuration;

		SoundManager.SetCrossDuration(.1f);
		SoundManager.StopSFX();
		// Load level async
		yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		//yield return Application.LoadLevelAdditiveAsync(sceneName);
		
		while (Time.time < endTime)
			yield return null;
		
		// Play music or perform other misc tasks
		
		// Fade to black
		yield return StartCoroutine(m_blackScreenCover.FadeIn());
		
		// !!! unload loading screen
		LoadingSceneManager.UnloadLoadingScene();
		
		// Fade to new screen
		yield return StartCoroutine(m_blackScreenCover.FadeOut());

		co = null;
	}


}
