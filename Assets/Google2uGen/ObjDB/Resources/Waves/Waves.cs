//----------------------------------------------
//    Google2u: Google Doc Unity integration
//         Copyright © 2015 Litteratus
//
//        This file has been auto-generated
//              Do not manually edit
//----------------------------------------------

using UnityEngine;
using System.Globalization;

namespace Google2u
{
	[System.Serializable]
	public class WavesRow : IGoogle2uRow
	{
		public string _nameenemy0;
		public float _timenext1;
		public string _nameenemy1;
		public float _timenext2;
		public string _nameenemy2;
		public float _timenext3;
		public string _nameenemy3;
		public float _timenext4;
		public string _nameenemy4;
		public float _timenext5;
		public string _nameenemy5;
		public float _timenext6;
		public string _nameenemy6;
		public float _timenext7;
		public string _nameenemy7;
		public float _timenext8;
		public string _nameenemy8;
		public float _timenext9;
		public string _nameenemy9;
		public float _timenext10;
		public WavesRow(string __googlefu_id, string __nameenemy0, string __timenext1, string __nameenemy1, string __timenext2, string __nameenemy2, string __timenext3, string __nameenemy3, string __timenext4, string __nameenemy4, string __timenext5, string __nameenemy5, string __timenext6, string __nameenemy6, string __timenext7, string __nameenemy7, string __timenext8, string __nameenemy8, string __timenext9, string __nameenemy9, string __timenext10) 
		{
			_nameenemy0 = __nameenemy0.Trim();
			{
			float res;
				if(float.TryParse(__timenext1, NumberStyles.Any, CultureInfo.InvariantCulture, out res))
					_timenext1 = res;
				else
					Debug.LogError("Failed To Convert _timenext1 string: "+ __timenext1 +" to float");
			}
			_nameenemy1 = __nameenemy1.Trim();
			{
			float res;
				if(float.TryParse(__timenext2, NumberStyles.Any, CultureInfo.InvariantCulture, out res))
					_timenext2 = res;
				else
					Debug.LogError("Failed To Convert _timenext2 string: "+ __timenext2 +" to float");
			}
			_nameenemy2 = __nameenemy2.Trim();
			{
			float res;
				if(float.TryParse(__timenext3, NumberStyles.Any, CultureInfo.InvariantCulture, out res))
					_timenext3 = res;
				else
					Debug.LogError("Failed To Convert _timenext3 string: "+ __timenext3 +" to float");
			}
			_nameenemy3 = __nameenemy3.Trim();
			{
			float res;
				if(float.TryParse(__timenext4, NumberStyles.Any, CultureInfo.InvariantCulture, out res))
					_timenext4 = res;
				else
					Debug.LogError("Failed To Convert _timenext4 string: "+ __timenext4 +" to float");
			}
			_nameenemy4 = __nameenemy4.Trim();
			{
			float res;
				if(float.TryParse(__timenext5, NumberStyles.Any, CultureInfo.InvariantCulture, out res))
					_timenext5 = res;
				else
					Debug.LogError("Failed To Convert _timenext5 string: "+ __timenext5 +" to float");
			}
			_nameenemy5 = __nameenemy5.Trim();
			{
			float res;
				if(float.TryParse(__timenext6, NumberStyles.Any, CultureInfo.InvariantCulture, out res))
					_timenext6 = res;
				else
					Debug.LogError("Failed To Convert _timenext6 string: "+ __timenext6 +" to float");
			}
			_nameenemy6 = __nameenemy6.Trim();
			{
			float res;
				if(float.TryParse(__timenext7, NumberStyles.Any, CultureInfo.InvariantCulture, out res))
					_timenext7 = res;
				else
					Debug.LogError("Failed To Convert _timenext7 string: "+ __timenext7 +" to float");
			}
			_nameenemy7 = __nameenemy7.Trim();
			{
			float res;
				if(float.TryParse(__timenext8, NumberStyles.Any, CultureInfo.InvariantCulture, out res))
					_timenext8 = res;
				else
					Debug.LogError("Failed To Convert _timenext8 string: "+ __timenext8 +" to float");
			}
			_nameenemy8 = __nameenemy8.Trim();
			{
			float res;
				if(float.TryParse(__timenext9, NumberStyles.Any, CultureInfo.InvariantCulture, out res))
					_timenext9 = res;
				else
					Debug.LogError("Failed To Convert _timenext9 string: "+ __timenext9 +" to float");
			}
			_nameenemy9 = __nameenemy9.Trim();
			{
			float res;
				if(float.TryParse(__timenext10, NumberStyles.Any, CultureInfo.InvariantCulture, out res))
					_timenext10 = res;
				else
					Debug.LogError("Failed To Convert _timenext10 string: "+ __timenext10 +" to float");
			}
		}

		public int Length { get { return 20; } }

		public string this[int i]
		{
		    get
		    {
		        return GetStringDataByIndex(i);
		    }
		}

		public string GetStringDataByIndex( int index )
		{
			string ret = System.String.Empty;
			switch( index )
			{
				case 0:
					ret = _nameenemy0.ToString();
					break;
				case 1:
					ret = _timenext1.ToString();
					break;
				case 2:
					ret = _nameenemy1.ToString();
					break;
				case 3:
					ret = _timenext2.ToString();
					break;
				case 4:
					ret = _nameenemy2.ToString();
					break;
				case 5:
					ret = _timenext3.ToString();
					break;
				case 6:
					ret = _nameenemy3.ToString();
					break;
				case 7:
					ret = _timenext4.ToString();
					break;
				case 8:
					ret = _nameenemy4.ToString();
					break;
				case 9:
					ret = _timenext5.ToString();
					break;
				case 10:
					ret = _nameenemy5.ToString();
					break;
				case 11:
					ret = _timenext6.ToString();
					break;
				case 12:
					ret = _nameenemy6.ToString();
					break;
				case 13:
					ret = _timenext7.ToString();
					break;
				case 14:
					ret = _nameenemy7.ToString();
					break;
				case 15:
					ret = _timenext8.ToString();
					break;
				case 16:
					ret = _nameenemy8.ToString();
					break;
				case 17:
					ret = _timenext9.ToString();
					break;
				case 18:
					ret = _nameenemy9.ToString();
					break;
				case 19:
					ret = _timenext10.ToString();
					break;
			}

			return ret;
		}

		public string GetStringData( string colID )
		{
			var ret = System.String.Empty;
			switch( colID.ToLower() )
			{
				case "_nameenemy0":
					ret = _nameenemy0.ToString();
					break;
				case "_timenext1":
					ret = _timenext1.ToString();
					break;
				case "_nameenemy1":
					ret = _nameenemy1.ToString();
					break;
				case "_timenext2":
					ret = _timenext2.ToString();
					break;
				case "_nameenemy2":
					ret = _nameenemy2.ToString();
					break;
				case "_timenext3":
					ret = _timenext3.ToString();
					break;
				case "_nameenemy3":
					ret = _nameenemy3.ToString();
					break;
				case "_timenext4":
					ret = _timenext4.ToString();
					break;
				case "_nameenemy4":
					ret = _nameenemy4.ToString();
					break;
				case "_timenext5":
					ret = _timenext5.ToString();
					break;
				case "_nameenemy5":
					ret = _nameenemy5.ToString();
					break;
				case "_timenext6":
					ret = _timenext6.ToString();
					break;
				case "_nameenemy6":
					ret = _nameenemy6.ToString();
					break;
				case "_timenext7":
					ret = _timenext7.ToString();
					break;
				case "_nameenemy7":
					ret = _nameenemy7.ToString();
					break;
				case "_timenext8":
					ret = _timenext8.ToString();
					break;
				case "_nameenemy8":
					ret = _nameenemy8.ToString();
					break;
				case "_timenext9":
					ret = _timenext9.ToString();
					break;
				case "_nameenemy9":
					ret = _nameenemy9.ToString();
					break;
				case "_timenext10":
					ret = _timenext10.ToString();
					break;
			}

			return ret;
		}
		public override string ToString()
		{
			string ret = System.String.Empty;
			ret += "{" + "_nameenemy0" + " : " + _nameenemy0.ToString() + "} ";
			ret += "{" + "_timenext1" + " : " + _timenext1.ToString() + "} ";
			ret += "{" + "_nameenemy1" + " : " + _nameenemy1.ToString() + "} ";
			ret += "{" + "_timenext2" + " : " + _timenext2.ToString() + "} ";
			ret += "{" + "_nameenemy2" + " : " + _nameenemy2.ToString() + "} ";
			ret += "{" + "_timenext3" + " : " + _timenext3.ToString() + "} ";
			ret += "{" + "_nameenemy3" + " : " + _nameenemy3.ToString() + "} ";
			ret += "{" + "_timenext4" + " : " + _timenext4.ToString() + "} ";
			ret += "{" + "_nameenemy4" + " : " + _nameenemy4.ToString() + "} ";
			ret += "{" + "_timenext5" + " : " + _timenext5.ToString() + "} ";
			ret += "{" + "_nameenemy5" + " : " + _nameenemy5.ToString() + "} ";
			ret += "{" + "_timenext6" + " : " + _timenext6.ToString() + "} ";
			ret += "{" + "_nameenemy6" + " : " + _nameenemy6.ToString() + "} ";
			ret += "{" + "_timenext7" + " : " + _timenext7.ToString() + "} ";
			ret += "{" + "_nameenemy7" + " : " + _nameenemy7.ToString() + "} ";
			ret += "{" + "_timenext8" + " : " + _timenext8.ToString() + "} ";
			ret += "{" + "_nameenemy8" + " : " + _nameenemy8.ToString() + "} ";
			ret += "{" + "_timenext9" + " : " + _timenext9.ToString() + "} ";
			ret += "{" + "_nameenemy9" + " : " + _nameenemy9.ToString() + "} ";
			ret += "{" + "_timenext10" + " : " + _timenext10.ToString() + "} ";
			return ret;
		}
	}
	public class Waves :  Google2uComponentBase, IGoogle2uDB
	{
		public enum rowIds {
			WAVE_1, WAVE_2, WAVE_3, WAVE_4, WAVE_5, WAVE_6, WAVE_7, WAVE_8, WAVE_9, WAVE_10, WAVE_11, WAVE_12
		};
		public string [] rowNames = {
			"WAVE_1", "WAVE_2", "WAVE_3", "WAVE_4", "WAVE_5", "WAVE_6", "WAVE_7", "WAVE_8", "WAVE_9", "WAVE_10", "WAVE_11", "WAVE_12"
		};
		public System.Collections.Generic.List<WavesRow> Rows = new System.Collections.Generic.List<WavesRow>();
		public override void AddRowGeneric (System.Collections.Generic.List<string> input)
		{
			Rows.Add(new WavesRow(input[0],input[1],input[2],input[3],input[4],input[5],input[6],input[7],input[8],input[9],input[10],input[11],input[12],input[13],input[14],input[15],input[16],input[17],input[18],input[19],input[20]));
		}
		public override void Clear ()
		{
			Rows.Clear();
		}
		public IGoogle2uRow GetGenRow(string in_RowString)
		{
			IGoogle2uRow ret = null;
			try
			{
				ret = Rows[(int)System.Enum.Parse(typeof(rowIds), in_RowString)];
			}
			catch(System.ArgumentException) {
				Debug.LogError( in_RowString + " is not a member of the rowIds enumeration.");
			}
			return ret;
		}
		public IGoogle2uRow GetGenRow(rowIds in_RowID)
		{
			IGoogle2uRow ret = null;
			try
			{
				ret = Rows[(int)in_RowID];
			}
			catch( System.Collections.Generic.KeyNotFoundException ex )
			{
				Debug.LogError( in_RowID + " not found: " + ex.Message );
			}
			return ret;
		}
		public WavesRow GetRow(rowIds in_RowID)
		{
			WavesRow ret = null;
			try
			{
				ret = Rows[(int)in_RowID];
			}
			catch( System.Collections.Generic.KeyNotFoundException ex )
			{
				Debug.LogError( in_RowID + " not found: " + ex.Message );
			}
			return ret;
		}
		public WavesRow GetRow(string in_RowString)
		{
			WavesRow ret = null;
			try
			{
				ret = Rows[(int)System.Enum.Parse(typeof(rowIds), in_RowString)];
			}
			catch(System.ArgumentException) {
				Debug.LogError( in_RowString + " is not a member of the rowIds enumeration.");
			}
			return ret;
		}

	}

}
