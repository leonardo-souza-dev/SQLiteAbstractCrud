using System;

namespace SQLiteAbstractCrud;

[AttributeUsage(AttributeTargets.Property)] 
public sealed class PrimaryKeyAttribute : Attribute
{
    
}
