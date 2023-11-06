using System;

public class WorldTimeAttribute : Attribute
{
    public bool NeedSec;
    public WorldTimeAttribute(bool sec = false)
    {
        NeedSec = sec;
    }
}
