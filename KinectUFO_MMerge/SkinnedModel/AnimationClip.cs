//------------------------------------------//
// AnimationClip.cs							//
// �A�j���[�V�����̃f�[�^���Ǘ�����N���X	//
// �����:2013/09/30						//
// �����:MicroSoftSample					//
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
	// �A�j���[�V�����N���X
    public class AnimationClip
    {
        /// <summary>
        /// Constructs a new animation clip object.
        /// </summary>
		/// 

		// �R���X�g���N�^
		// �A�j���[�V�����̒����A�L�[�t���[��
        public AnimationClip(TimeSpan duration, List<Keyframe> keyframes)
        {
			// �e�l��������
            Duration = duration;
            Keyframes = keyframes;
        }

		// �v���C�x�[�g�R���X�g���N�^
        private AnimationClip()
        {
        }

		// �A�j���[�V�����̒������擾
        [ContentSerializer]
        public TimeSpan Duration { get; private set; }


		// �A�j���[�V�����̃L�[�t���[�����擾
        [ContentSerializer]
        public List<Keyframe> Keyframes { get; private set; }
    }
}
