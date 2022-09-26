using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TrunkNinjaGirlController : EnemyController {

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
			SoundManager.PlaySFX("TrunkNinjaGirlHit");
			Fsm.ChangeState(hittingState);
			GetComponent<LifeModule>().OnLifeChange -= HandleOnLifeChange;
		}
	}

	public override void Attack ()
	{
		base.Attack ();
		SoundManager.PlaySFX("TrunkNinjaGirlAttack");
	}

	public void Disapear()
	{
		Instantiate(smoke, transform.position, transform.rotation);
		SoundManager.PlaySFX("TrunkNinjaGirlFX");

		if(Random.Range(0,100) > 50)
		{
			if(transform.position.x > 0)
			{
				transform.position = new Vector2( 2f, transform.position.y);
			}
			else
			{
				transform.position = new Vector2( -2f, transform.position.y);
			}
		}
		else
		{
			if(transform.position.x > 0)
			{
				transform.position = new Vector2( -2f, transform.position.y);
				transform.DOLocalRotate(new Vector3(0,180,0),0);
			}
			else
			{
				transform.position = new Vector2( 2f, transform.position.y);
				transform.DOLocalRotate(Vector3.zero,0);
			}
		}
		Fsm.ChangeState(movingState);
	}
	
	void HandleOnKill (GameObject _who)
	{
		SoundManager.PlaySFX("TrunkNinjaGirlDie");
		Fsm.ChangeState(deathState);
	}
	#region states
	
	[System.Serializable]
	public class HittingState : FSM.FSMState
	{
		
		private TrunkNinjaGirlController myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as TrunkNinjaGirlController;

		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			myOwner.animator.SetTrigger("Hit");

			//Instantiate(myOwner.smoke, myOwner.transform.position, myOwner.transform.rotation);

			Vector2 right = 	myOwner.transform.TransformDirection(Vector2.right);
			myOwner.rigid.velocity 	= Vector2.zero;
			myOwner.rigid.AddForce(new Vector2( right.x * 10, 0), ForceMode2D.Impulse);


			
		}
		
	}
	
	#endregion
}
