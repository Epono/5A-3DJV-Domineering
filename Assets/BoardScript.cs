using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardScript : MonoBehaviour {

    [SerializeField]
    List<CellScript> cellScripts;

    [SerializeField]
    GameObject prefab;

    bool verticalPlayer = true;

    void Start() {

    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Return)) {
            Debug.Log("mabite");
            onMouseDown((int)(Random.value * 7), (int)(Random.value * 7));
        }
    }

    public void onMouseOver(int posX, int posY) {
        CellScript cellScript = getCell(posX, posY);
        CellScript cellScript2 = getCell(posX + (verticalPlayer ? 0 : 1), posY + (verticalPlayer ? 1 : 0));

        if(!cellScript.isClicked && !cellScript2.isClicked) {
            cellScript.material.color = Color.white;
            cellScript2.material.color = Color.white;
        }
    }

    public void onMouseDown(int posX, int posY) {
        CellScript cellScript = getCell(posX, posY);
        CellScript cellScript2 = getCell(posX + (verticalPlayer ? 0 : 1), posY + (verticalPlayer ? 1 : 0));

        if(!cellScript.isClicked && !cellScript2.isClicked) {
            verticalPlayer = !verticalPlayer;

            cellScript.material.color = Color.blue;
            cellScript.isClicked = true;
            cellScript2.material.color = Color.blue;
            cellScript2.isClicked = true;

            GameObject go = (GameObject)Instantiate(prefab, new Vector3((cellScript.gameObject.transform.position.x + cellScript2.transform.position.x) / 2, (cellScript.gameObject.transform.position.y + cellScript2.transform.position.y) / 2, 0), Quaternion.identity);
            go.GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    public void onMouseExit(int posX, int posY) {
        CellScript cellScript = getCell(posX, posY);
        CellScript cellScript2 = getCell(posX + (verticalPlayer ? 0 : 1), posY + (verticalPlayer ? 1 : 0));

        if(!cellScript.isClicked) {
            cellScript.material.color = Color.black;
        }

        if(!cellScript2.isClicked) {
            cellScript2.material.color = Color.black;
        }
    }

    CellScript getCell(int posX, int posY) {
        foreach(CellScript cellScript in cellScripts) {
            if(cellScript.posX == posX && cellScript.posY == posY) {
                return cellScript;
            }
        }

        return null;
    }
}
