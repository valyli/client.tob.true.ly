//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.DataTable;
using UnityEngine.Assertions;
using UnityGameFramework.Runtime;

namespace Truely
{
    /// <summary>
    /// AI 工具类。
    /// </summary>
    public static class DataTableUtility
    {
        static DataTableUtility()
        {
        }

        public static T GetDataRow<T>(int tableId) where T: DataRowBase
        {
            IDataTable<T> allRows = GameEntry.DataTable.GetDataTable<T>();
            T[] selectedRows = allRows.GetDataRows(p=> p.Id == tableId);
            Assert.AreEqual(selectedRows.Length, 1);
            return selectedRows[0];
        }

        public static DRTrigger[] GetAllTriggersInScene(int sceneId)
        {
            IDataTable<DRTrigger> allRows = GameEntry.DataTable.GetDataTable<DRTrigger>();
            DRTrigger[] selectedRows = allRows.GetDataRows(p=> p.SceneId == sceneId);
            return selectedRows;
        }

        public static string GetAssetNameOfEntity(int entityTypeId)
        {
            IDataTable<DREntity> allRows = GameEntry.DataTable.GetDataTable<DREntity>();
            DREntity[] selectedRows = allRows.GetDataRows(p=> p.Id == entityTypeId);
            Assert.IsTrue(selectedRows.Length == 1);
            return selectedRows[0].AssetName;
        }
    }
}
