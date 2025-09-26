using TMPro;
using UnityEngine;

namespace PlanA
{
    public class BoardPresenter : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI pointsText;
        [SerializeField]
        private TextMeshProUGUI movesText;
        [SerializeField]
        private GameObject gameOverPanel;
        
        private void OnEnable()
        {
            BoardController.OnMovesChange += UpdateMoves;
            BoardController.OnPointsChange += UpdatePoints;
            BoardController.OnGameOver += SetGameOver;
        }

        private void OnDisable()
        {
            BoardController.OnMovesChange -= UpdateMoves;
            BoardController.OnPointsChange -= UpdatePoints;
            BoardController.OnGameOver -= SetGameOver;
        }

        private void UpdateMoves(int value)
        {
            UpdateValue(value, movesText);
        }
    
        private void UpdatePoints(int value)
        {
            UpdateValue(value, pointsText);
        }

        private void UpdateValue(int value, TextMeshProUGUI text)
        {
            if (GameManager.CurrentState != GameManager.GameState.Gameplay)
            {
                return;
            }
        
            if(!text)
            {
                Debug.LogWarning("Text Object is null");
                return;
            }
        
            text.text = value.ToString();
        }

        private void SetGameOver()
        {
            if (!gameOverPanel.activeInHierarchy)
            {
                gameOverPanel.SetActive(true);
            }
        }
    }
}
