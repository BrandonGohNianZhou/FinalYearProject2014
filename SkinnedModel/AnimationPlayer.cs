//--------------------------------------//
// AnimationPlayer.cs					//
// アニメーションの管理を行うクラス		//
// 作成日:2013/09/30					//
// 作成者:MicroSoftSample				//
// 改編者:Kouno Shin					//
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
	// アニメーションプレイヤー
    public class AnimationPlayer
    {

		// アニメーションクリップ
        public AnimationClip currentClipValue;
		// タイムスパン
        public TimeSpan currentTimeValue;
		// キーフレーム
        public int currentKeyframe;


		// ボーン行列
        Matrix[] boneTransforms;
		// ワールド行列
        Matrix[] worldTransforms;
		// スキン行列
        Matrix[] skinTransforms;


		// モデルの階層データ
        SkinningData skinningDataValue;

		// コンストラクタ
        public AnimationPlayer(SkinningData skinningData)
        {
			// スキニングデータが空なら
			if (skinningData == null)
			{
				throw new ArgumentNullException("skinningData");
			}

			// スキニングデータを格納
            skinningDataValue = skinningData;

			// 各行列をもらってくる
            boneTransforms = new Matrix[skinningData.BindPose.Count];
            worldTransforms = new Matrix[skinningData.BindPose.Count];
            skinTransforms = new Matrix[skinningData.BindPose.Count];
        }


		// アニメーションのスタート
        public void StartClip(AnimationClip clip)
        {
			// アニメーションクリップが空なら
			if (clip == null)
			{
				throw new ArgumentNullException("clip");
			}

			// アニメーションクリップを代入
            currentClipValue = clip;
			// ゼロからスタート
            currentTimeValue = TimeSpan.Zero;
			// キーフレームは0から
            currentKeyframe = 0;

			// ボーンの初期化
            skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
        }


        // アニメーションの更新
        public void Update(TimeSpan time, bool relativeToCurrentTime,
                           Matrix rootTransform)
        {
			// ボーンの更新
            UpdateBoneTransforms(time, relativeToCurrentTime);
			// ワールドの更新
            UpdateWorldTransforms(rootTransform);
			// スキントランスフォームの更新
            UpdateSkinTransforms();
        }


		// ボーンの更新処理
        public void UpdateBoneTransforms(TimeSpan time, bool relativeToCurrentTime)
        {
			// アニメーションクリップが入っていなければ
			if (currentClipValue == null)
			{
				throw new InvalidOperationException(
							"AnimationPlayer.Update was called before StartClip");
			}

			// アニメーションのポジションの更新
            if (relativeToCurrentTime)
            {
                time += currentTimeValue;

				// もしアニメーションが終わったらループに入る
				while (time >= currentClipValue.Duration)
				{
					time -= currentClipValue.Duration;
				}

            }

			// もし0以下のキーフレームに入ったり
			// キーフレームを超える場所に入ろうとしたら
			if ((time < TimeSpan.Zero) || (time >= currentClipValue.Duration))
			{
				throw new ArgumentOutOfRangeException("time");
			}

            // もしアニメーションが終わったら
            if (time < currentTimeValue)
            {
				/*
				// キーフレームを最初へ
                currentKeyframe = 0;
				// 初期のバインドポーズをさせる
                skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
				 * */
            }

			// アニメーションタイムを初期化
            currentTimeValue = time;

            // キーフレームの読み込み
            IList<Keyframe> keyframes = currentClipValue.Keyframes;

			// 現在のキーフレームがキーフレームの最大数より小さければ
            while (currentKeyframe < keyframes.Count)
            {
				// キーフレームを最大数で作成
                Keyframe keyframe = keyframes[currentKeyframe];

                // Stop when we've read up to the current time position.
				// 今の位置よりキーフレームが大きくなってしまったら
				if (keyframe.Time > currentTimeValue)
				{
					// ループを抜ける
					break;
				}

                // ボーントランスフォームにキーフレーム行列を使う
                boneTransforms[keyframe.Bone] = keyframe.Transform;

				// 現在のキーを更新
                currentKeyframe++;
            }
        }


        // ワールドの更新
        public void UpdateWorldTransforms(Matrix rootTransform)
        {
            // 親ボーン
            worldTransforms[0] = boneTransforms[0] * rootTransform;

            // 子ボーン
            for (int bone = 1; bone < worldTransforms.Length; bone++)
            {
				// 親ボーンを取得
                int parentBone = skinningDataValue.SkeletonHierarchy[bone];

				// ワールド行列に代入
                worldTransforms[bone] = boneTransforms[bone] *
                                             worldTransforms[parentBone];
            }
        }


        // スキントランスフォームの更新
        public void UpdateSkinTransforms()
        {
			// ボーンの数だけ
            for (int bone = 0; bone < skinTransforms.Length; bone++)
            {
				// 行列の更新
                skinTransforms[bone] = skinningDataValue.InverseBindPose[bone] *
                                            worldTransforms[bone];
            }
        }

        // ボーンのトランスフォームを取得
        public Matrix[] GetBoneTransforms()
        {
            return boneTransforms;
        }

		// ワールドトランスフォームを取得
        public Matrix[] GetWorldTransforms()
        {
            return worldTransforms;
        }


        // スキントランスフォームを取得
        public Matrix[] GetSkinTransforms()
        {
            return skinTransforms;
        }


		// 現在のアニメーションクリップを取得
        public AnimationClip CurrentClip
        {
            get { return currentClipValue; }
        }


        // 現在のタイムスパンを取得
        public TimeSpan CurrentTime
        {
            get { return currentTimeValue; }
        }
    }
}
