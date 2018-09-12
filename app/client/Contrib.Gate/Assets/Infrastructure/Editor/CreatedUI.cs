using UnityEditor.UI;
using UnityEngine.UI;
using UnityEditor;

[InitializeOnLoad]
public static class CreatedUI
{
    static CreatedUI()
    {
        MenuOptions.OnCreatedButton += OnCreatedButton;
    }

    static void OnCreatedButton(Button btn)
    {
        //btn.gameObject.AddComponent<ButtonSE>();
    }
}