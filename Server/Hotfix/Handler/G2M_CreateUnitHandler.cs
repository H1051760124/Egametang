﻿using System;
using Model;

namespace Hotfix
{
	[MessageHandler(AppType.Map)]
	public class G2M_CreateUnitHandler : AMRpcHandler<G2M_CreateUnit, M2G_CreateUnit>
	{
		protected override async void Run(Session session, G2M_CreateUnit message, Action<M2G_CreateUnit> reply)
		{
			M2G_CreateUnit response = new M2G_CreateUnit();
			try
			{
                Unit unit = Game.Scene.GetComponent<UnitComponent>().Get(message.UnitId);
                if (unit == null)
                {
                    if (message.UnitId > 0)
                        unit = EntityFactory.CreateWithId<Unit>(message.UnitId);
                    else
                        unit = EntityFactory.Create<Unit>();
                    Game.Scene.GetComponent<UnitComponent>().Add(unit);
                }
				//await unit.AddComponent<ActorComponent, IEntityActorHandler>(new MapUnitEntityActorHandler()).AddLocation();
				//unit.AddComponent<UnitGateComponent, long>(message.GateSessionId);
				response.UnitId = unit.Id;
                response.Count = Game.Scene.GetComponent<UnitComponent>().Count;
				reply(response);

				//if (response.Count == 1)
				{
                    //发送排行榜信息
     //               Response_RankList retRankList = new Response_RankList();

     //               Actor_CreateUnits actorCreateUnits = new Actor_CreateUnits();
					//Unit[] units = Game.Scene.GetComponent<UnitComponent>().GetAll();
     //               int tmpScore = 100;
					//foreach (Unit u in units)
					//{
					//	actorCreateUnits.Units.Add(new UnitInfo() {UnitId = u.Id, X = (int)(u.Position.X * 1000), Z = (int)(u.Position.Z * 1000) });
     //                   retRankList.Units.Add(new RankInfo() { Id = u.Id, name = "张三", score = tmpScore });
     //                   tmpScore -= 5;

     //               }
					//Log.Debug($"{MongoHelper.ToJson(actorCreateUnits)}");
					//MessageHelper.Broadcast(actorCreateUnits);

     //               MessageHelper.Broadcast(retRankList);

                }
			}
			catch (Exception e)
			{
				ReplyError(response, e, reply);
			}
		}
	}
}