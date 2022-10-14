//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
// using UnityStandardAssets.Cameras;

namespace StarForce
{
    public class OperateUIPortrait : UGuiForm
    {
        public void OnSettingButtonClick()
        {
            CanvasScaler canvasScaler = GetComponentInParent<CanvasScaler>();
            if (Screen.orientation == ScreenOrientation.LandscapeLeft || 
                Screen.orientation == ScreenOrientation.LandscapeRight)
            {
                float width = canvasScaler.referenceResolution.x;
                float height = canvasScaler.referenceResolution.y;
                canvasScaler.referenceResolution = new Vector2(height, width);
                Screen.orientation = ScreenOrientation.Portrait;
            }
            else if (Screen.orientation == ScreenOrientation.Portrait ||
                     Screen.orientation == ScreenOrientation.PortraitUpsideDown)
            {
                float width = canvasScaler.referenceResolution.x;
                float height = canvasScaler.referenceResolution.y;
                canvasScaler.referenceResolution = new Vector2(height, width);
                Screen.orientation = ScreenOrientation.LandscapeLeft;
            }
        }
    }
}
