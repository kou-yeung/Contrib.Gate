using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using Entities;

/// <summary>
/// スキルロジック
/// </summary>
public static class SkillLogic
{
    class ScriptData
    {
        public Script Script;
        public object Func;
    }

    static SkillLogic()
    {
        // タイプを登録する
        UserData.RegisterType<Unit>();
        UserData.RegisterType<Battle.Params>();
        UserData.RegisterType<Param>();
        UserData.RegisterType<Attribute>();
        UserData.RegisterType<Race>();
        UserData.RegisterType<Unit.Side>();
    }

    static Dictionary<string, ScriptData> scripts = new Dictionary<string, ScriptData>();

    public static int Exec(Unit behavior, Unit target, Skill skill)
    {
        var data = GetScriptData(skill);
        return (int)data.Script.Call(data.Func, behavior, target, ConflictTable[(int)behavior.Race, (int)target.Race]).Number;
    }

    public static int Exec(Unit behavior, Unit target)
    {
        var data = GetScriptData("Physical");
        var conflict = ConflictTable[(int)behavior.Race, (int)target.Race];
        return (int)data.Script.Call(data.Func, behavior, target, conflict).Number;
    }

    static ScriptData GetScriptData(string fn)
    {
        ScriptData res;
        if (!scripts.TryGetValue(fn, out res))
        {
            res = new ScriptData { Script = new Script() };

            res.Script.Options.DebugPrint = (s) => Debug.Log(s);
            // グローバル変数に設定
            res.Script.Globals["Param"] = UserData.CreateStatic<Param>();
            res.Script.Globals["Attribute"] = UserData.CreateStatic<Attribute>();
            res.Script.Globals["Race"] = UserData.CreateStatic<Race>();
            res.Script.Globals["Side"] = UserData.CreateStatic<Unit.Side>();
            var text = Resources.Load<TextAsset>($"Skill/{fn}").text;
            res.Script.DoString(text);
            res.Func = res.Script.Globals["Exec"];

            // キャッシュ
            scripts[fn] = res;
        }
        return res;
    }
    static ScriptData GetScriptData(Skill skill)
    {
        return GetScriptData(skill.Script);
    }

    static int[,] ConflictTable = new int[,]
    {
        // Beast  Undead  Fly  Insect  Plant  Amorphas  Metal  Dragon  Human
        {/*Beast*/+0,/*Undead*/+0, /*Fly*/+2,/*Insect*/+0,/*Plant*/+4,/*Amorphas*/+0,/*Metal*/+0,/*Dragon*/+3,/*Human*/-4}, // Beast
        {/*Beast*/+0,/*Undead*/+0, /*Fly*/-3,/*Insect*/+0,/*Plant*/-4,/*Amorphas*/+0,/*Metal*/+2,/*Dragon*/+0,/*Human*/+4}, // Undead
        {/*Beast*/-3,/*Undead*/+2, /*Fly*/+0,/*Insect*/+4,/*Plant*/+0,/*Amorphas*/+0,/*Metal*/+0,/*Dragon*/-4,/*Human*/+0}, // Fly
        {/*Beast*/+0,/*Undead*/+0, /*Fly*/-4,/*Insect*/+0,/*Plant*/+2,/*Amorphas*/+4,/*Metal*/+0,/*Dragon*/+0,/*Human*/-3}, // Insect
        {/*Beast*/-4,/*Undead*/+4, /*Fly*/+0,/*Insect*/-3,/*Plant*/+0,/*Amorphas*/+2,/*Metal*/+0,/*Dragon*/+0,/*Human*/+0}, // Plant
        {/*Beast*/+0,/*Undead*/+0, /*Fly*/+0,/*Insect*/-4,/*Plant*/-3,/*Amorphas*/+0,/*Metal*/+4,/*Dragon*/+2,/*Human*/+0}, // Amorphas
        {/*Beast*/+0,/*Undead*/-3, /*Fly*/+0,/*Insect*/+0,/*Plant*/+0,/*Amorphas*/-4,/*Metal*/+0,/*Dragon*/+4,/*Human*/+2}, // Metal
        {/*Beast*/+2,/*Undead*/+0, /*Fly*/+4,/*Insect*/+0,/*Plant*/+0,/*Amorphas*/-3,/*Metal*/-4,/*Dragon*/+0,/*Human*/+0}, // Dragon
        {/*Beast*/+4,/*Undead*/-4, /*Fly*/+0,/*Insect*/+2,/*Plant*/+0,/*Amorphas*/+0,/*Metal*/-3,/*Dragon*/+0,/*Human*/+0}, // Human
    };
}

