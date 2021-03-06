﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Entities;

namespace Network
{
    public class Communication
    {
        public string Command { get { return command; } }

        [SerializeField]
        private string command;
        [SerializeField]
        private List<string> data = new List<string>();

        public Communication(string command)
        {
            this.command = command;
        }

        /// <summary>
        /// 送信パラメータを PUSH する
        /// </summary>
        public Communication Push<T>(T obj)
        {
            data.Add(JsonUtility.ToJson(obj));
            return this;
        }

        /// <summary>
        /// 受信パラメータを POP する
        /// </summary>
        public T Pop<T>()
        {
            var res = JsonUtility.FromJson<T>(data[0]);
            data.RemoveAt(0);
            return res;
        }


        /// <summary>
        /// 送信する
        /// </summary>
        /// <param name="cb"></param>
        public void Send(Action<ErrorCode, string> cb)
        {
            new Requests().Send(command, JsonUtility.ToJson(this), cb);
        }
    }
}
