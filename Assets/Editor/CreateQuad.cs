using UnityEngine;
using System.Collections;
using UnityEditor;


/// <summary>
/// 编写一个产生 四边形的工具
/// </summary>
public class CreateQuad : ScriptableWizard {

	// 四边形资源的名称
	public string MeshName = "Quad";

	// 实例化对象的名称
	public string GameObjectName = "Plane_Object";

	// 包含这个四边形资源的文件夹的名字
	public string AssetFolder = "Assets";

	// 四边形的宽度
	public float Width = 1.0f;
	public float Height = 1.0f;


	public enum AnchorPoint
	{
		TopLeft,
		TopMiddle,
		TopRight,
		LeftMiddle,
		Center,
		RightMiddle,
		ButtomLeft,
		ButtomMiddle,
		ButtomRight,
		Custom,
	}

	// 四边形轴点(pivot)的位置
	public AnchorPoint Anchor = AnchorPoint.Center;

	// 轴点在四边形上的水平位置
	public float AnchorX = 0.5f;
	public float AnchorY = 0.5f;

	[MenuItem("GameObject/Create Other/Custom Plane")]
	// 用于生成工具的窗口
	static void CreateWizard(){
		ScriptableWizard.DisplayWizard ("Create Plane",typeof(CreateQuad));
	}

	// OnInspectorUpdate is called at 10 frames per second to give the inspector a chance to update.
	// 此事件每秒可以被调用10次
	void OnInspectorUpdate(){
		switch (Anchor) {
		case AnchorPoint.TopLeft:
			AnchorX = 0.0f * Width;
			AnchorY = 1.0f * Height;
			break;
		case AnchorPoint.TopMiddle:
			AnchorX = 0.5f * Width;
			AnchorY = 1.0f * Height;
			break;
		case AnchorPoint.TopRight:
			AnchorX = 1.0f * Width;
			AnchorY = 1.0f * Height;
			break;
		case AnchorPoint.LeftMiddle:
			AnchorX = 0.0f * Width;
			AnchorY = 0.5f * Height;
			break;
		case AnchorPoint.Center:
			AnchorX = 0.5f * Width;
			AnchorY = 0.5f * Height;
			break;
		case AnchorPoint.RightMiddle:
			AnchorX = 1.0f * Width;
			AnchorY = 0.5f * Height;
			break;
		case AnchorPoint.ButtomLeft:
			AnchorX = 0.0f * Width;
			AnchorY = 0.0f * Height;
			break;
		case AnchorPoint.ButtomMiddle:
			AnchorX = 0.5f * Width;
			AnchorY = 0.0f * Height;
			break;
		case AnchorPoint.ButtomRight:
			AnchorX = 1.0f * Width;
			AnchorY = 0.0f * Height;
			break;
		case AnchorPoint.Custom:
			default:
			break;
		}
	}


	// 当工具窗口第一次出现时,触发此函数
	void OnEnable(){
		GetFolderSelection ();
	}

	// 获取当前资源面板中被选中的资源
	void GetFolderSelection(){

		// for debug
		if(Selection.objects != null){
			Debug.Log ("Length : " + Selection.objects.Length);
		}

		if (Selection.objects != null &&
		   Selection.objects.Length == 1) {
			// 获取被选中资源的路径
			AssetFolder = AssetDatabase.GetAssetPath(Selection.objects[0]);
		}
	}



	// 用于创建四边形的函数
	void OnWizardCreate(){

		// 阶段1
		// 创建顶点************************************
		Vector3[] Vertices = new Vector3[4];

		// 左下角的顶点
		Vertices[0].x = -AnchorX;
		Vertices[0].y = -AnchorY;

		// 右下角的顶点
		Vertices[1].x = Width - AnchorX;
		Vertices [1].y = -AnchorY;

		// 左上角的顶点
		Vertices[2].x = -AnchorX;
		Vertices [2].y = Height - AnchorY;

		// 右上角的顶点
		Vertices[3].x = Width - AnchorX;
		Vertices [3].y = Height - AnchorY;

		// 创建UV************************************
		Vector2[] UVs = new Vector2[4];

		// 左下角
		UVs[0].x = 0f;
		UVs [0].y = 0f;

		// 右下角
		UVs[1].x = 1f;
		UVs [1].y = 0f;

		// 右上角
		UVs[2].x = 0f;
		UVs [2].y = 1f;

		// 右上角
		UVs[3].x = 1f;
		UVs [3].y = 1f;

		// 构建四边形的2个三角形
		int[] Triangles = new int[6];

		// 为三角形的各个顶点赋值
		Triangles[0] = 3;
		Triangles[1] = 1;
		Triangles[2] = 2;
		Triangles[3] = 2;
		Triangles[4] = 1;
		Triangles[5] = 0;

		// 得到四边形,生成Mesh
		Mesh mesh = new Mesh();
		mesh.name = MeshName;
		mesh.vertices = Vertices;
		mesh.uv = UVs;
		mesh.triangles = Triangles;
		mesh.RecalculateNormals ();

		// 阶段2
		// 还要在资源面板中创建
		AssetDatabase.CreateAsset(mesh,AssetDatabase.GenerateUniqueAssetPath(AssetFolder+"/"+MeshName)+".asset");
		AssetDatabase.SaveAssets ();

		// 阶段3
		// 在场景中实例化一个四边形
		// 先创建平面对象
		GameObject plane = new GameObject(GameObjectName);
		MeshFilter meshFilter = (MeshFilter)plane.AddComponent (typeof(MeshFilter));
		plane.AddComponent (typeof(MeshRenderer));

		// 赋予四边形 mesh filter组件
		meshFilter.sharedMesh = mesh;
		mesh.RecalculateBounds ();

		// 还可以赋予四边形box collider组件

	}
}
