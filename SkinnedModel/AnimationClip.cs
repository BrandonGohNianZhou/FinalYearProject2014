//------------------------------------------//
// AnimationClip.cs							//
// アニメーションのデータを管理するクラス	//
// 制作日:2013/09/30						//
// 制作者:MicroSoftSample					//
//------------------------------------------//
#region File Description
//-----------------------------------------------------------------------------
// AnimationClip.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
#endregion

namespace SkinnedModel
{
	// アニメーションクラス
    public class AnimationClip
    {
        /// <summary>
        /// Constructs a new animation clip object.
        /// </summary>
		/// 

		// コンストラクタ
		// アニメーションの長さ、キーフレーム
        public AnimationClip(TimeSpan duration, List<Keyframe> keyframes)
        {
			// 各値を初期化
            Duration = duration;
            Keyframes = keyframes;
        }

		// プライベートコンストラクタ
        private AnimationClip()
        {
        }

		// アニメーションの長さを取得
        [ContentSerializer]
        public TimeSpan Duration { get; private set; }


		// アニメーションのキーフレームを取得
        [ContentSerializer]
        public List<Keyframe> Keyframes { get; private set; }
    }
}
