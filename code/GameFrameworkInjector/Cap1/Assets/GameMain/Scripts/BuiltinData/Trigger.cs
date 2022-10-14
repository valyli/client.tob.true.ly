using System;
using UnityEngine;

namespace Truely
{
    [AddComponentMenu("GameMain/Trigger")]
    public class Trigger : MonoBehaviour
    {
        [SerializeField]
        private int m_TriggerId = 0;

        [SerializeField]
        private float m_Radius = 1.0f;

        [SerializeField]
        private EffectorType m_EffectorType = default;

        public int TriggerId
        {
            get => m_TriggerId;
            set => m_TriggerId = value;
        }

        public float Radius
        {
            get => m_Radius;
            set => m_Radius = value;
        }

        public EffectorType EffectorType
        {
            get => m_EffectorType;
            set => m_EffectorType = value;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
    }
}