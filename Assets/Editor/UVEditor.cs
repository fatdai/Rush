using UnityEngine;
using System.Collections;
using UnityEditor;

public class UVEditor : EditorWindow {

	// 表示纹理预置对象
	public GameObject AtlasDataObject = null;

	// 表示图集中的数据
	public AtlasData AtlasDataComponent = null;

	// 下拉列表中默认选中项的索引
	public int PopupIndex = 0;

	// 纹理选择模式:sprites 或者 custom
	public string[] Modes = {"Select By Sprites","Select By UVs"};

	// 选中的模式索引
	public int ModeIndex = 0;
	public Rect CustomRect = new Rect(0,0,0,0);


	[MenuItem("Window/Atlas UV Editor")]
	static void Init(){
		// 显示窗口
		GetWindow(typeof(UVEditor),false,"Texture Atlas",true);
	}


	void OnGUI(){
		// 绘制纹理图集选择器
		GUILayout.Label("Atlas Generation",EditorStyles.boldLabel);
		AtlasDataObject = (GameObject)EditorGUILayout.ObjectField ("Atlas Object",AtlasDataObject,typeof(GameObject),true);

		// 如果没有可用的纹理图集对象,则取消
		if(AtlasDataObject == null){
			return;
		}

		// 获取纹理预置对象的 atlas data 组件
		AtlasDataComponent = AtlasDataObject.GetComponent<AtlasData>();
		if (!AtlasDataComponent) {
			return;
		}

		// 显示弹出的选择器,选择可用的纹理
		ModeIndex = EditorGUILayout.Popup(ModeIndex,Modes);
		if (ModeIndex != 1) {
			// 弹出当前可用的纹理的选择器
			PopupIndex = EditorGUILayout.Popup (PopupIndex, AtlasDataComponent.TextureNames);

			// 点击按钮
			if(GUILayout.Button("Select From Atlas")){
				// 开始设置选中的网格对象的UV坐标
				if(Selection.gameObjects.Length > 0){
					foreach (GameObject obj in Selection.gameObjects) {
						// 确认是否是网格对象
						if(obj.GetComponent<MeshFilter>()){
							UpdateUVs (obj,AtlasDataComponent.UVs[PopupIndex]);
						}
					}
				}
			}

		} else {
			// select UVs
			GUILayout.Label("X");
			CustomRect.x = EditorGUILayout.FloatField(CustomRect.x);
			GUILayout.Label ("Y");
			CustomRect.y = EditorGUILayout.FloatField (CustomRect.y);
			GUILayout.Label ("Width");
			CustomRect.width = EditorGUILayout.FloatField (CustomRect.width);
			GUILayout.Label ("Height");
			CustomRect.height = EditorGUILayout.FloatField (CustomRect.height);

			// 点击按钮
			if(GUILayout.Button("Select From Atlas")){
				// 开始设置选中的网格对象的UV坐标
				if(Selection.gameObjects.Length > 0){
					foreach (GameObject obj in Selection.gameObjects) {
						// 确认是否为网格对象
						if(obj.GetComponent<MeshFilter>()){
							UpdateUVs (obj,CustomRect);
						}
					}
				}
			}
		}
	}


	// 更新UVs的方法
	void UpdateUVs(GameObject MeshObject,Rect AtlasUVs,bool Reset = false){
		// 获取 Mesh Filter 组件
		MeshFilter MFilter = MeshObject.GetComponent<MeshFilter>();
		Mesh mesh = MFilter.sharedMesh;

		Vector2[] UVs = new Vector2[mesh.vertices.Length];

		// 矩形左下角的坐标
		UVs[0].x = (Reset)?0f:AtlasUVs.x;
		UVs [0].y = (Reset) ? 0f : AtlasUVs.y;

		// 矩形右下角的坐标
		UVs[1].x = (Reset)?1f:AtlasUVs.x + AtlasUVs.width;
		UVs [1].y = (Reset) ? 0f : AtlasUVs.y;

		// 左上角的坐标
		UVs[2].x = (Reset)?0f:AtlasUVs.x;
		UVs [2].y = (Reset) ? 1f : AtlasUVs.y + AtlasUVs.height;

		// 矩形右上角的坐标
		UVs[3].x  = (Reset)?1.0f:AtlasUVs.x + AtlasUVs.width;
		UVs [3].y = (Reset) ? 1.0f : AtlasUVs.y + AtlasUVs.height;

		mesh.uv = UVs;
		AssetDatabase.Refresh ();
		AssetDatabase.SaveAssets ();
	}

}
