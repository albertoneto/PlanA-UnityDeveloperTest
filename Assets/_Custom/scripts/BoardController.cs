using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlanA
{
    public class BoardController : MonoBehaviour
    {
        
        public static event Action<int> OnPointsChange;
        public static event Action<int> OnMovesChange;
        public static event Action OnGameOver;
        
        private readonly int _delayAfterMatch = 1;
        private readonly int _initialPoints = 0;

        [SerializeField, Range(0, 99)] 
        private int boardWidth = 5;
        [SerializeField, Range(0, 99)] 
        private int boardHeight = 6;
        [SerializeField, Range(0, 50)]
        private int initialMoves = 5;
    
        private Tile[,] _board;
        private int _currentMoves;
        private int _currentPoints;
        
        private void Start()
        {
            CreateBoard();
        }

        private void CreateBoard()
        {
            _currentMoves = initialMoves;
            _currentPoints = _initialPoints;
            OnMovesChange?.Invoke(_currentMoves);
            OnPointsChange?.Invoke(_currentPoints);
            
            _board = new Tile[boardWidth, boardHeight];

            Vector3 offset = new Vector3(-boardWidth / 2f + .5f, -boardHeight / 2f + .5f, 0);

            for (int x = 0; x < boardWidth; x++)
            {
                for (int y = 0; y < boardHeight; y++)
                {
                    SetTile(x, y);
                }
            }
            
            GameManager.CurrentState = GameManager.GameState.Gameplay;
        }

        private void SetTile(int x, int y)
        {
            var tileGo = PoolingSystem.Instance.SpawnFromPool(PoolingSystem.PoolType.Tile, Vector3.zero, transform);
            var tile = tileGo.GetComponent<Tile>();
            var offset = new Vector3(-(boardWidth - 1) / 2f, -(boardHeight - 1) / 2f, 0);
            tile.Builder(x, y, offset, this);
            
            _board[x, y] = tile;
        }
        
        private void CheckForGameOver()
        {
            if (_currentMoves > 0)
            {
                return;
            }
            
            StopAllCoroutines();
            ReturnAllTiles();
            OnGameOver?.Invoke();
            GameManager.CurrentState = GameManager.GameState.GameOver;
        }

        private void ReturnAllTiles()
        {
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    if (!_board[i, j]) continue;
                    if (!_board[i, j].gameObject) continue;
                    PoolingSystem.Instance.ReturnToPool(PoolingSystem.PoolType.Tile, _board[i, j].gameObject);
                }
            }
        }

        private IEnumerator ReSetTile(Tile t)
        {
            yield return new WaitForSeconds(_delayAfterMatch);
            SetTile(t.x, t.y);
        }

        private List<Tile> GetNeighbors(Tile t)
        {
            List<Tile> neighbors = new List<Tile>();

            if (t.x + 1 < boardWidth) neighbors.Add(_board[t.x + 1, t.y]);
            if (t.x > 0) neighbors.Add(_board[t.x - 1, t.y]);
            if (t.y + 1 < boardHeight) neighbors.Add(_board[t.x, t.y + 1]);
            if (t.y > 0) neighbors.Add(_board[t.x, t.y - 1]);

            return neighbors;
        }

        public void ResetBoard()
        {
            CreateBoard();
        }

        // We use a breadth-first approach instead of a depth-first one to avoid stack overflow and expand clusters evenly
        public void OnTileClick(Tile clickedTile)
        {
            if (clickedTile == null) return;

            Queue<Tile> queue = new Queue<Tile>();
            HashSet<Tile> cluster = new HashSet<Tile>();

            queue.Enqueue(clickedTile);
            clickedTile.visited = true;

            while (queue.Count > 0)
            {
                Tile current = queue.Dequeue();
                cluster.Add(current);

                foreach (Tile neighbor in GetNeighbors(current))
                {
                    if (neighbor != null && !neighbor.visited && neighbor.color == current.color)
                    {
                        neighbor.visited = true;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            foreach (Tile t in cluster)
            {
                t.visited = false;
            }

            if (cluster.Count < 2) return;
            
            OnMovesChange?.Invoke(--_currentMoves);
            _currentPoints += cluster.Count;
            OnPointsChange?.Invoke(_currentPoints);
            
            foreach (Tile t in cluster)
            {
                StartCoroutine(ReSetTile(t));
                _board[t.x, t.y] = null;
                PoolingSystem.Instance.ReturnToPool(PoolingSystem.PoolType.Tile, t.gameObject);
            }

            CheckForGameOver();
        }
    }
}
