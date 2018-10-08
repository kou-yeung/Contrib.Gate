﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event;

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
        public Unit behavior;    // 誰が
        public Unit target;      // 誰に
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
                        if (commands.Count < Players.Count)
                        {
                            Observer.Instance.Notify(MoveEvent, NextPlayerUnit());
                        }
                        else
                        {
                            // 行動すべて選択したため、フェーズ移行
                            phase = Phase.Play;
                            var command = PopCommand();
                            Observer.Instance.Notify(PlayEvent, command);
                        }
                    }
                    break;
                case Phase.Play:
                    {
                        var command = PopCommand();
                        if (command != null)
                        {
                            Observer.Instance.Notify(PlayEvent, command);
                        }
                        else
                        {
                            if (trun <= 3)
                            {
                                phase = Phase.Turn;
                                Observer.Instance.Notify(TurnEvent, trun++);
                            }
                            else
                            {
                                phase = Phase.Result;
                                Observer.Instance.Notify(ResultEvent);
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
        public void AddPlayerCommand(Command command)
        {
            commands.Add(command);
        }

        Unit NextPlayerUnit()
        {
            return Players[commands.Count];
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
