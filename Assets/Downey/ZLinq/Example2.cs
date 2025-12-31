using System;
using UnityEngine;
using UnityEngine.UIElements;
using ZLinq;

namespace Downey.ZLinq
{
    public class Example2 : MonoBehaviour
    {
        public VisualTreeAsset uiAsset;

        private void OnEnable()
        {
            var root = uiAsset.CloneTree();
            var document = GetComponent<UIDocument>();
            document.rootVisualElement.Add(root);

            var buttons = new VisualElementTraverser(root)
                .Descendants<VisualElementTraverser, VisualElement>()
                .OfType<Button>();

            foreach (var button in buttons)
            {
                Debug.Log($"Hooking up:{ button.name }");

                switch (button.name)
                {
                    case "PlayButton":
                        button.clicked += ()=> Debug.Log("Play");
                        break;
                    case "SettingsButton":
                        button.clicked += () => Debug.Log("Settings");
                        break;
                    case "QuitButton":
                        button.clicked += () => Debug.Log("Quit");
                        break;
                }
            }
        }
    }
}