using System;
using UnityEngine;

public static class ScanHelper
{
    public static GameObject ScanGameObjectByName(GameObject root, string name)
    {
        GameObject result = null;
        try
        {
            int childCount = root.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                if (root.transform.GetChild(i).name.Equals(name))
                {
                    result = root.transform.GetChild(i).gameObject;
                }
            }
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            throw;
        }

        return result;
    }
}