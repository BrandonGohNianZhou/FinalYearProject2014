//--------------------------------------//
// StageCollisionPart2.cs				//
// 2ステージのあたり判定用クラス		//
// 制作日 : 2013/11/25					//
// 制作者 : Kouno Shin					//
//--------------------------------------//

//----------------------//
//----名前空間の省略----//
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAFrameWork;

namespace XNAFrameWork
{
	class StageCollisionPart4 : IStageCollision
    {
        #region PASS_SAFELY

        bool safepass1 = true;
        bool safepass2 = true;
        bool safepass3 = true;
        bool safepass4 = true;
        bool safepass5 = true;
        bool passed1 = false;
        bool passed2 = false;
        bool passed3 = false;
        bool passed4 = false;
        bool passed5 = false;

        #endregion

		#region Field

		// Poly plate to be used in stage
        BoardTexture[] boardTexture = new BoardTexture[6];

		#endregion

		#region Constructor

		public StageCollisionPart4()
		{
			// Initialization
			for (int i = 0; i < boardTexture.Length; i++)
			{
				this.boardTexture[i] = new BoardTexture();
				this.boardTexture[i].Pos = Vector3.Zero;
				this.boardTexture[i].Scale = Vector3.One * 5;
				this.boardTexture[i].Rotation = Vector3.Zero;
				this.boardTexture[i].LoadTexture(TextureManager.GetInstance().GetTexture(TextureName.SHADOW));
            }
            this.safepass1 = true;
            this.safepass2 = true;
            this.safepass3 = true;
            this.safepass4 = true;
            this.safepass5 = true;
            this.passed1 = false;
            this.passed2 = false;
            this.passed3 = false;
            this.passed4 = false;
            this.passed5 = false;
		}

		#endregion

		#region 関数

		//--------------------------------------------------------------//
		// 関数名	Update												//
		//	Function Update
		// 機能		ステージの移動に合わせてコリジョン用BOXを更新する		//
		//	Update the collision for BOX in accordance with the movement of the stage function
		// 引数		ステージのポジション									//
		//	Position of the argument stage
		// 戻り値	なし													//
		//	No return value
		//--------------------------------------------------------------//
		public void Update(Vector3 stageCenterPoint)
		{
            //// Wall 1
            //this.boardTexture[0].PosX = 16.0f;  // 15
            //this.boardTexture[0].PosY = stageCenterPoint.Y + 0.5f;
            //this.boardTexture[0].PosZ = stageCenterPoint.Z + 420.0f;    // 400
            //this.boardTexture[0].scaleX = 13.0f;    // 14
            //this.boardTexture[0].scaleY = 50.0f;    // 50
            //this.boardTexture[0].scaleZ = 90.0f - 45.0f;


            //// Wall 2
            //this.boardTexture[1].PosX = -10.0f;
            //this.boardTexture[1].PosY = stageCenterPoint.Y + 20.0f;
            //this.boardTexture[1].PosZ = stageCenterPoint.Z + 20.0f; // -50
            //this.boardTexture[1].scaleX = 20.0f;
            //this.boardTexture[1].scaleY = 30.0f;
            //this.boardTexture[1].scaleZ = 130.0f - 70f;


            //// Wall 3
            //this.boardTexture[2].PosX = 8.0f;  // 23
            //this.boardTexture[2].PosY = stageCenterPoint.Y + 0.5f;
            //this.boardTexture[2].PosZ = stageCenterPoint.Z - 420.0f;    // -500
            //this.boardTexture[2].scaleX = 17.0f;    //15
            //this.boardTexture[2].scaleY = 17.0f;
            //this.boardTexture[2].scaleZ = 150.0f - 70f;

            //this.boardTexture[3].PosX = boardTexture[2].PosX;
            //this.boardTexture[3].PosY = boardTexture[2].PosY;
            //this.boardTexture[3].PosZ = this.boardTexture[2].PosZ - this.boardTexture[2].scaleZ;
            //this.boardTexture[3].Scale = this.boardTexture[2].Scale;


            // Wall 1
            this.boardTexture[0].PosX = -15.0f;  // 15
            this.boardTexture[0].PosY = stageCenterPoint.Y + 0.5f;
            this.boardTexture[0].PosZ = stageCenterPoint.Z + 420.0f;    // 400
            this.boardTexture[0].scaleX = 13.0f;    // 14
            this.boardTexture[0].scaleY = 15.0f;
            this.boardTexture[0].scaleZ = 10.0f;

            // Wall 2
            this.boardTexture[1].PosX = 15.0f;
            this.boardTexture[1].PosY = stageCenterPoint.Y + 10.0f;
            this.boardTexture[1].PosZ = stageCenterPoint.Z + 260.0f; // -50
            this.boardTexture[1].scaleX = 20.0f;
            this.boardTexture[1].scaleY = 15.0f;
            this.boardTexture[1].scaleZ = 15.0f;

            // Wall 3
            this.boardTexture[2].PosX = 3.0f;  // 23
            this.boardTexture[2].PosY = stageCenterPoint.Y + 0.5f;
            this.boardTexture[2].PosZ = stageCenterPoint.Z - 70.0f;    // -500
            this.boardTexture[2].scaleX = 7.0f;    //15
            this.boardTexture[2].scaleY = 15.0f;
            this.boardTexture[2].scaleZ = 5.0f;

            // Wall 4
            this.boardTexture[3].PosX = -10.0f;  // 23
            this.boardTexture[3].PosY = stageCenterPoint.Y + 5.0f;
            this.boardTexture[3].PosZ = stageCenterPoint.Z - 200.0f;    // -500
            this.boardTexture[3].scaleX = 11.0f;    //15
            this.boardTexture[3].scaleY = 7.0f;
            this.boardTexture[3].scaleZ = 5.0f;

            // Wall 5
            this.boardTexture[4].PosX = -10.0f;  // 23
            this.boardTexture[4].PosY = stageCenterPoint.Y + 0.5f;
            this.boardTexture[4].PosZ = stageCenterPoint.Z - 320.0f;    // -500
            this.boardTexture[4].scaleX = 10.0f;    //15
            this.boardTexture[4].scaleY = 17.0f;
            this.boardTexture[4].scaleZ = 5.0f;
		}

		//--------------------------------------------------------------//
		// 関数名	Hit													//
		//	Function name Hit
		// 機能		プレイヤーが壁に当たっているかどうかを判定する			//
		//	The function of determining the player whether against the wall
		// 引数		プレイヤーのポジション								//
		//	Position of argument Player
		// 戻り値	当たっている	: true									//
		//	Is hitting return value: true
		//			当たっていない	: false								//
		//	You do not hit: false
		//--------------------------------------------------------------//
        public bool Hit(Vector3 playerPos, bool playerDuck)
        {
            // check for player pass by obstacles
            this.CheckPass(playerPos);

			// If the hit
            if (!this.passed1)
            {
                if (Collision.BoundingBox(playerPos, Vector3.One * 0.1f,
                                         boardTexture[0].Pos, boardTexture[0].Scale))
                {
                    this.safepass1 = false;
                    return true;
                }
            }

			// If the hit
            if (!this.passed2)
            {
                if (Collision.BoundingBox(playerPos, Vector3.One * 0.1f,
                                         boardTexture[1].Pos, boardTexture[1].Scale))
                {
                    this.safepass2 = false;
                    return true;
                }
            }

			// If the hit
            if (!this.passed3)
            {
                if (Collision.BoundingBox(playerPos, Vector3.One * 0.1f,
                                         boardTexture[2].Pos, boardTexture[2].Scale))
                {
                    this.safepass3 = false;
                    return true;
                }
            }

			// If the hit
            if (!this.passed4)
            {
                if (Collision.BoundingBox(playerPos, Vector3.One * 0.1f,
                                         boardTexture[3].Pos, boardTexture[3].Scale, playerDuck))
                {
                    this.safepass4 = false;
                    return true;
                }
            }

			// If the hit
            if (!this.passed5)
            {
                if (Collision.BoundingBox(playerPos, Vector3.One * 0.1f,
                                         boardTexture[4].Pos, boardTexture[4].Scale))
                {
                    this.safepass5 = false;
                    return true;
                }
            }
			// If you do not hit the wall all
			return false;
		}

		public void Draw()
		{
			// Debugging drawing
			for (int i = 0; i < boardTexture.Length; i++)
			{
				boardTexture[i].Draw();
			}
		}

        public void CheckPass(Vector3 playerPos)
        {
            // if player pass the 1st obstacle of this stage
            if (playerPos.Z < this.boardTexture[0].PosZ - this.boardTexture[0].scaleZ - 3.0f)
            {
                this.passed1 = true;
            }

            // if player pass the 2nd obstacle of this stage
            if (playerPos.Z < this.boardTexture[1].PosZ - this.boardTexture[1].scaleZ - 3.0f)
            {
                this.passed2 = true;
            }

            // if player pass the 3rd obstacle of this stage
            if (playerPos.Z < this.boardTexture[2].PosZ - this.boardTexture[2].scaleZ - 3.0f)
            {
                this.passed3 = true;
            }

            // if player pass the 4th obstacle of this stage
            if (playerPos.Z < this.boardTexture[3].PosZ - this.boardTexture[3].scaleZ - 3.0f)
            {
                this.passed4 = true;
            }

            // if player pass the 5th obstacle of this stage
            if (playerPos.Z < this.boardTexture[4].PosZ - this.boardTexture[4].scaleZ - 3.0f)
            {
                this.passed5 = true;
            }
        }

        public bool CheckSafePass()
        {
            // if passes SAFELY
            if (this.passed1 && this.safepass1)
            {
                // if never call this funciton before
                this.safepass1 = false;
                return true;
            }

            // if passes SAFELY
            if (this.passed2 && this.safepass2)
            {
                // if never call this funciton before
                this.safepass2 = false;
                return true;
            }

            // if passes SAFELY
            if (this.passed3 && this.safepass3)
            {
                // if never call this funciton before
                this.safepass3 = false;
                return true;
            }

            // if passes SAFELY
            if (this.passed4 && this.safepass4)
            {
                // if never call this funciton before
                this.safepass4 = false;
                return true;
            }

            // if passes SAFELY
            if (this.passed5 && this.safepass5)
            {
                // if never call this funciton before
                this.safepass5 = false;
                return true;
            }
            return false;
        }
		#endregion
	}
}
