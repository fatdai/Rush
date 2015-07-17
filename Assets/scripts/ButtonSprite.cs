using UnityEngine;
using System.Collections;


/// <summary>
/// 主要是按钮:
///     a. 默认的点击效果
/// 	b. 切换点击的图片
/// </summary>
public class ButtonSprite : MonoBehaviour{

	public enum BTN_TYPE
	{
		DEFAULT,
		NORMAL,
	};

	public string name = "ButtonSprite";
	public BTN_TYPE btnType = BTN_TYPE.DEFAULT;

	private Sprite defauleSprite;
	public Sprite clickedSprite;

	private SpriteRenderer render;
	private Color defaultColor;
	void Start(){
		render = GetComponent<SpriteRenderer> ();
		defauleSprite = render.sprite;
		defaultColor = render.color;
	}

	void OnMouseDown() {
		if (btnType == BTN_TYPE.DEFAULT) {
			render.color = Color.gray;
		} else if (btnType == BTN_TYPE.NORMAL) {
			render.sprite = clickedSprite;
		}
	}

	void OnMouseUp(){
		if (btnType == BTN_TYPE.DEFAULT) {
			render.color = defaultColor;
		}else if(btnType == BTN_TYPE.NORMAL){
			render.sprite = defauleSprite;
		}
		Debug.Log ("btn clicked!!!");
	}
}
