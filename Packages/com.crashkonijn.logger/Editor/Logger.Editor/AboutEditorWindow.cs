using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Logger.Editor
{
    public class AboutEditorWindow : EditorWindow
    {
        private const int Version = 1;

        private static string applicationKey = "b6a6c0f8-1bc4-46a3-b5d4-e43a6fd9d6dc";

        private static string loggerVersion = "loading";
        private static string newtonsoftVersion = "loading";
        private static string websocketVersion = "loading";

        private static ListRequest request;
        private static AboutEditorWindow instance;

        [MenuItem("Tools/Logger/About")]
        private static void ShowWindow()
        {
            var window = GetWindow<AboutEditorWindow>();
            window.titleContent = new GUIContent("Rabbit Logger (About)");
            window.Show();

            instance = window;

            CheckProgress();

            EditorApplication.update += CheckProgress;
        }

        private void OnFocus()
        {
            instance = GetWindow<AboutEditorWindow>();

            CheckProgress();
            this.Render();
        }

        private void Render()
        {
            this.rootVisualElement.Clear();

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{LoggerEditorSettings.BasePath}/Styles/Generic.uss");
            this.rootVisualElement.styleSheets.Add(styleSheet);

            styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{LoggerEditorSettings.BasePath}/Styles/About.uss");
            this.rootVisualElement.styleSheets.Add(styleSheet);

            // Add image
            this.rootVisualElement.Add(new Image
            {
                image = AssetDatabase.LoadAssetAtPath<Texture2D>(LoggerEditorSettings.BasePath + "/Textures/card_image_logger.png"),
            });

            var scrollView = new ScrollView();
            scrollView.verticalScrollerVisibility = ScrollerVisibility.Auto;

            scrollView.Add(new Card((card) =>
            {
                card.Add(new Label(this.GetText())
                {
                    style =
                    {
                        whiteSpace = WhiteSpace.Normal,
                        overflow = Overflow.Hidden,
                    },
                });
            }));

            scrollView.Add(new Card((card) =>
            {
                card.Add(new Label($"Application Key: {applicationKey}"));

                card.Add(new Button(this.CopyApplicationKey)
                {
                    text = "Copy Application Key to Clipboard",
                });
            }));

            scrollView.Add(new Card((card) =>
            {
                card.Add(new Label(this.GetDebugText()));

                this.AddButtons(card);
            }));

            scrollView.Add(new Card((card) =>
            {
                card.Add(new Label("Links"));

                this.AddLink(card, "Documentation", "https://logger.crashkonijn.com");
                this.AddLink(card, "Discord", "https://discord.gg/dCPnHaYNrm");
                this.AddLink(card, "Asset Store", "https://assetstore.unity.com/packages/slug/305396");
            }));

            this.rootVisualElement.Add(scrollView);
        }

        private void AddLink(VisualElement parent, string text, string url)
        {
            parent.Add(new Button(() =>
            {
                Application.OpenURL(url);
            })
            {
                text = text,
            });
        }

        private void AddButtons(Card card)
        {
            if (loggerVersion == "loading" || newtonsoftVersion == "loading" || websocketVersion == "loading")
            {
                card.Add(new Button(CheckProgress)
                {
                    text = "Refresh",
                });

                return;
            }

            card.Add(new Button(this.CopyDebug)
            {
                text = "Copy debug to Clipboard",
            });
        }

        private string GetDebugText()
        {
            return @$"Logger Version:                  {loggerVersion}
Unity Version:                   {Application.unityVersion}
NewtonSoft Version:         {newtonsoftVersion}
Websocket Version:         {websocketVersion}";
        }

        private string GetText()
        {
            return $@"Thank you for trying out my Logger package!

If you have any questions or need help, please don't hesitate to contact us on Discord or check out the documentation.

Please consider leaving a review on the Asset Store if you like it! 

I hope you enjoy using the Logger!";
        }

        public void CopyDebug()
        {
            EditorGUIUtility.systemCopyBuffer = this.GetDebugText();
        }

        public void CopyApplicationKey()
        {
            EditorGUIUtility.systemCopyBuffer = applicationKey;
        }

        private static void CheckProgress()
        {
            if (request == null)
                request = Client.List();

            if (!request.IsCompleted)
                return;

            EditorApplication.update -= CheckProgress;

            loggerVersion = LoggerEditorSettings.Version;
            newtonsoftVersion = request.Result.Where(x => x.name == "com.unity.nuget.newtonsoft-json").Select(x => x.version).FirstOrDefault();
            websocketVersion = request.Result.Where(x => x.name == "com.endel.nativewebsocket").Select(x => x.version).FirstOrDefault();

            instance.Render();
        }
    }
}
