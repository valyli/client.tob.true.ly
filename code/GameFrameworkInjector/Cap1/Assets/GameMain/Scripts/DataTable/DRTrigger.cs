//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：__DATA_TABLE_CREATE_TIME__
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Truely
{
    /// <summary>
    /// 触发器表。
    /// </summary>
    public class DRTrigger : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取触发器编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取场景编号。
        /// </summary>
        public int SceneId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取触发半径。
        /// </summary>
        public float Radius
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取触发类型。
        /// </summary>
        public string EffectorType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取被触发id。
        /// </summary>
        public int EffectedId
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            SceneId = int.Parse(columnStrings[index++]);
            Radius = float.Parse(columnStrings[index++]);
            EffectorType = columnStrings[index++];
            EffectedId = int.Parse(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    SceneId = binaryReader.Read7BitEncodedInt32();
                    Radius = binaryReader.ReadSingle();
                    EffectorType = binaryReader.ReadString();
                    EffectedId = binaryReader.Read7BitEncodedInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
