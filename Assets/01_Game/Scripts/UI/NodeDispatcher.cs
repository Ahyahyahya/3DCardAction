using R3;
using UnityEngine;

public class NodeDispatcher : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private MapGenerator _mapGenerator;
    [SerializeField] private SelectPresenter _selectPresenter;
    [SerializeField] private CustomButton _nodePrefab;

    private void Start()
    {
        _mapGenerator.OnGenerateNode
            .Subscribe(node =>
            {
                var createNode = Instantiate(
                    _nodePrefab,
                    _parent);

                node.gameObject = createNode.gameObject;

                createNode.GetComponent<RectTransform>().localPosition = node.pos;

                _selectPresenter.OnCreateNode(createNode, node);
            })
            .AddTo(this);
    }
}
