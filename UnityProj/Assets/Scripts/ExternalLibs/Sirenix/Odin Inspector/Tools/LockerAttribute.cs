using System;

public class LockerAttribute : Attribute
{
    public LockerAttribute(bool value = false)
    {
        isLock = value;
    }
    public bool isLock = false;
}
