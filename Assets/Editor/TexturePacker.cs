using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class TexturePacker : ScriptableWizard {

	// 指向一系列纹理的指针
	public Texture2D[] Textures;

	public string AtlasName = "Atlas_Texture";

	[MenuItem("GameObject/Create Other/Atlas Texture")]
	static void CreateWizard(){
		ScriptableWizard.DisplayWizard ("Create Atlas",typeof(TexturePacker));
	}


	void OnEnable(){
		// 新的纹理列表
		List<Texture2D> TextureList = new List<Texture2D>();

		// 遍历每个被选中的对象
		if(Selection.objects != null && Selection.objects.Length > 0){

			// 取得所有的
			Object[] objects = EditorUtility.CollectDependencies (Selection.objects);
			foreach (Object o in objects) {
				// 得到选中的对象
				Texture2D tex = o as Texture2D;

				// 检查对象是否是纹理资源
				if(tex != null){
					// 把纹理加入到列表
					TextureList.Add(tex);
				}
			}
		}

		// 检查列表中是否有纹理
		if(TextureList.Count > 0){
			Textures = new Texture2D[TextureList.Count];
			for (int i = 0; i < TextureList.Count; ++i) {
				Textures [i] = TextureList [i];
			}
		}
	}


	// 步骤1  优化输入的纹理
	private void ConfigureForTextureAtlas(string TexturePath){
		// 1. 获取指定路径下的纹理
		TextureImporter TexImport = AssetImporter.GetAtPath(TexturePath) as TextureImporter;
		TexImport.textureType = TextureImporterType.Advanced;

		// 2.修改此纹理的设置
		TextureImporterSettings tiSettings = new TextureImporterSettings();
		TexImport.ReadTextureSettings (tiSettings);

		tiSettings.mipmapEnabled = false;
		tiSettings.readable = true;
		tiSettings.maxTextureSize = 4096;
		tiSettings.textureFormat = TextureImporterFormat.ARGB32;
		tiSettings.filterMode = FilterMode.Point;
		tiSettings.wrapMode = TextureWrapMode.Clamp;
		tiSettings.npotScale = TextureImporterNPOTScale.None;
		TexImport.SetTextureSettings (tiSettings);

		// 3.重新把纹理导入到 unity 中
		AssetDatabase.ImportAsset(TexturePath,ImportAssetOptions.ForceUpdate);
		AssetDatabase.Refresh ();

	}

	// 步骤 2 ,  构建纹理图集
	private void GenerateAtlas(){
		//   
		GameObject AtlasObject = new GameObject("obj_"+AtlasName);

		AtlasData AtlasComp = AtlasObject.AddComponent<AtlasData> ();
		AtlasComp.TextureNames = new string[Textures.Length];

		// 使用循环配置每一个要加入到图集的纹理
		for(int i = 0; i < Textures.Length;++i){

			// 获取纹理在资源中的路径
			string TexturePath = AssetDatabase.GetAssetPath (Textures[i]);

			// 修改纹理的配置信息
			ConfigureForTextureAtlas(TexturePath);

			// 将所有的纹理的名字都加入到列表里面
			AtlasComp.TextureNames[i] = TexturePath;
		}

		// 开始生成
		Texture2D texture = new Texture2D(1,1,TextureFormat.ARGB32,false);
		AtlasComp.UVs = texture.PackTextures (Textures,1,4096);

		// 生成唯一的资源路径
		string AssetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/"+AtlasName+".png");

		// 把纹理集保存成文件
		byte[] bytes = texture.EncodeToPNG();

		System.IO.File.WriteAllBytes (AssetPath,bytes);
		bytes = null;

		// 删除生成的纹理图集
		UnityEngine.Object.DestroyImmediate(texture);

		// 导入纹理资源
		AssetDatabase.ImportAsset(AssetPath);

		// 获取导入的纹理
		AtlasComp.AtlasTexture = AssetDatabase.LoadAssetAtPath(AssetPath,typeof(Texture2D)) as Texture2D;

		// 配置纹理图集
		ConfigureForTextureAtlas(AssetDatabase.GetAssetPath(AtlasComp.AtlasTexture));


		//************************************************
		// 保存图集为 预置
		AssetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/atlasdata_"+AtlasName+".prefab");

		// 创建预置对象
		Object prefab = PrefabUtility.CreateEmptyPrefab(AssetPath);

		// 更新和保存预置对象
		PrefabUtility.ReplacePrefab(AtlasObject,prefab,ReplacePrefabOptions.ConnectToPrefab);
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh ();

		// 销毁原来的对象
		DestroyImmediate(AtlasObject);
	}

	void OnWizardCreate(){
		GenerateAtlas ();
	}
}
