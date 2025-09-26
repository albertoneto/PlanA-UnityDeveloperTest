using UnityEngine;

namespace PlanA
{
    public class GameManager : MonoBehaviour
    {
        public enum GameState{Gameplay, GameOver}
        public static GameState CurrentState = GameState.Gameplay;

        //TODO: Game State Machine
    }
}