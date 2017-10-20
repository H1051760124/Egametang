﻿using UnityEngine;

namespace Model
{
    [ObjectEvent]
    public class OperaComponentEvent : ObjectEvent<OperaComponent>, IUpdate, IAwake
    {
        public void Update()
        {
            this.Get().Update();
        }

	    public void Awake()
	    {
		    this.Get().Awake();
	    }
    }

    public class OperaComponent: Component
    {
        public Vector3 ClickPoint;

	    public int mapMask;

	    public void Awake()
	    {
		    this.mapMask = LayerMask.GetMask("Map");
	    }

        public void Update()
        {
            /*if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
	            if (Physics.Raycast(ray, out hit, 1000, this.mapMask))
	            {
					this.ClickPoint = hit.point;
		            SessionComponent.Instance.Session.Send(new Frame_ClickMap() { X = (int)(this.ClickPoint.x * 1000), Z = (int)(this.ClickPoint.z * 1000) });
				}
            }*/
	        
	        Vector2 mInputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	        Vector2 direction = mInputVector;
	        direction.Normalize();
	        SessionComponent.Instance.Session.Send(new Frame_ClickMap() { X = (int)(direction.x * 1000), Z = (int)(direction.y * 1000) });

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000, this.mapMask))
                {
                    this.ClickPoint = hit.point;
                    //Log.Debug($"Send Frame_ClickMap x: " + (int)(this.ClickPoint.x * 1000) + " Z: " + (int)(this.ClickPoint.z * 1000));
                    //SessionComponent.Instance.Session.Send(new Frame_TestMsg() { X = (int)(this.ClickPoint.x * 1000), Z = (int)(this.ClickPoint.z * 1000) });
                    Unit unit = Game.Scene.GetComponent<UnitComponent>().Get(PlayerComponent.Instance.MyPlayer.UnitId);
                    var animatorComponent = unit.GetComponent<AnimatorComponent>();
                    if (animatorComponent != null)
                    {
                        animatorComponent.Play("attack_2");
                    }
                }
            }

        }
    }
}
