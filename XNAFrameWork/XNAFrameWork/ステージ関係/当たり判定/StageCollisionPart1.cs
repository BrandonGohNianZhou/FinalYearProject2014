//--------------------------------------//
// StageCollisionPart1.cs				//
// 1ステージのあたり判定用クラス			//
//	Per class determination of stage 1
// 制作日 : 2013/11/25					//
// 制作者 : Kouno Shin					//
//--------------------------------------//

//----------------------//
//----名前空間の省略-----//
//	Abbreviation of the name space
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAFrameWork;

namespace XNAFrameWork
{
	class StageCollisionPart1 : IStageCollision
    {
        #region PASS_SAFELY

        bool safepass1 = true;
        bool safepass2 = true;
        bool passed1 = false;
        bool passed2 = false;

        #endregion

		#region Field

		// Poly plate to be used in stage
		BoardTexture[] boardTexture = new BoardTexture[3];

		#endregion

		#region Constructor

		public StageCollisionPart1()
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
            this.passed1 = false;
            this.passed2 = false;
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
			// Wall 1
			this.boardTexture[0].PosX = -8.0f; // -22f
			this.boardTexture[0].PosY = stageCenterPoint.Y + 0.5f;
			this.boardTexture[0].PosZ = stageCenterPoint.Z + 110.0f;
            this.boardTexture[0].scaleX = 12.0f * 0.39f;
            this.boardTexture[0].scaleY = 35.0f;
			this.boardTexture[0].scaleZ = 4.0f;

			
			// Wall 2
            this.boardTexture[1].PosX = -12.5f; // -30f
            this.boardTexture[1].PosY = stageCenterPoint.Y + 0.5f;
            this.boardTexture[1].PosZ = stageCenterPoint.Z - 260.0f; ;
            this.boardTexture[1].scaleX = 10.0f * 0.39f;
            this.boardTexture[1].scaleY = 19.0f * 0.39f;
            this.boardTexture[1].scaleZ = 4.0f;


            //this.boardTexture[2].PosX = boardTexture[1].PosX;
            //this.boardTexture[2].PosY = boardTexture[1].PosY;
            //this.boardTexture[2].PosZ = this.boardTexture[1].PosZ - this.boardTexture[1].scaleZ;
            //this.boardTexture[2].Scale = this.boardTexture[1].Scale;
			
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
		//	You do not: false
		//--------------------------------------------------------------//
        public bool Hit(Vector3 playerPos, bool playerDuck)
        {
            // check for player pass by obstacles
            this.CheckPass(playerPos);

			// If the hit
            if (!this.passed1)
            {
                if (Collision.PlayerAndTextureCollision(playerPos, boardTexture[0]))
                {
                    this.safepass1 = false;
                    return true;
                }
            }

			// If the hit
            if (!this.passed2)
            {
                if (Collision.PlayerAndTextureCollision(playerPos, boardTexture[1], playerDuck))
                {
                    this.safepass2 = false;
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
            if (playerPos.Z < this.boardTexture[0].PosZ - this.boardTexture[0].scaleZ - 5.0f)
            {
                this.passed1 = true;
            }

            // if player pass the 2nd obstacle of this stage
            if (playerPos.Z < this.boardTexture[1].PosZ - this.boardTexture[1].scaleZ - 5.0f)
            {
                this.passed2 = true;
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

            return false;
        }
		#endregion
	}
}
