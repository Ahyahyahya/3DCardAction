using R3;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectPresenter : BasePresenter
{
    [Header("ƒ‚ƒfƒ‹")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private List<Sprite> _nodeSprites = new();

    [Header("ƒrƒ…[")]
    [SerializeField] private LineGenerator _lineGenerator;

    // ---------- Unity
    protected override void Start()
    {
        base.Start();

        _mapGenerator.OnConnectNode
            .Subscribe(node =>
            {
                _lineGenerator.GenerateLine(node.pos, node.nextNodes.Last().pos);
            })
            .AddTo(this);
    }

    // ---------- Method
    public void OnCreateNode(CustomButton btn, Node node)
    {
        btn.GetComponent<Image>().sprite = _nodeSprites[(int)node.type];

        btn.OnButtonClicked
            .Subscribe(_ =>
            {
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
                        _gameManager.ChangeGameState(GameState.SHOP);
                        break;
                }
            })
            .AddTo(this);
    }
}
