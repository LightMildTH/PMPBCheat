using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace PMPBCheat
{
	public class SamildCheat : MonoBehaviour
	{
		private void Start()
		{
			this.roomNotifStyle.normal.textColor = Color.gray;
			this.roomNotifStyle.alignment = 0;
			this.roomNotifStyle.fontSize = 23;
			this.roomTempStyle.normal.textColor = Color.gray;
			this.roomTempStyle.alignment = 0;
			this.roomTempStyle.fontSize = 23;
			new Thread(delegate ()
			{
				for (; ; )
				{
					base.StartCoroutine(this.FindObjects());
					Thread.Sleep(3000);
				}
			}).Start();
		}

		private void Update()
		{
			distance = Vector3.Distance(Ghost.transform.position, this.LocalPlayer.firstPersonController.transform.position);
			if (Keyboard.current.f1Key.wasPressedThisFrame)
			{
				this.OnGUI();
			}
			if (Keyboard.current.nKey.wasPressedThisFrame)
			{
				if (this.nightVision)
				{
					try
					{
						LevelController.instance.gameController.myPlayer.player.cam.gameObject.GetComponent<Nightvision>().enabled = false;
						LevelController.instance.gameController.myPlayer.player.cam.gameObject.GetComponent<Light>().enabled = false;
					}
					catch (Exception)
					{
					}
					this.nightVision = false;
				}
				else
				{
					Nightvision component;
					Light component2;
					if (LevelController.instance.gameController.myPlayer.player.cam.gameObject.GetComponent<Nightvision>() == null)
					{
						LevelController.instance.gameController.myPlayer.player.cam.gameObject.AddComponent<Nightvision>();
						component = LevelController.instance.gameController.myPlayer.player.cam.gameObject.GetComponent<Nightvision>();
						component.EffectColor = Color.grey;
						component.Power = LevelController.instance.nightVisionPower;
						LevelController.instance.gameController.myPlayer.player.cam.gameObject.AddComponent<Light>();
						component2 = LevelController.instance.gameController.myPlayer.player.cam.gameObject.GetComponent<Light>();
						component2.type = 0;
						component2.intensity = 0.4f;
						component2.spotAngle *= 2f;
						component2.shape = 0;
					}
					else
					{
						component = LevelController.instance.gameController.myPlayer.player.cam.gameObject.GetComponent<Nightvision>();
						component2 = LevelController.instance.gameController.myPlayer.player.cam.gameObject.GetComponent<Light>();
					}
					component.enabled = true;
					component2.enabled = true;
					this.nightVision = true;
				}
			}
			if (Keyboard.current.f9Key.wasPressedThisFrame)
			{
				if (GameController.instance)
				{
					GameController.instance.myPlayer.player.transform.position = MultiplayerController.instance.spawns[0].position;
				}
				else
				{
					MainManager.instance.localPlayer.transform.position = MainManager.instance.spawns[0].position;
				}
			}
			if (Keyboard.current.f10Key.wasPressedThisFrame)
			{
				if (!this.speedHack)
				{
					GameController.instance.myPlayer.player.firstPersonController.m_RunSpeed = 5f;
					this.speedHack = true;
				}
				else
				{
					GameController.instance.myPlayer.player.firstPersonController.m_RunSpeed = this.normalRS;
					this.speedHack = false;
				}
			}
		}

		private void OnGUI()
		{
			this.enableesp = !this.enableesp;
			if (this.enableesp == true)
			{
				GUI.color = Color.red;
				GUI.skin.label.fontSize = 11;
				GUI.Label(new Rect(20f, 10f, 200f, 200f), "PhasmophobiaCheat | ซามายด์.#4198");
				this.playerInfoGUI();
				this.ghostInfoGUI();
				this.GhostESP();
				this.OuijaESP();
				this.BoneESP();
				this.PlayerESP();
				this.roomInfo();
				return;
			}
		}

		private void playerInfoGUI()
		{
			GUI.color = Color.white;
			GUI.skin.label.fontSize = 14;
			GUI.Label(new Rect(20f, 25f, 200f, 200f), "Hi " + PhotonNetwork.playerName);
			GUI.Label(new Rect(20f, 40f, 200f, 200f), "Sanity : " + Convert.ToInt32(100f - this.LocalPlayer.insanity).ToString());
		}

		private void ghostInfoGUI()
		{
			GUIStyle guistyle = new GUIStyle();
			guistyle.richText = true;
			guistyle.fontSize = 14;
			string str = this.GhostTypeData(this.Ghost.ghostInfo.ghostTraits.ghostType);
			GUI.Label(new Rect(20f, 80f, 900f, 900f), "<color=white>Name : </color>" + this.ghostInfo.ghostTraits.ghostName);
			GUI.Label(new Rect(20f, 100f, 900f, 900f), "<color=white>Ghost :</color>" + str, guistyle);
			GUI.Label(new Rect(20f, 130f, 200f, 20f), "Room : " + this.ghostInfo.favouriteRoom.roomName.ToString());
			GUI.Label(new Rect(20f, 170f, 900f, 900f), "Distance : " + distance.ToString());
			if (!this.Ghost.isHunting)
			{
				GUI.Label(new Rect(20f, 150f, 900f, 900f), "<color=white>is Hunting :</color> <color=red>No</color>", guistyle);
				return;
			}
			GUI.Label(new Rect(20f, 150f, 900f, 900f), "<color=white>is Hunting :</color> <color=green>Yes</color>", guistyle);
		}

		private void GhostESP()
		{
			if (this.Ghost)
			{
				Vector3 vector = Camera.main.WorldToScreenPoint(this.Ghost.transform.position);
				if (vector.z > 0f)
				{
					vector.y = (float)Screen.height - (vector.y + 1f);
					GUI.color = Color.red;
					GUI.Label(new Rect(new Vector2(vector.x, vector.y), new Vector2(100f, 100f)), this.ghostInfo.ghostTraits.ghostType.ToString() + "\n" + distance.ToString());
				}
			}
		}

		private void PlayerESP()
		{
			foreach (PlayerData playerData in GameController.instance.playersData)
			{
				Vector3 vector = Camera.main.WorldToScreenPoint(playerData.player.transform.position);
				if (vector.z > 0f)
				{
					vector.y = (float)Screen.height - (vector.y + 1f);
					GUI.color = Color.cyan;
					GUI.Label(new Rect(new Vector2(vector.x, vector.y), new Vector2(100f, 100f)), playerData.playerName);
				}
			}
		}

		private void OuijaESP()
		{
			if (this.ouija)
			{
				Vector3 vector = Camera.main.WorldToScreenPoint(this.ouija.transform.position);
				if (vector.z > 0f)
				{
					vector.y = (float)Screen.height - (vector.y + 1f);
					GUI.color = Color.yellow;
					GUI.Label(new Rect(new Vector2(vector.x, vector.y), new Vector2(100f, 100f)), "Ouija Board");
				}
			}
		}

		private void BoneESP()
		{
			if (this.boneDNA)
			{
				Vector3 vector = Camera.main.WorldToScreenPoint(this.boneDNA.transform.position);
				if (vector.z > 0f)
				{
					vector.y = (float)Screen.height - (vector.y + 1f);
					GUI.color = Color.white;
					GUI.Label(new Rect(new Vector2(vector.x, vector.y), new Vector2(100f, 100f)), "Bone");
				}
			}
		}

		private void roomInfo()
		{
			if (this.roomInfoEsp)
			{
				try
				{
					if (GameController.instance.myPlayer.player.currentRoom.roomName.ToString() == LevelController.instance.currentGhostRoom.roomName.ToString())
					{
						this.roomNotifStyle.normal.textColor = Color.yellow;
					}
					else
					{
						this.roomNotifStyle.normal.textColor = Color.gray;
					}
					GUI.Label(new Rect(10f, this.scrHeight - 30f, 250f, 100f), "<b>" + GameController.instance.myPlayer.player.currentRoom.roomName.ToString() + "</b>", this.roomNotifStyle);
					GUI.Label(new Rect(10f, this.scrHeight - 70f, 250f, 100f), "<b>" + ((float)Math.Round((double)(GameController.instance.myPlayer.player.currentRoom.temperature * 100f)) / 100f).ToString() + "°C</b>", this.roomTempStyle);
				}
				catch (Exception)
				{
					GUI.Label(new Rect(10f, this.scrHeight - 30f, 250f, 100f), "<b>Pre-Game</b>", this.roomNotifStyle);
				}
			}
		}

		private string GhostTypeData(GhostTraits.Type type)
		{
			switch (type)
			{
				case GhostTraits.Type.none:
					return "None";
				case GhostTraits.Type.Spirit:
					return " <color=red>Spirit</color> \n<color=green>Box</color> <color=white>|</color> <color=green>Fingerprint</color> <color=white>|</color> <color=green>Writing</color>";
				case GhostTraits.Type.Wraith:
					return " <color=red>Wraith</color> \n<color=green>Fingerprint</color> <color=white>|</color> <color=green>Freezing</color> <color=white>|</color> <color=green>Box</color>";
				case GhostTraits.Type.Phantom:
					return " <color=red>Phantom</color> \n<color=green>EMF5</color> <color=white>|</color> <color=green>Orbs</color> <color=white>|</color> <color=green>Freezing</color>";
				case GhostTraits.Type.Poltergeist:
					return " <color=red>Poltergeist</color> \n<color=green>Box</color> <color=white>|</color> <color=green>Fingerprint</color> <color=white>|</color> <color=green>Orbs</color>";
				case GhostTraits.Type.Banshee:
					return " <color=red>Banshee</color> \n<color=green>EMF5</color> <color=white>|</color> <color=green>Fingerprint</color> <color=white>|</color> <color=green>Freezing</color>";
				case GhostTraits.Type.Jinn:
					return " <color=red>Jinn</color> \n<color=green>Box</color> <color=white>|</color> <color=green>Orbs</color> <color=white>|</color> <color=green>EMF5</color>";
				case GhostTraits.Type.Mare:
					return " <color=red>Mare</color> \n<color=green>Box</color> <color=white>|</color> <color=green>Orbs</color> <color=white>|</color> <color=green>Freezing</color>";
				case GhostTraits.Type.Revenant:
					return " <color=red>Revenant</color> \n<color=green>EMF5</color> <color=white>|</color> <color=green>Fingerprints</color> <color=white>|</color> <color=green>Writing</color>";
				case GhostTraits.Type.Shade:
					return " <color=red>Shade</color> \n<color=green>EMF5</color> <color=white>|</color> <color=green>Orbs</color> <color=white>|</color> <color=green>Writing</color>";
				case GhostTraits.Type.Demon:
					return " <color=red>Demon</color> \n<color=green>Box</color> <color=white>|</color> <color=green>Writing</color> <color=white>|</color> <color=green>Freezing</color>";
				case GhostTraits.Type.Yurei:
					return " <color=red>Yurei</color> \n<color=green>Orb</color> <color=white>|</color> <color=green>Writing</color> <color=white>|</color> <color=green>Freezing</color>";
				case GhostTraits.Type.Oni:
					return " <color=red>Oni</color> \n<color=green>EMF5</color> <color=white>|</color> <color=green>Box</color> <color=white>|</color> <color=green>Writing</color>";
				default:
					return "";
			}
		}

		private IEnumerator FindObjects()
		{
			this.Ghost = Object.FindObjectOfType<GhostAI>();
			yield return new WaitForSeconds(0.1f);
			this.ghostInfo = Object.FindObjectOfType<GhostInfo>();
			yield return new WaitForSeconds(0.1f);
			this.ouija = Object.FindObjectOfType<OuijaBoard>();
			yield return new WaitForSeconds(0.1f);
			this.boneDNA = Object.FindObjectOfType<DNAEvidence>();
			yield return new WaitForSeconds(0.1f);
			yield break;
		}

		private Player LocalPlayer
		{
			get
			{
				return GameController.instance.myPlayer.player;
			}
		}

        public SamildCheat()
		{
		}

		public GhostAI Ghost;

		public GhostInfo ghostInfo;

		public OuijaBoard ouija;

		public DNAEvidence boneDNA;

		public GhostTraits gTraits;

		private GUIStyle notifStyle = new GUIStyle();

		private GUIStyle roomNotifStyle = new GUIStyle();

		private GUIStyle roomTempStyle = new GUIStyle();

		private float scrHeight = (float)Screen.height;

		private float scrWidth = (float)Screen.width;

		private float normalRS = 1.6f;

		private bool speedHack;

		private bool nightVision;

		private bool roomInfoEsp = true;

		private bool enableesp = false;

		private float distance;
    }
}
