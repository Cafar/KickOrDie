using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SimpleNinjaController : EnemyController {

	
	public override void Awake()
	{
		if(Random.Range(0,100) >= 90)
			velocity *= 2;
		base.Awake();


		Fsm.ChangeState(movingState);


		
		GetComponent<LifeModule>().OnKill += HandleOnKill;
	}

	public override void Attack ()
	{
		base.Attack ();
		SoundManager.PlaySFX("SimpleNinjaAttack");
	}
	
	
	void HandleOnKill (GameObject _who)
	{
		SoundManager.PlaySFX("SimpleNinjaDie");
		SoundManager.PlaySFX("SimpleNinjaHit");
		Fsm.ChangeState(deathState);
	}
}
