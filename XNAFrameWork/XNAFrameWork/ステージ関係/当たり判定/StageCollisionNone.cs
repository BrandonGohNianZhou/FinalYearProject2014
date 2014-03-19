//--------------------------------------//
// StageCollisionNone.cs				//
// ステージのコリジョンなしクラス			//
//	Class without collision of the stage
// 制作日 : 2013/11/26					//
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
	class StageCollisionNone : IStageCollision
	{
		#region Function
		// It is not nothing

		public void Update(Vector3 stageCenterPoint)
		{
		}

		
		public bool Hit(Vector3 playerPos, bool playerDuck)
		{
			return false;
		}


		public void Draw()
		{
		}

        public void CheckPass(Vector3 playerPos)
        {

        }

        public bool CheckSafePass()
        {
            return false;
        }
		#endregion
	}
}
