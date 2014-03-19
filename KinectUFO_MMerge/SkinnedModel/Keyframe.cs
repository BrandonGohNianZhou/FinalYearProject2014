//--------------------------------------------------//
// Keyframe.cs										//
// �A�j���[�V�����̃L�[�t���[�����Ǘ�����N���X		//
// �����:2013/09/30								//
// �����:MicroSoftSample							//
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
    // �L�[�t���[���N���X
    public class Keyframe
    {
        // �R���X�g���N�^
        public Keyframe(int bone, TimeSpan time, Matrix transform)
        {
            Bone = bone;
            Time = time;
            Transform = transform;
        }

		// �v���C�x�[�g�R���X�g���N�^
        private Keyframe()
        {
        }


        // �{�[�����擾
        [ContentSerializer]
        public int Bone { get; private set; }


        // �^�C���X�p�����擾
        [ContentSerializer]
        public TimeSpan Time { get; private set; }


        // �g�����X�t�H�[�����擾
        [ContentSerializer]
        public Matrix Transform { get; private set; }
    }
}
