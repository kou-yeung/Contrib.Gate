using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;
using Entities;
using System.Linq;
using Util;

namespace Battle
{
    public enum Phase {
        Init,   // 初期化中
        Start,  // 登場演出
        Turn,   // ターン
        Move,   // 行動
        Play,   // 内容再生:勝負 あり(Result) なし(Turn)
        Result, // 結果表示
    }


    /// <summary>
    /// コマンド
    /// </summary>
    public class Command
    {
        public Unit behavior;       // 誰が
        public Unit target;         // 誰に
        public Identify action;     // 何を
    }

    /// <summary>
    /// バトル中のパラメータ
    /// </summary>
    public class Params
    {
        public List<Buff> buffs = new List<Buff>();        // バフ効果一覧
        int[] param = new int[(int)Param.Count];           // 基礎パラメータ

        public Params(EnemyItem item)
        {
            for (int i = 0; i < (int)Param.Count; i++)
            {
                param[i] = item.GetParam((Param)i);
            }
        }
        public Params(PetItem item)
        {
            for (int i = 0; i < (int)Param.Count; i++)
            {
                param[i] = item.GetParam((Param)i);
            }
        }
        public int this[Param param]
        {
            get
            {
                /// TODO : バフ効果を足す
                return this.param[(int)param];
            }
            set
            {
                this.param[(int)param] = value;
            }
        }
    }

    /// <summary>
    /// 属性
    /// </summary>
    public class Attributes
    {
        int[] attribute;
        public Attributes(int[] attribute)
        {
            this.attribute = attribute;
        }
        public int this[Attribute att]
        {
            get
            {
                return this.attribute[(int)att];
            }
        }
    }

    /// <summary>
    /// スキル
    /// </summary>
    public class Skill
    {
        Entities.Skill skill;
        public Attributes Attributes { get; private set; }
        public Skill(Entities.Skill skill)
        {
            this.skill = skill;
            this.Attributes = new Attributes(skill.Attribute);
        }
    }

    /// <summary>
    /// バフ効果
    /// </summary>
    public class Buff
    {
        public Identify skii;   // どのスキルによるバフか
        public int remain;      // 残りターン数
    }

    /// <summary>
    /// バトル全体の管理
    /// </summary>
    public class Combat
    {
        public const string StartEvent = @"Phase:Start";
        public const string TurnEvent = @"Phase:Turn";
        public const string MoveEvent = @"Phase:Move";
        public const string PlayEvent = @"Phase:Play";
        public const string ResultEvent = @"Phase:Result";


        public Phase phase { get; private set; }
        List<Command> commands = new List<Command>();
        List<Unit> Enemies = new List<Unit>();
        List<Unit> Players = new List<Unit>();

        int trun = 1;   // ターン数

        public void AddEnemy(Unit enemy)
        {
            Enemies.Add(enemy);
        }
        public void AddPlayer(Unit player)
        {
            Players.Add(player);
        }

        public void Next()
        {
            switch (phase)
            {
                case Phase.Init:
                    phase = Phase.Start;
                    Observer.Instance.Notify(StartEvent);
                    break;
                case Phase.Start:
                    phase = Phase.Turn;
                    Observer.Instance.Notify(TurnEvent, trun++);
                    break;
                case Phase.Turn:
                    phase = Phase.Move;
                    Observer.Instance.Notify(MoveEvent, NextPlayerUnit());
                    break;
                case Phase.Move:
                    {
                        if (commands.Count < Players.Where(v => !v.IsDead).Count())
                        {
                            Observer.Instance.Notify(MoveEvent, NextPlayerUnit());
                        }
                        else
                        {
                            // 敵行動を追加する
                            foreach (var enemy in Enemies)
                            {
                                if (enemy.IsDead) continue;
                                var cmd = new Command();
                                cmd.behavior = enemy;
                                cmd.target = Players.Where(v => !v.IsDead).Shuffle().First();   // 将来はAIスクリプトから選択する
                                cmd.action = Identify.Empty;
                                AddCommand(cmd);
                            }

                            // コマンドを行動者の素早さでソードする
                            commands.Sort((a, b) =>
                            {
                                return b.behavior.Params[Param.Agility].CompareTo(a.behavior.Params[Param.Agility]);
                            });

                            // 行動すべて選択したため、フェーズ移行
                            phase = Phase.Play;
                            var command = PopCommand();
                            Observer.Instance.Notify(PlayEvent, command);
                        }
                    }
                    break;
                case Phase.Play:
                    {
                        if (Enemies.All(v => v.IsDead))
                        {
                            // 敵がすべて死亡
                            phase = Phase.Result;
                            Observer.Instance.Notify(ResultEvent, "WIN");
                        }
                        else if (Players.All(v => v.IsDead))
                        {
                            // 味方がすべて死亡
                            phase = Phase.Result;
                            Observer.Instance.Notify(ResultEvent, "LOSE");
                        } else
                        {
                            var command = PopCommand();
                            if (command != null)
                            {
                                // 次の行動
                                Observer.Instance.Notify(PlayEvent, command);
                            }
                            else
                            {
                                // 次のターン
                                phase = Phase.Turn;
                                Observer.Instance.Notify(TurnEvent, trun++);
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// プレイヤーのコマンド追加
        /// </summary>
        /// <param name="command"></param>
        public void AddCommand(Command command)
        {
            commands.Add(command);
        }

        Unit NextPlayerUnit()
        {
            return Players.Where(v=>!v.IsDead).ElementAt(commands.Count);
        }

        Command PopCommand()
        {
            if (commands.Count <= 0) return null;
            var command = commands[0];
            commands.RemoveAt(0);
            return command;
        }
    }
}

