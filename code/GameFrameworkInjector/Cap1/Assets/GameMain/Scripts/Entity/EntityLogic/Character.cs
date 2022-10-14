//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEngine;
using UnityGameFramework.Runtime;
// using UnityStandardAssets.Cameras;

namespace StarForce
{
    /// <summary>
    /// 特效类。
    /// </summary>
    public class Character : Entity
    {
        [SerializeField]
        private CharacterData m_CharacterData = null;
        private CharacterController m_Controller;
        private GameObject m_CameraObject;
        private enum ControllerType : byte
        {
            Unknown = 0,
            Third,
            First,
        }
        private ControllerType m_ControllerType = ControllerType.Third;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_CharacterData = userData as CharacterData;
            if (m_CharacterData == null)
            {
                Log.Error("Effect data is invalid.");
                return;
            }
            m_Controller = GetComponent<CharacterController>();
            m_Controller.enableOverlapRecovery = true;
            m_CameraObject = GameObject.Find("Camera");
            InitType();
        }
        
        private void InitType()
        {
            if (m_ControllerType == ControllerType.Third)
            {
                m_CameraObject.SetActive(true);
                transform.Find("ThirdPersoController").gameObject.SetActive(true);
                transform.Find("FirstPersonController").gameObject.SetActive(false);
                // m_CameraObject.GetComponent<FreeLookCam>().SetTarget(transform.Find("ThirdPersoController"));
            }
            else if (m_ControllerType == ControllerType.First)
            {
                m_CameraObject.SetActive(false);
                transform.Find("FirstPersonController").gameObject.SetActive(true);
                transform.Find("ThirdPersoController").gameObject.SetActive(false);
            }
        }

        public void ChangeControllerType()
        {
            if (m_ControllerType == ControllerType.First)
            {
                m_ControllerType = ControllerType.Third;
            } 
            else if (m_ControllerType == ControllerType.Third)
            {
                m_ControllerType = ControllerType.First;
            }
            InitType();
        }
        
    }
}
