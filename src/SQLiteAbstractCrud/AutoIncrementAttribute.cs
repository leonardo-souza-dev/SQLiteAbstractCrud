using System;

namespace SQLiteAbstractCrud
{
    [AttributeUsage(AttributeTargets.Property)] 
    public class AutoIncrementAttribute : Attribute
    {
    }
}