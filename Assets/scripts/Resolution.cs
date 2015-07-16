using UnityEngine;
using System.Collections;

public class Resolution : MonoBehaviour {

	private Camera mainCamera;

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
		mainCamera.aspect = 1136f / 640f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
