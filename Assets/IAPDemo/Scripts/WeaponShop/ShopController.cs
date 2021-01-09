using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopController : MonoBehaviour {


	public Canvas shopCanvas;
	public GameObject iapPanel;
	public GameObject mainPanel;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Player"))
			OpenShop ();


	}

	void OpenShop()
	{
		shopCanvas.enabled = true;
		Time.timeScale = 0;
	}

	public void CloseShop()
	{
		shopCanvas.enabled = false;
		Time.timeScale = 1;
	}



	public void OpenIAP()
	{
		mainPanel.SetActive (false);
		iapPanel.SetActive (true);
	}

	public void CloseIAP()
	{
		mainPanel.SetActive (true);
		iapPanel.SetActive (false);
	}

}
