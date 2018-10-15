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
    }

    static Dictionary<string, ScriptData> scripts = new Dictionary<string, ScriptData>();

    public static int Exec(Unit behavior, Unit target, Skill skill)
    {
        var data = GetScriptData(skill);
        return (int)data.Script.Call(data.Func, behavior, target, skill.Coefficient).Number;
    }

    public static int Exec(Unit behavior, Unit target)
    {
        var data = GetScriptData("Normal");
        return (int)data.Script.Call(data.Func, behavior, target, 1).Number;
    }

    static ScriptData GetScriptData(string fn)
    {
        ScriptData res;
        if (!scripts.TryGetValue(fn, out res))
        {
            res = new ScriptData { Script = new Script() };

            // グローバル変数に設定
            res.Script.Globals["Param"] = UserData.CreateStatic<Param>();
            res.Script.DoString(Resources.Load<TextAsset>($"Skill/{fn}").text);
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
}

