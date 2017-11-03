﻿using System;
using Model;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace Hotfix
{
	[ObjectEvent]
	public class UIBattleMainComponentEvent : ObjectEvent<UIBattleMainComponent>, IAwake
	{
		public void Awake()
		{
			this.Get().Awake();
		}
	}
	
	public class UIBattleMainComponent: Component
	{
        public static UIBattleMainComponent Instance { get; private set; }

        //for test
        private List<Effect> m_effects = new List<Effect>();

		private GameObject JoyStickBg;
        private GameObject JoyStickBtn;
		private GameObject CommonAttack;
        private GameObject Skill1;
        private GameObject Skill2;
        private GameObject Skill3;

        //榜单
        private List<GameObject> rankList = new List<GameObject>();
        private List<Text> nameList = new List<Text>();
        private List<Text> scoreList = new List<Text>();


        //小地图
        private List<GameObject> markList = new List<GameObject>();


        public void Awake()
		{
            Instance = this;

            ReferenceCollector rc = this.GetEntity<UI>().GameObject.GetComponent<ReferenceCollector>();
            CommonAttack = rc.Get<GameObject>("CommonAttack");
            if (CommonAttack != null)
            {
                CommonAttack.GetComponent<Button>().onClick.Add(OnCommonAttack);
            }
            
            Skill1 = rc.Get<GameObject>("Skill1");
            Skill1.GetComponent<Button>().onClick.Add(OnSkill1);
            Skill2 = rc.Get<GameObject>("Skill2");
            Skill2.GetComponent<Button>().onClick.Add(OnSkill2);
            Skill3 = rc.Get<GameObject>("Skill3");
            Skill3.GetComponent<Button>().onClick.Add(OnSkill3);

            JoyStickBg = rc.Get<GameObject>("ControllerBG");
            JoyStickBtn = rc.Get<GameObject>("ControllerButton");


            //榜单
            GameObject gridGroup = rc.Get<GameObject>("Grid_Group");
            for (int i=1; i<= 10; i++)
            {
                string cellName = "cell_" + i;
                GameObject cell = gridGroup.transform.Find(cellName).gameObject;
                if (cell != null)
                {
                    rankList.Add(cell);
                    Text name = cell.transform.Find("name").GetComponent<Text>();
                    nameList.Add(name);
                    name.text = "张三" + i;
                    Text score = cell.transform.Find("score").GetComponent<Text>();
                    score.text = (100+ i* 10).ToString();
                    scoreList.Add(score);
                }
            }

        }

        public void PlayAnimation(string actName)
        {
            Unit unit = Game.Scene.GetComponent<UnitComponent>().Get(PlayerComponent.Instance.MyPlayer.UnitId);
            var animatorComponent = unit.GetComponent<AnimatorComponent>();
            if (animatorComponent != null)
            {
                animatorComponent.Play(actName);
            }
        }

        public void PlayEffect(string effectName, long id = 0)
        {
            Unit unit = Game.Scene.GetComponent<UnitComponent>().Get(PlayerComponent.Instance.MyPlayer.UnitId);
            //Effect eff = EffectFactory.Create(unit.Id * 10 + id, effectName, unit.GameObject.transform);
            //m_effects.Add(eff);

            //DelayManager.instance.delay(5, () =>
            //{
            //    DisposeEffect();
            //});

            EffectManager.instance.AddFxAutoRemove(effectName, unit.GameObject.transform);
        }

        void DisposeEffect()
        {
            if (m_effects.Count > 0)
            {
                m_effects[0].GameObject.SetActive(false);
                m_effects[0].Dispose();
                m_effects.RemoveAt(0);
            }
        }

        public void OnCommonAttack()
        {
            SessionComponent.Instance.Session.Send(new Request_UseSkill() { Id = PlayerComponent.Instance.MyPlayer.UnitId, skillId = 1 });

            Log.Debug("OnCommonAttack");
        }

        public void OnSkill1()
        {
            SessionComponent.Instance.Session.Send(new Request_UseSkill() { Id = PlayerComponent.Instance.MyPlayer.UnitId, skillId = 2 });
            Log.Debug("OnSkill1");

            //DisposeEffect();
        }

        public void OnSkill2()
        {
            SessionComponent.Instance.Session.Send(new Request_UseSkill() { Id = PlayerComponent.Instance.MyPlayer.UnitId, skillId = 3 });
            Log.Debug("OnSkill2");
        }

        public void OnSkill3()
        {
            SessionComponent.Instance.Session.Send(new Request_UseSkill() { Id = PlayerComponent.Instance.MyPlayer.UnitId, skillId = 4 });
            Log.Debug("OnSkill3");
        }

        public void ProcessRankList(Response_RankList message)
        {
            int i = 0;
            foreach (RankInfo unitInfo in message.Units)
            {
                if (i < 10)
                {
                    rankList[i].SetActive(true);
                    nameList[i].text = unitInfo.Id.ToString();
                    scoreList[i].text = unitInfo.score.ToString();
                }

                i++;
            }

            for (int j=i; j<10; ++j)
            {
                rankList[j].SetActive(false);
            }
        }

	}
}
