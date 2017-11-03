﻿using UnityEngine;

namespace Model
{
	[MessageHandler((int)Opcode.Frame_ClickMap)]
	public class Frame_ClickMapHandler : AMHandler<Frame_ClickMap>
	{
		protected override void Run(Frame_ClickMap message)
		{
			Unit unit = Game.Scene.GetComponent<UnitComponent>().Get(message.Id);
			MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
			Vector3 dest = new Vector3(message.X / 1000f,0,message.Z / 1000f);
            //Log.Debug("Move dir: " + dest);
			moveComponent.MoveToDir(dest, 3);
			//moveComponent.Turn2D(dest - unit.Position);
		}
	}
}
