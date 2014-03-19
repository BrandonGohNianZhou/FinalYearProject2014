//------------------------------------------//
// SkinningData.cs							//
// スキンメッシュのデータを格納するクラス	//
// 制作日:2013/09/30						//
// 制作者:MicrosoftSample					//
//------------------------------------------//
#region File Description
//-----------------------------------------------------------------------------
// SkinningData.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
#endregion

namespace SkinnedModel
{
    // スキニングデータ
    public class SkinningData
    {
        // コンストラクタ
        public SkinningData(Dictionary<string, AnimationClip> animationClips,
                            List<Matrix> bindPose, List<Matrix> inverseBindPose,
                            List<int> skeletonHierarchy)
        {
            AnimationClips = animationClips;
            BindPose = bindPose;
            InverseBindPose = inverseBindPose;
            SkeletonHierarchy = skeletonHierarchy;
        }


        // プライベートコンストラクタ
        private SkinningData()
        {
        }


        // ディレクトリーを取得
        [ContentSerializer]
        public Dictionary<string, AnimationClip> AnimationClips { get; private set; }


		// BindPoseを取得
        [ContentSerializer]
        public List<Matrix> BindPose { get; private set; }


		 // InverseBindPoseを取得
        [ContentSerializer]
        public List<Matrix> InverseBindPose { get; private set; }


        // スケルトンの階層構造を取得
        [ContentSerializer]
        public List<int> SkeletonHierarchy { get; private set; }
    }
}
