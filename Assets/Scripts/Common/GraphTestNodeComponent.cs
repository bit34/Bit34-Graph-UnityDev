using UnityEngine;
using Com.Bit34Games.Graphs;


public class GraphTestNodeComponent : MonoBehaviour
{
    //  MEMBERS
    public int NodeId { get{ return Node.Id; } }
    public GraphNode Node { get; private set; }
    //      For Editor
#pragma warning disable 0649
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private SpriteRenderer _shape;
    [SerializeField] private Sprite         _regularSprite;
    [SerializeField] private Sprite         _notAccesibleSprite;
    [SerializeField] private Sprite         _pathStartSprite;
    [SerializeField] private Sprite         _pathTargetSprite;
    [SerializeField] private TextMesh       _infoText;
#pragma warning restore 0649


    //  METHODS
    public void Init(GraphNode node, string infoText)
    {
        Node = node;
        gameObject.name = "Node" + node.Id;

        _infoText.text = infoText;
    }

    public bool IsIntersectingObstacle()
    {
        return Physics.CheckSphere(transform.position, _collider.radius, LayerMasks.Obstacles);
    }
    
    public void SetAsRegular()
    {
        _shape.sprite = _regularSprite;
    }

    public void SetAsNotAccessible()
    {
        _shape.sprite = _notAccesibleSprite;
    }

    public void SetAsStart()
    {
        _shape.sprite = _pathStartSprite;
    }

    public void SetAsTarget()
    {
        _shape.sprite = _pathTargetSprite;
    }

}
