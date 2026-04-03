using TMPro;
using UnityEngine.TextCore.LowLevel;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;

[InitializeOnLoad]
public static class OrbitronFontBootstrap
{
    private const int AtlasSize = 1024;
    private const int Padding = 9;
    private const string RegularFontPath = "Assets/Fonts/Orbitron-Regular.ttf";
    private const string BoldFontPath = "Assets/Fonts/Orbitron-Bold.ttf";
    private const string RegularSdfPath = "Assets/Fonts/Orbitron-Regular SDF.asset";
    private const string BoldSdfPath = "Assets/Fonts/Orbitron-Bold SDF.asset";

    static OrbitronFontBootstrap()
    {
        EditorApplication.delayCall += Ensure;
    }

    [MenuItem("Orbit Escape/Setup Orbitron TMP Fonts")]
    public static void MenuEnsure()
    {
        Debug.Log("Orbit Escape: MenuEnsure START");
        try { Ensure(); }
        catch (System.Exception ex) { Debug.LogError("Orbit Escape: " + ex); }
        Debug.Log("Orbit Escape: MenuEnsure END");
    }

    [MenuItem("Orbit Escape/Fix TMP Fonts In Scene")]
    private static void MenuFix() => AssignFonts(forceReassign: true);

    private static void Ensure()
    {
        if (AssetDatabase.LoadAssetAtPath<Font>(RegularFontPath) == null ||
            AssetDatabase.LoadAssetAtPath<Font>(BoldFontPath) == null)
        {
            Debug.LogWarning("Orbit Escape: TTF files not found. Import them first.");
            return;
        }

        bool rebuilt = false;
        rebuilt |= EnsureFont(RegularFontPath, RegularSdfPath, "Orbitron-Regular SDF");
        rebuilt |= EnsureFont(BoldFontPath, BoldSdfPath, "Orbitron-Bold SDF");

        var regularSdf = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(RegularSdfPath);
        if (regularSdf != null && TMP_Settings.defaultFontAsset != regularSdf)
        {
            TMP_Settings.defaultFontAsset = regularSdf;
            EditorUtility.SetDirty(TMP_Settings.GetSettings());
        }

        if (rebuilt)
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            AssignFonts(forceReassign: false);
            Debug.Log("Orbit Escape: fonts rebuilt. Run 'Fix TMP Fonts In Scene' if text is still invisible.");
        }
    }

    private static bool EnsureFont(string fontPath, string assetPath, string assetName)
    {
        var existing = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(assetPath);

        // Force rebuild: delete only the .asset file (keep .meta to preserve GUID)
        if (existing != null && !HasValidAtlas(existing))
        {
            string fullPath = Path.GetFullPath(assetPath);
            File.Delete(fullPath);
            AssetDatabase.Refresh();
            existing = null;
            Debug.Log($"Orbit Escape: deleted broken '{assetName}', will recreate.");
        }

        if (existing != null) return false;

        var sourceFont = AssetDatabase.LoadAssetAtPath<Font>(fontPath);
        if (sourceFont == null)
        {
            Debug.LogError($"Orbit Escape: cannot load font at {fontPath}");
            return false;
        }

        var fa = TMP_FontAsset.CreateFontAsset(
            sourceFont, 90, Padding, GlyphRenderMode.SDFAA,
            AtlasSize, AtlasSize, AtlasPopulationMode.Dynamic);

        fa.name = assetName;
        fa.TryAddCharacters(BuildCharSet(), out _);
        fa.ReadFontAssetDefinition();

        AssetDatabase.CreateAsset(fa, assetPath);
        if (fa.material != null)
        {
            fa.material.name = assetName + " Material";
            AssetDatabase.AddObjectToAsset(fa.material, assetPath);
        }
        if (fa.atlasTexture != null)
        {
            fa.atlasTexture.name = assetName + " Atlas";
            AssetDatabase.AddObjectToAsset(fa.atlasTexture, assetPath);
        }
        EditorUtility.SetDirty(fa);
        Debug.Log($"Orbit Escape: created '{assetName}' (Dynamic SDFAA).");
        return true;
    }

    private static bool HasValidAtlas(TMP_FontAsset fa)
    {
        if (fa == null) return false;
        if (fa.sourceFontFile == null) return false;
        if (fa.atlasTextures == null || fa.atlasTextures.Length == 0) return false;
        var tex = fa.atlasTextures[0];
        if (tex == null || tex.width == 0) return false;
        if (fa.atlasPopulationMode == AtlasPopulationMode.Static) return false;
        // Check that the atlas has actual data (not all zeros)
        var pixels = tex.GetPixels32();
        foreach (var p in pixels)
            if (p.a > 0) return true;
        return false; // all-zero atlas = needs rebuild
    }

    private static void AssignFonts(bool forceReassign)
    {
        var font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(RegularSdfPath);
        if (font == null) { Debug.LogError("Orbit Escape: run Setup first."); return; }

        int count = 0;
        foreach (var tmp in Resources.FindObjectsOfTypeAll<TMP_Text>())
        {
            if (tmp.font == null || forceReassign) { tmp.font = font; EditorUtility.SetDirty(tmp); count++; }
        }
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        AssetDatabase.SaveAssets();
        Debug.Log($"Orbit Escape: assigned font to {count} TMP components. Save scene (Ctrl+S).");
    }

    private static string BuildCharSet()
    {
        var chars = new char[95];
        for (int i = 0; i < chars.Length; i++) chars[i] = (char)(32 + i);
        return new string(chars);
    }
}
