﻿using System;
using Model;

namespace Hotfix
{
	[MessageHandler(AppType.Gate)]
	public class C2G_LoginGateHandler : AMRpcHandler<C2G_LoginGate, G2C_LoginGate>
	{
		protected override async void Run(Session session, C2G_LoginGate message, Action<G2C_LoginGate> reply)
		{
			G2C_LoginGate response = new G2C_LoginGate();
			try
			{
				string account = Game.Scene.GetComponent<GateSessionKeyComponent>().Get(message.Key);
				if (account == null)
				{
					response.Error = ErrorCode.ERR_ConnectGateKeyError;
					response.Message = "Gate key验证失败!";
					reply(response);
					return;
				}

                
                PlayerComponent playerComponent = Game.Scene.GetComponent<PlayerComponent>();
                
                

                Player player = playerComponent.Get(account);
                if (player == null)
                {
                    player = EntityFactory.Create<Player, string>(account);
                    playerComponent.Add(player);
                }
                else
                {
                    if (player.PlayerStatus == PlayerStatus.Online)
                    {
                        response.Error = ErrorCode.ERR_ConnectGateKeyError;
                        response.Message = "已经在登陆状态!";
                        reply(response);
                        return;
                    }
                }
                session.AddComponent<SessionPlayerComponent>().Player = player;
				await session.AddComponent<ActorComponent, IEntityActorHandler>(new GateSessionEntityActorHandler()).AddLocation();

				response.PlayerId = player.Id;
				reply(response);
			}
			catch (Exception e)
			{
				ReplyError(response, e, reply);
			}
		}
	}
}