using UnityEngine;
using UnityEditor;

namespace Google2u
{
	[CustomEditor(typeof(Waves))]
	public class WavesEditor : Editor
	{
		public int Index = 0;
		public override void OnInspectorGUI ()
		{
			Waves s = target as Waves;
			WavesRow r = s.Rows[ Index ];

			EditorGUILayout.BeginHorizontal();
			if ( GUILayout.Button("<<") )
			{
				Index = 0;
			}
			if ( GUILayout.Button("<") )
			{
				Index -= 1;
				if ( Index < 0 )
					Index = s.Rows.Count - 1;
			}
			if ( GUILayout.Button(">") )
			{
				Index += 1;
				if ( Index >= s.Rows.Count )
					Index = 0;
			}
			if ( GUILayout.Button(">>") )
			{
				Index = s.Rows.Count - 1;
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "ID", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.LabelField( s.rowNames[ Index ] );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_nameenemy0", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.TextField( r._nameenemy0 );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_timenext1", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.FloatField( (float)r._timenext1 );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_nameenemy1", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.TextField( r._nameenemy1 );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_timenext2", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.FloatField( (float)r._timenext2 );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_nameenemy2", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.TextField( r._nameenemy2 );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_timenext3", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.FloatField( (float)r._timenext3 );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_nameenemy3", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.TextField( r._nameenemy3 );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_timenext4", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.FloatField( (float)r._timenext4 );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_nameenemy4", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.TextField( r._nameenemy4 );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_timenext5", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.FloatField( (float)r._timenext5 );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_nameenemy5", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.TextField( r._nameenemy5 );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_timenext6", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.FloatField( (float)r._timenext6 );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_nameenemy6", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.TextField( r._nameenemy6 );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_timenext7", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.FloatField( (float)r._timenext7 );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_nameenemy7", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.TextField( r._nameenemy7 );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_timenext8", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.FloatField( (float)r._timenext8 );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_nameenemy8", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.TextField( r._nameenemy8 );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_timenext9", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.FloatField( (float)r._timenext9 );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_nameenemy9", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.TextField( r._nameenemy9 );
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label( "_timenext10", GUILayout.Width( 150.0f ) );
			{
				EditorGUILayout.FloatField( (float)r._timenext10 );
			}
			EditorGUILayout.EndHorizontal();

		}
	}
}
