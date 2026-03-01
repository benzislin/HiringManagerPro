using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StarRatingSwapFade : MonoBehaviour
{
    [Header("Assign the two stacked UI Images")]
    [SerializeField] private Image imgA;
    [SerializeField] private Image imgB;

    [Header("Assign sprites in order: 0..4 (starImages_0 -> starImages_4)")]
    [SerializeField] private Sprite[] ratingSprites; // size 5

    [SerializeField] private float fadeTime = 0.50f;

    private bool aIsFront = true;
    
    // 1. Initialize logically at 0 stars instead of -1
    private int currentRating = 0; 
    private Coroutine routine;

    void Awake()
    {
        // 1. Force imgB to perfectly match imgA's physical UI size and scale
        imgB.rectTransform.sizeDelta = imgA.rectTransform.sizeDelta;
        imgB.rectTransform.localScale = imgA.rectTransform.localScale;

        if (ratingSprites != null && ratingSprites.Length > 0)
        {
            imgA.sprite = ratingSprites[0];
            imgA.preserveAspect = true;
            
            // Prep imgB just in case
            imgB.sprite = ratingSprites[0];
            imgB.preserveAspect = true;
        }

        SetAlpha(imgA, 1f);
        SetAlpha(imgB, 0f);
    }

    public void SetRating(int rating0to4)
    {
        rating0to4 = Mathf.Clamp(rating0to4, 0, ratingSprites.Length - 1);
        
        // Because currentRating starts at 0, the first SetRating(0) call will safely hit this return
        if (rating0to4 == currentRating) return;

        currentRating = rating0to4;

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(CrossfadeTo(ratingSprites[rating0to4]));
    }

    private IEnumerator CrossfadeTo(Sprite nextSprite)
    {
        Image front = aIsFront ? imgA : imgB;
        Image back  = aIsFront ? imgB : imgA;

        back.sprite = nextSprite;
        back.preserveAspect = true;

        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float u = Mathf.Clamp01(t / fadeTime);

            SetAlpha(front, 1f - u);
            SetAlpha(back, u);

            yield return null;
        }

        SetAlpha(front, 0f);
        SetAlpha(back, 1f);

        aIsFront = !aIsFront;
    }

    private void SetAlpha(Image img, float a)
    {
        var c = img.color;
        c.a = a;
        img.color = c;
    }
}