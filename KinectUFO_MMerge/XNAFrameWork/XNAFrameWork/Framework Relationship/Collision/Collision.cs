//----------------------------------//
// Collision.cs						//
// あたり判定を管理するクラス			//
//	Class that manages per decision
// 制作日:2013/10/17					//
// 制作者:Kouno Shin					//
//----------------------------------//

//----------------------//
//----名前空間の省略-----//
//	Abbreviation of the name space
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAFrameWork
{
	class Collision
	{
		//--------------------------------------------------------------//
		// 関数名	BoundingBox											//
		//	Function name BoundingBox
		// 機能		二つの物体が当たっているかどうかAABBを用いて判定		//
		//	Determined using AABB object function of two whether the hit
		// 引数		二つの物体のvector2座標								//
		//	Vector2 coordinates of the object argument of two
		// 戻り値	当たっている	true									//
		//	True, which is hitting the return value
		//			当たっていない	false								//
		//	False you do not hit
		//--------------------------------------------------------------//
		static public bool BoundingBox(Vector3 object1, Vector3 offset1,Vector3 object2,Vector3 offset2)
		{
            // bang into object bounding box
			if (object1.X + offset1.X >= object2.X - offset2.X
			&& object1.X - offset1.X <= object2.X + offset2.X
			&& object1.Y + offset1.Y >= object2.Y - offset2.Y
			&& object1.Y - offset1.Y <= object2.Y + offset2.Y
			&& object1.Z + offset1.Z >= object2.Z - offset2.Z
			&& object1.Z - offset1.Z <= object2.Z + offset2.Z)
            {
                Score.acc_level = 1;
				// Is hitting
				return true;
			}
			else
            {
				// Not hit
				return false;
			}
		}

        static public bool BoundingBox(Vector3 object1, Vector3 offset1, Vector3 object2, Vector3 offset2, bool playerDuck)
        {
            if (object1.Z + offset1.Z >= object2.Z - offset2.Z
            && object1.Z - offset1.Z <= object2.Z + offset2.Z)
            {
                // bang into object bounding box
                if (object1.X + offset1.X >= object2.X - offset2.X
                && object1.X - offset1.X <= object2.X + offset2.X
                && object1.Y + offset1.Y >= object2.Y - offset2.Y
                && object1.Y - offset1.Y <= object2.Y + offset2.Y)
                {
                    if (!playerDuck)
                    {
                        Score.acc_level = 1;
						// Is hitting
                        return true;
                    }
                    return false;
                }
                else
                {
					// Not hit
                    return false;
                }
            }
            return false;
        }

		//------------------------------------------------------------------//
		// 関数名	BoundingBox2D											//
		//	Function name BoundingBox2D
		// 機能		二つのオブジェクトが当たっているかAABBを用いて判定			//
		//	Determined using AABB object of two or function is hitting
		//			(2Dver.)												//
		// 引数		二つの物体の座標とオフセット								//
		//	Offset the corrdinates of the object argument of two
		// 戻り値	当たっている	true										//
		//	True, which is hitting the return value
		//			当たっていない	false									//
		//	False you do not hit
		//------------------------------------------------------------------//
		static public bool BoundingBox2D(Vector2 pos1,Vector2 offset1,Vector2 pos2,Vector2 offset2)
		{
            // within the gap
			if (pos1.X + offset1.X >= pos2.X - offset2.X
			&& pos1.X - offset1.X <= pos2.X + offset2.X
			&& pos1.Y + offset1.Y >= pos2.Y - offset2.Y
			&& pos1.Y - offset1.Y <= pos2.Y + offset2.Y)
			{
                Accuracy(pos1, pos2, offset2);
				// Is hitting
				return true;
			}
			else
            {
				// Not hit
				return false;
			}
		}

		//--------------------------------------------------//
		// 関数名	PlayerAndTextureCollision				//
		//	Function name PlayerAndTextureCollision
		// 機能		プレイヤーと板ポリのあたり判定を行う		//
		//	Do per decision of the plate and poly function Player
		// 引数		プレイヤーのポジション					//
		//	Posotion of argument Player
		//			板ポリクラス								//
		//	Plate Porikurasu
		// 戻り値	当たっている	: true						//
		//	Is hitting return value: true
		//			当たっていない	: false					//
		//	You do not hit: false
		//--------------------------------------------------//
		static public bool PlayerAndTextureCollision(Vector3 playerPos,BoardTexture tex)
		{
			// If we had been in the determination per plate if
			if (playerPos.Z <= tex.PosZ + tex.scaleZ
				&& playerPos.Z >= tex.PosZ - tex.scaleZ)
			{
				// Check whether the hit in the Box
				if (Collision.BoundingBox2D(new Vector2(playerPos.X, playerPos.Y), Vector2.One * 0.1f,
										new Vector2(tex.Pos.X, tex.Pos.Y),
										new Vector2(tex.Scale.X, tex.Scale.Y)))
				{
					// (Not against the wall), which is hitting the texture
					return false;
				}
				// (And against the walls) that do not hit the texture
				return true;
			}
			// Is not in contact with the wall
			return false;
		}

        static public bool PlayerAndTextureCollision(Vector3 playerPos, BoardTexture tex, bool playerDuck)
        {
			// If we had been in the determination per plate if
            if (playerPos.Z <= tex.PosZ + tex.scaleZ
                && playerPos.Z >= tex.PosZ - tex.scaleZ)
            {
                if (playerDuck)
                {
					// Check whether the hit in the Box
                    if (Collision.BoundingBox2D(new Vector2(playerPos.X, playerPos.Y), Vector2.One * 0.1f,
                                            new Vector2(tex.Pos.X, tex.Pos.Y),
                                            new Vector2(tex.Scale.X, tex.Scale.Y)))
                    {
						// (Not against the wall), which is hitting the texture
                        return false;
                    }
                }
				// (And against the walls) that do not hit the texture
                return true;
            }
			// Is not in contact with the wall
            return false;
        }

        // accuracy of evasion
        static public void Accuracy(Vector3 pPos, Vector3 oPos, Vector3 oSize)
        {

        }

        // accuracy of evasion
        static public void Accuracy(Vector2 pPos, Vector2 oPos, Vector2 oSize)
        {
            // within the gap
            if (pPos.X >= oPos.X - oSize.X * 0.25
             && pPos.X <= oPos.X + oSize.X * 0.25)
             //&& pPos.Y >= oPos.Y - oSize.Y * 0.2
             //&& pPos.Y <= oPos.Y + oSize.Y * 0.2)
            {
                // accurate
                Score.acc_level = 2;
            }
            else
            {
                // near miss
                Score.acc_level = 1;
            }
        }
	}
}
