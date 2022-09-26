using UnityEngine;
using System.Collections;

public class ShieldNinjaController: EnemyController {

	public float timeStuned = 2;
	

	public HittingState 	hittingState;


	public override void Awake()
	{
//		if(Random.Range(0,100) >= 95)
//			velocity *= 2;
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
			if(_currentLife > 0)
			{
				Fsm.ChangeState(hittingState);
				Invoke("ChangeState", timeStuned);
			}
		}
	}

	public override void Attack ()
	{
		base.Attack ();
		if(mainCharacter.GetComponent<LifeModule>().currentLife > 0)
			SoundManager.PlaySFX("ShieldNinjaAttack");
	}

	void HandleOnKill (GameObject _who)
	{
		SoundManager.PlaySFX("ShieldNinjaDie");
		Fsm.ChangeState(deathState);
	}

	void ChangeState()
	{
		Fsm.ChangeState(movingState);
	}

	#region states

	[System.Serializable]
	public class HittingState : FSM.FSMState
	{
		
		private ShieldNinjaController myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as ShieldNinjaController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			myOwner.animator.SetTrigger("Hit");
			SoundManager.PlaySFX("ShieldNinjaHit");
			Vector2 right = myOwner.transform.TransformDirection(Vector2.right);
			myOwner.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			myOwner.GetComponent<Rigidbody2D>().AddForce(new Vector2( right.x * 5, 10), ForceMode2D.Impulse);

		}
		
	}

	#endregion
}
