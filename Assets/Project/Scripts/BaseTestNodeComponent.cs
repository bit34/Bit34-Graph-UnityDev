using UnityEngine;


abstract public class BaseTestNodeComponent : MonoBehaviour
{
    //  MEMBERS
    abstract public int NodeId{ get; }
    //      For Editor
#pragma warning disable 0649
    [SerializeField] private SphereCollider _Collider;
    [SerializeField] private SpriteRenderer _Shape;
    [SerializeField] private Sprite         _RegularSprite;
    [SerializeField] private Sprite         _NotAccesibleSprite;
    [SerializeField] private Sprite         _PathStartSprite;
    [SerializeField] private Sprite         _PathTargetSprite;
    [SerializeField] private TextMesh       _InfoText;
#pragma warning restore 0649


    //  METHODS
    public bool IsIntersectingObstacle()
    {
        return Physics.CheckSphere(transform.position, _Collider.radius, LayerMasks.Obstacles);
    }
    
    public void SetAsRegular()
    {
        _Shape.sprite = _RegularSprite;
    }

    public void SetAsNotAccessible()
    {
        _Shape.sprite = _NotAccesibleSprite;
    }

    public void SetAsStart()
    {
        _Shape.sprite = _PathStartSprite;
    }

    public void SetAsTarget()
    {
        _Shape.sprite = _PathTargetSprite;
    }

    protected void SetInfoText(string text)
    {
        _InfoText.text = text;
    }

}
