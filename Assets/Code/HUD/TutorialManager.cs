using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System;


public class TutorialManager : MonoBehaviour {

	public Image filledSkillBg, rightButton, leftButton;
	public Canvas leftKick, rightKick;
	public AutoType textCanvasText;
	public Transform CanvasText;
	public HUDManager hud;

	private AutoType.FinishTextHandler handler;
	

	public MainCharacterController mainCharacter;

	public EnemyController enemy1, enemy2, enemy3, enemy4;

	#region FSMSkill
	
	public FSM	FsmTuto{get;private set;}
	
	public Tuto1 				tuto1;
	
	public Tuto2 				tuto2;

	public Tuto3 				tuto3;

	public Tuto4 				tuto4;

	public Tuto5 				tuto5;

	public Tuto6 				tuto6;

	public Tuto7 				tuto7;
	
	
	#endregion




	void Start () 
	{
		//Initialize FSM
		FsmTuto = FSM.CreateFSM(this);

		FsmTuto.AddFSMState (tuto1, tuto2, tuto3, tuto4, tuto5, tuto6, tuto7);
		FsmTuto.ChangeState(tuto1);

		leftButton.enabled = false;
		rightButton.enabled = false;
		filledSkillBg.gameObject.SetActive(false);

		HUDManager.OnLeftClick 	+= 	HandleOnLeftClick;
		HUDManager.OnRightClick += 	HandleOnRightClick;
		mainCharacter.GetComponent<LifeModule>().OnLifeChange += HandleOnLifeChange;
		enemy1.GetComponent<LifeModule>().OnLifeChange += HandleOnLifeChangeEnemy;
		SkillController.OnDestroy += HandleOnDestroySkill;

	 	handler = null;
	}



	void HandleOnRightClick ()
	{
		//Cuando fallo el primer golpe aposta...
		if(FsmTuto.GetCurrentState() == tuto1)
		{
			//Espero hasta que el enemigo me golpee
			FsmTuto.ChangeState(tuto2);
		}

		//Cuando le golpeo al enemigo...
		if(FsmTuto.GetCurrentState() == tuto3)
		{
			FsmTuto.ChangeState(tuto4);
		}
	}

	void HandleOnLeftClick ()
	{
		FsmTuto.ChangeState(tuto6);
	}

	
	void HandleOnDestroySkill ()
	{
		FsmTuto.ChangeState(tuto7);
	}

	//Cuando mato al primer enemigo paso al tutorial 4
	void HandleOnLifeChangeEnemy (LifeModule _who, float _currentLife, float _previous, float _percentage)
	{
		FsmTuto.ChangeState(tuto4);
	}

	//Cuando me hacen daño paso al tutorial 3
	void HandleOnLifeChange (LifeModule _who, float _currentLife, float _previous, float _percentage)
	{
		//Cuando me hace daño cambio al tuto3
		FsmTuto.ChangeState(tuto3);
	}

	void OnDisable()
	{
		HUDManager.OnLeftClick 	-= 	HandleOnLeftClick;
		HUDManager.OnRightClick -= 	HandleOnRightClick;
		SkillController.OnDestroy -= HandleOnDestroySkill;
	}

	#region stateSkill
	[System.Serializable]
	public class Tuto1 : FSM.FSMState
	{
		
		private TutorialManager myOwner;
		private bool flag;

		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as TutorialManager;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			flag = true;
		}
		
		
		public override void Update ()
		{
			base.Update ();

			float distance = Vector2.Distance(myOwner.enemy1.transform.position, myOwner.mainCharacter.transform.position);
			if(distance < 2.4f && flag)
			{
				flag = false;
				myOwner.enemy1.Fsm.ChangeState(myOwner.enemy1.idleState);
				myOwner.textCanvasText.message = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Tuto1).GetStringData(Blackboard.Language);
				myOwner.textCanvasText.gameObject.SetActive(true);

				//Mi yo del futuro no se acordará de esto, son eventos anónimos para poder desubscribirse de ellos sin necesidad de un metodo para cada uno.
				myOwner.handler = (() =>
				{
					myOwner.rightKick.gameObject.SetActive(true);
					myOwner.rightButton.enabled = true;
					myOwner.rightButton.DOFade(.3f,1);
					
					myOwner.textCanvasText.OnFinishText -= myOwner.handler;
				});
				myOwner.textCanvasText.OnFinishText += myOwner.handler;
			}
		}	
	}

	[System.Serializable]
	public class Tuto2 : FSM.FSMState
	{
		
		private TutorialManager myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as TutorialManager;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			myOwner.enemy1.Fsm.ChangeState(myOwner.enemy1.movingState);
			myOwner.CanvasText.gameObject.SetActive(false);
			myOwner.leftButton.enabled = false;
			myOwner.rightButton.enabled = false;
			myOwner.rightKick.gameObject.SetActive(false);
		}
	}

	[System.Serializable]
	public class Tuto3 : FSM.FSMState
	{
		
		private TutorialManager myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as TutorialManager;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			myOwner.enemy1.Fsm.ChangeState(myOwner.enemy1.idleState);
			myOwner.textCanvasText.message = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Tuto3).GetStringData(Blackboard.Language);
			myOwner.textCanvasText.gameObject.SetActive(true);

			
			myOwner.handler = (() =>
			{
				myOwner.rightKick.gameObject.SetActive(true);
				myOwner.rightButton.enabled = true;
				myOwner.rightButton.DOFade(.3f,1);

				myOwner.textCanvasText.OnFinishText -= myOwner.handler;
			});
			myOwner.textCanvasText.OnFinishText += myOwner.handler;
			
		}
	}

	[System.Serializable]
	public class Tuto4 : FSM.FSMState
	{
		
		private TutorialManager myOwner;

		private bool flag;
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as TutorialManager;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			flag = true;

			myOwner.enemy2.gameObject.SetActive(true);
			myOwner.CanvasText.gameObject.SetActive(false);
			myOwner.leftButton.enabled = false;
			myOwner.rightButton.enabled = false;
			myOwner.rightKick.gameObject.SetActive(false);
		}
		
		
		public override void Update ()
		{
			base.Update ();
			
			float distance = Vector2.Distance(myOwner.enemy2.transform.position, myOwner.mainCharacter.transform.position);
			if(distance < 4 && flag)
			{
				flag = false;

				//Roto al personaje para que quede más molón
				Quaternion rot = myOwner.transform.rotation;
				rot.y = 180;
				myOwner.mainCharacter.transform.rotation = rot;

				myOwner.textCanvasText.message = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Tuto4).GetStringData(Blackboard.Language);
				myOwner.CanvasText.gameObject.SetActive(true);

				
				myOwner.handler = (() =>
				{
					if(Input.GetButtonDown("Fire1"))
					{
						myOwner.enemy2.Fsm.ChangeState(myOwner.enemy2.movingState);
						myOwner.textCanvasText.OnFinishText -= myOwner.handler;
						ChangeState(myOwner.tuto5);
					}

					
				});

				myOwner.textCanvasText.OnFinishText += myOwner.handler;
				
				myOwner.enemy2.Fsm.ChangeState(myOwner.enemy2.idleState);
			}
		}	
	}

	[System.Serializable]
	public class Tuto5 : FSM.FSMState
	{
		
		private TutorialManager myOwner;
		
		private bool flag;
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as TutorialManager;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			
			flag = true;

			myOwner.CanvasText.gameObject.SetActive(false);
			myOwner.leftButton.enabled = false;
			myOwner.rightButton.enabled = false;
			myOwner.rightKick.gameObject.SetActive(false);
		}
		
		
		public override void Update ()
		{
			base.Update ();
			
			float distance = Vector2.Distance(myOwner.enemy2.transform.position, myOwner.mainCharacter.transform.position);
			if(distance < 2 && flag)
			{
				flag = false;
				myOwner.enemy2.Fsm.ChangeState(myOwner.enemy2.idleState);
				myOwner.textCanvasText.message = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Tuto5).GetStringData(Blackboard.Language);
				myOwner.CanvasText.gameObject.SetActive(true);
				
				myOwner.handler = (() =>
				{
					myOwner.leftKick.gameObject.SetActive(true);
					myOwner.leftButton.enabled = true;
					myOwner.leftButton.DOFade(.3f,1);

					myOwner.textCanvasText.OnFinishText -= myOwner.handler;
				});

				myOwner.textCanvasText.OnFinishText += myOwner.handler;
				
			}
		}	
	}

	[System.Serializable]
	public class Tuto6 : FSM.FSMState
	{
		
		private TutorialManager myOwner;
		
		private bool flag;
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as TutorialManager;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			
			flag = true;
			
			myOwner.CanvasText.gameObject.SetActive(false);
			myOwner.leftButton.enabled = false;
			myOwner.rightButton.enabled = false;
			myOwner.leftKick.gameObject.SetActive(false);
			myOwner.enemy3.gameObject.SetActive(true);
			myOwner.enemy4.gameObject.SetActive(true);
		}
		
		
		public override void Update ()
		{
			base.Update ();
			
			float distance = Vector2.Distance(myOwner.enemy3.transform.position, myOwner.mainCharacter.transform.position);
			if(distance < 4 && flag)
			{
				flag = false;
				myOwner.enemy3.Fsm.ChangeState(myOwner.enemy3.idleState);
				myOwner.enemy4.Fsm.ChangeState(myOwner.enemy4.idleState);
				myOwner.textCanvasText.message = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Tuto6).GetStringData(Blackboard.Language);
				myOwner.CanvasText.gameObject.SetActive(true);
				
				myOwner.handler = (() =>
				                   {
					myOwner.filledSkillBg.gameObject.SetActive(true);
					myOwner.hud.FsmSkill.ChangeState(myOwner.hud.skillOff);
					myOwner.leftButton.enabled = false;
					
					myOwner.textCanvasText.OnFinishText -= myOwner.handler;
				});
				
				myOwner.textCanvasText.OnFinishText += myOwner.handler;
				
			}
		}	
	}

	[System.Serializable]
	public class Tuto7 : FSM.FSMState
	{
		
		private TutorialManager myOwner;
		private bool flag;
		

		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as TutorialManager;
			
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			flag = true;
			
			myOwner.CanvasText.gameObject.SetActive(false);
			myOwner.leftButton.enabled = false;
			myOwner.rightButton.enabled = false;
			myOwner.rightKick.gameObject.SetActive(false);
			myOwner.filledSkillBg.gameObject.SetActive(false);
			HUDManager.OnLeftClick 	-=  myOwner.HandleOnLeftClick;
			HUDManager.OnRightClick -= 	myOwner.HandleOnRightClick;
		}
		
		
		public override void Update ()
		{
			base.Update ();

			if(flag)
			{
				flag = false;
				myOwner.textCanvasText.message = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Tuto7).GetStringData(Blackboard.Language);
				myOwner.CanvasText.gameObject.SetActive(true);
					
					myOwner.handler = (() =>
					                   {
					if(Input.GetButtonDown("Fire1"))
					{
						PlayerPrefs.SetInt("TutorialFinish",1);
						Blackboard.Instance.StartCoroutine(Blackboard.Instance.ChangeLevel("Menu"));
						myOwner.textCanvasText.OnFinishText -= myOwner.handler;
					}
						
					});
					
					myOwner.textCanvasText.OnFinishText += myOwner.handler;
			}
		}	
	}
	#endregion
}
