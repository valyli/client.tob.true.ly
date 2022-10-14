//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace StarForce
{
    /// <summary>
    /// 界面编号。
    /// </summary>
    public enum UIFormId : byte
    {
        Undefined = 0,

        /// <summary>
        /// 弹出框。
        /// </summary>
        DialogForm = 1,

        /// <summary>
        /// 主菜单。
        /// </summary>
        MenuForm = 100,

        /// <summary>
        /// 设置。
        /// </summary>
        SettingForm = 101,

        /// <summary>
        /// 关于。
        /// </summary>
        AboutForm = 102,

        /// <summary>
        /// 输入。
        /// </summary>
        InputForm = 103,
        
        /// <summary>
        /// 输入。
        /// </summary>
        InputFormPortrait = 104,
        
        /// <summary>
        /// 操作。
        /// </summary>
        OperateUI = 105,
        
        /// <summary>
        /// 操作（竖屏)。
        /// </summary>
        OperateUIPortrait = 106,
        
    }
}
