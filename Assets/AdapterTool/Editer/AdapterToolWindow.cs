using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ZQ.AdapterTool
{
    public class AdapterToolWindow : EditorWindow
    {
        public static Image image = null;

        [MenuItem("Tool/AdapterTool/一键生成到桌面", false, 0)]
        private static void Onekey()
        {
            InitImage();
            image.StartCoroutine(AutoCaputure());
        }

        private static void InitImage()
        {
            if (image != null)
            {
                return;
            }

            GameObject canvasGameObject = new GameObject("Canvas");
            Canvas canvas = canvasGameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler canvasScaler = canvasGameObject.AddComponent<CanvasScaler>();

            GameObject imageGameObject = new GameObject("Image");
            imageGameObject.transform.parent = canvasGameObject.transform;
            imageGameObject.transform.localPosition = Vector3.zero;

            image = imageGameObject.AddComponent<Image>();
        }

        private static void DestroyImage()
        {
            if (image != null && image.transform.parent != null)
            {
                Destroy(image.transform.parent.gameObject);
            }
        }

        public static IEnumerator AutoCaputure()
        {
            Dictionary<string, List<Device>> dic = Config.GetCaputureDevice();

            foreach (var item in dic)
            {
                List<Device> allDevice = item.Value;

                for (int i = 0; i < allDevice.Count; i++)
                {
                    Device device = allDevice[i];

                    SetGameWindowSize(device, Config.DefaultScreenOrientation);

                    yield return new WaitForEndOfFrame();

                    SetDeviceMask(device);

                    yield return new WaitForEndOfFrame();

                    Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

                    screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                    screenShot.Apply();

                    byte[] bytes = screenShot.EncodeToPNG();

                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory).Replace(@"\", "/");

                    string outputPath = desktopPath + "/截图/" + item.Key + "/";

                    if (!System.IO.Directory.Exists(outputPath))
                    {
                        System.IO.Directory.CreateDirectory(outputPath);
                    }

                    string filename = outputPath + "/" + device.GetName(ScreenOrientation.landscape_L) + ".png";
                    System.IO.File.WriteAllBytes(filename, bytes);
                    Debug.Log(string.Format("截屏了一张图片: {0}", filename));
                }
            }

            SetGameWindowSize(Config.DefaultDevice, Config.DefaultScreenOrientation);

            Debug.LogError("完成");
        }

        public static void SetGameWindowSize(Device device, ScreenOrientation screenOrientation)
        {
            switch (screenOrientation)
            {
                case ScreenOrientation.Portrait:
                    {
                        SetGameWindowSize(device.resolution, device.GetName(screenOrientation));
                    }
                    break;
                case ScreenOrientation.landscape_L:
                case ScreenOrientation.landscape_R:
                    {
                        SetGameWindowSize(new Vector2(device.resolution.y, device.resolution.x), device.GetName(screenOrientation));
                    }
                    break;
            }
        }

        public static void SetGameWindowSize(Vector2 resolution, string text)
        {
            SetGameWindowSize((int)resolution.x, (int)resolution.y, text);
        }

        public static void SetGameWindowSize(int width, int height, string text)
        {
            Type gameViewType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
            EditorWindow window = GetWindow(gameViewType);

            MethodInfo get_currentGameViewSize = gameViewType.GetMethod("get_currentGameViewSize", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            object gameViewSize = get_currentGameViewSize.Invoke(window, new object[] { });
            Type gameViewSizeType = gameViewSize.GetType();

            MethodInfo widthMethodInfo = gameViewSizeType.GetMethod("set_width", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            widthMethodInfo.Invoke(gameViewSize, new object[] { width });

            MethodInfo heightMethodInfo = gameViewSizeType.GetMethod("set_height", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            heightMethodInfo.Invoke(gameViewSize, new object[] { height });

            MethodInfo baseTextMethodInfo = gameViewSizeType.GetMethod("set_baseText", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            baseTextMethodInfo.Invoke(gameViewSize, new object[] { text });
        }

        public static void SetDeviceMask(Device deviceInfo)
        {
            if (deviceInfo == null)
            {
                return;
            }

            if (image == null)
            {
                InitImage();
            }

            string name = string.Format("{0} {1}_{2}.png", deviceInfo.brand, deviceInfo.model, (int)Config.DefaultScreenOrientation);

            Texture2D texture2D = EditorGUIUtility.Load(name) as Texture2D;

            if (texture2D == null)
            {
                image.enabled = false;
                return;
            }
            image.enabled = true;
            image.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
            image.SetNativeSize();
        }

        private void OnDestroy()
        {
            DestroyImage();
        }
    }

}
