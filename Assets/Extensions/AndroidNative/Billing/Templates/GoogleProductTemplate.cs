////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

[System.Serializable]
public class GoogleProductTemplate  {

	//Editor Only
	public bool IsOpen = true;

	public string SKU = string.Empty;
	public string priceAmountMicros = string.Empty;
	public string priceCurrencyCode = "USD";

	private string _OriginalJson = string.Empty;

	[SerializeField]
	private string _Price = "0.99";

	[SerializeField]
	private string _Description = string.Empty;

	[SerializeField]
	private string _Title =  "New Product";

	[SerializeField]
	private Texture2D _Texture;

	[SerializeField]
	private AN_InAppType _ProductType = AN_InAppType.Consumable;

	[System.Obsolete("originalJson is deprectaed, please use OriginalJson instead")]
	public string originalJson {
		get {
			return _OriginalJson;
		} 
		
		set {
			_OriginalJson = value;
		}
	}

	public string OriginalJson {
		get {
			return _OriginalJson;
		} 
		
		set {
			_OriginalJson = value;
		}
	}

	[System.Obsolete("price is deprectaed, please use Price instead")]
	public string price {
		get {
			return _Price;
		} 
		
		set {
			_Price = value;
		}
	}

	public string Price {
		get {
			return _Price;
		} 
		
		set {
			_Price = value;
		}
	}

	[System.Obsolete("description is deprectaed, please use Description instead")]
	public string description {
		get {
			return _Description;
		}
		
		set {
			_Description = value;
		}
	}

	public string Description {
		get {
			return _Description;
		}
		
		set {
			_Description = value;
		}
	}

	[System.Obsolete("title is deprectaed, please use Title instead")]
	public string title {
		get {
			return _Title;
		}
		
		set {
			_Title = value;
		}
	}

	public string Title {
		get {
			return _Title;
		}
		
		set {
			_Title = value;
		}
	}

	public Texture2D Texture {
		get {
			return _Texture;
		}
		
		set {
			_Texture = value;
		}
	}

	public AN_InAppType ProductType {
		get {
			return _ProductType;
		}
		
		set {
			_ProductType =  value;
		}
	}
}
