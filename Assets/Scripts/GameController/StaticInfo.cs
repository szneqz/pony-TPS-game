using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticInfo
{
    private static PassDataScript script;

    public static PassDataScript datScript
    {
        get
        {
            return script;
        }
        set
        {
            script = value;
        }
    }
}
