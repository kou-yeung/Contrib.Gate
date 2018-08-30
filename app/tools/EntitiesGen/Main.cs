using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

class EntitiesGen
{
    static string entitiesPath;// = @"C:\git\Contrib.Gate\app\tools\EntitiesGen\template\entities.csv";
    //static string tempPath = @"C:\git\Contrib.Gate\app\tools\EntitiesGen\template\TypeScript\Entities";

    public enum Type
    {
        CSharp,
        TypeScript,
    }

    interface IParam
    {
        IParam Parse(params string[] data);
        string CSharp();
        string TypeScript();
        string ToString(Type type);
    }

    class EnumParam : IParam
    {
        public string name;
        public string value;
        public string comment;

        public IParam Parse(string[] data)
        {
            name = data[0];
            value = data.Length >= 2 ? data[1] : "";
            comment = data.Length >= 3 ? data[2] : "";
            return this;
        }
        public string CSharp()
        {
            string res = name;
            if (!string.IsNullOrEmpty(value)) res += string.Format("={0}", value);
            res += ",";
            if (!string.IsNullOrEmpty(comment)) res += string.Format("// {0}", comment);
            return res;
        }

        public string TypeScript()
        {
            string res = name;
            if (!string.IsNullOrEmpty(value)) res += string.Format("={0}", value);
            res += ",";
            if (!string.IsNullOrEmpty(comment)) res += string.Format("// {0}", comment);
            return res;
        }

        public string ToString(Type type)
        {
            switch (type)
            {
                case Type.CSharp: return CSharp();
                case Type.TypeScript: return TypeScript();
                default: return "";
            }
        }
    }

    class ClassParam : IParam
    {
        public string type;
        public string name;
        public string comment;

        public IParam Parse(string[] data)
        {
            type = data[0];
            name = data[1];
            comment = data.Length >= 3 ? data[2] : "";
            return this;
        }
        public string CSharp()
        {
            string res = string.Format("public {0} {1};", type, name);
            if (!string.IsNullOrEmpty(comment)) res += string.Format("// {0}", comment);
            return res;
        }

        public string TypeScript()
        {
            string res = string.Format("{1}: {0};", ReplaceType(type), name);
            if (!string.IsNullOrEmpty(comment)) res += string.Format("// {0}", comment);
            return res;
        }
        public string ToString(Type type)
        {
            switch (type)
            {
                case Type.CSharp: return CSharp();
                case Type.TypeScript: return TypeScript();
                default: return "";
            }
        }

        public string ReplaceType(string type)
        {
            Dictionary<string, string> replaces = new Dictionary<string, string>()
            {
                {"bool", "boolean"},
                {"int", "number"},
                {"float", "number"},
            };
            foreach (var r in replaces)
            {
                type = type.Replace(r.Key, r.Value);
            }
            return type;
        }
    }

    class Entity<T> where T : IParam, new()
    {
        public List<IParam> param = new List<IParam>();

        public string name;
        public string comment;

        public Entity(string name, string comment)
        {
            this.name = name;
            this.comment = string.IsNullOrEmpty(comment)? name: comment;
        }

        public void Add(string[] data)
        {
            param.Add(new T().Parse(data));
        }
    }
    class Output
    {
        public Type type;
        public string path;
    }

    class Setting
    {
        public string templatePath;
        public List<Output> outputs = new List<Output>();

    }

    static List<Entity<EnumParam>> enums = new List<Entity<EnumParam>>();
    static List<Entity<ClassParam>> classies = new List<Entity<ClassParam>>();

    static void Main()
    {
        /// entitiesPath=<fn>

        var args = Environment.GetCommandLineArgs();

        if (args.Length <= 1)
        {
            Console.WriteLine("entitiesPath=<fn>");
        }
        else
        {
            // MEMO : デバッグ時に起動パラメータの設定 : entitiesPath=./../../template/entities.csv
            foreach (var arg in args)
            {
                var data = arg.Split('=');
                switch (data[0])
                {
                    case "entitiesPath":
                        entitiesPath = data[1];
                        break;
                }
            }
            var setting = ParseSetting(entitiesPath);
            ParseEntities(entitiesPath);

            foreach (var output in setting.outputs)
            {
                var fn = Path.Combine(output.path, $"Entities.{GetExtension(output.type)}");
                var temp = Path.Combine(setting.templatePath, output.type.ToString());
                temp = Path.Combine(temp, "Entities");

                File.WriteAllText(fn, GenEntities(temp, output.type));
            }
        }
    }

    static Setting ParseSetting(string fn)
    {
        var res = new Setting();
        foreach (var str in File.ReadAllLines(fn))
        {
            if (str.StartsWith("//")) continue;
            if (string.IsNullOrEmpty(str.Trim())) continue;

            var data = str.Split(',');
            switch (data[0])
            {
                case "template":
                    res.templatePath = data[1];
                    break;
                case "output":
                    res.outputs.Add(new Output { type = (Type)Enum.Parse(typeof(Type), data[1]), path = data[2] });
                    break;
            }
        }
        return res;
    }

    /// <summary>
    /// enum / class を解析する
    /// </summary>
    /// <param name="fn"></param>
    /// <returns></returns>
    static void ParseEntities(string fn)
    {
        Action<string[]> addFunc = null;

        foreach (var str in File.ReadAllLines(fn))
        {
            if (string.IsNullOrEmpty(str)) continue;
            if (str.StartsWith("//")) continue;

            var data = str.Split(',');
            switch (data[0])
            {
                case "enum":
                    {
                        var name = data[1];
                        var comment = (data.Length >= 2) ? data[2] : "";
                        enums.Add(new Entity<EnumParam>(name, comment));
                        addFunc = enums.Last().Add;
                    }
                    break;
                case "class":
                    {
                        var name = data[1];
                        var comment = (data.Length >= 2) ? data[2] : "";
                        classies.Add(new Entity<ClassParam>(name, comment));
                        addFunc = classies.Last().Add;
                    }
                    break;
                default:
                    if(addFunc != null) addFunc(data);
                    break;
            }
        }
    }

    static string GenEntities(string fn, Type type)
    {
        var temp = File.ReadAllText(fn);

        // {{ENUM}} 差し替え
        temp = Regex.Replace(temp, @"{{ENUM}}([^.]*){{ENUM}}", (m) =>
        {
            var enumTemp = m.Groups[1].ToString();
            var sb = new StringBuilder();

            foreach (var e in enums)
            {
                var res = enumTemp.Replace("{{NAME}}", e.name);
                res = res.Replace("{{COMMENT}}", e.comment);
                res = Regex.Replace(res, @"^(\s*){{MEMBERS}}", match =>
                {
                    var indent = match.Groups[1].ToString();
                    var param = e.param.Select(p => $"{indent}{p.ToString(type)}");
                    return string.Join("\n", param);

                }, RegexOptions.Multiline);
                sb.Append(res);
            }
            return sb.ToString();
        }, RegexOptions.Multiline);

        // {{CLASS}} 差し替え
        temp = Regex.Replace(temp, @"{{CLASS}}([^.]*){{CLASS}}", (m) =>
        {
            var classTemp = m.Groups[1].ToString();
            var sb = new StringBuilder();

            foreach (var c in classies)
            {
                var res = classTemp.Replace("{{NAME}}", c.name);
                res = res.Replace("{{COMMENT}}", c.comment);
                res = Regex.Replace(res, @"^(\s*){{MEMBERS}}", match =>
                {
                    var indent = match.Groups[1].ToString();
                    var param = c.param.Select(p => $"{indent}{p.ToString(type)}");
                    return string.Join("\n", param);

                }, RegexOptions.Multiline);
                sb.Append(res);
            }
            return sb.ToString();
        }, RegexOptions.Multiline);

        return temp;
    }

    static string GetExtension(Type type)
    {
        switch (type)
        {
            case Type.CSharp: return "cs";
            case Type.TypeScript: return "ts";
            default: return "";
        }
    }
}
