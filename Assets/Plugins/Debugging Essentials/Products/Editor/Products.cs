using UnityEngine;
using UnityEditor;
using System;

namespace DebuggingEssentials
{
	public class Products
	{
        public Texture mcsIcon;
        public Texture mcsCavesIcon;
        public Texture deIcon;
        public Texture tcIcon;
        public Texture wcIcon;
        
        public bool mcsInProject, mcsCavesInProject, deInProject, tcInProject, wcInProject;
        
        float scrollBarX;

        public Products()
        {
			mcsInProject = (GetType("MeshCombineStudio.MeshCombiner") != null);
			mcsCavesInProject = (GetType("MeshCombineStudio.RemoveOverlappingTris") != null);
			deInProject = (GetType("DebuggingEssentials.RuntimeInspector") != null);
			tcInProject = (GetType("TerrainComposer2.TC_Generate") != null);
			wcInProject = (GetType("WorldComposer.terrain_area_class") != null);
		}

		public static Type GetType(string typeName)
		{
			Type type = Type.GetType(typeName + ", Assembly-CSharp");
			if (type != null) return type;

			type = Type.GetType(typeName + ", Assembly-CSharp-firstpass");
			return type;
		}

		public void Draw(MonoBehaviour monoBehaviour)
        {
            if (mcsCavesIcon == null || deIcon == null)
            {
                string path = AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(monoBehaviour)).Replace("WindowManager/WindowManager.cs", "Products/");
                mcsIcon = AssetDatabase.LoadAssetAtPath(path + "MCSIcon.jpg", typeof(Texture)) as Texture;
                mcsCavesIcon = AssetDatabase.LoadAssetAtPath(path + "MCSCavesIcon.jpg", typeof(Texture)) as Texture;
                deIcon = AssetDatabase.LoadAssetAtPath(path + "deIcon.jpg", typeof(Texture)) as Texture;
                tcIcon = AssetDatabase.LoadAssetAtPath(path + "tcIcon.jpg", typeof(Texture)) as Texture;
                wcIcon = AssetDatabase.LoadAssetAtPath(path + "wcIcon.jpg", typeof(Texture)) as Texture; 
            }

            Rect rect1 = GUILayoutUtility.GetLastRect();

            Rect rect = new Rect(rect1.x, rect1.y, 128, 128);
            rect.x += 4;
            rect.y += 2;
            rect.x -= scrollBarX;
            float width = rect.width + 4;

            float found = 0;

            // Debug.Log(Screen.width + " " + (width * 4));

            if (!mcsInProject)
            {
                if (GUI.Button(rect, new GUIContent(string.Empty, mcsIcon, "50% Discount on Unity's Super Sale NOW!\nFrom $45 only for $22.50!\n\nClick to go to 'Mesh Combine Studio 2'.\n\nMesh Combine Studio is an automatic grid cell based mesh combiner which can dramatically improve the performance of your game. MCS can give up to 20x better performance compared to Unity’s static batching, while giving a more smooth and stable FPS.\n\nWe use MCS grid cell based combining in our game DRONE for the modular building in our arena editor and pre - made arenas, without MCS our game wouldn't run.\n\nInstead of manually combining meshes, which is very tedious, MCS will do this automatically for you sorted in grid cells, and the performance improvements it gives cannot be achieved with manual combining. Just simply drag and drop a MCS prefab in your Scene and tweak the many available conditions to your specific needs and you are ready to go.")))
                {
                    Application.OpenURL("https://assetstore.unity.com/packages/tools/modeling/mesh-combine-studio-2-101956?aid=1011le9dK&utm_source=aff");
                }
                rect.x += width;
                found++;
            }
            if (!mcsCavesInProject)
            {
                if (GUI.Button(rect, new GUIContent(string.Empty, mcsCavesIcon, "Click to go to 'MCS Caves & Overhangs' Extension.\n\nThis 'Mesh Combine Studio 2' extension gives the best performance possible by using the easiest way to get amazing looking Caves and Overhangs on any terrain solution.\n\nYou can use any Rock Asset pack from the Unity Asset Store and stack them together to create caves and overhangs, and this extension will combine all rocks and remove all inside polygons that are never visible. Resulting in a ~60-80% polygon removal + combining which gives unbeatable performance and lightmap texture reduction.\n\nOur arenas in DRONE are optimized with this MCS extension. It can also be used on any other kind of closed meshes like a level consisting out of snapped cubes.")))
                {
                    Application.OpenURL("https://assetstore.unity.com/packages/tools/terrain/mcs-caves-overhangs-144413?aid=1011le9dK");
                }
                rect.x += width;
                found++;
            }
            if (!deInProject)
            {
                if (GUI.Button(rect, new GUIContent(string.Empty, deIcon, "50% Discount on Unity's Super Sale NOW!\nFrom $35 only for $17.50!\n\nClick to go to 'Debugging Essentials'.\n\nDebugging Essentials contains 5 crucial tools which will save you tons of time needed for coding while avoiding debugging headaches, making developing a lot more enjoyable!\n\n* Runtime Hierarchy.\n* Runtime Deep Inspector.\n* Runtime Camera Navigation.\n* Runtime Console.\n* HTML Debug Logs.")))
                {
                    Application.OpenURL("https://assetstore.unity.com/packages/tools/utilities/debugging-essentials-170773?aid=1011le9dK");
                }
                rect.x += width;
                found++;
            }
            if (!tcInProject)
            {
                if (GUI.Button(rect, new GUIContent(string.Empty, tcIcon, "Click to go to 'Terrain Composer 2'.\n\nTerrainComposer2 is a powerful node based multi-terrain tile generator. TC2 makes use of the latest GPU technology to give you instant real-time results, which makes creating terrains faster and more easy than ever before.\n\nTC2 its folder like layer system and workflow is similar to that of Photoshop, which makes it possible to have full control and make quick changes any time during the workflow.")))
                {
                    Application.OpenURL("https://assetstore.unity.com/packages/tools/terrain/terrain-composer-2-65563?aid=1011le9dK");
                }
                rect.x += width;
                found++;
            }
            if (!wcInProject)
            {
                if (GUI.Button(rect, new GUIContent(string.Empty, wcIcon, "50% Discount on Unity's Super Sale NOW!\nFrom $45 only for $22.50!\n\nClick to go to 'World Composer'.\n\nWorldComposer is a tool to extract heightmap data and satellite images from the real world. It uses Bing maps, like in the new Microsoft Flight Simulator 2020.\n\nWorldComposer can create terrains by itself, but the exported heightmaps(e.g. as stamps) and satellite images can also be used in other terrain tools like TerrainComposer 2, Gaia, WorldMachine, MapMapic, WorldCreator, etc.")))
                {
                    Application.OpenURL("https://assetstore.unity.com/packages/tools/terrain/world-composer-13238?aid=1011le9dK");
                }
                rect.x += width;
                found++;
            }

            float size = (128 * (found + 0.5f)) - Screen.width;

            if (found > 0)
            {
                GUILayout.Space(132);
                if (size > 0)
                {
                    scrollBarX = GUILayout.HorizontalScrollbar(scrollBarX, 25, 0, size);
                }
                DrawSpacer(0, 5, 0);
            }
        }

        static public void DrawSpacer(float spaceBegin = 5, float height = 5, float spaceEnd = 5)
        {
            GUILayout.Space(spaceBegin - 1);
            EditorGUILayout.BeginHorizontal();
            GUI.color = new Color(0.5f, 0.5f, 0.5f, 1);
            GUILayout.Button(string.Empty, GUILayout.Height(height));
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(spaceEnd - 1);

            GUI.color = Color.white;
        }
    }
}
