using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Tube : MonoBehaviour
{
    [HideInInspector] public int maxCapacity = 4;

    [Header("Visual References")]
    public GameObject selectedArrow;
    public SpriteRenderer[] liquidSprite;
    public LiquidPalette colorPalette;

    [Header("Liquid data")]
    public List<LiquidColor> liquidStack = new List<LiquidColor>();

    private void Start()
    {
        UpdateScence();
    }
    public void UpdateScence()
    {
        for (int i = 0; i < maxCapacity; i++)
        {
            if (i < liquidStack.Count)
            {
                liquidSprite[i].gameObject.SetActive(true);

                LiquidColor liquidColor = liquidStack[i];
                if (colorPalette != null)
                {
                    liquidSprite[i].color = colorPalette.GetColor(liquidColor);
                }
            } else
            {
                liquidSprite[i].gameObject.SetActive(false);
            }
        }
    }

    public bool CanReceiveColor(LiquidColor color)
    {
        if (liquidStack.Count >= maxCapacity) return false;
        if (liquidStack.Count == 0) return true;
        return GetTopColor() == color;
    }

    public LiquidColor GetTopColor()
    {
        if (liquidStack.Count == 0) return LiquidColor.None;
        return liquidStack[liquidStack.Count - 1];
    }

    public int GetTopColorCount()
    {
        if (liquidStack.Count == 0)
        {
            return 0;
        }

        LiquidColor topColor = GetTopColor();
        int count = 0;
        for (int i = liquidStack.Count - 1; i >= 0; i--)
        {
            if (topColor == liquidStack[i])
            {
                count++;
            } else
            {
                break;
            }
        }
        return count;
    }

    public void AddColor(LiquidColor color)
    {
        if (liquidStack.Count < maxCapacity)
        {
            liquidStack.Add(color);
            UpdateScence();
            Debug.Log("add thanh cong");
        }
    }

    public void RemoveTopColor()
    {
        if (liquidStack.Count > 0)
        {
            liquidStack.RemoveAt(liquidStack.Count - 1);
            UpdateScence();
            Debug.Log("remove thanh cong");
        }
    }

    public bool IsCompleted()
    {
        if (liquidStack.Count == 0) return true;
        if (liquidStack.Count < maxCapacity) return false;

        LiquidColor colorType = liquidStack[0];
        foreach(var color in liquidStack)
        {
            if (color != colorType) return false;
        }
        return true;
    }

    public SpriteRenderer GetNextEmptySprite()
    {
        if (liquidStack.Count >= maxCapacity) return null;
        return liquidSprite[liquidStack.Count];
    }

    public SpriteRenderer GetTopLiquidSprite()
    {
        if (liquidStack.Count == 0) return null;
        return liquidSprite[liquidStack.Count - 1];
    }

    public bool IsEmpty()
    {
        return liquidStack.Count == 0;
    }

    private void OnValidate()
    {
        
        if (liquidStack.Count > maxCapacity)
        {
            liquidStack = liquidStack.GetRange(0, maxCapacity);
        }
        UpdateScence();
    }
}
