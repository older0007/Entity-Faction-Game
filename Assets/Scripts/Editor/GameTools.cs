using System.IO;
using GamePlay.Actions;
using GamePlay.Actions.Effects;
using GamePlay.Fractions;
using GamePlay.Units;
using UnityEditor;
using UnityEngine;
using World;

namespace Editor
{
   public static class GameTools
{
    const string FractionRoot = "Assets/Data/Fraction";
    const string EffectsRoot = "Assets/Data/Effects";
    const string ActionRoot = "Assets/Data/ActionConfigs";
    const string TickRoot = "Assets/Data/TickConfigs";
    [MenuItem("GameTool/Create New Fraction")]
    static void CreateNewFraction()
    {
        EnsureFolder(FractionRoot);
        var folderName = UniqueFolderName(FractionRoot, "NewFraction");
        AssetDatabase.CreateFolder(FractionRoot, folderName);
        var folder = $"{FractionRoot}/{folderName}";
        var fraction = CreateAsset<FractionConfig>($"{folder}/{folderName}.asset");
        SetString(fraction, "fractionName", folderName);
        for (var i = 1; i <= 3; i++)
        {
            var unit = CreateAsset<UnitConfig>($"{folder}/{folderName}_Unit_{i}.asset");
            SetString(unit, "unitName", $"{folderName}_Unit_{i}");
            SetObject(unit, "fractionConfig", fraction);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorGUIUtility.PingObject(fraction);
    }
    [MenuItem("GameTool/Update Effects")]
    static void UpdateEffects()
    {
        EnsureFolder(EffectsRoot);
        foreach (var type in TypeCache.GetTypesDerivedFrom<BaseEffect>())
        {
            if (type.IsAbstract) continue;
            var path = $"{EffectsRoot}/{type.Name}.asset";
            if (AssetDatabase.LoadAssetAtPath<Object>(path) != null)
                continue;
            var asset = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(asset, path);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    [MenuItem("GameTool/Create/ActionConfig")]
    static void CreateActionConfig() => CreateUnique<ActionConfig>(ActionRoot, "ActionConfig");
    [MenuItem("GameTool/Create/TickConfig")]
    static void CreateTickConfig() => CreateUnique<TickConfig>(TickRoot, "TickConfig");
    static void CreateUnique<T>(string folder, string fileName) where T : ScriptableObject
    {
        EnsureFolder(folder);
        var path = AssetDatabase.GenerateUniqueAssetPath($"{folder}/{fileName}.asset");
        var asset = CreateAsset<T>(path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorGUIUtility.PingObject(asset);
    }
    static T CreateAsset<T>(string path) where T : ScriptableObject
    {
        var asset = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(asset, path);
        return asset;
    }
    static string UniqueFolderName(string parent, string name)
    {
        var candidate = name;
        var i = 1;
        while (AssetDatabase.IsValidFolder($"{parent}/{candidate}"))
            candidate = $"{name}_{i++}";
        return candidate;
    }
    static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path)) return;
        var parent = Path.GetDirectoryName(path)?.Replace("\\", "/");
        var leaf = Path.GetFileName(path);
        if (string.IsNullOrEmpty(parent) || parent == path) return;
        EnsureFolder(parent);
        AssetDatabase.CreateFolder(parent, leaf);
    }
    static void SetString(Object asset, string field, string value)
    {
        var so = new SerializedObject(asset);
        so.FindProperty(field).stringValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
    static void SetObject(Object asset, string field, Object value)
    {
        var so = new SerializedObject(asset);
        so.FindProperty(field).objectReferenceValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
}
}