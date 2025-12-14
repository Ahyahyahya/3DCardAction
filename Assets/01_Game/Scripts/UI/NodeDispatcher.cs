using System.Collections.Generic;
using R3;
using UnityEngine;

public class NodeDispatcher : MonoBehaviour
{
    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private SelectPresenter _selectPresenter;
    [SerializeField] private CustomButton _nodePrefab;
    [SerializeField] private List<Sprite> _nodeSprites = new();

    private void Start()
    {
        _mapGenerator.OnGenerateNode
            .Subscribe(node =>
            {
                var createNode = Instantiate(
                    _nodePrefab,
                    transform);

                createNode.GetComponent<RectTransform>().localPosition = node.pos;

                _selectPresenter.OnCreateNode(createNode, node);
            })
            .AddTo(this);
    }
}
