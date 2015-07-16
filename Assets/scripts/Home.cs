using UnityEngine;
using System.Collections;

public class Home : MonoBehaviour {

	private Camera mainCamera;

	// Use this for initialization
	void Start () {

		mainCamera = Camera.main;

		Debug.Log (Screen.width+":"+Screen.height);
		Screen.SetResolution (Screen.width, Screen.height, true);
//		float aspect = Screen.width / Screen.height;
//		mainCamera.aspect = aspect;
//		mainCamera.orthographicSize = Screen.height / 200;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
