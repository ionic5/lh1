using LikeLion.LH1.Client.Core.View.OmokScene;
using System;
using UnityEngine;


namespace LikeLion.LH1.Client.UnityWorld.View.OmokScene
{
    public class CheckerboardBuilder : MonoBehaviour
    {
        [SerializeField]
        private GameObject _blockPrefab;
        [SerializeField]
        private float _gap;
        [SerializeField]
        private GameObject _root;
        [SerializeField]
        private GameObject _originPoint;
        [SerializeField]
        private float _blockSize;
        [SerializeField]
        private Vector2 _offset;
        [SerializeField]
        private Vector3 _blockPivot;
        [SerializeField]
        private Vector2 _checkerboardSize;

        void Start()
        {
            Vector3 origin = _originPoint.transform.position;
            int rows = (int)_checkerboardSize.x;
            int cols = (int)_checkerboardSize.y;

            float boardWidth = cols * _blockSize + (cols - 1) * _gap;
            float boardHeight = rows * _blockSize + (rows - 1) * _gap;

            void SpawnElement(Vector3 localPos, Vector3 localScale)
            {
                GameObject obj = Instantiate(_blockPrefab, _root.transform);
                obj.transform.localPosition = localPos;
                obj.transform.localScale = localScale;
            }

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    float localX = _offset.x + x * (_blockSize + _gap);
                    float localZ = _offset.y + y * (_blockSize + _gap);

                    Vector3 localPos = new Vector3(localX, 0, localZ);
                    SpawnElement(localPos, new Vector3(_blockSize, _blockSize, _blockSize));
                }
            }

            float centerX = _offset.x + boardWidth / 2f - _blockPivot.x;
            float centerZ = _offset.y + boardHeight / 2f - _blockPivot.z;
            float edgeOffset = _gap + _blockSize / 2f;

            Vector3 horizontalScale = new Vector3(boardWidth + _gap * 2 + _blockSize * 2, _blockSize, _blockSize);
            Vector3 verticalScale = new Vector3(_blockSize, _blockSize, boardHeight + _gap * 2 + _blockSize * 2);

            // Top
            SpawnElement(new Vector3(centerX, 0, _offset.y - edgeOffset - _blockPivot.z), horizontalScale);
            // Bottom
            SpawnElement(new Vector3(centerX, 0, _offset.y + boardHeight + edgeOffset - _blockPivot.z), horizontalScale);
            // Left
            SpawnElement(new Vector3(_offset.x - edgeOffset - _blockPivot.x, 0, centerZ), verticalScale);
            // Right
            SpawnElement(new Vector3(_offset.x + boardWidth + edgeOffset - _blockPivot.x, 0, centerZ), verticalScale);
        }

        void Update()
        {

        }
    }

}