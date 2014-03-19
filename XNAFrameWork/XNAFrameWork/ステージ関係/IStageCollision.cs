//--------------------------------------------------//
// IStageCollision.cs								//
// ステージデータのあたり判定用インターフェース			//
//	Per interface for determining stage of data
// 制作日 : 2013/11/21								//
// 制作者 : Kouno Shin								//
//--------------------------------------------------//

//----------------------//
//----名前空間の省略-----//
//	Abbreviation of the name space
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNAFrameWork
{
	interface IStageCollision
	{
		// I do collision detection
		bool Hit(Vector3 playerPos, bool playerDuck);

		// And updates depending on the location of the stage
		void Update(Vector3 stageCenterPoint);

		void Draw();

        // checking player pass by obstacle
        void CheckPass(Vector3 playerPos);

        bool CheckSafePass();
	}
}
