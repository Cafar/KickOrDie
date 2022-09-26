////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class PaymentManagerAndroid {
	
	private static bool _isInited = false;
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	public const string INFINITE 	=  "com.boomfiregames.kickordie.infinite";
	
	public const string BASE64 	=		"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAw6XcjZokcIuydZYXFIfZA/Cm4gvhINJhkEdv+FTtwxQ4nLdCkKvZ3OC7doQQCk2594XpWDRKbi0mQTnNjd8dIrl1YE/3LiDLu0RkU/BxIXJphGQLB2C8lvMmsNg2sggGI/XCh23UjKRTy3fklFM06BXhMEkYHvcNleQWTmZ/ukXemQGrmPlcjyqZAUiCJXsPs/GseGtgYB7hSA9QF2FKY5E46tF0Fjiq6CUFsr2XStG+cMvkg48luJ2sE0TJUsIKAcQOJ9VBtiUE7+5B6oOnBLl1LOnNB5pmyfnOaDpLHD07fLw+Z/5tfNIQTW9aI27CqGA+PMhZaOEKLY0XH1km1wIDAQAB";

	private static bool IsInitialized = false;
	public static void init() {

		//Filling product list

		//When you will add your own proucts you can skip this code section of you already have added
		//your products ids under the editor setings menu
		AndroidInAppPurchaseManager.instance.AddProduct(INFINITE);


		//listening for purchase and consume events
		AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased;  
		AndroidInAppPurchaseManager.ActionProductConsumed  += OnProductConsumed;


		//listening for store initilaizing finish
		AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;


		//you may use loadStore function without parametr if you have filled base64EncodedPublicKey in plugin settings
		//AndroidInAppPurchaseManager.Instance.LoadStore();

		//or You can pass base64EncodedPublicKey using scirption:
		AndroidInAppPurchaseManager.instance.loadStore(BASE64);

	}



	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------


	public static void buy(string SKU) 
	{
		AndroidInAppPurchaseManager.Instance.Purchase (SKU);
	}

	//--------------------------------------
	//  GET / SET
	//--------------------------------------

	public static bool isInited {
		get {
			return _isInited;
		}
	}


	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	private static void OnProcessingPurchasedProduct(GooglePurchaseTemplate purchase) {
		PlayerPrefs.SetString("Comprado","Si");
		GooglePlayManager.Instance.UnlockAchievementById(Achievements.NOPUBLI);
		Analytics.Transaction(purchase.SKU, decimal.Parse(AndroidInAppPurchaseManager.Instance.Inventory.GetProductDetails(PaymentManagerAndroid.INFINITE).Price), AndroidInAppPurchaseManager.Instance.Inventory.GetProductDetails(PaymentManagerAndroid.INFINITE).priceCurrencyCode, null, null);
	}

	private static void OnProcessingConsumeProduct(GooglePurchaseTemplate purchase) {
		//some stuff for processing product consume. Reduse tip anount, reduse gold token, etc
	}

	private static void OnProductPurchased(BillingResult result) {


		if(result.isSuccess) {
			AndroidMessage.Create ("Product Purchased", result.purchase.SKU+ "\n Full Response: " + result.purchase.originalJson);
			OnProcessingPurchasedProduct (result.purchase);
		} else {
			AndroidMessage.Create("Product Purchase Failed", result.response.ToString() + " " + result.message);
		}

		Debug.Log ("Purchased Responce: " + result.response.ToString() + " " + result.message);
		Debug.Log (result.purchase.originalJson);
	}


	private static void OnProductConsumed(BillingResult result) {

		if(result.isSuccess) {
			AndroidMessage.Create ("Product Consumed", result.purchase.SKU + "\n Full Response: " + result.purchase.originalJson);
			OnProcessingConsumeProduct (result.purchase);
		} else {
			AndroidMessage.Create("Product Cousume Failed", result.response.ToString() + " " + result.message);
		}

		Debug.Log ("Cousume Responce: " + result.response.ToString() + " " + result.message);
	}


	private static void OnBillingConnected(BillingResult result) {
		AndroidInAppPurchaseManager.ActionBillingSetupFinished -= OnBillingConnected;


		if(result.isSuccess) {
			//Store connection is Successful. Next we loading product and customer purchasing details
			AndroidInAppPurchaseManager.Instance.RetrieveProducDetails();
			AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += OnRetrieveProductsFinised;
		}

		//AndroidMessage.Create("Connection Responce asfg", result.response.ToString() + " " + result.message);
		Debug.Log ("Connection Responce: " + result.response.ToString() + " " + result.message);
	}




	private static void OnRetrieveProductsFinised(BillingResult result) {
		AndroidInAppPurchaseManager.ActionRetrieveProducsFinished -= OnRetrieveProductsFinised;


		if(result.isSuccess) {
			_isInited = true;
			//AndroidMessage.Create("Success", "Billing init complete inventory contains: " + AndroidInAppPurchaseManager.instance.Inventory.Purchases.Count + " products");

			Debug.Log("Loaded products names");
			foreach(GoogleProductTemplate tpl in AndroidInAppPurchaseManager.instance.Inventory.Products) {
				Debug.Log(tpl.Title);
				Debug.Log(tpl.OriginalJson);
			}
		} else {
			AndroidMessage.Create("Connection Responce", result.response.ToString() + " " + result.message);
		}

		Debug.Log ("Connection Responce: " + result.response.ToString() + " " + result.message);

	}






}
