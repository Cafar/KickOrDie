using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class Shop : FloatingPopUp {

	public delegate void HandleOnCloseShop();
	public static event HandleOnCloseShop OnClose;

	public Button buyInfinte, watchVideo;
	public Text textPay, textVideo;

	private ContinuesManager continuesManager;

	public override void Start ()
	{
		buyInfinte.interactable = false;

		if(Application.platform == RuntimePlatform.Android)
		{
			PaymentManagerAndroid.init();
			AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += AndroidInAppPurchaseManager_ActionRetrieveProducsFinished;
		}
		else if( Application.platform == RuntimePlatform.IPhonePlayer)
		{
			PaymentManagerIos.init();
			IOSInAppPurchaseManager.OnStoreKitInitComplete += IOSInAppPurchaseManager_OnStoreKitInitComplete;
		}
			
		continuesManager = GameObject.Find("Continues").GetComponent<ContinuesManager>();

		if(continuesManager != null)
		{
			if(continuesManager.LivesManager.Lives > 0)
			{
				watchVideo.interactable = false;
			}
		}

		textVideo.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_GetContinue).GetStringData(Blackboard.Language);
	
		base.Start ();
	}

	void IOSInAppPurchaseManager_OnStoreKitInitComplete (ISN_Result obj)
	{
		IOSInAppPurchaseManager.OnStoreKitInitComplete -= IOSInAppPurchaseManager_OnStoreKitInitComplete;
		textPay.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Pay).GetStringData(Blackboard.Language) + "(" + IOSInAppPurchaseManager.Instance.GetProductById(PaymentManagerIos.INFINITE).LocalizedPrice + ")";
		ActiveBuy();
	}

	void AndroidInAppPurchaseManager_ActionRetrieveProducsFinished (BillingResult obj)
	{
		AndroidInAppPurchaseManager.ActionRetrieveProducsFinished -= AndroidInAppPurchaseManager_ActionRetrieveProducsFinished;
		textPay.text = Blackboard.localization.GetRow(Google2u.MyLocalization.rowIds.Text_Pay).GetStringData(Blackboard.Language) + "(" + AndroidInAppPurchaseManager.Instance.Inventory.GetProductDetails(PaymentManagerAndroid.INFINITE).Price + ")";
		ActiveBuy();	
	}

	private void ActiveBuy()
	{
		buyInfinte.interactable = true;
	}

	public void Buy()
	{
		
		if(Application.platform == RuntimePlatform.Android)
		{
			PaymentManagerAndroid.buy(PaymentManagerAndroid.INFINITE);
		}
		else if( Application.platform == RuntimePlatform.IPhonePlayer)
		{
			PaymentManagerIos.buyItem(PaymentManagerIos.INFINITE);
		}
	}

	public void OnDestroy()
	{
		if (OnClose != null)
			OnClose();
	}

	public void Video()
	{
		if(continuesManager != null)
		{
			if (Advertisement.IsReady("rewardedVideo"))
			{
				var options = new ShowOptions { resultCallback = HandleShowResult };
				Advertisement.Show("rewardedVideo", options);
			}
		}


	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log("The ad was successfully shown.");
			continuesManager.LivesManager.GiveOneLife();
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}
}
