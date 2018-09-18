using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Window : MonoBehaviour
    {
        public enum Layer
        {
            Normal = 1000,
            Dialog = 2000,
            Debug = 9000,
        }
        public Layer layer;

        void Start()
        {
            foreach (var btn in GetComponentsInChildren<Button>(true))
            {
                btn.onClick.AddListener(() => OnButtonClick(btn));
            }
            OnStart();
        }
        protected virtual void OnStart()
        {

        }

        public virtual void OnButtonClick(Button btn)
        {
            switch (btn.name)
            {
                case "CloseButton":
                    Close();
                    break;
            }
        }

        public virtual void OnOpen(params object[] args)
        {
        }

        public virtual void OnClose()
        {
        }

        static Dictionary<Layer, List<Window>> windows = new Dictionary<Layer, List<Window>>();

        public static T Open<T>() where T : Window
        {
            var name = typeof(T).Name;
            var go = Instantiate(Resources.Load<GameObject>($"UI/{name.Replace("Window", "")}/{name}"));
            var res = go.GetComponent<T>();
            res.OnOpen();

            List<Window> list;
            if (!windows.TryGetValue(res.layer, out list))
            {
                list = new List<Window>();
                windows.Add(res.layer, list);
            }
            go.GetComponent<Canvas>().sortingOrder = (int)res.layer + list.Count;

            return res;
        }

        public static T Open<T>(string fn, params object[] args) where T : Window
        {
            var go = Instantiate(Resources.Load<GameObject>(fn));
            var res = go.GetComponent<T>();
            res.OnOpen();

            List<Window> list;
            if (!windows.TryGetValue(res.layer, out list))
            {
                list = new List<Window>();
                windows.Add(res.layer, list);
            }
            go.GetComponent<Canvas>().sortingOrder = (int)res.layer + list.Count;

            return res;
        }

        public void Close()
        {
            OnClose();
            windows[layer].Remove(this);
            Destroy(this.gameObject);
        }
    }
}
