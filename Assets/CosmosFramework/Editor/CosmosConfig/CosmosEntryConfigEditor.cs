﻿using UnityEditor;
using Cosmos.Resource;
using Cosmos;

namespace CosmosEditor
{
    [CustomEditor(typeof(CosmosEntryConfig), true)]
    public class CosmosEntryConfigEditor : UnityEditor.Editor
    {
        SerializedObject targetObject;
        CosmosEntryConfig cosmosConfig;
        SerializedProperty sp_LoadDefaultHelper, sp_LaunchAppDomainModules, sp_PrintModulePreparatory,
           sp_ResourceLoadMode, sp_QuarkAssetLoadMode, sp_QuarkRemoteUrl, sp_QuarkLocalUrl;
        public override void OnInspectorGUI()
        {
            targetObject.Update();
            sp_LoadDefaultHelper.boolValue = EditorGUILayout.Toggle("LoadDefaultHelper", sp_LoadDefaultHelper.boolValue);
            sp_LaunchAppDomainModules.boolValue = EditorGUILayout.Toggle("LaunchAppDomainModules", sp_LaunchAppDomainModules.boolValue);
            if (sp_LaunchAppDomainModules.boolValue)
            {
                sp_PrintModulePreparatory.boolValue = EditorGUILayout.Toggle("PrintModulePreparatory", sp_PrintModulePreparatory.boolValue);
            }
            sp_ResourceLoadMode.enumValueIndex = (byte)(ResourceLoadMode)EditorGUILayout.EnumPopup("ResourceLoadMode", (ResourceLoadMode)sp_ResourceLoadMode.enumValueIndex);
            switch ((ResourceLoadMode)sp_ResourceLoadMode.enumValueIndex)
            {
                //case ResourceLoadMode.QuarkAsset:
                //    {
                //        sp_QuarkAssetLoadMode.enumValueIndex = (byte)(QuarkAssetLoadMode)EditorGUILayout.EnumPopup("QuarkAssetLoadMode", (QuarkAssetLoadMode)sp_QuarkAssetLoadMode.enumValueIndex);
                //        switch ((QuarkAssetLoadMode)sp_QuarkAssetLoadMode.enumValueIndex)
                //        {
                //            case QuarkAssetLoadMode.BuiltAssetBundle:
                //                {
                //                    sp_QuarkRemoteUrl.stringValue = EditorGUILayout.TextField("QuarkRemoteUrl", sp_QuarkRemoteUrl.stringValue);
                //                    sp_QuarkLocalUrl.stringValue = EditorGUILayout.TextField("QuarkLocalUrl", sp_QuarkLocalUrl.stringValue);
                //                }
                //                break;
                //        }
                //    }
                //    break;
            }
            targetObject.ApplyModifiedProperties();
        }
        private void OnEnable()
        {
            cosmosConfig = target as CosmosEntryConfig;
            targetObject = new SerializedObject(cosmosConfig);
            sp_LoadDefaultHelper = targetObject.FindProperty("LoadDefaultHelper");
            sp_LaunchAppDomainModules = targetObject.FindProperty("LaunchAppDomainModules");
            sp_PrintModulePreparatory = targetObject.FindProperty("PrintModulePreparatory");
            sp_ResourceLoadMode = targetObject.FindProperty("ResourceLoadMode");
            //sp_QuarkAssetLoadMode = targetObject.FindProperty("QuarkAssetLoadMode");
            sp_QuarkRemoteUrl = targetObject.FindProperty("QuarkRemoteUrl");
            sp_QuarkLocalUrl = targetObject.FindProperty("QuarkLocalUrl");
        }
    }
}