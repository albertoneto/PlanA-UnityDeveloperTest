using System;
using TMPro;
using UnityEngine;

public class BoardPresenter : MonoBehaviour
{
    public enum GameState{Gameplay, GameOver}
    [SerializeField]
    private int initialPoints = 0;
    [SerializeField]
    private int initialMoves = 5;
    [SerializeField]
    private TextMeshProUGUI pointsText;
    [SerializeField]
    private TextMeshProUGUI movesText;
    [SerializeField]
    private GameObject gameOverPanel;
    
    private GameState _gameState = GameState.Gameplay;
    private int _currentMoves;
    private int _currentPoints;

    private void Start()
    {
        ResetGame();
    }

    public void UpdateMoves(int value)
    {
        UpdateValue(value, movesText, ref _currentMoves);
        CheckForGameOver();
    }
    
    public void UpdatePoints(int value)
    {
        UpdateValue(value, pointsText, ref _currentPoints);
    }

    public void ResetGame()
    {
        _currentPoints = initialPoints;
        pointsText.text = _currentPoints.ToString();
        _currentMoves = initialMoves;
        movesText.text = _currentMoves.ToString();
        if (gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(false);
        }
        _gameState = GameState.Gameplay;
    }

    private void UpdateValue(int value, TextMeshProUGUI text, ref int currentValue)
    {
        if (_gameState != GameState.Gameplay)
        {
            return;
        }
        
        currentValue += value;
        if(!text)
        {
            Debug.LogWarning("Text Object is null");
            return;
        }
        
        text.text = currentValue.ToString();
    }

    private void CheckForGameOver()
    {
        if (_currentMoves > 0)
        {
            return;
        }
        gameOverPanel.SetActive(true);
        _gameState = GameState.GameOver;
    }
}
