using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;
using System;
using Util.Time;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CsvHelper;
using System.IO;
using System.Text;

public class Boot : MonoBehaviour
{
    public InputField input;
    public Button btn;

    public class Foo
    {
        public string name = "FOO!!!";
    }
    void Start()
    {
        KiiInitialize.Init();

        // KiiCloudの認証
        Auth.Authentication((user) =>
        {
            if (user != null)
            {
            }
        });

        // CSV の 検証
        var str = Resources.Load<TextAsset>("Entities/familiar").text.Trim();
        using (var csv = new CsvHelper.CsvReader(new StringReader(str)))
        {
            csv.Configuration.HeaderValidated = null;
            csv.Configuration.RegisterClassMap<Entities.FamiliarMap>();
            var records = csv.GetRecords<Entities.Familiar>().ToArray();
        }
    }

    public void OnCreate()
    {
    }

    public void Update()
    {
        //Debug.Log(UnixTime.FromUnixTime(ServerTime.CurrentUnixTime));
    }
}

