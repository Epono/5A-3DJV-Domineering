using UnityEngine;
using System.Collections;

public class CellScript : MonoBehaviour {

    [SerializeField]
    BoardScript boardScript;

    public int posX, posY;

    public bool isClicked = false;
    public bool isTrigger = true;

    public Material material;

    void Start() {
        posX = (int)(transform.position.x + 3.5);
        posY = (int)(transform.position.y + 3.5);
        material = gameObject.GetComponent<Renderer>().material;
    }

    void OnMouseOver() {
        if(isTrigger) {
            boardScript.onMouseOver(posX, posY);
        }
    }

    void OnMouseExit() {
        if(isTrigger) {
            boardScript.onMouseExit(posX, posY);
        }
    }

    void OnMouseDown() {
        if(isTrigger) {
            boardScript.onMouseDown(posX, posY);
        }
    }
}
