using UnityEngine;
using System.Collections;


/// <summary>
/// 切换多个 sprite
/// </summary>
public class TiledSprite : MonoBehaviour {

	private SpriteRenderer render;
	public Sprite[] sprites;
	public int index = 0;

	private int length;

	private Color defaultColor;

	// Use this for initialization
	void Start () {
		render = GetComponent<SpriteRenderer> ();
		length = sprites.Length;
		defaultColor = render.color;
		if (index >= length || index < 0) {
			index = 0;
		}
		render.sprite = sprites[index];
	}
	

	void OnMouseDown(){
		render.color = Color.gray;
	}


	void OnMouseUp(){
		
		render.color = defaultColor;

		++index;
		if (index >= length) {
			index = 0;
		}
		render.sprite = sprites[index];
	}


}
