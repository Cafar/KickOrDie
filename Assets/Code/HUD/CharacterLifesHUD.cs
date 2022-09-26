using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class CharacterLifesHUD : MonoBehaviour {

	public Sprite life, noLife;

	public Transform prefabFace;

	private List<Transform> facesHUD;

	private LifeModule lifeChar;

	void Start()
	{
		Init();
	}

	public void Init () 
	{
		facesHUD = new List<Transform>();
		lifeChar = GameObject.FindGameObjectWithTag("MainCharacter").GetComponent<LifeModule>();

		lifeChar.currentLife = Blackboard.currentLifeMainCharacter;
		

		lifeChar.OnLifeChange += HandleOnLifeChange;

		for(int i = 0; i < Blackboard.currentLifeMainCharacter; i++)
		{
			GenerateOneLife(i);
		}
	
	}

	void HandleOnLifeChange (LifeModule _who, float _currentLife, float _previous, float _percentage)
	{
		//Si me quitan vida...
		if(_previous > _currentLife)
		{
			Blackboard.currentLifeMainCharacter = _currentLife;
			Image img = facesHUD[(int)_currentLife].GetComponent<Image>();
			img.sprite = noLife;
			img.DOFade(.5f,.2f).SetLoops(3,LoopType.Yoyo).OnComplete(()=>{
				img.DOFade(1f,.2f);}
				);
		}
		//Si consigo una vida
		else
		{
			if(lifeChar.currentLife > lifeChar.maxLife)
			{
				GenerateOneLife(facesHUD.Count);
			}
			else
			{
				facesHUD[(int)_currentLife-1].GetComponent<Image>().sprite = life;
			}
		}
	}


	void GenerateOneLife(int _i)
	{
		facesHUD.Add( Instantiate(prefabFace));
		facesHUD[_i].SetParent(transform, false);
	}

	void RemoveLifes()
	{

	}

}
