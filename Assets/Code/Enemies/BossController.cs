using UnityEngine;
using System.Collections;
using DG.Tweening;


public class BossController : MonoBehaviour {



	public IdleState 		idleState;

	public float velocity;
	
	public float attackVelocity = 1;
	public float damage = 1;
	public float distanceToAttack = 1;

	protected Animator animator;

	public FSM	Fsm{get;set;}


	[System.Serializable]
	public class IdleState : FSM.FSMState
	{
		private BossController myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as BossController;
			
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			
			myOwner.animator.SetTrigger("Idle");
			
			//When is idle, velocity is 0 and the enemies make a movement like Street Of Rage ñaaaaa
			myOwner.GetComponent<Rigidbody2D>().velocity =  Vector2.zero;
			
		}
		
		public override void Update ()
		{
			base.Update ();
			
			
			
			
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			
		}
		
	}


}
