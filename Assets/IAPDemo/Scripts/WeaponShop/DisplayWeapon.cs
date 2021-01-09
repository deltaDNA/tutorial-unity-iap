using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace CompleteProject
{
	public class DisplayWeapon : MonoBehaviour {

		public PlayerShooting playerShooting;
		public int weaponNumber;
		public ShopController shopController;


		public Text weaponName;
		public Text cost;
		public Text description;
		 

		private AudioSource source;


		// Use this for initialization
		void Start () 
		{
			source = GetComponent<AudioSource> ();
			SetButton ();
		}

		void SetButton()
		{
			weaponName.text = playerShooting.weapons [weaponNumber].weaponName;
			cost.text = "$" + playerShooting.weapons [weaponNumber].cost;
			description.text = playerShooting.weapons [weaponNumber].description;
		}

		public void OnClick()
		{
			if (ScoreManager.score >= playerShooting.weapons [weaponNumber].cost) {
				ScoreManager.score -= playerShooting.weapons [weaponNumber].cost;
				playerShooting.SetWeapon(weaponNumber);
				shopController.CloseShop ();
			} else 
			{
				source.Play();
			}

		}


	}
}
