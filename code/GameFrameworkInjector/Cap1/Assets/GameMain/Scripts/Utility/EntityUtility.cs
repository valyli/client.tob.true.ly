//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.DataTable;
using UnityEngine.Assertions;

namespace StarForce
{
    /// <summary>
    /// AI 工具类。
    /// </summary>
    public static class EntityUtility
    {
        static EntityUtility()
        {
        }

        public static UnityGameFramework.Runtime.Entity GetCharacter()
        {
            UnityGameFramework.Runtime.Entity[] entities = GameEntry.Entity.GetEntities(
                AssetUtility.GetEntityAsset(
                    DataTableUtility.GetAssetNameOfEntity(Constant.EntityTypeId.Character)));
            Assert.IsTrue(entities.Length <= 1);
            if (entities.Length == 1)
            {
                return entities[0];
            }
            return null;
        }
    }
}
