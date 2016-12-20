using UnityEngine;
using System.Linq;

public class GraphicsOrderFix : MonoBehaviour {

    SpriteRenderer[] spriteRenderers;    

    void LateUpdate()
    {
        spriteRenderers = FindObjectsOfType<SpriteRenderer>();
        spriteRenderers = spriteRenderers.OrderBy(sr => sr.transform.parent.position.y).ToArray();

        print(spriteRenderers.Length);

        for(int i = spriteRenderers.Length - 1; i >= 0 ; i--)
        {
            spriteRenderers[i].sortingOrder = i+1;
        }
    }

}
