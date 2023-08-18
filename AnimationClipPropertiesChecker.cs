#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PogapogaEditor.RightClick
{
    /// <summary>
    /// AnimationClipに設定されたPath、Type、PropertyNameを確認するためのツールです。
    /// スクリプトからAnimationClipを設定する場合に、
    /// clip.SetCurveに必要な引数の一部をコピーしてペーストすることができます。
    /// </summary>
    public class AnimationClipPropertiesChecker : EditorWindow
    {
        private static AnimationClip animationClip;
        private Vector2 scrollPosition = Vector2.zero;

        [MenuItem("Assets/PogapogaTools/AnimationClipのProperty名を確認")]

        static void Init()
        {
            animationClip = Selection.objects[0] as AnimationClip;

            AnimationClipPropertiesChecker window = (AnimationClipPropertiesChecker)EditorWindow.GetWindow(typeof(AnimationClipPropertiesChecker));
            window.titleContent = new GUIContent("AnimationClipPropertiesChecker");

            window.Show();
        }


        private void OnGUI()
        {
            #region // LayoutOption
            List<GUILayoutOption> gUILayoutOptions = new List<GUILayoutOption>();
            gUILayoutOptions.Add(GUILayout.Width(100));
            List<GUILayoutOption> gUILayoutOptionsButton = new List<GUILayoutOption>();
            gUILayoutOptionsButton.Add(GUILayout.Width(30));
            #endregion

            // AnimationClipの選択
            animationClip = EditorGUILayout.ObjectField("AnimationClip", animationClip, typeof(AnimationClip), false) as AnimationClip;
            if (animationClip == null) { return; } // 未選択時に処理中断

            #region // AnimationClipのPropertiesの取得
            EditorCurveBinding[] editorCurveBindings = AnimationUtility.GetCurveBindings(animationClip);
            int bindingsCount = editorCurveBindings.Length;

            string[] hierarchyPaths = new string[bindingsCount];
            string[] propertyNames = new string[bindingsCount];
            string[] types = new string[bindingsCount];
            for (int ei = 0; ei < bindingsCount; ei++)
            {
                hierarchyPaths[ei] = editorCurveBindings[ei].path;
                propertyNames[ei] = editorCurveBindings[ei].propertyName;
                types[ei] = editorCurveBindings[ei].type.Name;
            }
            #endregion

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            #region // 表示
            for (int ei = 0; ei < bindingsCount; ei++)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    // コピー用ボタン
                    using (new EditorGUILayout.VerticalScope(gUILayoutOptions.ToArray()))
                    {
                        if (ei == 0)
                        {
                            GUILayout.Label("Copy");
                        }
                        if (GUILayout.Button("Copy"))
                        {
                            // コピー処理
                            string copyText = $"\"{hierarchyPaths[ei]}\", typeof({types[ei]}), \"{propertyNames[ei]}\"";
                            GUIUtility.systemCopyBuffer = copyText;
                        }
                    }
                    // Path
                    using (new EditorGUILayout.VerticalScope())
                    {
                        if (ei == 0)
                        {
                            GUILayout.Label("Path");
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            if (GUILayout.Button("C", gUILayoutOptionsButton.ToArray()))
                            {
                                // コピー処理
                                string copyText = $"\"{hierarchyPaths[ei]}\"";
                                GUIUtility.systemCopyBuffer = copyText;
                            }
                            EditorGUILayout.TextField($"{hierarchyPaths[ei]}");
                        }
                    }
                    // Type
                    using (new EditorGUILayout.VerticalScope())
                    {
                        if (ei == 0)
                        {
                            GUILayout.Label("Type");
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            if (GUILayout.Button("C", gUILayoutOptionsButton.ToArray()))
                            {
                                // コピー処理
                                string copyText = $"typeof({types[ei]})";
                                GUIUtility.systemCopyBuffer = copyText;
                            }
                            EditorGUILayout.TextField($"{types[ei]}");
                        }
                    }
                    // PropertyName
                    using (new EditorGUILayout.VerticalScope())
                    {
                        if (ei == 0)
                        {
                            GUILayout.Label("PropertyName");
                        }
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            if (GUILayout.Button("C", gUILayoutOptionsButton.ToArray()))
                            {
                                // コピー処理
                                string copyText = $"{propertyNames[ei]}";
                                GUIUtility.systemCopyBuffer = copyText;
                            }
                            EditorGUILayout.TextField($"{propertyNames[ei]}");
                        }
                    }
                }
            }
            #endregion
            EditorGUILayout.EndScrollView();
        }
    }
}
#endif