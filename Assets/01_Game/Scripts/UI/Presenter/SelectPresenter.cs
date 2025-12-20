using EasyTransition;
using R3;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectPresenter : BasePresenter
{
    [Header("モデル")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private PlayerDataProvider _playerDataProvider;
    [SerializeField] private PlayerInputer _inputer;
    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private List<Sprite> _nodeSprites = new();

    [Header("ビュー")]
    [SerializeField] private LineGenerator _lineGenerator;
    [SerializeField] private ScrollView _scrollView;

    // ---------- Unity
    protected override void Start()
    {
        base.Start();

        Debug.Log("[SelectPresenter] イベント登録");
        _playerDataProvider = PlayerDataProvider.Instance;

        _playerDataProvider.CurrentNode
            .Pairwise()
            .Subscribe(nodes =>
            {
                // 現在のノードに繋がっているノードを押下できるように
                foreach (var nextNode in nodes.Current.nextNodes)
                {
                    nextNode.gameObject.GetComponent<CustomButton>().Active();
                }

                // 初回の押下の時最前線のノードを全て選択できないように
                if (nodes.Previous == null)
                {
                    foreach (var initialNode in _mapGenerator.NodesList[0])
                    {
                        initialNode.gameObject.GetComponent<CustomButton>().Inactive();
                    }

                    return;
                }

                // 前回のノードに繋がっていたノードを選択できないようにする
                foreach (var unselectNode in nodes.Previous.nextNodes)
                {
                    unselectNode.gameObject.GetComponent<CustomButton>().Inactive();
                }
            })
            .AddTo(this);

        _inputer.MouseMidBtn
            .Subscribe(input =>
            {
                _scrollView.Scroll(input);
            })
            .AddTo(this);

        _mapGenerator.OnConnectNode
            .Subscribe(node =>
            {
                Debug.Log("[SelectPresenter] ノード接続");

                _lineGenerator.GenerateLine(node.pos, node.nextNodes.Last().pos);
            })
            .AddTo(this);
    }

    // ---------- Method
    public void OnCreateNode(CustomButton btn, Node node)
    {
        // タイプによってUIを変える
        btn.GetComponent<Image>().sprite = _nodeSprites[(int)node.type];

        var scalingView = node.gameObject.GetComponent<ScalingView>();

        var nodeColorView = node.gameObject.GetComponent<NodeColorView>();

        btn.IsActived
            .Subscribe(value =>
            {
                if (value)
                {
                    scalingView.ScalingLoop();
                    nodeColorView.ChangeToActiveColor();
                }
                else
                {
                    scalingView.ScalingStop();
                    nodeColorView.ChangeToInactiveColor();
                }
            })
            .AddTo(this);

        btn.OnButtonClicked
            .Subscribe(_ =>
            {
                _playerDataProvider.SetNode(node);
                nodeColorView.ChangeToSelectedColor();

                switch (node.type)
                {
                    case NodeType.Enemy:
                    case NodeType.Elite:
                    case NodeType.Boss:
                        _gameManager.ChangeGameState(GameState.BATTLE);
                        break;
                    case NodeType.Event:
                        break;
                    case NodeType.Tresure:
                        break;
                    case NodeType.Rest:
                        break;
                    case NodeType.Shop:
                        //_gameManager.ChangeGameState(GameState.SHOP);
                        break;
                }
            })
            .AddTo(this);

        if (node.index.y == 0)
        {
            btn.Active();
        }
        else
        {
            btn.Inactive();
        }
    }
}
