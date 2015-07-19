using UnityEngine;
using System.Collections;

public class FlipBookAnimation : MonoBehaviour {

	// 自定义的枚举类型: 播放类型
	public enum PlayType{
		Forward = 0,
		Reverse = 1,
		PingPong = 2,
		Custom = 3,
		Randomized = 4
	};

	// 循环类型
	public enum LoopType{
		PlayOnce = 0,
		Loop = 1
	};

	// 指向一些里的UV坐标的指针
	public Rect[] UVs;

	// 指定是否自动播放
	public bool AutoPlay = false;

	// 播放动画时,每秒的帧数
	public float FramePerSecond = 10.0f;

	// 默认情况下的播放类型
	public PlayType PlayMethod = PlayType.Forward;

	// 默认情况下的循环类型
	public LoopType LoopMethod = LoopType.PlayOnce;

	// 当 PlayMethod 为 Custom时,设置的第一帧和最后一帧
	public int CustomStartFrame = 0;
	public int CustomEndFrame = 0;

	// 指定是否播放动画的变量
	public bool IsPlaying = false;

	// Use this for initialization
	void Start () {
		if (AutoPlay) {
			StartCoroutine ("Play");
		}
	}

	public IEnumerator Play(){
		// 设置播放的状态
		IsPlaying = true;

		// 获取图片的个数
		int AnimLength = UVs.Length;

		// 循环方式
		int Direction = (PlayMethod == PlayType.Reverse) ? -1 : 1;

		// 由前向后播放
		int StartFrame = (PlayMethod == PlayType.Reverse) ? AnimLength - 1 : 0;

		// 帧数
		int FrameCount = AnimLength - 1;

		// 如果此时没有图片,则退出
		if(FrameCount <= 0){
			yield break;
		}

		// 如果播放模式是 Custom
		if(PlayMethod == PlayType.Custom){
			StartFrame = CustomStartFrame;
			FrameCount = (CustomEndFrame > CustomStartFrame) ? CustomEndFrame - CustomStartFrame : CustomStartFrame - CustomEndFrame;
			Direction = (CustomEndFrame > CustomStartFrame) ? 1 : -1;
		}

		do{
			// 计算当前已经播放的帧的个数
			int FramesPlayed = 0;
			while(FramesPlayed <= FrameCount){
				// 如果是随机播放
				Rect rect = (PlayMethod == PlayType.Randomized)?
					UVs[Mathf.FloorToInt(Random.value * FrameCount)]:UVs[StartFrame+(FramesPlayed * Direction)];
				SetFrame(rect);
				++FramesPlayed;
				// 一直等到下一帧
				yield return new WaitForSeconds(1.0f/FramePerSecond);
			}

			// 如果播放模式是PingPong
			if(PlayMethod == PlayType.PingPong){
				Direction = -Direction;
				StartFrame = (StartFrame == 0)?AnimLength-1 : 0;
			}
		}while(LoopMethod == LoopType.Loop);

		IsPlaying = false ;
	}

	void Stop(){
		if (!IsPlaying) {
			return;
		}

		StopCoroutine ("Play");
		IsPlaying = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// 使用 UV 坐标,给网格对象设置指定帧
	void SetFrame(Rect rect){
		// 获取 mesh filter
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector2[] UVs = new Vector2[mesh.vertices.Length];

		// 左下角的点
		UVs[0].x = rect.x;
		UVs [0].y = rect.y;

		// 右下角的点
		UVs[1].x = rect.x + rect.width;
		UVs [1].y = rect.y;

		// 左上角的点
		UVs[2].x = rect.x;
		UVs [2].y = rect.y + rect.height;

		// 右上角的点
		UVs[3].x = rect.x + rect.width;
		UVs [3].y = rect.y + rect.height;

		mesh.uv = UVs;
	}

	void OnDisable(){
		if (IsPlaying) {
			StopCoroutine ("Play");
		}
	}

	void OnEnable(){
		if (IsPlaying) {
			StartCoroutine ("Play");
		}
	}
}
