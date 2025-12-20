using R3;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // ---------- Field
    [SerializeField] private int _rowCnt;
    [SerializeField] private int _columnCnt;
    [SerializeField] private float _maxPosX;
    [SerializeField] private float _minPosX;
    [SerializeField] private float _startPosY;
    [SerializeField] private float _maxPosY;
    [SerializeField] private float _rowDistance;

    private List<List<Node>> _nodesList = new();
    public List<List<Node>> NodesList => _nodesList;

    private Subject<Node> _connectNode = new();
    public Observable<Node> OnConnectNode => _connectNode;

    private Subject<Node> _generateNode = new();
    public Observable<Node> OnGenerateNode => _generateNode;

    private bool _isGenerated;

    // ---------- Method
    public void GenerateMap()
    {
        if (_isGenerated) return;

        DecideNodesDetail();

        ConnectNodes();

        RemoveNoConnectNodes();

        GenerateNodes();

        _isGenerated = true;
    }

    /// <summary>
    /// ノードの位置やタイプを決定
    /// </summary>
    private void DecideNodesDetail()
    {
        for (int y = 0; y < _rowCnt; y++)
        {
            var nodes = new List<Node>();

            for (int x = 0; x < _columnCnt; x++)
            {
                var node = new Node();

                node.index = new Vector2(x, y);

                if (x == 0)
                {
                    node.pos = new Vector2(
                        Random.Range(
                            _minPosX,
                            _maxPosX - 100f * (_columnCnt - x - 1)),
                        _startPosY + y * _rowDistance + Random.value);
                }
                else
                {
                    node.pos = new Vector2(
                        Random.Range(
                            nodes.Last().pos.x + 100f,
                            _maxPosX - 100f * (_columnCnt - x - 1)),
                        _startPosY + y * _rowDistance + Random.value);
                }

                var isLast = y == _rowCnt - 1;

                node.type = isLast ? NodeType.Boss : DecideNodeType();

                nodes.Add(node);

                if (isLast) break;
            }

            _nodesList.Add(nodes);
        }
    }

    /// <summary>
    /// ランダムにボス以外のタイプを返す
    /// </summary>
    /// <returns></returns>
    private NodeType DecideNodeType()
    {
        return (NodeType)Random.Range(0, (int)NodeType.NodeTypeCount - 1);
    }

    /// <summary>
    /// ノードを線で繋ぐ
    /// </summary>
    private void ConnectNodes()
    {
        for (int i = 0; i < _nodesList.First().Count; i++)
        {
            var prevNode = _nodesList[0][i];

            for (int y = 1; y < _rowCnt;  y++)
            {
                var index = Random.Range(0, _nodesList[y].Count);

                var nextNode = _nodesList[y][index];

                nextNode.previousNodes.Add(prevNode);

                prevNode.nextNodes.Add(_nodesList[y][index]);

                _connectNode.OnNext(prevNode);

                prevNode = nextNode;
            }
        }
    }

    /// <summary>
    /// 繋がっていないノードを消す
    /// </summary>
    private void RemoveNoConnectNodes()
    {
        foreach (var nodes in _nodesList)
        {
            nodes.RemoveAll(x =>
            x.previousNodes.Count == 0 && x.nextNodes.Count == 0);
        }
    }

    /// <summary>
    /// ノードを生成
    /// </summary>
    private void GenerateNodes()
    {
        foreach(var nodes in _nodesList)
        {
            foreach (var node in nodes)
            {
                _generateNode.OnNext(node);
            }
        }
    }
}
