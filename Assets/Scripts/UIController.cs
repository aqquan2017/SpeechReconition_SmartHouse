using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
	public GameObject UIAbout;

	private Camera fpsCam;
	private int layerMask;
	private MeshRenderer meshRenderer;
	// Start is called before the first frame update
	void Start()
    {
		fpsCam = Camera.main;
		LayerMask iRayLM = LayerMask.NameToLayer("MyUI");
		layerMask = 1 << iRayLM.value;
		meshRenderer = GetComponent<MeshRenderer>();
	}

    // Update is called once per frame
    void Update()
    {
		Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
		RaycastHit hit;

		if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, 3, layerMask))
		{
			if (!isEqualToParent(hit.collider)) return;
			PlayerVoiceProcess.instance.micState = true;
			meshRenderer.material.color = Color.cyan;

			//	//check playerspech here
			CallUI(hit.collider);
        }
		else
        {
			meshRenderer.material.color = Color.white;
		}

	}

    private void CallUI(Collider collider)
    {
		if (PlayerVoiceProcess.instance.playerSpeech != collider.gameObject.name)
		{
			PlayerVoiceProcess.instance.playerSpeech = "";
			return;
		}

			if (PlayerVoiceProcess.instance.playerSpeech == "Start")
        {
			PlayerMovement.instance.enabled = true;
        }
		else if(PlayerVoiceProcess.instance.playerSpeech == "About")
        {
			UIAbout.SetActive(true);
			StartCoroutine(OffUI(5f, UIAbout));
        }
		else if (PlayerVoiceProcess.instance.playerSpeech == "Quit")
        {
			Application.Quit();
        }
		else if (PlayerVoiceProcess.instance.playerSpeech == "Restart")
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		PlayerVoiceProcess.instance.playerSpeech = "";
	}

    private IEnumerator OffUI(float v, GameObject uIAbout)
    {
		yield return new WaitForSeconds(v);
		uIAbout.SetActive(false);
    }

    private bool isEqualToParent(Collider other)
	{
		bool rtnVal = false;
		try
		{
			int maxWalk = 5;

			GameObject currentGO = other.gameObject;
			for (int i = 0; i < maxWalk; i++)
			{
				if (currentGO.Equals(this.gameObject))
				{
					rtnVal = true;
					break;          //exit loop early.
				}

				//not equal to if reached this far in loop. move to parent if exists.
				if (currentGO.transform.parent != null)     //is there a parent
				{
					currentGO = currentGO.transform.parent.gameObject;
				}
			}
		}
		catch (System.Exception e)
		{
			Debug.Log(e.Message);
		}

		return rtnVal;
	}
}
