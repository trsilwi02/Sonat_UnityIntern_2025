using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;

public enum LiquidColor {
    None = 0,
    Red = 1,
    Blue = 2,
    Yellow = 3,
    Purple = 4,
    Green = 5
}


public class GameManager : MonoBehaviour
{
    [Header("Cac tube trong scence")]
    public List<Tube> tubeList;

    [HideInInspector] public Tube selectedTube;

    private bool isAnimating = false;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            Tube clickedTube = GetTubeFromMouse();
            if (clickedTube == null) return;
            if (selectedTube == null)
            {
                if (clickedTube.liquidStack.Count > 0)
                {
                    selectedTube = clickedTube;
                    AudioManager.Instance.PlayClick();
                    HighLightTubeOn(selectedTube);
                    //animation nhac ong len cho vao ong con lai
                }
            }
            else
            {
                if (selectedTube == clickedTube)
                {
                    // click lai chinh no
                    AudioManager.Instance.PlayClick();
                    ResetSelectedTube();
                    selectedTube = null;
                }
                else
                {
                    AudioManager.Instance.PlayClick();
                    TryPour(selectedTube, clickedTube);
                }
            }
        }
    }

    public void TryPour(Tube sourceTube,  Tube targetTube)
    {
        //if (sourceTube.liquidStack.Count == 0) return;
        //LiquidColor colorTomove = sourceTube.GetTopColor();

        //if (!targetTube.CanReceiveColor(colorTomove)) return;

        //int amountSource = sourceTube.GetTopColorCount();
        //int spaceInTarget = targetTube.maxCapacity - targetTube.liquidStack.Count;

        //int amountToMove = Mathf.Min(spaceInTarget, amountSource); //lay so nho nhat giua cung va cau

        //for (int i = 0; i < amountToMove; i++)
        //{
        //    sourceTube.RemoveTopColor();
        //    targetTube.AddColor(colorTomove);
        //    ResetSelectedTube();
        //    CheckWin();
        //}

        if (!targetTube.CanReceiveColor(sourceTube.GetTopColor())) return;

        StartCoroutine(PourSequence(sourceTube, targetTube));
    }

    private Tube GetTubeFromMouse()
    {
        bool isInputDetected = false;
        Vector3 inputPosition = Vector3.zero;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isInputDetected = true;
                inputPosition = touch.position;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            isInputDetected = true;
            inputPosition = Input.mousePosition;
        }

        if (isInputDetected)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(inputPosition);
            Vector2 worldPos2D = new Vector2(worldPos.x, worldPos.y);

            RaycastHit2D hit = Physics2D.Raycast(worldPos2D, Vector2.zero);

            if (hit.collider != null)
            {
                Tube tube = hit.collider.GetComponent<Tube>();
                if (tube != null)
                {
                    return tube;
                }
            }
        }

        return null;
    }

    private void HighLightTubeOn(Tube tube)
    {
        tube.selectedArrow.SetActive(true);
    }

    private void HighLightTubeOff(Tube tube)
    {
        tube.selectedArrow.SetActive(false);
    }

    private void ResetSelectedTube()
    {
        if (selectedTube != null )
        {
            HighLightTubeOff(selectedTube);
            selectedTube = null;
        }
        return;
    }

    public bool CheckWin()
    {
        foreach (var t in tubeList)
        {
            if (!t.IsCompleted())
            {
                return false;
            }
        }
        Debug.Log("da win");
        AudioManager.Instance.PlayWin();
        return true;
    }

    public bool CheckLose()
    {
        foreach (Tube tube in tubeList)
        {
            if (tube.IsEmpty()) return false;
        }

        for (int i = 0; i < tubeList.Count; i++)
        {
            Tube source = tubeList[i];

            if (source.IsCompleted() || source.IsEmpty()) continue;

            LiquidColor colorToCheck = source.GetTopColor();

            for (int j = 0; j < tubeList.Count; j++)
            {
                if (i == j) continue;

                Tube target = tubeList[j];

                if (target.liquidStack.Count >= target.maxCapacity) continue;

                if (target.CanReceiveColor(colorToCheck))
                {
                    return false;
                }
            }
        }

        Debug.Log("Game over - da thua");
        return true;
    }

    IEnumerator PourSequence(Tube source, Tube target)
    {
        isAnimating = true;

        // L?u tr?ng thái g?c
        Vector3 originalPos = source.transform.position;
        Quaternion originalRot = source.transform.rotation;

        // 1. DI CHUY?N ??N MI?NG L?
        yield return StartCoroutine(MoveToTarget(source, target));

        // 2. CH?Y HI?U ?NG RÓT N??C
        // Tính toán s? l??ng c?n rót tr??c khi truy?n vào hàm
        int moveAmount = Mathf.Min(source.GetTopColorCount(), target.maxCapacity - target.liquidStack.Count);
        LiquidColor colorToMove = source.GetTopColor();

        yield return StartCoroutine(AnimateLiquid(source, target, moveAmount, colorToMove));

        // 3. C?P NH?T D? LI?U & D?N D?P (Ch?y ng?m, không c?n yield)
        UpdateGameData(source, target, moveAmount, colorToMove);

        // 4. TR? V? V? TRÍ C?
        yield return StartCoroutine(ReturnToOriginal(source, originalPos, originalRot));

        ResetSelectedTube();
        isAnimating = false;
        if (CheckWin())
        {
            WinGame();
        } 
        else if (CheckLose())
        {
            LoseGame();
        }
    }

    IEnumerator MoveToTarget(Tube source, Tube target)
    {
        bool isSourceLeft = source.transform.position.x < target.transform.position.x;
        float directionMultiplier = isSourceLeft ? 1f : -1f;

        // Tính v? trí: Cao h?n 1.5 ??n v?, l?ch trái/ph?i 0.5 ??n v?
        Vector3 pourPosition = target.transform.position + (Vector3.up * 2.5f) + (Vector3.left * directionMultiplier * 1.5f);

        // Tính góc xoay: 50 ??
        Vector3 pourRotation = new Vector3(0, 0, -50f * directionMultiplier);

        Sequence moveSeq = DOTween.Sequence();
        moveSeq.Append(source.transform.DOMove(pourPosition, 0.5f).SetEase(Ease.OutQuad));
        moveSeq.Join(source.transform.DORotate(pourRotation, 0.5f).SetEase(Ease.OutQuad));

        yield return moveSeq.WaitForCompletion();
    }

    private void UpdateGameData(Tube source, Tube target, int moveAmount, LiquidColor colorToMove)
    {
        // L?y l?i scale chu?n (?? reset)
        float originalScaleX = source.liquidSprite[0].transform.localScale.x;
        // L?u ý: N?u trong game scale Y c?a b?n không c? ??nh thì c?n cách l?u khác, 
        // nh?ng th??ng các ??t n??c ??u b?ng nhau nên l?y ??t 0 làm m?u là ???c.
        float defaultScaleY = 4.79f; // Ho?c l?y t? bi?n config nào ?ó n?u có
        if (source.liquidSprite.Length > 0)
        {
            defaultScaleY = source.liquidSprite[0].transform.localScale.y;
        }

        for (int i = 0; i < moveAmount; i++)
        {
            // 1. C?p nh?t Logic List
            source.RemoveTopColor();
            target.AddColor(colorToMove);

            // 2. D?n d?p Source Sprite (Reset tr?ng thái ?? tái s? d?ng)
            // Vì List ?ã remove nên index hi?n t?i tr? vào ?úng ph?n t? v?a b? coi là "r?ng"
            int cleanIndex = source.liquidStack.Count;
            SpriteRenderer spriteToClean = source.liquidSprite[cleanIndex];

            spriteToClean.gameObject.SetActive(false);

            // RESET V? TRÍ & SCALE (B?t bu?c vì nãy mình ?ã Move & Scale nó r?i)
            spriteToClean.transform.localScale = new Vector3(originalScaleX, defaultScaleY, 1);

            // Reset v? trí: C?ng tr? l?i 0.5f nãy mình ?ã tr? ?i
            spriteToClean.transform.localPosition += (Vector3.up * 0.5f);
        }
    }

    IEnumerator AnimateLiquid(Tube source, Tube target, int moveAmount, LiquidColor colorToMove)
    {
        // L?y scale g?c ?? tham chi?u
        float originalScaleX = source.liquidSprite[0].transform.localScale.x;
        float originalScaleY = source.liquidSprite[0].transform.localScale.y;
        float originalScaleZ = source.liquidSprite[0].transform.localScale.z;

        Sequence liquidSeq = DOTween.Sequence();
        float flowDuration = 0.2f; // T?c ?? ch?y m?i ??t

        AudioManager.Instance.PlayPour();

        for (int i = 0; i < moveAmount; i++)
        {
            // L?y Sprite theo index d? ki?n
            SpriteRenderer sourceSprite = source.liquidSprite[source.liquidStack.Count - 1 - i];
            SpriteRenderer targetSprite = target.liquidSprite[target.liquidStack.Count + i];

            // --- SETUP TARGET ---
            targetSprite.gameObject.SetActive(true);
            targetSprite.color = source.colorPalette.GetColor(colorToMove);

            // Hack Pivot Target: ??t scale Y=0 và h? v? trí xu?ng th?p
            Vector3 targetFullPos = targetSprite.transform.localPosition;
            Vector3 targetStartPos = targetFullPos - (Vector3.up * 0.5f);

            targetSprite.transform.localScale = new Vector3(originalScaleX, 0, originalScaleZ);
            targetSprite.transform.localPosition = targetStartPos;

            // --- SETUP SOURCE ---
            // Hack Pivot Source: Tính v? trí s? t?t xu?ng
            Vector3 sourceEndPos = sourceSprite.transform.localPosition - (Vector3.up * 0.5f);

            // --- T?O ANIMATION ---
            // Source: Rút n??c (Scale v? 0 + T?t xu?ng)
            liquidSeq.Append(sourceSprite.transform.DOScaleY(0, flowDuration).SetEase(Ease.InQuad));
            liquidSeq.Join(sourceSprite.transform.DOLocalMove(sourceEndPos, flowDuration).SetEase(Ease.InQuad));

            // Target: Dâng n??c (Scale lên full + Nh?c lên)
            liquidSeq.Join(targetSprite.transform.DOScaleY(originalScaleY, flowDuration).SetEase(Ease.OutQuad));
            liquidSeq.Join(targetSprite.transform.DOLocalMove(targetFullPos, flowDuration).SetEase(Ease.OutQuad));
        }

        yield return liquidSeq.WaitForCompletion();
    }

    IEnumerator ReturnToOriginal(Tube source, Vector3 originalPos, Quaternion originalRot)
    {
        Sequence returnSeq = DOTween.Sequence();
        returnSeq.Append(source.transform.DOMove(originalPos, 0.4f).SetEase(Ease.InOutSine));
        returnSeq.Join(source.transform.DORotateQuaternion(originalRot, 0.4f).SetEase(Ease.InOutSine));

        yield return returnSeq.WaitForCompletion();
    }

    private void WinGame()
    {
        UIManager.Instance.WinPanelOn();
    }

    private void LoseGame()
    {
        UIManager.Instance.LosePanelOn();
    }
}
