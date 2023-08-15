// Copyright 2018-2020 tripolygon Inc. All Rights Reserved.

#define UMODELER_LITE

using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using System;
using System.Linq;
#if !UMODELER_LITE
using tripolygon.UModeler;
#else
using tripolygon.UModelerLite;
#endif

namespace TPUModelerLiteEditor
{
    [CustomEditor(typeof(MeshFilter), isFallback = true)]
    [UnityEditor.InitializeOnLoad]
    public class MeshFilterEditor : Editor
    {
        private static readonly Dictionary<Type, MethodInfo> methodByType;
        static MeshFilterEditor()
        {
            var query = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                        from type in assembly.GetTypes()
                        where type.Name == "MeshFilterEditor"
                        let methodInfo = type.GetMethod(nameof(OnMeshFilterGUI), BindingFlags.Static | BindingFlags.Public)
                        let parameters = methodInfo.GetParameters()
                        where parameters.Length > 0 && parameters[0].ParameterType.Name == nameof(UModeler)
                        select new { Type = parameters[0].ParameterType, Method = methodInfo };
            methodByType = query.ToDictionary(item => item.Type, item => item.Method);
        }
        bool foldedOut = true;
        MethodInfo methodInfo;

        public void OnEnable()
        {

        }

        public void OnDisable()
        {
            methodInfo = null;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            MeshFilter mf = (MeshFilter)target;
            var modelerComponent = mf.GetComponents(typeof(Component)).FirstOrDefault(item => item != null && item.GetType().Name == nameof(UModeler));
            if (methodInfo == null)
            {
                if (modelerComponent != null && methodByType.ContainsKey(modelerComponent.GetType()) == true)
                {
                    methodInfo = methodByType[modelerComponent.GetType()];
                }
            }

            if (methodInfo != null)
            {
                methodInfo.Invoke(null, new object[] { modelerComponent, foldedOut });
            }
        }

        public static void OnMeshFilterGUI(UModeler modeler, bool foldedOut)
        {
            if (modeler != null && !EditorUtil.IsPrefabOnProject(modeler.gameObject))
            {
                var mf = modeler.GetComponent<MeshFilter>();
                foldedOut = EditorUtil.Foldout(foldedOut, "UModeler Light .asset");
                Mesh mesh = modeler.renderableMeshFilter != null ? modeler.renderableMeshFilter.sharedMesh : null;
                if (foldedOut && mesh != null)
                {
                    if (modeler.IsAssetPathValid())
                        GUILayout.Label("File name : " + modeler.AssetFileName);
                    else
                        GUILayout.Label(".Asset file of this mesh doesn't exist yet.");

                    GUI.changed = false;

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Save"))
                    {
                        string path = modeler.IsAssetPathValid() ? modeler.assetPath : SystemUtil.MeshAssetFolder + "/" + mesh.name + ".asset";
                        if (SystemUtil.SaveMeshAsset(modeler, path))
                        {
                            EditorUtility.SetDirty(mf);
                        }
                    }
                    else if (GUILayout.Button("Save As"))
                    {
                        if (SystemUtil.SaveMeshAsset(modeler))
                        {
                            EditorUtility.SetDirty(mf);
                        }
                    }
                    else
                    {
                        GUILayout.EndHorizontal();
                    }

                    if (GUI.changed)
                    {
                        if (modeler.IsAssetPathValid())
                        {
                            modeler.renderableMeshFilter.sharedMesh.name = modeler.MeshName;
                            EditorMode.commentaryViewer.AddTitle("The mesh has been saved as " + modeler.AssetFileName);
                        }
                    }
                }
            }
        }
    }
}