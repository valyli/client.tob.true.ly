//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
#if UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#endif
using UnityEngine.SceneManagement;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 默认资源辅助器。
    /// </summary>
    public class DefaultResourceHelper : ResourceHelperBase
    {
        /// <summary>
        /// 直接从指定文件路径加载数据流。
        /// </summary>
        /// <param name="fileUri">文件路径。</param>
        /// <param name="loadBytesCallbacks">加载数据流回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public override void LoadBytes(string fileUri, LoadBytesCallbacks loadBytesCallbacks, object userData)
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                LoadBytesFromIndexedDB(fileUri, loadBytesCallbacks, userData);
            }
            else
            {
                StartCoroutine(LoadBytesCo(fileUri, loadBytesCallbacks, userData));
            }
        }

        /// <summary>
        /// 卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public override void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
        {
#if UNITY_5_5_OR_NEWER
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(UnloadSceneCo(sceneAssetName, unloadSceneCallbacks, userData));
            }
            else
            {
                SceneManager.UnloadSceneAsync(SceneComponent.GetSceneName(sceneAssetName));
            }
#else
            if (SceneManager.UnloadScene(SceneComponent.GetSceneName(sceneAssetName)))
            {
                if (unloadSceneCallbacks.UnloadSceneSuccessCallback != null)
                {
                    unloadSceneCallbacks.UnloadSceneSuccessCallback(sceneAssetName, userData);
                }
            }
            else
            {
                if (unloadSceneCallbacks.UnloadSceneFailureCallback != null)
                {
                    unloadSceneCallbacks.UnloadSceneFailureCallback(sceneAssetName, userData);
                }
            }
#endif
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        /// <param name="objectToRelease">要释放的资源。</param>
        public override void Release(object objectToRelease)
        {
            AssetBundle assetBundle = objectToRelease as AssetBundle;
            if (assetBundle != null)
            {
                assetBundle.Unload(true);
                return;
            }

            /* Unity 当前 Resources.UnloadAsset 在 iOS 设备上会导致一些诡异问题，先不用这部分
            SceneAsset sceneAsset = objectToRelease as SceneAsset;
            if (sceneAsset != null)
            {
                return;
            }

            Object unityObject = objectToRelease as Object;
            if (unityObject == null)
            {
                Log.Warning("Asset is invalid.");
                return;
            }

            if (unityObject is GameObject || unityObject is MonoBehaviour)
            {
                // UnloadAsset may only be used on individual assets and can not be used on GameObject's / Components or AssetBundles.
                return;
            }

            Resources.UnloadAsset(unityObject);
            */
        }

        private void Start()
        {
        }

        private void LoadBytesFromIndexedDB(string fileUri, LoadBytesCallbacks loadBytesCallbacks, object userData)
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                fileUri = PathTranslator.GetIndexedDBPath(fileUri);
            }

            bool isError = false;
            byte[] bytes = null;
            string errorMessage = null;
            DateTime startTime = DateTime.UtcNow;

            try
            {
                Log.Debug("---> DefaultResourceHelper.LoadBytesFromIndexedDB {0}", fileUri);
                using (FileStream fileStream = new FileStream(fileUri, FileMode.Open, FileAccess.Read))
                {
                    bytes = new byte[fileStream.Length];
                    fileStream.Read(bytes, 0, (int) fileStream.Length);
                    Log.Debug("---> DefaultResourceHelper.LoadBytesFromIndexedDB {0} {1}", fileUri, bytes == null ? "null" : bytes.Length.ToString());
                }
            }
            catch (Exception e)
            {
                isError = true;
                errorMessage = e.Message;
            }
            
            if (!isError)
            {
                float elapseSeconds = (float)(DateTime.UtcNow - startTime).TotalSeconds;
                loadBytesCallbacks.LoadBytesSuccessCallback(fileUri, bytes, elapseSeconds, userData);
            }
            else if (loadBytesCallbacks.LoadBytesFailureCallback != null)
            {
                loadBytesCallbacks.LoadBytesFailureCallback(fileUri, errorMessage, userData);
            }
        }

        private IEnumerator LoadBytesCo(string fileUri, LoadBytesCallbacks loadBytesCallbacks, object userData)
        {
            bool isError = false;
            byte[] bytes = null;
            string errorMessage = null;
            DateTime startTime = DateTime.UtcNow;

#if UNITY_5_4_OR_NEWER
            Log.Debug("---> DefaultResourceHelper.LoadBytesCo {0}", fileUri);
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(fileUri);
#if UNITY_2017_2_OR_NEWER
            yield return unityWebRequest.SendWebRequest();
#else
            yield return unityWebRequest.Send();
#endif

#if UNITY_2020_2_OR_NEWER
            isError = unityWebRequest.result != UnityWebRequest.Result.Success;
#elif UNITY_2017_1_OR_NEWER
            isError = unityWebRequest.isNetworkError || unityWebRequest.isHttpError;
#else
            isError = unityWebRequest.isError;
#endif
            bytes = unityWebRequest.downloadHandler.data;
            errorMessage = isError ? unityWebRequest.error : null;
            unityWebRequest.Dispose();
#else
            WWW www = new WWW(fileUri);
            yield return www;

            isError = !string.IsNullOrEmpty(www.error);
            bytes = www.bytes;
            errorMessage = www.error;
            www.Dispose();
#endif

            if (!isError)
            {
                Log.Debug("---> DefaultResourceHelper.LoadBytesCo {0} success", fileUri);
                float elapseSeconds = (float)(DateTime.UtcNow - startTime).TotalSeconds;
                loadBytesCallbacks.LoadBytesSuccessCallback(fileUri, bytes, elapseSeconds, userData);
            }
            else if (loadBytesCallbacks.LoadBytesFailureCallback != null)
            {
                loadBytesCallbacks.LoadBytesFailureCallback(fileUri, errorMessage, userData);
            }
        }

#if UNITY_5_5_OR_NEWER
        private IEnumerator UnloadSceneCo(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(SceneComponent.GetSceneName(sceneAssetName));
            if (asyncOperation == null)
            {
                yield break;
            }

            yield return asyncOperation;

            if (asyncOperation.allowSceneActivation)
            {
                if (unloadSceneCallbacks.UnloadSceneSuccessCallback != null)
                {
                    unloadSceneCallbacks.UnloadSceneSuccessCallback(sceneAssetName, userData);
                }
            }
            else
            {
                if (unloadSceneCallbacks.UnloadSceneFailureCallback != null)
                {
                    unloadSceneCallbacks.UnloadSceneFailureCallback(sceneAssetName, userData);
                }
            }
        }
#endif
    }
}
