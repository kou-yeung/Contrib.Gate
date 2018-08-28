using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

class ProtocolGen
{
    static string protocolPath;

    public enum Type
    {
        CSharp,
        TypeScript,
    }

    class Param
    {
        public string type;
        public string name;
        public string comment;

        // C# のフォーマットで返す
        string CSharp()
        {
            if(string.IsNullOrEmpty(comment)) return $"public {type} {name};";
            else return $"public {type} {name}; // {comment}";
        }

        // TypeScript のフォーマットで返す
        string TypeScript()
        {
            if (string.IsNullOrEmpty(comment)) return $"{name}: {type};";
            else return $"{name}: {type}; // {comment}";
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

    class Protocol
    {
        public string name;
        public List<Param> up = new List<Param>();
        public List<Param> down = new List<Param>();
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

    static void Main()
    {
        /// protocol=<fn>
        
        var args = Environment.GetCommandLineArgs();

        if (args.Length <= 1)
        {
            Console.WriteLine("protocolPath=<fn>");
        }
        else
        {
            // MEMO : デバッグ時に起動パラメータの設定 : protocolPath=./../../template/protocol.csv
            foreach (var arg in args)
            {
                var data = arg.Split('=');
                switch (data[0])
                {
                    case "protocolPath":
                        protocolPath = data[1];
                        break;
                }
            }

            var setting = ParseSetting(protocolPath);
            foreach (var protocol in ParseProtocol(protocolPath))
            {
                foreach (var output in setting.outputs)
                {
                    var fn = Path.Combine(output.path, $"{protocol.name}.{GetExtension(output.type)}");
                    var temp = Path.Combine(setting.templatePath, output.type.ToString());
                    temp = Path.Combine(temp, "Protocol");

                    File.WriteAllText(fn, GenProtocol(temp, protocol, output.type));
                }
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

    static List<Protocol> ParseProtocol(string fn)
    {
        var res = new List<Protocol>();

        foreach (var str in File.ReadAllLines(fn))
        {
            if (str.StartsWith("//")) continue;
            if (string.IsNullOrEmpty(str.Trim())) continue;

            var data = str.Split(',');
            switch (data[0])
            {
                case "protocol":
                    res.Add(new Protocol { name = data[1] });
                    break;
                case "up":
                    {
                        var type = data[1];
                        var name = data[2];
                        var comment = (data.Length >= 4) ? data[3] : "";
                        res.Last().up.Add(new Param { type = type, name = name, comment = comment });
                    }
                    break;
                case "down":
                    {
                        var type = data[1];
                        var name = data[2];
                        var comment = (data.Length >= 4) ? data[3] : "";
                        res.Last().down.Add(new Param { type = type, name = name, comment = comment });
                    }
                    break;
            }
        }
        return res;
    }

    static string GenProtocol(string fn, Protocol protocol, Type type)
    {
        var temp = File.ReadAllText(fn);

        temp = temp.Replace("{{PROTOCOL}}", protocol.name);
        temp = Regex.Replace(temp, @"^(\s*){{UP}}", match =>
        {
            var indent = match.Groups[1].ToString();
            var param = protocol.up.Select(p => $"{indent}{p.ToString(type)}");
            return string.Join("\n", param);
        }, RegexOptions.Multiline);
        temp = Regex.Replace(temp, @"^(\s*){{DOWN}}", match =>
        {
            var indent = match.Groups[1].ToString();
            var param = protocol.down.Select(p => $"{indent}{p.ToString(type)}");
            return string.Join("\n", param);
        }, RegexOptions.Multiline);
        return temp;
    }

    static string GetExtension(Type type)
    {
        switch (type)
        {
            case Type.CSharp: return "cs";
            case Type.TypeScript: return "ts";
            default:return "";
        }
    }
}


