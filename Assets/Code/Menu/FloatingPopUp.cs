using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class FloatingPopUp : MonoBehaviour {

	public virtual void Start () 
	{
		transform.localScale = Vector3.zero;
		transform.DOScale(Vector3.one, .2f).SetUpdate(true);
	}
	

	public virtual void Close()
	{
		transform.DOScale(Vector2.zero, .2f).SetUpdate(true).OnComplete(()=>{
			Destroy(transform.parent.gameObject);
			Time.timeScale = 1;
		}
		);
	}

	public void PlaySfxUI()
	{
		SoundManager.PlaySFX("UIButton");
	}
}
