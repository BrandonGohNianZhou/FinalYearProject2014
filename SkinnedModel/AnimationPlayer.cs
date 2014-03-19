//--------------------------------------//
// AnimationPlayer.cs					//
// �A�j���[�V�����̊Ǘ����s���N���X		//
// �쐬��:2013/09/30					//
// �쐬��:MicroSoftSample				//
// ���Ҏ�:Kouno Shin					//
//--------------------------------------//
#region File Description
//-----------------------------------------------------------------------------
// AnimationPlayer.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace SkinnedModel
{
	// �A�j���[�V�����v���C���[
    public class AnimationPlayer
    {

		// �A�j���[�V�����N���b�v
        public AnimationClip currentClipValue;
		// �^�C���X�p��
        public TimeSpan currentTimeValue;
		// �L�[�t���[��
        public int currentKeyframe;


		// �{�[���s��
        Matrix[] boneTransforms;
		// ���[���h�s��
        Matrix[] worldTransforms;
		// �X�L���s��
        Matrix[] skinTransforms;


		// ���f���̊K�w�f�[�^
        SkinningData skinningDataValue;

		// �R���X�g���N�^
        public AnimationPlayer(SkinningData skinningData)
        {
			// �X�L�j���O�f�[�^����Ȃ�
			if (skinningData == null)
			{
				throw new ArgumentNullException("skinningData");
			}

			// �X�L�j���O�f�[�^���i�[
            skinningDataValue = skinningData;

			// �e�s���������Ă���
            boneTransforms = new Matrix[skinningData.BindPose.Count];
            worldTransforms = new Matrix[skinningData.BindPose.Count];
            skinTransforms = new Matrix[skinningData.BindPose.Count];
        }


		// �A�j���[�V�����̃X�^�[�g
        public void StartClip(AnimationClip clip)
        {
			// �A�j���[�V�����N���b�v����Ȃ�
			if (clip == null)
			{
				throw new ArgumentNullException("clip");
			}

			// �A�j���[�V�����N���b�v����
            currentClipValue = clip;
			// �[������X�^�[�g
            currentTimeValue = TimeSpan.Zero;
			// �L�[�t���[����0����
            currentKeyframe = 0;

			// �{�[���̏�����
            skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
        }


        // �A�j���[�V�����̍X�V
        public void Update(TimeSpan time, bool relativeToCurrentTime,
                           Matrix rootTransform)
        {
			// �{�[���̍X�V
            UpdateBoneTransforms(time, relativeToCurrentTime);
			// ���[���h�̍X�V
            UpdateWorldTransforms(rootTransform);
			// �X�L���g�����X�t�H�[���̍X�V
            UpdateSkinTransforms();
        }


		// �{�[���̍X�V����
        public void UpdateBoneTransforms(TimeSpan time, bool relativeToCurrentTime)
        {
			// �A�j���[�V�����N���b�v�������Ă��Ȃ����
			if (currentClipValue == null)
			{
				throw new InvalidOperationException(
							"AnimationPlayer.Update was called before StartClip");
			}

			// �A�j���[�V�����̃|�W�V�����̍X�V
            if (relativeToCurrentTime)
            {
                time += currentTimeValue;

				// �����A�j���[�V�������I������烋�[�v�ɓ���
				while (time >= currentClipValue.Duration)
				{
					time -= currentClipValue.Duration;
				}

            }

			// ����0�ȉ��̃L�[�t���[���ɓ�������
			// �L�[�t���[���𒴂���ꏊ�ɓ��낤�Ƃ�����
			if ((time < TimeSpan.Zero) || (time >= currentClipValue.Duration))
			{
				throw new ArgumentOutOfRangeException("time");
			}

            // �����A�j���[�V�������I�������
            if (time < currentTimeValue)
            {
				/*
				// �L�[�t���[�����ŏ���
                currentKeyframe = 0;
				// �����̃o�C���h�|�[�Y��������
                skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
				 * */
            }

			// �A�j���[�V�����^�C����������
            currentTimeValue = time;

            // �L�[�t���[���̓ǂݍ���
            IList<Keyframe> keyframes = currentClipValue.Keyframes;

			// ���݂̃L�[�t���[�����L�[�t���[���̍ő吔��菬�������
            while (currentKeyframe < keyframes.Count)
            {
				// �L�[�t���[�����ő吔�ō쐬
                Keyframe keyframe = keyframes[currentKeyframe];

                // Stop when we've read up to the current time position.
				// ���̈ʒu���L�[�t���[�����傫���Ȃ��Ă��܂�����
				if (keyframe.Time > currentTimeValue)
				{
					// ���[�v�𔲂���
					break;
				}

                // �{�[���g�����X�t�H�[���ɃL�[�t���[���s����g��
                boneTransforms[keyframe.Bone] = keyframe.Transform;

				// ���݂̃L�[���X�V
                currentKeyframe++;
            }
        }


        // ���[���h�̍X�V
        public void UpdateWorldTransforms(Matrix rootTransform)
        {
            // �e�{�[��
            worldTransforms[0] = boneTransforms[0] * rootTransform;

            // �q�{�[��
            for (int bone = 1; bone < worldTransforms.Length; bone++)
            {
				// �e�{�[�����擾
                int parentBone = skinningDataValue.SkeletonHierarchy[bone];

				// ���[���h�s��ɑ��
                worldTransforms[bone] = boneTransforms[bone] *
                                             worldTransforms[parentBone];
            }
        }


        // �X�L���g�����X�t�H�[���̍X�V
        public void UpdateSkinTransforms()
        {
			// �{�[���̐�����
            for (int bone = 0; bone < skinTransforms.Length; bone++)
            {
				// �s��̍X�V
                skinTransforms[bone] = skinningDataValue.InverseBindPose[bone] *
                                            worldTransforms[bone];
            }
        }

        // �{�[���̃g�����X�t�H�[�����擾
        public Matrix[] GetBoneTransforms()
        {
            return boneTransforms;
        }

		// ���[���h�g�����X�t�H�[�����擾
        public Matrix[] GetWorldTransforms()
        {
            return worldTransforms;
        }


        // �X�L���g�����X�t�H�[�����擾
        public Matrix[] GetSkinTransforms()
        {
            return skinTransforms;
        }


		// ���݂̃A�j���[�V�����N���b�v���擾
        public AnimationClip CurrentClip
        {
            get { return currentClipValue; }
        }


        // ���݂̃^�C���X�p�����擾
        public TimeSpan CurrentTime
        {
            get { return currentTimeValue; }
        }
    }
}
