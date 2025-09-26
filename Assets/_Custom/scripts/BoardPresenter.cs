using System;
using TMPro;
using UnityEngine;

public class BoardPresenter : MonoBehaviour
{
    [SerializeField]
    private int initialPoints;
    [SerializeField]
    private int initialMoves = 5;
    [SerializeField]
    private TextMeshProUGUI pointsText;
    [SerializeField]
    private TextMeshProUGUI movesText;
    [SerializeField]
    private GameObject gameOverPanel;
    
    private int _currentMoves;
    private int _currentPoints;

    private void Start()
    {
        ResetGame();
    }

    public void UpdateMoves(int value)
    {
        _currentMoves -= value;
        if (!movesText)
        {
            Debug.LogWarning("Points Text is null");
            return;
        }
        
        movesText.text = _currentMoves.ToString();
        CheckForGameOver();
    }
    
    public void UpdatePoints(int value)
    {
        _currentPoints += value;
        if(!pointsText)
        {
            Debug.LogWarning("Moves Text is null");
            return;
        }
        
        pointsText.text = _currentPoints.ToString();
    }

    public void ResetGame()
    {
        _currentMoves = initialMoves;
        _currentPoints = initialPoints;
        UpdateMoves(0);
        UpdatePoints(0);
        if (gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void CheckForGameOver()
    {
        if (_currentMoves > 0) return;
        gameOverPanel.SetActive(true);
    }
}
