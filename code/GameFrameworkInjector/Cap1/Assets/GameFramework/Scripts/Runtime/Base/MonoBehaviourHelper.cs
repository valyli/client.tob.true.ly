using UnityEngine;
using System.Collections;
/// <summary>
/// Mono behaviour helper. Starts the coroutine in not MonoBehaviour class.
/// </summary>

namespace UnityGameFramework.Runtime
{
    public class MonoBehaviourHelper
    {
        /// <returns>The coroutine.</returns>
        /// <param name="routine">Routine.</param>
        public static Coroutine StartCoroutine(IEnumerator routine, bool persistent=false)
        {
            MonoBehaviourInstance MonoHelper = new GameObject ("Coroutine").AddComponent<MonoBehaviourInstance>();
 
            return MonoHelper.DestroyWhenComplete(routine, persistent);
        }
 
        public class MonoBehaviourInstance : MonoBehaviour
        {
            public Coroutine DestroyWhenComplete(IEnumerator routine, bool persistent)
            {
                if(persistent)
                    DontDestroyOnLoad(this.gameObject);
                return StartCoroutine(DestroyObjHandler(routine));
            }
            IEnumerator DestroyObjHandler(IEnumerator routine)
            {
                yield return StartCoroutine(routine);
                Log.Info("destroy coroutine");
                Destroy(this.gameObject);
            }
        }
    }
}

