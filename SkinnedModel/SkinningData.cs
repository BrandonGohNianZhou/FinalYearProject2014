//------------------------------------------//
// SkinningData.cs							//
// �X�L�����b�V���̃f�[�^���i�[����N���X	//
// �����:2013/09/30						//
// �����:MicrosoftSample					//
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
    // �X�L�j���O�f�[�^
    public class SkinningData
    {
        // �R���X�g���N�^
        public SkinningData(Dictionary<string, AnimationClip> animationClips,
                            List<Matrix> bindPose, List<Matrix> inverseBindPose,
                            List<int> skeletonHierarchy)
        {
            AnimationClips = animationClips;
            BindPose = bindPose;
            InverseBindPose = inverseBindPose;
            SkeletonHierarchy = skeletonHierarchy;
        }


        // �v���C�x�[�g�R���X�g���N�^
        private SkinningData()
        {
        }


        // �f�B���N�g���[���擾
        [ContentSerializer]
        public Dictionary<string, AnimationClip> AnimationClips { get; private set; }


		// BindPose���擾
        [ContentSerializer]
        public List<Matrix> BindPose { get; private set; }


		 // InverseBindPose���擾
        [ContentSerializer]
        public List<Matrix> InverseBindPose { get; private set; }


        // �X�P���g���̊K�w�\�����擾
        [ContentSerializer]
        public List<int> SkeletonHierarchy { get; private set; }
    }
}
