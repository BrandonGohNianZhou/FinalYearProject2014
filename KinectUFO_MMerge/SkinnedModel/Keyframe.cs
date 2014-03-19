//--------------------------------------------------//
// Keyframe.cs										//
// アニメーションのキーフレームを管理するクラス		//
// 制作日:2013/09/30								//
// 制作者:MicroSoftSample							//
//--------------------------------------------------//
#region File Description
//-----------------------------------------------------------------------------
// Keyframe.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
#endregion

namespace SkinnedModel
{
    // キーフレームクラス
    public class Keyframe
    {
        // コンストラクタ
        public Keyframe(int bone, TimeSpan time, Matrix transform)
        {
            Bone = bone;
            Time = time;
            Transform = transform;
        }

		// プライベートコンストラクタ
        private Keyframe()
        {
        }


        // ボーンを取得
        [ContentSerializer]
        public int Bone { get; private set; }


        // タイムスパンを取得
        [ContentSerializer]
        public TimeSpan Time { get; private set; }


        // トランスフォームを取得
        [ContentSerializer]
        public Matrix Transform { get; private set; }
    }
}
