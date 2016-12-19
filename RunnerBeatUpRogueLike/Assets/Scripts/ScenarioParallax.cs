using UnityEngine;
using System.Collections;

public class ScenarioParallax : MonoBehaviour {

    public bool moveWithPlayer;
    public float speedFactor;
    public GameObject player;
    public float divFactor = 100;

    PlayerInfo playerInfo;
    Material bgMaterial;

	// Use this for initialization
	void Start ()
    {        
        if(moveWithPlayer)
        {
            playerInfo = player.GetComponent<PlayerInfo>();
            speedFactor = playerInfo.speed - speedFactor;
        }

        bgMaterial = GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update ()
    {

        float xOffset = moveWithPlayer? ((playerInfo.state == PlayerInfo.PlayerState.Moving && playerInfo.facingRight)? speedFactor/divFactor * Time.deltaTime : 0) : speedFactor/divFactor * Time.deltaTime;

        bgMaterial.mainTextureOffset = new Vector2(bgMaterial.mainTextureOffset.x + xOffset, bgMaterial.mainTextureOffset.y);

	}
}
