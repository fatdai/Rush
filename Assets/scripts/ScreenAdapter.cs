using UnityEngine;
using System.Collections;

public class ScreenAdapter : MonoBehaviour {
	 
	    private Camera mainCamera;
	 
	    private static float devWidth = 960f;
	    private static float devHeight = 640f;
	    private static float devAspect = devWidth / devHeight;
	 
	    // Use this for initialization
	    void Start () {
		        mainCamera = Camera.main;
		 
		        mainCamera.aspect = devAspect;
		        mainCamera.orthographicSize = devHeight * 0.01f / 2;
		 }
}
