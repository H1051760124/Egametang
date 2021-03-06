﻿using System.Collections.Generic;
using System.Linq;

namespace Model
{
	[ObjectEvent]
	public class MatchRoomEvent : ObjectEvent<MatchRoom>, IAwake
	{
		public void Awake()
		{
			this.Get().Awake();
		}
	}
    
    public class MatchRoom : Entity
	{
        public int Frame;
        public FrameMessage cacheFrameMessage;
        public FrameMessage FrameMessage;
        public UnitComponent unitComponent;
        public void Awake()
        {
            Frame = 0;
            cacheFrameMessage = new FrameMessage() { Frame = Frame };
            FrameMessage = new FrameMessage() { Frame = Frame };
            UpdateFrameAsync();
            unitComponent = this.AddComponent<UnitComponent>();
        }

        public async void UpdateFrameAsync()
        {
            TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();

            while (true)
            {
                if (Id == 0)
                {
                    return;
                }

                await timerComponent.WaitAsync(40);
                Broadcast(FrameMessage,unitComponent.GetAll());
                ++Frame;
                FrameMessage = new FrameMessage() { Frame = Frame };
            }
        }

        public void Add(AFrameMessage message)
        {
            cacheFrameMessage.Messages.Add(message);
            FrameMessage.Messages.Add(message);
        }

        void Broadcast(AActorMessage message, Unit[] units)
        {
            ActorProxyComponent actorProxyComponent = Game.Scene.GetComponent<ActorProxyComponent>();
            foreach (Unit unit in units)
            {
                UnitGateComponent ugComponent = unit.GetComponent<UnitGateComponent>();
                if (ugComponent != null)
                {
                    long gateSessionId = ugComponent.GateSessionId;
                    actorProxyComponent.Get(gateSessionId).Send(message);
                }
            }
        }
        
        public override void Dispose()
		{
			if (this.Id == 0)
			{
				return;
			}

			base.Dispose();

            unitComponent.Dispose();

        }
	}
}