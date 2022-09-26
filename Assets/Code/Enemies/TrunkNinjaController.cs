using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TrunkNinjaController : EnemyController {

	public GameObject smoke;

	public HittingState 	hittingState;

	public override void Awake()
	{
		//if(Random.Range(0,100) >= 95)
		//	velocity *= 2;
		base.Awake();

		Fsm.AddFSMState (hittingState);
		Fsm.ChangeState(movingState);

		GetComponent<LifeModule>().OnKill += HandleOnKill;
		GetComponent<LifeModule>().OnLifeChange += HandleOnLifeChange;
	}
	
	void HandleOnLifeChange (LifeModule _who, float _currentLife, float _previous, float _percentage)
	{
		if(_previous > _currentLife)
		{
			SoundManager.PlaySFX("TrunkNinjaHit");
			Fsm.ChangeState(hittingState);
			GetComponent<LifeModule>().OnLifeChange -= HandleOnLifeChange;
		}
	}

	public override void Attack ()
	{
		base.Attack ();
		SoundManager.PlaySFX("TrunkNinjaAttack");
	}
	
	void HandleOnKill (GameObject _who)
	{
		SoundManager.PlaySFX("TrunkNinjaDie");
		Fsm.ChangeState(deathState);
	}
	#region states
	
	[System.Serializable]
	public class HittingState : FSM.FSMState
	{
		
		private TrunkNinjaController myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as TrunkNinjaController;

		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			myOwner.animator.SetTrigger("Idle");

			Instantiate(myOwner.smoke, myOwner.transform.position, myOwner.transform.rotation);

			SoundManager.PlaySFX("TrunkNinjaFx");

			if(myOwner.transform.position.x > 0)
			{
				myOwner.transform.position = new Vector2( -2.5f, myOwner.transform.position.y);
				myOwner.transform.DOLocalRotate(new Vector3(0,180,0),0);
			}
			else
			{
				myOwner.transform.position = new Vector2( 2.5f, myOwner.transform.position.y);
				myOwner.transform.DOLocalRotate(Vector3.zero,0);
			}
			fsm.ChangeState(myOwner.movingState);
			
		}
		
	}
	
	#endregion
}
