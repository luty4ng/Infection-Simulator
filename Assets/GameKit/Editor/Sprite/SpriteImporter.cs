using System;
using System.IO;
using UnityEngine;
using UnityEditor;

public static class ImportPreset
{
    public static class Pixel64
    {
        public static int PIXEL_PER_UNIT = 120;
        public static FilterMode FILTER_MODE = FilterMode.Bilinear;
        public static TextureWrapMode WRAP_MODE = TextureWrapMode.Clamp;
        public static TextureImporterType TEXTURE_TYPE = TextureImporterType.Sprite;
        public static TextureImporterCompression COMPRESSION = TextureImporterCompression.CompressedHQ;
    }

    public static class Pixel16
    {
        public static int PIXEL_PER_UNIT = 16;
        public static FilterMode FILTER_MODE = FilterMode.Point;
        public static TextureWrapMode WRAP_MODE = TextureWrapMode.Clamp;
        public static TextureImporterType TEXTURE_TYPE = TextureImporterType.Sprite;
        public static TextureImporterCompression COMPRESSION = TextureImporterCompression.Uncompressed;
    }
}
public class SpriteImporter : AssetPostprocessor
{

    [MenuItem("Assets/GameKit/Tools/Reimport Sprites")]
    public static void SetAllTextureType()
    {
        //获取鼠标点击图片目录
        var arr = Selection.GetFiltered(typeof(DefaultAsset), SelectionMode.Assets);
        string folder = AssetDatabase.GetAssetPath(arr[0]);
        Debug.Log("Reimport Path:" + folder);

        //针对目录下的所有文件进行遍历 取出.png和.jpg文件进行处理 可自行拓展
        DirectoryInfo direction = new DirectoryInfo(folder);
        FileInfo[] pngFiles = direction.GetFiles("*.png", SearchOption.AllDirectories);
        FileInfo[] jpgfiles = direction.GetFiles("*.jpg", SearchOption.AllDirectories);

        try
        {
            SetTexture(pngFiles);
            SetTexture(jpgfiles);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }
    }

    static void SetTexture(FileInfo[] fileInfo)
    {
        for (int i = 0; i < fileInfo.Length; i++)
        {
            string fullpath = fileInfo[i].FullName.Replace("\\", "/");
            string path = fullpath.Replace(Application.dataPath, "Assets");
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

            EditorUtility.DisplayProgressBar("批量处理图片", fileInfo[i].Name, i / (float)fileInfo.Length);
            SetTextureFormat(textureImporter);
            AssetDatabase.ImportAsset(path);
        }
    }

    //设置图片格式
    static void SetTextureFormat(TextureImporter textureImporter)
    {
        string AtlasName = new DirectoryInfo(Path.GetDirectoryName(textureImporter.assetPath)).Name;
        textureImporter.spritePackingTag = AtlasName;
        textureImporter.textureType = ImportPreset.Pixel64.TEXTURE_TYPE;
        textureImporter.spritePixelsPerUnit = ImportPreset.Pixel64.PIXEL_PER_UNIT;
        textureImporter.filterMode = ImportPreset.Pixel64.FILTER_MODE;
        textureImporter.wrapMode = ImportPreset.Pixel64.WRAP_MODE;
        textureImporter.npotScale = TextureImporterNPOTScale.None;

        TextureImporterPlatformSettings setting = new TextureImporterPlatformSettings();
        setting.format = TextureImporterFormat.Automatic;
        textureImporter.SetPlatformTextureSettings(setting);

        textureImporter.textureCompression = ImportPreset.Pixel64.COMPRESSION;
    }
}