using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace PlanA
{
    public class Tile : MonoBehaviour, IPointerClickHandler
    {   
        [SerializeField] 
        private TileInfo[] tileInfos;
        
        public int x, y;
        public int color;
        public bool visited ;
        
        private SpriteRenderer _spriteRenderer;
        private BoardController _boardController;

        [Serializable]
        private struct TileInfo
        {
            public int colorId;
            public Sprite sprite;
        }
        
        private void OnEnable()
        {
            if (!_spriteRenderer)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }
        
        public void Builder(int x, int y, Vector3 offset, BoardController boardController)
        {
            _boardController = boardController;
            this.x = x;
            this.y = y;
            
            var spriteSize = _spriteRenderer.bounds.size;
            transform.position = new Vector3(x * spriteSize.x + offset.x, y * spriteSize.y + offset.x, 0);

            color = Random.Range(0, 5);
            _spriteRenderer.sprite = tileInfos[color].sprite;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _boardController.OnTileClick(this);
        }
    }
}
