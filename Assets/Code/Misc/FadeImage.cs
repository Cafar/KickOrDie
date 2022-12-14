// ************************************************************************ 
// File Name:   FadeSprite.cs 
// Purpose:    	Fades sprite in or out.
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2013 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// ************************************************************************ 
// Class: FadeSprite
// ************************************************************************ 
using UnityEngine.UI;


public class FadeImage : MonoBehaviour {

	
	// ********************************************************************
	// Exposed Data Members 
	// ********************************************************************
	[SerializeField]
	private bool m_startsVisible = false;
	[SerializeField]
	private bool m_fadeOnAwake = false;
	[SerializeField]
	private bool m_continuous = false;
	[SerializeField]
	private float m_fadeSpeed = 1.0f;
	[SerializeField]
	private float m_minAlpha = 0;
	[SerializeField]
	private float m_maxAlpha = 1.0f;


	// ********************************************************************
	// Private Data Members 
	// ********************************************************************
	private Image m_Image = null;


	// ********************************************************************
	// Properties 
	// ********************************************************************
	public bool isVisible {
		get { 
			if (m_Image.color.a == m_maxAlpha)
				return true;
			else return false;
		}
	}
	public bool isHidden {
		get { 
			if (m_Image.color.a == m_minAlpha)
				return true;
			else return false;
		}
	}
	public float fadeSpeed {
		get { return m_fadeSpeed; }
		set { m_fadeSpeed = value; }
	}
	public float minAlpha {
		get { return m_minAlpha; }
		set { m_minAlpha = value; }
	}
	public float maxAlpha {
		get { return m_maxAlpha; }
		set { m_maxAlpha = value; }
	}
	public bool continuous {
		get { return m_continuous; }
		set { m_continuous = value; }
	}


	// ********************************************************************
	// Function:	Start()
	// Purpose:		Run when new instance of the object is created.
	// ********************************************************************
	void Start () {
		m_Image = GetComponent<Image>();
		if (m_Image == null)
		{
			Debug.LogError("FadeImage: No Image found!");
			return;
		}
		
		
		if (m_startsVisible)
		{
			Color spriteColor = m_Image.color;
			spriteColor.a = m_maxAlpha;
			m_Image.color = spriteColor;
			if (m_fadeOnAwake)
				StartCoroutine(FadeOut());
		}
		else
		{
			Color spriteColor = m_Image.color;
			spriteColor.a = m_minAlpha;
			m_Image.color = spriteColor;
			if (m_fadeOnAwake)
				StartCoroutine(FadeIn());
		}
	}

	
	// ********************************************************************
	// Function:	Fade()
	// Purpose:		Tells the sprite to fade in or out
	// ********************************************************************
	public IEnumerator FadeIn()
	{
		Color spriteColor = m_Image.color;

		while (spriteColor.a < m_maxAlpha)
		{
			yield return null;
			spriteColor.a += m_fadeSpeed * Time.deltaTime;
			m_Image.color = spriteColor;
		}

		spriteColor.a = m_maxAlpha;
		m_Image.color = spriteColor;

		if (m_continuous)
			StartCoroutine(FadeOut());
	}

	// ********************************************************************
	// Function:	FadeIn()
	// Purpose:		Tells the sprite to fade in
	// ********************************************************************
	public IEnumerator FadeOut()
	{
		Color spriteColor = m_Image.color;

		while (spriteColor.a > m_minAlpha)
		{
			yield return null;
			spriteColor.a -= m_fadeSpeed * Time.deltaTime;
			m_Image.color = spriteColor;
		}
		spriteColor.a = m_minAlpha;
		m_Image.color = spriteColor;
		
		if (m_continuous)
			StartCoroutine(FadeIn());
	}

}
