using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
	public SimpleAnimationImage[]			simpleAnimation;
	public Text								textLoading;
	

	void Start()
	{
		for(int i = 0; i < simpleAnimation.Length; i++)
		{
			simpleAnimation[i].gameObject.SetActive(false);
		}


		simpleAnimation[Random.Range(0,simpleAnimation.Length)].gameObject.SetActive(true);
		textLoading.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Loading).GetStringData(Blackboard.Language);
	}
}