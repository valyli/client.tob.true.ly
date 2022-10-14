using System.Collections;
using System.Collections.Generic;
using GameFramework.Resource;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    public class ResourceMode : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        [MenuItem("Game Framework/Resource Mode/Updatable")]
        public static void SetResourceModeUpdatable()
        {
            SetResourceModeData(GameFramework.Resource.ResourceMode.Updatable);
        }
        
        [MenuItem("Game Framework/Resource Mode/Standalone")]
        public static void SetResourceModeStandalone()
        {
            SetResourceModeData(GameFramework.Resource.ResourceMode.Package);
        }

        private static void SetResourceModeData(GameFramework.Resource.ResourceMode targetMode)
        {
            // fix bug: Unity will not open any scene in batch mode.
            EditorSceneManager.OpenScene("Assets/Game Launcher.unity");
            Scene scene = EditorSceneManager.GetActiveScene();
            Debug.Assert(scene.name == "Game Launcher");
            GameObject[] rootGameObjects = scene.GetRootGameObjects();
            Debug.Assert(rootGameObjects.Length == 3);
            ResourceComponent[] rcs = GameObject.FindObjectsOfType<ResourceComponent>();
            Debug.Assert(rcs.Length == 1);
            
            ResourceComponent rc = rcs[0];
            rc.SetResourceModeEditor(targetMode);

            SerializedObject serializedObject = new UnityEditor.SerializedObject(rc);
            SerializedProperty rmp1 = serializedObject.FindProperty("m_ResourceMode");
            rmp1.enumValueIndex = (int) targetMode;
            
            // Fix bug: If only change enum from code could not set dirty success 
            SerializedProperty rmp2 = serializedObject.FindProperty("m_MinUnloadUnusedAssetsInterval");
            rmp2.floatValue = rmp2.floatValue + 1;
            EditorUtility.SetDirty(rc.gameObject);
            bool r = serializedObject.ApplyModifiedProperties();
            rmp2.floatValue = rmp2.floatValue - 1;
            r = serializedObject.ApplyModifiedProperties();

            // GameObject go = PrefabUtility.GetNearestPrefabInstanceRoot(rc.gameObject);
            // GameObject o = PrefabUtility.SaveAsPrefabAssetAndConnect(go, "Assets/GameFramework/GameFramework.prefab", InteractionMode.UserAction, out bool success);
            // PrefabUtility.ApplyPrefabInstance(go, InteractionMode.AutomatedAction);
            // // EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        }
    }
}