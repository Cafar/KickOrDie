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
	public class MyLocalizationRow : IGoogle2uRow
	{
		public string _es;
		public string _en;
		public string _fr;
		public string _it;
		public MyLocalizationRow(string __string_id, string __es, string __en, string __fr, string __it) 
		{
			_es = __es.Trim();
			_en = __en.Trim();
			_fr = __fr.Trim();
			_it = __it.Trim();
		}

		public int Length { get { return 4; } }

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
					ret = _es.ToString();
					break;
				case 1:
					ret = _en.ToString();
					break;
				case 2:
					ret = _fr.ToString();
					break;
				case 3:
					ret = _it.ToString();
					break;
			}

			return ret;
		}

		public string GetStringData( string colID )
		{
			var ret = System.String.Empty;
			switch( colID.ToLower() )
			{
				case "es":
					ret = _es.ToString();
					break;
				case "en":
					ret = _en.ToString();
					break;
				case "fr":
					ret = _fr.ToString();
					break;
				case "it":
					ret = _it.ToString();
					break;
			}

			return ret;
		}
		public override string ToString()
		{
			string ret = System.String.Empty;
			ret += "{" + "es" + " : " + _es.ToString() + "} ";
			ret += "{" + "en" + " : " + _en.ToString() + "} ";
			ret += "{" + "fr" + " : " + _fr.ToString() + "} ";
			ret += "{" + "it" + " : " + _it.ToString() + "} ";
			return ret;
		}
	}
	public sealed class MyLocalization : IGoogle2uDB
	{
		public enum rowIds {
			Text_Tuto1, Text_Tuto3, Text_Tuto4, Text_Tuto5, Text_Tuto6, Text_Tuto7, Text_Loading, Text_LastChance, Text_Miss, Text_Full, Text_Tap, Text_Story, Text_Normal, Text_Hard, Text_Arcade, Text_Tutorial, Text_O, Text_UnlokedHard, Text_Unloked
			, Text_Settings, Text_Exit, Text_Back, Text_Hint1, Text_Hint2, Text_Hint3, Text_Hin4, Text_Select, Text_Day, Text_Night, Text_Snow, Text_Music, Text_Credits, Text_Shop, Text_Restore, Text_Lang, Text_Rate, Text_Like, Text_Pay, Text_Restart
			, Text_GetContinue, Text_Rateit, Text_NewFeature
		};
		public string [] rowNames = {
			"Text_Tuto1", "Text_Tuto3", "Text_Tuto4", "Text_Tuto5", "Text_Tuto6", "Text_Tuto7", "Text_Loading", "Text_LastChance", "Text_Miss", "Text_Full", "Text_Tap", "Text_Story", "Text_Normal", "Text_Hard", "Text_Arcade", "Text_Tutorial", "Text_O", "Text_UnlokedHard", "Text_Unloked"
			, "Text_Settings", "Text_Exit", "Text_Back", "Text_Hint1", "Text_Hint2", "Text_Hint3", "Text_Hin4", "Text_Select", "Text_Day", "Text_Night", "Text_Snow", "Text_Music", "Text_Credits", "Text_Shop", "Text_Restore", "Text_Lang", "Text_Rate", "Text_Like", "Text_Pay", "Text_Restart"
			, "Text_GetContinue", "Text_Rateit", "Text_NewFeature"
		};
		public System.Collections.Generic.List<MyLocalizationRow> Rows = new System.Collections.Generic.List<MyLocalizationRow>();

		public static MyLocalization Instance
		{
			get { return NestedMyLocalization.instance; }
		}

		private class NestedMyLocalization
		{
			static NestedMyLocalization() { }
			internal static readonly MyLocalization instance = new MyLocalization();
		}

		private MyLocalization()
		{
			Rows.Add( new MyLocalizationRow("Text_Tuto1", "¡Cuidado! viene un enemigo. Daré un patada para vencerle", "Watch out! Here comes an enemy. I'll use a kick to beat him", "Attention! Il est un ennemi. Je vais vous donner un coup de pied pour le battre", "Attenzione! Egli è un nemico. Vi darò un calcio a batterlo"));
			Rows.Add( new MyLocalizationRow("Text_Tuto3", "Si no golpeo a nadie puedo caerme, debo de darle cuando esté a esta distancia", "If I don't hit anyone I might fall, I must kick him when he is this far", "Si vous ne frappez pas n'importe qui peut tomber, devrais-je donner lorsque cette distance", "Se non colpire chiunque può cadere, dovrei dare quando questa distanza"));
			Rows.Add( new MyLocalizationRow("Text_Tuto4", "¡Bien, creo que sigo en forma!...¡Viene un enemigo por el otro lado!", "Good, I think I'm still in shape!...An enemy is coming from the other side!", "Eh bien, je pense que je suis en forme! ... Comes un ennemi de l'autre côté!", "Beh, penso di essere in forma! ... Arriva un nemico sul lato opposto!"));
			Rows.Add( new MyLocalizationRow("Text_Tuto5", "Esta vez no voy a fallar", " I will not fail this time!", "Cette fois, je ne manquerai pas", "Questa volta non mancherò"));
			Rows.Add( new MyLocalizationRow("Text_Tuto6", "Vaya, me han acorralado. Usaré mi ataque especial", "I've been cornered. It's time for my special attack", "Eh bien, je l'ai été coincés. Je vais utiliser mon attaque spéciale", "Beh, ho le spalle al muro. Userò il mio attacco speciale"));
			Rows.Add( new MyLocalizationRow("Text_Tuto7", "¡Creo que ya estoy preparado!", "I think I'm ready!", "Je pense que je suis prêt!", "Penso di essere pronto!"));
			Rows.Add( new MyLocalizationRow("Text_Loading", "Cargando...", "Loading...", "Chargement ...", "Caricamento in corso ..."));
			Rows.Add( new MyLocalizationRow("Text_LastChance", "Última Oportunidad", "Last chance", "Dernière chance", "Last Chance"));
			Rows.Add( new MyLocalizationRow("Text_Miss", "Fallo", "Miss", "Échec", "Fallimento"));
			Rows.Add( new MyLocalizationRow("Text_Full", "Lleno", "Full", "Plein", "Pieno"));
			Rows.Add( new MyLocalizationRow("Text_Tap", "Tap para jugar", "Tap to play", "Appuyez à jouer", "Toccare per riprodurre"));
			Rows.Add( new MyLocalizationRow("Text_Story", "Modo Historia", "Story Mode", "Story Mode", "Story Mode"));
			Rows.Add( new MyLocalizationRow("Text_Normal", "Fácil", "Easy", "Aisé", "Facile"));
			Rows.Add( new MyLocalizationRow("Text_Hard", "Normal", "Hard", "Difficile", "Duro"));
			Rows.Add( new MyLocalizationRow("Text_Arcade", "Modo Arcade", "Arcade Mode", "Arcade Mode", "Arcade Mode"));
			Rows.Add( new MyLocalizationRow("Text_Tutorial", "Tutorial", "Tutorial", "Tutorial", "Tutorial"));
			Rows.Add( new MyLocalizationRow("Text_O", "o", "or", "ou", "o"));
			Rows.Add( new MyLocalizationRow("Text_UnlokedHard", "Completa el Modo Normal para desbloquear el Modo Difícil", "Complete the Normal mode to unlock Hard Mode", "Terminez le mode Normal pour débloquer le mode dur", "Completa la modalità Normale per sbloccare modalità Difficile"));
			Rows.Add( new MyLocalizationRow("Text_Unloked", "Completa el Tutorial para desbloquear los demás modos", "Complete Tutorial to unlock other modes", "Tutoriel complet pour débloquer d'autres modes", "Tutorial completo per sbloccare altri modi"));
			Rows.Add( new MyLocalizationRow("Text_Settings", "Ajustes", "Settings", "Paramètres", "Impostazioni"));
			Rows.Add( new MyLocalizationRow("Text_Exit", "Salir", "Exit", "Sortie", "Uscita"));
			Rows.Add( new MyLocalizationRow("Text_Back", "Atrás", "Back", "Arrière", "Indietro"));
			Rows.Add( new MyLocalizationRow("Text_Hint1", "Golpea justo cuando estén cerca", "Hit them just when they are close", "Frappez-les juste au moment où ils sont proches", "Colpirli proprio quando sono vicino"));
			Rows.Add( new MyLocalizationRow("Text_Hint2", "Utiliza tu habilidad solo en casos extremos", "Use your special skills only in extreme situations", "Utilisez vos compétences spéciales que dans des situations extrêmes", "Usa la tua abilità speciali solo in situazioni estreme"));
			Rows.Add( new MyLocalizationRow("Text_Hint3", "Si golpeas rápido puedes matar a enemigos sin que caigan al suelo", "if you hit them quick you can kill enemies before they touch the floor", "si vous les touchez rapide, vous pouvez tuer les ennemis avant qu'ils ne touchent le sol", "se si ha colpito veloce è possibile uccidere i nemici prima che tocchino il pavimento"));
			Rows.Add( new MyLocalizationRow("Text_Hin4", "Para los enemigos más duros utiliza tu habilidad especial", "For the toughest enemies use your special skill", "Pour les ennemis les plus difficiles utiliser votre compétence spéciale", "Per i nemici più duri Usa la tua abilità speciale"));
			Rows.Add( new MyLocalizationRow("Text_Select", "Selecciona un mapa", "Choose a map", "Choisissez une carte", "Scegli una mappa"));
			Rows.Add( new MyLocalizationRow("Text_Day", "Montañas Kiro-Masawa", "Kiro-Massawa Mountains", "Kiro-Massawa Montagnes", "Kiro-Massawa Monti"));
			Rows.Add( new MyLocalizationRow("Text_Night", "Templo de Saka-Bó", "Saka-Bo Temple", "Saka-Bo Temple", "Saka-Bo Tempio"));
			Rows.Add( new MyLocalizationRow("Text_Snow", "Casas de Saka-Yama", "Houses of Saka-Yama", "Maisons de Saka-Yama", "Case di Saka-Yama"));
			Rows.Add( new MyLocalizationRow("Text_Music", "Música", "Music", "musique", "Musica"));
			Rows.Add( new MyLocalizationRow("Text_Credits", "Créditos", "Credits", "Crédits", "Credits"));
			Rows.Add( new MyLocalizationRow("Text_Shop", "Tienda", "Shop", "Boutique", "Negozio"));
			Rows.Add( new MyLocalizationRow("Text_Restore", "Restaurar Compras", "Restore Purchases", "Restaurer achats", "Ripristinare gli acquisti"));
			Rows.Add( new MyLocalizationRow("Text_Lang", "Idioma", "Language", "Langue", "Lingua"));
			Rows.Add( new MyLocalizationRow("Text_Rate", "Valora", "Rate", "Taux", "Tasso"));
			Rows.Add( new MyLocalizationRow("Text_Like", "Me Gusta", "Like", "Aimer", "Come"));
			Rows.Add( new MyLocalizationRow("Text_Pay", "Si consigues continues infinitos, nunca más tendrás que ver un video y podrás disfrutar de tu vida extra", "If you get infinite CONTINUES you'll never have to watch a video and you would be able to enjoy your extra life", "Si vous obtenez infinie persiste, vous aurez jamais à regarder une vidéo et vous seriez en mesure de profiter de votre vie supplémentaire", "Se si ottiene infinita CONTINUA non dovrete mai di guardare un video e si sarebbe in grado di godere la vostra vita extra"));
			Rows.Add( new MyLocalizationRow("Text_Restart", "Reiniciar", "Restart", "Redémarrer", "Ricomincia"));
			Rows.Add( new MyLocalizationRow("Text_GetContinue", "¿Te has quedado sin continues? Aquí puedes ver un video y conseguir un continue gratis. (Los videos son limitados)", "Are you out of CONTINUES? You can watch a video and get one continue for free. (Videos are limited)", "Êtes-vous sur CONTINUE? Vous pouvez regarder une vidéo et en obtenir un gratuitement continuent. (Les vidéos sont limitées)", "Sei fuori di CONTINUA? È possibile guardare un video e ottenere uno continua gratuitamente. (Video sono limitati)"));
			Rows.Add( new MyLocalizationRow("Text_Rateit", "Si te gusta Kick or Die, puntúalo y consigue un logro. ¡Gracias!", "If you like Kick or Die, please rate it and get an achievement. Thanks!", "Si vous aimez coup de pied ou mourir, s'il vous plaît noter et obtenir un résultat. Merci!", "Se ti piace calciare o morire, si prega di votarla e ottenere un risultato. Grazie!"));
			Rows.Add( new MyLocalizationRow("Text_NewFeature", "¡Ahora puedes compartir un video de tu partida y que todos tus amigos vean cómo has jugado!", "You can now share your gameplay session with your friends so they can see how you played!", "Vous pouvez maintenant partager votre session de jeu avec vos amis afin qu'ils puissent voir comment vous avez joué!", "Ora è possibile condividere la tua sessione di gioco con i tuoi amici in modo che possano vedere come si gioca!"));
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
		public MyLocalizationRow GetRow(rowIds in_RowID)
		{
			MyLocalizationRow ret = null;
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
		public MyLocalizationRow GetRow(string in_RowString)
		{
			MyLocalizationRow ret = null;
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
