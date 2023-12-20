using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParallaxLayer {

    public Sprite sprite;
	public string sortingLayerName;
    public int orderInLayer;

	[Header("Speed")]
    public float speedX;
    public float speedY;

	[Header("Constant speed")]
    public float constantSpeedX;
    public float constantSpeedY;

}
