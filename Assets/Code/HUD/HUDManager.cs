using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class HUDManager : MonoBehaviour {

	public delegate void HandleOnButtonClick();
	public static event HandleOnButtonClick OnLeftClick, OnRightClick;

	//Create child
	protected GameObject CreateChild(GameObject _prefab)
	{
		GameObject copy = Instantiate(_prefab) as GameObject;
		copy.transform.SetParent(transform, false);
		
		return copy;
	}

	public static HUDManager instance;

	//Prefabs to instantiate
	public Image filledSkillBg, rightButton, leftButton;
	public Text textTotalKills, textMiss;
	public Transform readyCanvas, pauseCanvas;
	public LoseCanvas loseCanvas;

	public MainCharacterController mainCharacter;

	public AudioClip intro, mainTheme, bossIntro, bossTheme, bossEnd;

	private bool isTutorial, isBossFinal;

	#region FSMSkill
	
	public FSM	FsmSkill{get;private set;}
	
	public SkillIdle 				skillIdle;

	public SkillOn 					skillOn;
	
	public SkillOff 				skillOff;

	public SkillPush 				skillPush;



	
	#endregion

	#region FSMHud

	public FSM	FsmHUD{get;private set;}

	public ReadyState 				readyState;

	public PlayState		 		playState;

	public LossState		 		lossState;

	#endregion

	protected float currentCouldown;

	/// <summary>
	/// Resets the game.
	/// </summary>
	void OnDisable()
	{
		//ESto lo dejamos aqui temporal, mas adelante vemos como hcerlo mejor

		EnemyController.OnEnemyDead 		-= HandleOnEnemyDead;
		MainCharacterController.OnMiss 		-= HandleOntextMissPrefab;
		LoseCanvas.OnClickContinue			-= HandleOnClickContinue;
	}

	void OnEnable()
	{
		//Whene a enemy Dead
		EnemyController.OnEnemyDead 	+= HandleOnEnemyDead;
		
		//When i textMissPrefab
		MainCharacterController.OnMiss 	+= HandleOntextMissPrefab;

		LoseCanvas.OnClickContinue		+= HandleOnClickContinue;
	}


	void Start () 
	{
		instance = this;

		isTutorial = false;
		if(Blackboard.nameLoadedlevel == "Tutorial")
		{
			isTutorial = true;
			SoundManager.PlayImmediately(mainTheme,true);
		}

		isBossFinal = false;
		if( Blackboard.nameLoadedlevel == "Level3Normal" || Blackboard.nameLoadedlevel == "Level3Hard")
			isBossFinal = true;
		//GET HUDS PREFABS
		textTotalKills.text = Blackboard.totalEnemiesDead.ToString();

		//Initialize FSM
		FsmSkill = FSM.CreateFSM(this);
		FsmSkill.AddFSMState (skillOn, skillOff, skillPush, skillIdle);
		FsmSkill.ChangeState(skillIdle);


		FsmHUD = FSM.CreateFSM(this);
		FsmHUD.AddFSMState (readyState, playState, lossState);
		FsmHUD.ChangeState(readyState);
	}

	void HandleOntextMissPrefab ()
	{
		textMiss.GetComponent<Animator>().SetTrigger("Miss");
	}

	void HandleOnEnemyDead ()
	{
		Blackboard.totalEnemiesDead++;
		textTotalKills.rectTransform.DOPunchScale(new Vector3(1.1f,1.1f,1), .1f, 10, 1).OnComplete(()=>{
			textTotalKills.rectTransform.localScale = Vector3.one;
		});
		textTotalKills.text = Blackboard.totalEnemiesDead.ToString();
	}

	void HandleOnClickContinue ()
	{
		FsmHUD.ChangeState(readyState);	
		FsmSkill.ChangeState(skillOn);
	}

	#region Buttons

	/// <summary>
	/// Skills the button push.
	/// </summary>
	public void SkillButtonPush()
	{
		if(mainCharacter.Fsm.GetCurrentState() != mainCharacter.deathState && FsmSkill.GetCurrentState() == skillOn)
		{
			mainCharacter.Fsm.ChangeState(mainCharacter.skillState);
			FsmSkill.ChangeState(skillPush);
		}
	}

	/// <summary>
	/// Attacks the button.
	/// </summary>
	/// <param name="_right">If set to <c>true</c> _right.</param>
	public void AttackButton(bool _right)
	{
		if(_right)
		{
			mainCharacter.RightButtonPush();
			rightButton.color = new Color(rightButton.color.r,rightButton.color.g,rightButton.color.b,.2f);
			
			if(OnRightClick != null)
				OnRightClick();
			
		}
		else
		{
			mainCharacter.LeftButtonPush();
			leftButton.color = new Color(rightButton.color.r,rightButton.color.g,rightButton.color.b,.2f);
			
			if(OnLeftClick != null)
				OnLeftClick();
		}
	}

	/// <summary>
	/// Attacks the button release.
	/// </summary>
	/// <param name="_right">If set to <c>true</c> _right.</param>
	public void AttackButtonRelease(bool _right)
	{
		if(_right)
		{
			rightButton.color = new Color(rightButton.color.r,rightButton.color.g,rightButton.color.b,0f);
			
		}
		else
		{
			leftButton.color = new Color(rightButton.color.r,rightButton.color.g,rightButton.color.b,0f);
		}
	}


	public void Pause()
	{
		Instantiate(pauseCanvas);
		Time.timeScale = 0;
	}

	#endregion

	#region stateSkill
	[System.Serializable]
	public class SkillIdle : FSM.FSMState
	{
		
		private HUDManager myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as HUDManager;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			myOwner.filledSkillBg.fillAmount = 0;
		}
		
		

		
	}


	[System.Serializable]
	public class SkillOn : FSM.FSMState
	{
		
		private HUDManager myOwner;

		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as HUDManager;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			myOwner.filledSkillBg.GetComponent<Animator>().SetBool("Blink",true);
		}
		
		
		public override void Update ()
		{
			base.Update ();

			if(Input.GetKeyDown(KeyCode.Space))
			{
				myOwner.SkillButtonPush();
			}
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			myOwner.filledSkillBg.GetComponent<Animator>().SetBool("Blink",false);
		}
		
	}

	[System.Serializable]
	public class SkillOff : FSM.FSMState
	{
		
		private HUDManager myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as HUDManager;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			myOwner.currentCouldown = 0;
		}
		
		
		public override void Update ()
		{
			base.Update ();

			myOwner.currentCouldown += Time.deltaTime;

			myOwner.filledSkillBg.fillAmount = myOwner.currentCouldown/myOwner.mainCharacter.couldownSkill;

			if(myOwner.currentCouldown >= myOwner.mainCharacter.couldownSkill)
			{
				ChangeState(myOwner.skillOn);
			}
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			
		}
		
	}

	[System.Serializable]
	public class SkillPush : FSM.FSMState
	{
		
		private HUDManager myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as HUDManager;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			foreach(Image image in myOwner.transform.GetComponentsInChildren<Image>())
			{
				image.enabled = false;
			}

			SkillController.OnDestroy += HandleOnDestroy;
		}

		void HandleOnDestroy ()
		{
			ChangeState(myOwner.skillOff);
		}
		
		
		public override void Update ()
		{
			base.Update ();
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			foreach(Image image in myOwner.transform.GetComponentsInChildren<Image>())
			{
				image.enabled = true;
			}

			SkillController.OnDestroy -= HandleOnDestroy;
		}
		
	}

	[System.Serializable]
	public class ReadyState : FSM.FSMState
	{
		
		private HUDManager myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as HUDManager;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			if(!myOwner.isTutorial)
			{
				if(Google2u.GameController.Instance != null)
					Google2u.GameController.Instance.GetComponent<Google2u.GameController>().pauseSpawnWaves = true;

				if(myOwner.loseCanvas != null)
					myOwner.loseCanvas.gameObject.SetActive(false);
				if(myOwner.readyCanvas != null)
					myOwner.readyCanvas.gameObject.SetActive(true);

			}

//			if(myOwner.isBossFinal)
//			{
//				myOwner.readyCanvas.gameObject.SetActive(false);
//				myOwner.FsmHUD.ChangeState(myOwner.playState);
//				
//			}

			myOwner.FsmSkill.ChangeState(myOwner.skillIdle);

			if(myOwner.intro != null)
				SoundManager.PlayImmediately(myOwner.intro,true);

		}
		
		
		public override void Update ()
		{
			base.Update ();

			if(Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
			{
			 	if(!myOwner.isTutorial)
				{
					myOwner.readyCanvas.gameObject.SetActive(false);
					myOwner.FsmSkill.ChangeState(myOwner.skillOff);
					SoundManager.PlaySFX("UIStart");
					myOwner.FsmHUD.ChangeState(myOwner.playState);
				}
			}

		}
		
		public override void Exit (FSM.FSMState _nextState)
		{

		}
		
	}

	[System.Serializable]
	public class PlayState : FSM.FSMState
	{
		
		private HUDManager myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as HUDManager;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			if(Everyplay.IsRecordingSupported() && Everyplay.IsSupported())
			{
				Everyplay.StartRecording();
			}


			if(!myOwner.isTutorial)
			{
				if(Google2u.GameController.Instance != null)
				Google2u.GameController.Instance.GetComponent<Google2u.GameController>().pauseSpawnWaves = false;

				myOwner.mainCharacter.GetComponent<LifeModule>().OnKill += delegate {
					myOwner.FsmHUD.ChangeState(myOwner.lossState);
				};
			}
			SoundManager.PlayImmediately(myOwner.mainTheme,true);
		}
		
		
		public override void Update ()
		{
			base.Update ();
			

			
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{

		}
		
	}

	[System.Serializable]
	public class LossState : FSM.FSMState
	{
		
		private HUDManager myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as HUDManager;
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			if(!myOwner.isTutorial)
			{
				myOwner.StartCoroutine(myOwner.loseCanvas.Init());

				if(Google2u.GameController.Instance != null)
					Google2u.GameController.Instance.GetComponent<Google2u.GameController>().pauseSpawnWaves = true;
			}

		}
		
		
		public override void Update ()
		{
			base.Update ();

			
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			
		}
		
	}
	#endregion
}
