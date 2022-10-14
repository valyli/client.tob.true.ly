//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
using GameFramework.Event;
using System.Collections.Generic;
using System.Linq;
using GameFramework.DataTable;
using UnityEngine;
using UnityGameFramework.Runtime;

using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Truely
{
    public class ProcedureMain : ProcedureBase
    {
        private const float GameOverDelayedSeconds = 2f;

        private readonly Dictionary<GameMode, GameBase> m_Games = new Dictionary<GameMode, GameBase>();
        private GameBase m_CurrentGame = null;
        private bool m_GotoMenu = false;
        private float m_GotoMenuDelaySeconds = 0f;
        private OperateUI m_OperateUI = null;
        private InputForm m_InputForm = null;
        // private OperateUIPortrait m_OperateUIPortrait = null;
        // private InputFormPortrait m_InputFormPortrait = null;
        private Character m_character = null;

        private Trigger[] m_Triggers;
        private Trigger m_LastVisitTrigger = null;
        private int m_TargetTransferPointId = 0;

        public int sceneId = 0;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public void GotoMenu()
        {
            m_GotoMenu = true;
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

            m_Games.Add(GameMode.Survival, new SurvivalGame());
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);

            m_Games.Clear();
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_GotoMenu = false;

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);

            GameMode gameMode = (GameMode)procedureOwner.GetData<VarByte>("GameMode").Value;
            m_CurrentGame = m_Games[gameMode];
            m_CurrentGame.Initialize();
            GameEntry.UI.OpenUIForm(UIFormId.InputForm);
            GameEntry.UI.OpenUIForm(UIFormId.OperateUI);
            // GameEntry.UI.OpenUIForm(UIFormId.InputFormPortrait);
            // GameEntry.UI.OpenUIForm(UIFormId.OperateUIPortrait);
            InitTriggers();
            Vector3 entryPosition = new Vector3(10, 10, 10);
            sceneId = procedureOwner.GetData<VarInt32>("NextSceneId");
            if (procedureOwner.HasData("NextTargetTransferPointId"))
            {
                m_TargetTransferPointId = procedureOwner.GetData<VarInt32>("NextTargetTransferPointId");
                DRTransferPoint drTransferPoint = DataTableUtility.GetDataRow<DRTransferPoint>(m_TargetTransferPointId);
            }
            else
            {
                m_TargetTransferPointId = 0;
                IDataTable<DRTransferPoint> allRows = GameEntry.DataTable.GetDataTable<DRTransferPoint>();
                DRTransferPoint drTransferPoint = allRows.First(p => p.SceneId == sceneId);
                m_TargetTransferPointId = drTransferPoint.Id;
            }
            TransferPoint[] transferPoints = GameObject.FindObjectsOfType<TransferPoint>();
            TransferPoint targetTransferPoint = transferPoints.First(p => p.TransferPointId == m_TargetTransferPointId);
            entryPosition = targetTransferPoint.gameObject.transform.position;
            GameEntry.Entity.ShowCharacter(new CharacterData(GameEntry.Entity.GenerateSerialId(), Constant.EntityTypeId.Character)
            {
                Position = entryPosition
            });
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (m_CurrentGame != null)
            {
                m_CurrentGame.Shutdown();
                m_CurrentGame = null;
            }

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);

            if (m_OperateUI != null)
            {
            
                m_OperateUI.Close(true);
                m_OperateUI = null;
            }
            
            if (m_InputForm != null)
            {
            
                m_InputForm.Close(true);
                m_InputForm = null;
            }

            // if (m_OperateUIPortrait != null)
            // {
            //
            //     m_OperateUIPortrait.Close(true);
            //     m_OperateUIPortrait = null;
            // }

            // if (m_InputFormPortrait != null)
            // {
            //
            //     m_InputFormPortrait.Close(true);
            //     m_InputFormPortrait = null;
            // }

            if (m_character != null)
            {

                GameEntry.Entity.HideEntity(m_character);
                m_character = null;
            }
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_CurrentGame != null && !m_CurrentGame.GameOver)
            {
                m_CurrentGame.Update(elapseSeconds, realElapseSeconds);
                
                DRTrigger[] drTriggers = DataTableUtility.GetAllTriggersInScene(sceneId);

                if (m_character != null)
                {
                    foreach (Trigger trigger in m_Triggers)
                    {
                        float distance = (trigger.gameObject.transform.position - m_character.gameObject.transform.position).magnitude;
                        if (distance <= trigger.Radius)
                        {
                            if (m_LastVisitTrigger == null || m_LastVisitTrigger != trigger)
                            {
                                m_LastVisitTrigger = trigger;

                                DRTrigger drTrigger = DataTableUtility.GetDataRow<DRTrigger>(trigger.TriggerId);
                                if (drTrigger.EffectorType == EffectorType.Transfer.ToString())
                                {
                                    // int transferId = drTrigger.EffectedId;
                                    // DRTransferPath drTransferPath = DataTableUtility.GetDataRow<DRTransferPath>(transferId);
                                    // int targetTransferPointId = drTransferPath.TargetTransferPointId;
                                    // DRTransferPoint drTransferPoint = DataTableUtility.GetDataRow<DRTransferPoint>(targetTransferPointId);
                                    //
                                    // procedureOwner.SetData<VarInt32>("NextSceneId", drTransferPoint.SceneId);
                                    // procedureOwner.SetData<VarInt32>("NextTargetTransferPointId", drTransferPoint.Id);
                                    // ChangeState<ProcedureChangeScene>(procedureOwner);
                                    m_character.ChangeControllerType();
                                }
                            }
                        }
                        else
                        {
                            m_LastVisitTrigger = null;
                        }
                    }
                }

                return;
            }

            if (!m_GotoMenu)
            {
                m_GotoMenu = true;
                m_GotoMenuDelaySeconds = 0;
            }

            m_GotoMenuDelaySeconds += elapseSeconds;
            if (m_GotoMenuDelaySeconds >= GameOverDelayedSeconds)
            {
                procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            string name = ne.UIForm.Logic.Name;
            if (name == "InputForm(Clone)")
            {
                m_InputForm = (InputForm)ne.UIForm.Logic;
            }
            else if (name == "OperateUI(Clone)")
            {
                m_OperateUI = (OperateUI)ne.UIForm.Logic;
            }
            // if (name == "InputFormPortrait(Clone)")
            // {
            //     m_InputFormPortrait = (InputFormPortrait)ne.UIForm.Logic;
            // }
            // else if (name == "OperateUIPortrait(Clone)")
            // {
            //     m_OperateUIPortrait = (OperateUIPortrait)ne.UIForm.Logic;
            // }
        }

        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne.EntityLogicType == typeof(Character))
            {
                m_character = (Character)ne.Entity.Logic;
            }
        }

        private void InitTriggers()
        {
            // Scene scene = GameEntry.Scene.LastActiveScene;
            m_Triggers = GameObject.FindObjectsOfType<Trigger>();
        }
    }
}
