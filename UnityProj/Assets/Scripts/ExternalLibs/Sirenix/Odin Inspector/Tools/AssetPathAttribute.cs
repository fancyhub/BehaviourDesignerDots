using System;

public class AssetPathAttribute : Attribute
{
    public Type AssetType;
    public bool ShowPreview = false;
    public float PreviewHeight = 120;
    public AssetPathAttribute(Type t, bool preview = false, float height = 120)
    {
        AssetType = t;
        ShowPreview = preview;
        PreviewHeight = height;
    }
}