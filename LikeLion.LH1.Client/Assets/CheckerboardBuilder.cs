using UnityEngine;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 origin = _originPoint.transform.position;

        int rows = 18;
        int cols = 18;

        // 내부 격자 블록 생성
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                float posX = origin.x + _offset.x + x * (_blockSize + _gap);
                float posZ = origin.z + _offset.y + y * (_blockSize + _gap);

                Vector3 pos = new Vector3(posX, origin.y, posZ);

                GameObject block = Instantiate(_blockPrefab, pos, Quaternion.identity);
                block.transform.localScale = new Vector3(_blockSize, _blockSize, _blockSize);
                block.transform.SetParent(_root.transform, true);
            }
        }

        float boardWidth = cols * _blockSize + (cols - 1) * _gap;
        float boardHeight = rows * _blockSize + (rows - 1) * _gap;

        var cubePivot = new Vector3(0.5f, 0.5f, 0.5f);

        // 위쪽
        Vector3 topPos = new Vector3(origin.x + _offset.x + boardWidth / 2f - cubePivot.x,
                                     origin.y,
                                     origin.z + _offset.y - _gap - _blockSize / 2f - cubePivot.z);
        GameObject topBorder = Instantiate(_blockPrefab, topPos, Quaternion.identity);
        topBorder.transform.localScale = new Vector3(boardWidth + 2 * _gap + 2 * _blockSize, _blockSize, _blockSize);
        topBorder.transform.SetParent(_root.transform, true);

        // 아래쪽
        Vector3 bottomPos = new Vector3(origin.x + _offset.x + boardWidth / 2f - cubePivot.x,
                                        origin.y,
                                        origin.z + _offset.y + boardHeight + _gap + _blockSize / 2f - cubePivot.z);
        GameObject bottomBorder = Instantiate(_blockPrefab, bottomPos, Quaternion.identity);
        bottomBorder.transform.localScale = new Vector3(boardWidth + 2 * _gap + 2 * _blockSize, _blockSize, _blockSize);
        bottomBorder.transform.SetParent(_root.transform, true);

        // 왼쪽
        Vector3 leftPos = new Vector3(origin.x + _offset.x - _gap - _blockSize / 2f - cubePivot.x,
                                      origin.y,
                                      origin.z + _offset.y + boardHeight / 2f - cubePivot.z);
        GameObject leftBorder = Instantiate(_blockPrefab, leftPos, Quaternion.identity);
        leftBorder.transform.localScale = new Vector3(_blockSize, _blockSize, boardHeight + 2 * _gap + 2 * _blockSize);
        leftBorder.transform.SetParent(_root.transform, true);

        // 오른쪽
        Vector3 rightPos = new Vector3(origin.x + _offset.x + boardWidth + _gap + _blockSize / 2f - cubePivot.x,
                                       origin.y,
                                       origin.z + _offset.y + boardHeight / 2f - cubePivot.z);
        GameObject rightBorder = Instantiate(_blockPrefab, rightPos, Quaternion.identity);
        rightBorder.transform.localScale = new Vector3(_blockSize, _blockSize, boardHeight + 2 * _gap + 2 * _blockSize);
        rightBorder.transform.SetParent(_root.transform, true);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
