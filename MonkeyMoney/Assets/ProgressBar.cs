using UnityEngine;

public class FillProgressBar : MonoBehaviour
{
    [Header("progress states")]
    public Sprite[] progressSprites;

    [Header("Love Monkey Love")]
    public GameObject loveButton;

    private SpriteRenderer spriteRenderer;

    private int currentState = 4;
    private int maxState;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Set maxState based on the number of sprites (last index)
        maxState = progressSprites.Length - 1;
        
        // Only try to hide if loveButton is assigned
        if (loveButton != null)
        {
            loveButton.SetActive(false);
        }
        
        UpdateSprite();
    }

    public void IncreaseProgress()
    {
        if (currentState < maxState)
        {
            currentState++;
            UpdateSprite();
            CheckIfFull();
        }
    }

    private void UpdateSprite()
    {
        if (currentState < progressSprites.Length)
        {
            spriteRenderer.sprite = progressSprites[currentState];   
        }
    }

    private void CheckIfFull()
    {
        if (currentState >= maxState)
        {
            ShowLoveButton();
        }
    }

    private void ShowLoveButton()
    {
        if (loveButton != null)
        {
            loveButton.SetActive(true);
        }
    }
}