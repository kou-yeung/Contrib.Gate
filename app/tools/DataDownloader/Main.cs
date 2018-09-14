using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ComponentModel;
using System.Globalization;
using Util;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

class DataDownloader
{
    [DataContract]
    public class Sheet
    {
        [DataMember(Name = "range")]
        public string Range { get; set; }

        [DataMember(Name = "majorDimension")]
        public string MajorDimension { get; set; }

        [DataMember(Name = "values")]
        public List<List<string>> Values { get; set; }
    }

    static readonly string urlFormat = @"https://sheets.googleapis.com/v4/spreadsheets/{0}/values/{1}?key={2}";

    class Setting
    {
        // spreadsheets の設定
        public string SheetId;      // シートID
        public string ApiKey;       // APIキー
        public string SheetName;    // 対象シート名

        // 出力設定
        public int FlagLine;        // 出力フラグの行(空白は無視する)
        public int DataStart;       // データ開始の行（この行から最後まで読み込む
        public string Output;       // 出力パス(ファイル名まで)

        // 暗号化設定
        public string crypt_iv;
        public string crypt_key;

    }
    static void Main()
    {
        if (Environment.GetCommandLineArgs().Length < 2)
        {
            // sheetid=<id> apikey=<key> sheetname=<name> flagLine=<index> dataStart=<index> output=<path>
            Console.WriteLine(@"sheetid=<id> apikey=<key> sheetname=<name> flagLine=<index> dataStart=<index> output=<path>");
            return;
        }

        var setting = new Setting();
        foreach (var arg in Environment.GetCommandLineArgs())
        {
            var match = Regex.Match(arg, @"(\w+)=(.+)");
            if (match == Match.Empty) continue;

            switch (match.Groups[1].ToString())
            {
                case "sheetid":
                    setting.SheetId = match.Groups[2].ToString();
                    break;
                case "apikey":
                    setting.ApiKey = match.Groups[2].ToString();
                    break;
                case "sheetname":
                    setting.SheetName = match.Groups[2].ToString();
                    break;
                case "flagLine":
                    setting.FlagLine = int.Parse(match.Groups[2].ToString()) - 1;
                    break;
                case "dataStart":
                    setting.DataStart = int.Parse(match.Groups[2].ToString()) - 1;
                    break;
                case "output":
                    setting.Output = match.Groups[2].ToString();
                    break;
                case "crypt_iv":
                    setting.crypt_iv = match.Groups[2].ToString();
                    break;
                case "crypt_key":
                    setting.crypt_key = match.Groups[2].ToString();
                    break;
            }
            Console.WriteLine(arg);
        }

        // 変換する配列型を登録します
        Converter.RegisterArray<string>();
        Converter.RegisterArray<int>();
        Converter.RegisterArray<float>();

        var url = string.Format(urlFormat, setting.SheetId, setting.SheetName, setting.ApiKey);
        var sheet = Download<Sheet>(url);

        // 出力パス
        string path = setting.Output;

        // ファイル名指定されてない場合、<シート名>.csv で保存する
        var fn = Path.GetFileName(path);
        if (string.IsNullOrEmpty(fn))
        {
            path = Path.Combine(path, $"{setting.SheetName}.csv");
        }

        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        File.WriteAllText(path, Encrypt(Parse(sheet, setting), setting.crypt_iv, setting.crypt_key));
    }

    static string Parse(Sheet sheet, Setting setting)
    {
        // 出力フラグ行を指定 1
        int flagIndex = setting.FlagLine;
        // データ開始行を指定
        int dataIndex = setting.DataStart;

        // 有効データの番号を取得
        List<int> columns = new List<int>();
        List<Tuple<Type, string>> types = new List<Tuple<Type, string>>();

        for (int i = 0; i < sheet.Values[flagIndex].Count; i++)
        {
            if (string.IsNullOrEmpty(sheet.Values[flagIndex][i])) continue;
            columns.Add(i);
        }

        var sb = new StringBuilder();
        // データ開始行から最後まで読み込む
        for (int i = dataIndex; i < sheet.Values.Count; i++)
        {
            var data = sheet.Values[i];
            if (data[0].StartsWith("#")) continue;
            foreach (var column in columns)
            {
                sb.Append((column < data.Count) ? data[column] : "");
                sb.Append(",");
            }
            sb.AppendLine();
        }
        return sb.ToString();

    }
    private static T Download<T>(string url)
    {
        var client = new HttpClient();
        var str = client.GetStringAsync(url).Result;
        return Deserialize<T>(str);
    }

    /// <summary>
    /// Jsonメッセージをオブジェクトへデシリアライズします。
    /// </summary>
    private static T Deserialize<T>(string json)
    {
        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            return (T)serializer.ReadObject(stream);
        }
    }

    private static string Encrypt(string plainText, string iv, string key)
    {
        if (string.IsNullOrEmpty(iv) || string.IsNullOrEmpty(key)) return plainText;

        using (var manager = new AesManaged())
        {
            manager.Key = Convert.FromBase64String(key);
            manager.IV = Convert.FromBase64String(iv);
            var encryptor = manager.CreateEncryptor(manager.Key, manager.IV);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
    }

}

