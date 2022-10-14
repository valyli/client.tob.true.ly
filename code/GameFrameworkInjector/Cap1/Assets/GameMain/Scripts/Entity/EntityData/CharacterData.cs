//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using UnityEngine;

namespace StarForce
{
    [Serializable]
    public class CharacterData : EntityData
    {
        private int m_UserId;

        public int UserId
        {
            get => m_UserId;
            set => m_UserId = value;
        }

        public CharacterData(int entityId, int typeId)
            : base(entityId, typeId)
        {
        }
    }
}
