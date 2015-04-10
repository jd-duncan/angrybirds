using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	public GUIText countText;
	public GUIText winText;
	private int count;

	void Start () {
		count = 0;
		SetCountText ();
		winText.text = "";
	}

	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {

		if (other.gameObject.tag == "Pickup") {
			other.gameObject.SetActive (false);
			count = count + 1;
			SetCountText (); 
		}
		//Destroy(other.gameObject);
		//gameObject.tag = "Player";
	}

	void SetCountText () {
		countText.text = "Count: " + count.ToString ();
		if (count >= 12) {
			winText.text = "YOU WIN!!!";
		}
	}

}
