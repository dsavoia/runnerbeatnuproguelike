using UnityEngine;
using System.Collections;

public class ScenarioParallax : MonoBehaviour {

    public bool moveWithPlayer;
    public float speedFactor;
    public GameObject player;
    public float divFactor = 100;
    
    Material bgMaterial;

	// Use this for initialization
	void Start ()
    {        
        if(moveWithPlayer)
        {
            speedFactor = PlayerInfo.instance.speed - speedFactor;
        }

        bgMaterial = GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!GameManager.instance.isPaused)
        {
            /*bool moving = false;

            if(PlayerInfo.instance.state == PlayerInfo.PlayerState.MovingToPosition || PlayerInfo.instance.state == PlayerInfo.PlayerState.MovingForward)
            {
                moving = true;
            }*/

            float xOffset = moveWithPlayer ? ((PlayerInfo.instance.state == PlayerInfo.PlayerState.MovingForward && PlayerInfo.instance.facingRight) ? speedFactor / divFactor * Time.deltaTime : 0) : speedFactor / divFactor * Time.deltaTime;

            bgMaterial.mainTextureOffset = new Vector2(bgMaterial.mainTextureOffset.x + xOffset, bgMaterial.mainTextureOffset.y);
        }

	}
}
