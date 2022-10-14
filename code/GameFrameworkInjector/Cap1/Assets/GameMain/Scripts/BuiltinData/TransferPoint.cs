using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Truely
{
    [AddComponentMenu("GameMain/TransferPoint")]
    public class TransferPoint : MonoBehaviour
    {
        [SerializeField]
        private int m_TransferPointId = 0;

        public int TransferPointId
        {
            get => m_TransferPointId;
            set => m_TransferPointId = value;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(3f, 3f, 3f));
        }
    }
}