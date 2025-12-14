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

    private List<List<Node>> _nodesList = new();

    private Subject<Node> _connectNode = new();
    public Observable<Node> OnConnectNode => _connectNode;

    private Subject<Node> _generateNode = new();

    public Observable<Node> OnGenerateNode => _generateNode;

    // ---------- UnityMessage
    private void Start()
    {
        GenerateMap();
    }

    // ---------- Method
    public void GenerateMap()
    {
        DecideNodesDetail();

        ConnectNodes();

        RemoveNoConnectNodes();

        GenerateNodes();
    }

    /// <summary>
    /// ノードの位置やタイプを決定
    /// </summary>
    private void DecideNodesDetail()
    {
        for (int i = 0; i < _rowCnt; i++)
        {
            var nodes = new List<Node>();

            for (int j = 0; j < _columnCnt; j++)
            {
                var node = new Node();

                node.pos = new Vector2(
                    Random.Range(_minPosX, _maxPosX),
                    i * 200f + Random.value);

                node.type = DecideNodeType();

                nodes.Add(node);
            }

            _nodesList.Add(nodes);
        }
    }

    /// <summary>
    /// ランダムにタイプを返す
    /// </summary>
    /// <returns></returns>
    private NodeType DecideNodeType()
    {
        return (NodeType)Random.Range(0, (int)NodeType.NodeTypeCount);
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
