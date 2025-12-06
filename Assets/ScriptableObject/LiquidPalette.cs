using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LiquidPalette", menuName = "Scriptable Objects/LiquidPalette")]
public class LiquidPalette : ScriptableObject
{
    [System.Serializable]
    public struct ColorMap
    {
        public LiquidColor type;
        public Color colorValue; //RBG
    }

    public List<ColorMap> mapping;

    public Color GetColor(LiquidColor type)
    {
        foreach (var m in mapping)
        {
            if (m.type == type) return m.colorValue;
        }
        return Color.white;
    }
}
