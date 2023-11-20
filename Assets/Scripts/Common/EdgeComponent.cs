using UnityEngine;

public class EdgeComponent : MonoBehaviour
{
    public GameObject lineObject;

    public void Setup(Vector3 start, Vector3 end, Material material, bool isThick)
    {
        Vector3 direction  = end - start;
        float   angle      = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        transform.position = start;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (isThick)
        {
            lineObject.transform.localScale = new Vector3(direction.magnitude, 1.5f, 1.5f);
            lineObject.transform.localPosition = new Vector3(0, 0, 0.1f);
        }
        else
        {
            lineObject.transform.localScale = new Vector3(direction.magnitude, 0.5f, 0.5f);
        }

        lineObject.GetComponent<SpriteRenderer>().material = material;
    }
}