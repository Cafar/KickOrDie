using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FatNinjaController: EnemyController {

	public float timeStuned = .1f;

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
			SoundManager.PlaySFX(SoundManager.LoadFromGroup("FatNinjaHits"));
			SoundManager.PlaySFX("FatNinjaHurt");
			if(_currentLife == 2)
			{
				Fsm.ChangeState(hittingState);
				GetComponent<SpriteRenderer>().DOColor(Color.red, 2);
				Invoke("ChangeState", timeStuned);
			}
		}
	}

	public override void Attack ()
	{
		base.Attack ();
		SoundManager.PlaySFX("FatNinjaAttack");
	}
	
	void HandleOnKill (GameObject _who)
	{
		SoundManager.PlaySFX("FatNinjaDie");
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
		
		private FatNinjaController myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as FatNinjaController;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

				myOwner.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			
		}
		
	}
	#endregion
}
