using R3;
using UnityEngine;

public class PlayerCore : MonoBehaviour, IDamageble
{
    // ---------- Field
    [SerializeField]
    private SerializableReactiveProperty<int> _maxHp = new(100);
    public ReadOnlyReactiveProperty<int> MaxHp => _maxHp;

    [SerializeField]
    private SerializableReactiveProperty<int> _hp = new(100);
    public ReadOnlyReactiveProperty<int> Hp => _hp;

    private ReactiveProperty<Node> _currentNode = new();
    public ReadOnlyReactiveProperty<Node> CurrentNode => _currentNode;

    // ---------- UnityMessage
    private void Start()
    {
        _maxHp
            .Chunk(2, 1)
            .Subscribe(values =>
            {
                // Œ³‚ÌHP‚Æ•Ï‰»Œã‚ÌHP‚Ì·
                var diff = values[1] - values[0];

                // Å‘åHP‚ª‘‚¦‚Ä‚¢‚½‚ç
                if (diff > 0)
                {
                    // ‚»‚Ì•ªHP‚ğ‰ñ•œ‚·‚é
                    _hp.Value = Mathf.Clamp(_hp.Value + diff, 0, _maxHp.Value);
                }
                // Å‘åHP‚ªŒ¸‚èAŒ»İ‚ÌHP‚ğ‰º‰ñ‚Á‚½‚ç
                else if(diff < 0 && _hp.Value > _maxHp.Value)
                {
                    // Œ»İ‚ÌHP‚ğÅ‘åHP‚É‡‚í‚¹‚é
                    _hp.Value = _maxHp.Value;
                }
            })
            .AddTo(this);
    }

    // ---------- Method
    public void SetNode(Node node)
    {
        _currentNode.Value = node;
    }

    // ---------- Interface
    public void TakeDamage(int damage)
    {
        _hp.Value -= damage;
    }
}
