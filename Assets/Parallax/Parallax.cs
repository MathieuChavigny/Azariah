using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public GameObject followTarget;
    public bool enableVertical;
    public List<ParallaxLayer> parallaxLayers; // List of parallax layer configs

    // HideInInspector necessary to keep the in-editor value at runtime
    [HideInInspector] public List<GameObject> parallaxLayerMoveGOs; // Constant speed movable container
    [HideInInspector] public List<GameObject> parallaxLayerGOs; // Parallax speed movable container
    [HideInInspector] public List<GameObject> allSprites; // Tilable sprites
    [HideInInspector] public List<Vector2> bounds;
    [HideInInspector] public List<Vector2> startPositions;

    private void Start()
    {
        if (parallaxLayerGOs.Count > 0) {
            for (int i = 0; i < parallaxLayerGOs.Count; i++) {
                startPositions.Add(parallaxLayerGOs[i].transform.position);
            }
        }
    }

    public void CreateGUI()
    {

        if (parallaxLayerGOs.Count > 0) {
            DeleteGUI();
        }

        for (int layerIdx = 0; layerIdx < parallaxLayers.Count; layerIdx++) {

            GameObject parallaxObject = new GameObject();
            parallaxLayerGOs.Add(parallaxObject);
            parallaxObject.transform.parent = gameObject.transform;
            if (parallaxLayers[layerIdx].sprite != null) {
                parallaxObject.name = "Parallax Layer " + (parallaxLayers[layerIdx].sprite.name);
            } else {
                parallaxObject.name = "Parallax Layer ERROR";
            }

            GameObject moveBackground = new GameObject();
            parallaxLayerMoveGOs.Add(moveBackground);
            moveBackground.transform.parent = parallaxLayerGOs[layerIdx].transform;
            moveBackground.name = "Move Background Layer " + layerIdx;
            
            // Horizontal tiling
            for (int spriteX = 0; spriteX < 3; spriteX++) {
                if (enableVertical) {
                    // Vertical tiling
                    string[] names = {"Bottom Horizontal Parent", "Middle Horizontal Parent", "Top Horizontal Parent"};
                    GameObject parentHorizontal = new GameObject();
                    parentHorizontal.name = names[spriteX];
                    parentHorizontal.transform.parent = parallaxLayerMoveGOs[layerIdx].transform;
                    
                    for (int spriteY = 0; spriteY < 3; spriteY++) {
                        GameObject sp = InitSprite(parentHorizontal, parallaxLayers[layerIdx], spriteX, spriteY);
                    }
                } else { 
                    InitSprite(parallaxLayerMoveGOs[layerIdx], parallaxLayers[layerIdx], spriteX, 0); 
                }
            }
        }
    }

    public void DeleteGUI()
    {
        for (int i = transform.childCount; i > 0; i--) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        if (parallaxLayerGOs != null) parallaxLayerGOs.Clear();
        if (parallaxLayerMoveGOs != null) parallaxLayerMoveGOs.Clear();
        if (allSprites != null) allSprites.Clear();
        if (bounds != null) bounds.Clear();
        if (startPositions != null) startPositions.Clear();
    }

    void LateUpdate()
    {
        if (parallaxLayerGOs.Count > 0) {
            for (int i = 0; i < parallaxLayers.Count; i++) {
                // Parallax displacement
                parallaxLayerGOs[i].transform.position = new Vector2(
                    startPositions[i].x + followTarget.transform.position.x * parallaxLayers[i].speedX,
                    startPositions[i].y + followTarget.transform.position.y * parallaxLayers[i].speedY
                );

                // Constant speed motion
                parallaxLayerMoveGOs[i].transform.position = new Vector2(
                    parallaxLayerMoveGOs[i].transform.position.x + Time.deltaTime * parallaxLayers[i].constantSpeedX,
                    parallaxLayerMoveGOs[i].transform.position.y + Time.deltaTime * parallaxLayers[i].constantSpeedY
                );
            }

            for (int j = 0; j < allSprites.Count; j++) {
                CheckPosition(allSprites[j], j);
            }
        }
    }

    void CheckPosition(GameObject myObject, int j)
    {
        // Left or Right
        if (myObject.transform.position.x < followTarget.transform.position.x - 1.5f * bounds[j].x) {
            myObject.transform.position = new Vector2(myObject.transform.position.x + bounds[j].x * 3, myObject.transform.position.y);
        
        } else if (myObject.transform.position.x > followTarget.transform.position.x + bounds[j].x * 1.5f) {
            myObject.transform.position = new Vector2(myObject.transform.position.x - bounds[j].x * 3, myObject.transform.position.y);
        }

        // Down or Up
        if (enableVertical) {
            if (myObject.transform.position.y < followTarget.transform.position.y - 1.5f * bounds[j].y) {
                myObject.transform.position = new Vector2(myObject.transform.position.x, myObject.transform.position.y + bounds[j].y * 3);
            
            } else if (myObject.transform.position.y > followTarget.transform.position.y + bounds[j].y * 1.5f) {
                myObject.transform.position = new Vector2(myObject.transform.position.x, myObject.transform.position.y - bounds[j].y * 3);
            }
        }
    }

    
    GameObject InitSprite(GameObject parent, ParallaxLayer parallaxLayer, int xOffset, int yOffset)
    {
        GameObject spriteGO = new GameObject();
        spriteGO.name = "Parallax Sprite " + allSprites.Count;
        spriteGO.transform.parent = parent.transform;

        spriteGO.AddComponent<SpriteRenderer>();
        spriteGO.GetComponent<SpriteRenderer>().sortingLayerName = parallaxLayer.sortingLayerName;
        spriteGO.GetComponent<SpriteRenderer>().sortingOrder = parallaxLayer.orderInLayer;
        
        if (parallaxLayer.sprite == null) {
            if (xOffset == 0) {
                // Show debug once
                Debug.LogWarning("Le layer de parallaxe n'a pas de sprite associé dans l'inspecteur pour l'objet '" + gameObject.name + "'.");
            } 
            spriteGO.GetComponent<SpriteRenderer>().sprite = Sprite.Create(new Texture2D(100, 100), new Rect(0, 0, 100, 100), new Vector2(0.5f, 0.5f), 5, 0);
            spriteGO.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        } else {
            spriteGO.GetComponent<SpriteRenderer>().sprite = parallaxLayer.sprite;
        }

        Vector2 boundsVector = new Vector2(
            spriteGO.GetComponent<SpriteRenderer>().sprite.bounds.size.x, 
            spriteGO.GetComponent<SpriteRenderer>().sprite.bounds.size.y
        );
        bounds.Add(boundsVector);

        int flag = (enableVertical) ? 1 : 0;

        spriteGO.transform.position = new Vector2(
            -bounds[bounds.Count - 1].x + bounds[bounds.Count - 1].x * xOffset, 
            -bounds[bounds.Count - 1].y * flag + bounds[bounds.Count - 1].y * yOffset
        );

        allSprites.Add(spriteGO);
        
        return spriteGO;
    }
}
