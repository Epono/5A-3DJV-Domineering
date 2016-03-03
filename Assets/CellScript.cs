using UnityEngine;
using System.Collections;

public class CellScript : MonoBehaviour {

    [SerializeField]
    BoardScript boardScript;

    public int posX, posY;

    public bool isClicked = false;

    public Material material;

    void Start() {
        posX = (int)(transform.position.x + 3.5);
        posY = (int)(transform.position.y + 3.5);
        material = gameObject.GetComponent<Renderer>().material;
    }

    void Update() {

    }

    void OnMouseOver() {
        boardScript.onMouseOver(posX, posY);
    }

    void OnMouseExit() {
        boardScript.onMouseExit(posX, posY);
    }

    void OnMouseDown() {
        boardScript.onMouseDown(posX, posY);
    }
}
