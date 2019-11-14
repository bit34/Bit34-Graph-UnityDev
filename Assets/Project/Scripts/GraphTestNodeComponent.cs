using UnityEngine;


public class GraphTestNodeComponent : MonoBehaviour
{
    //  MEMBERS
    public TestNode Node { get; private set; }
    //      For Editor
#pragma warning disable 0649
    [SerializeField] private SpriteRenderer _Shape;
    [SerializeField] private Sprite         _RegularSprite;
    [SerializeField] private Sprite         _PathStartSprite;
    [SerializeField] private Sprite         _PathTargetSprite;
    [SerializeField] private TextMesh       _InfoText;
#pragma warning restore 0649


    //  METHODS
    public void Init(TestNode node)
    {
        Node = node;
        gameObject.name = "Node" + node.Id;

        _InfoText.text = Node.GetData().Value.ToString();
    }

    public void SetAsStart()
    {
        _Shape.sprite = _PathStartSprite;
    }

    public void SetAsTarget()
    {
        _Shape.sprite = _PathTargetSprite;
    }

    public void SetAsRegular()
    {
        _Shape.sprite = _RegularSprite;
    }

}
