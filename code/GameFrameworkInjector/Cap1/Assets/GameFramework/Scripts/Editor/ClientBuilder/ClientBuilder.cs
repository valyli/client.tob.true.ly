using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameFramework.Resource;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    public class ClientBuilder : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        [MenuItem("Game Framework/Client Builder/Build Android")]
        public static void BuildAndroid()
        {
            EditorSceneManager.OpenScene("Assets/Game Launcher.unity");
            Scene firstScene = EditorSceneManager.GetActiveScene();
            Debug.Assert(firstScene.name == "Game Launcher");

            List<string> want_to_defines = new List<string>(){"ENABLE_LOG"};
            PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.Android, out string[] defines);
            List<string> target_defines = defines.ToList();
            foreach (string want_to_define in want_to_defines)
            {
                if (!defines.Contains(want_to_define))
                {
                    Debug.Log("Add Scripting Define Symbols: " + want_to_define);
                    target_defines.Add(want_to_define);
                }
            }
            PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.Android, target_defines.ToArray());
            
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.locationPathName = "../output/android.apk";
            buildPlayerOptions.target = BuildTarget.Android;
            buildPlayerOptions.options = BuildOptions.None;

            List<string> sceneNames = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                sceneNames.Add(scene.path);
            }

            buildPlayerOptions.scenes = sceneNames.ToArray();
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            }

            if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
            }
        }
    }
}