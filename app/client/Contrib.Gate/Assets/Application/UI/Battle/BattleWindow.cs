using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Event;
using Network;
using Entities;
using System.Linq;
using Battle;
using System;
using Util;

namespace UI
{
    public class BattleWindow : Window
    {
        public const string CloseEvent = @"BattleWindow:Close";
        public GameObject unitPrefab;
        public GameObject enemiesArea;  // 敵配置エリア
        public GameObject playersArea;  // プレイヤー配置エリア
        public Toggle normal;
        public Toggle skill;
        public ToggleGroup actionGroup;

        BattleBeginReceive battleInfo;
        StageInfo stageInfo;
        Combat combat = new Combat();
        Command currentCommand;

        List<Unit> units = new List<Unit>();

        public enum ActionType
        {
            Normal,
            Skill,
        }

        protected override void OnOpen(params object[] args)
        {
            battleInfo = args[0] as BattleBeginReceive;
            stageInfo = Entity.Instance.StageInfo;

            // 敵配置
            for (int i = 0; i < battleInfo.enemies.Length; i++)
            {
                var enemy = Instantiate(unitPrefab, enemiesArea.transform);
                enemy.name = enemy.name.Replace("(Clone)", "");
                var unit = enemy.GetComponent<Unit>();
                unit.Setup(battleInfo.enemies[i]);
                combat.AddEnemy(unit);
                units.Add(unit);
            }
            // 自分のユニット配置
            for (int i = 0; i < stageInfo.pets.Length; i++)
            {
                var item = Entity.Instance.PetList.Find(stageInfo.pets[i]);
                if (item != null)
                {
                    var pet = Instantiate(unitPrefab, playersArea.transform);
                    pet.name = pet.name.Replace("(Clone)", "");
                    var unit = pet.GetComponent<Unit>();
                    unit.Setup(item);
                    combat.AddPlayer(unit);
                    units.Add(unit);
                }
            }

            Observer.Instance.Subscribe(Combat.StartEvent, OnSubscribe);
            Observer.Instance.Subscribe(Combat.TurnEvent, OnSubscribe);
            Observer.Instance.Subscribe(Combat.MoveEvent, OnSubscribe);
            Observer.Instance.Subscribe(Combat.PlayEvent, OnSubscribe);
            Observer.Instance.Subscribe(Combat.ResultEvent, OnSubscribe);

            combat.Next();
            base.OnOpen(args);
        }

        protected override void OnClose()
        {
            Observer.Instance.Notify(CloseEvent);
            Observer.Instance.Unsubscribe(Combat.StartEvent, OnSubscribe);
            Observer.Instance.Unsubscribe(Combat.TurnEvent, OnSubscribe);
            Observer.Instance.Unsubscribe(Combat.MoveEvent, OnSubscribe);
            Observer.Instance.Unsubscribe(Combat.PlayEvent, OnSubscribe);
            Observer.Instance.Unsubscribe(Combat.ResultEvent, OnSubscribe);
            base.OnClose();
        }

        protected override void OnButtonClick(Button btn)
        {
            switch(btn.name)
            {
                case "DebugEnd":

                    //BattleEnd();
                    break;
                case "Unit":
                    var unit = btn.GetComponent<Unit>();
                    if (unit.Selectable)
                    {
                        currentCommand.target = unit;

                        if (actionGroup.ActiveToggles().Contains(normal))
                        {
                            // 通常攻撃
                            currentCommand.action = Identify.Empty;
                        }
                        else
                        {
                            // スキル
                            currentCommand.action = new Identify(currentCommand.behavior.PetItem.skill);
                        }
                        combat.AddCommand(currentCommand);
                        combat.Next();
                    }
                    break;
                default:
                    base.OnButtonClick(btn);
                    break;
            }
        }
        public void BattleWin()
        {
            Protocol.Send(new BattleEndSend { guid = battleInfo.guid }, battleEnd =>
            {
                // コインと経験値入手
                Entity.Instance.UserState.AddCoin(battleEnd.coin);
                Protocol.Send(new BattleExpSend { guid = battleEnd.guid }, battleExp =>
                {
                    Entity.Instance.PetList.Modify(battleExp.exps);
                    Protocol.Send(new BattleRewardSend { guid = battleExp.guid }, battleReward =>
                    {
                        // アイテムとタマゴ入手
                        Entity.Instance.Inventory.Add(battleReward.items);
                        Entity.Instance.EggList.Modify(battleReward.eggs);

                        // 結果を表示する
                        Open<BattleResultWindow>(battleEnd, battleExp, battleReward);
                        Observer.Instance.Subscribe(BattleResultWindow.CloseEvent, OnSubscribe);
                    });
                });
            });
        }
        public void BattleLose()
        {
            Close();
            SceneManager.LoadScene(SceneName.Home);
        }

        public static Rect RectTransformToScreenSpace(RectTransform transform)
        {
            Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
            return new Rect((Vector2)transform.position - (size * 0.5f), size);
        }

        void UpdateSelectable(Unit behavior, Identify skill)
        {
            var s = Array.Find(Entity.Instance.Skills, v => v.Identify == skill);

            if (s == null)
            {
                // 味方すべて選択不可、敵すべて選択可能
                units.ForEach(unit => unit.Selectable = unit.side == Unit.Side.Enemy);
            }
            else
            {
                switch (s.Target)
                {
                    case SkiiTarget.Self:
                        units.ForEach(unit => unit.Selectable = unit == behavior);
                        break;
                    case SkiiTarget.Friend:
                        units.ForEach(unit => unit.Selectable = unit.side == behavior.side);
                        break;
                    case SkiiTarget.Rival:
                        units.ForEach(unit => unit.Selectable = unit.side != behavior.side);
                        break;
                }
            }
        }

        void ClearSelectable()
        {
            units.ForEach(unit => unit.Selectable = false);
        }

        public void OnToggleChange(Toggle toggle)
        {
            if (!toggle.isOn) return;

            if (toggle == normal)
            {
                UpdateSelectable(currentCommand.behavior, Identify.Empty);
            }
            else
            {
                UpdateSelectable(currentCommand.behavior, currentCommand.behavior.PetItem.skill);
            }
        }

        void OnSubscribe(string name, object o)
        {
            switch (name)
            {
                case BattleResultWindow.CloseEvent:
                    Observer.Instance.Unsubscribe(BattleResultWindow.CloseEvent, OnSubscribe);
                    this.Close();
                    break;
                case Combat.StartEvent:
                    DialogWindow.OpenOk("", "バトルスタート!!!", () =>
                    {
                        combat.Next();
                    });
                    break;
                case Combat.TurnEvent:
                    // 配置済なのでレイアウト無効
                    enemiesArea.GetComponent<LayoutGroup>().enabled = false;
                    playersArea.GetComponent<LayoutGroup>().enabled = false;
                    actionGroup.gameObject.SetActive(true);
                    combat.Next();
                    //DialogWindow.OpenOk("", string.Format("ターン{0}", (int)o), () =>
                    //{
                    //});
                    break;
                case Combat.MoveEvent:
                    var player = o as Unit;
                    player.Focus();
                    currentCommand = new Command();
                    currentCommand.behavior = player;
                    // 行動選択
                    normal.isOn = true;
                    skill.gameObject.SetActive(player.PetItem.skill != 0);
                    skill.interactable = true;  /// TODO : MPが足りなかったら暗転対応

                    UpdateSelectable(player, Identify.Empty);

                    break;
                case Combat.PlayEvent:
                    actionGroup.gameObject.SetActive(false);
                    ClearSelectable();
                    var command = o as Command;

                    if (command.behavior.IsDead)
                    {
                        // 行動する側が既に死亡した
                        combat.Next();
                    }
                    else
                    {
                        // 対象が死亡した
                        if (command.target.IsDead)
                        {
                            // 同じサイト＆生きているのをランダム決める
                            command.target = units.Where(unit => unit.side == command.target.side && !unit.IsDead).Shuffle().First();
                        }

                        // 行動再生
                        command.behavior.Focus(() =>
                        {
                            var skill = Array.Find(Entity.Instance.Skills, v => v.Identify == command.action);
                            var rect = RectTransformToScreenSpace(command.target.GetComponent<RectTransform>());
                            var handle = EffectWindow.Instance.Play("Simple_GeneratingPosition1", rect.center, ()=>
                            {
                                if (command.action == Identify.Empty)
                                {
                                    command.target.Damage(SkillLogic.Exec(command.behavior, command.target));
                                }
                                else
                                {
                                    switch (skill.Type)
                                    {
                                        case SkillType.Physical:
                                        case SkillType.Magic:
                                            command.target.Damage(SkillLogic.Exec(command.behavior, command.target, skill));
                                            break;
                                        case SkillType.Buff:
                                        case SkillType.Debuff:
                                            var buff = new Buff();
                                            buff.remain = skill.Turn;
                                            buff.skii = command.action;
                                            command.target.Params.buffs.Add(buff);
                                            break;
                                    }
                                }
                                combat.Next();
                            });
                            handle.SetScale(Vector3.one * .2f);
                        });
                    }
                    break;
                case Combat.ResultEvent:
                    switch ((string)o)
                    {
                        case "WIN":
                            BattleWin();
                            break;
                        default:
                            BattleLose();
                            break;
                    }
                    break;
            }
        }
    }
}



///=======================
/// 対応メモ
///=======================
// フェーズ
// 1. バトルスタート
// 登場演出、バトルスタートカットイン
// 2. ターン開始
// ターン数表示。地形効果？
// 3. コマンド選択
// プレイヤーユニットの行動選択
// 4. バトル内容表示
// 素早さによる行動演出
// >バトル継続の場合、 2.へ
// 5.勝利・敗北演出&経験値表示
//