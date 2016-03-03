using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoardScript : MonoBehaviour {

    [SerializeField]
    List<CellScript> cellScripts;

    [SerializeField]
    GameObject prefab;

    [SerializeField]
    Text text;

    bool verticalPlayer = true;

    void Start() {

    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Return)) {
            onMouseDown((int)(Random.value * 7), (int)(Random.value * 7));
        }
        text.text = "Possibilités joueur vertical : " + getPossibilities(true) + "\n" + "Possibilités joueur horizontal : " + getPossibilities(false);
    }

    public void onMouseOver(int posX, int posY) {
        CellScript cellScript = getCell(posX, posY);
        CellScript cellScript2 = getCell(posX + (verticalPlayer ? 0 : 1), posY + (verticalPlayer ? 1 : 0));

        if(cellScript != null && cellScript2 != null && !cellScript.isClicked && !cellScript2.isClicked) {
            cellScript.material.color = Color.white;
            cellScript2.material.color = Color.white;
        }
    }

    public void onMouseDown(int posX, int posY) {
        CellScript cellScript = getCell(posX, posY);
        CellScript cellScript2 = getCell(posX + (verticalPlayer ? 0 : 1), posY + (verticalPlayer ? 1 : 0));

        if(cellScript != null && cellScript2 != null && !cellScript.isClicked && !cellScript2.isClicked) {
            verticalPlayer = !verticalPlayer;

            cellScript.material.color = Color.blue;
            cellScript.isClicked = true;
            cellScript2.material.color = Color.blue;
            cellScript2.isClicked = true;

            GameObject go = (GameObject)Instantiate(prefab, new Vector3((cellScript.gameObject.transform.position.x + cellScript2.transform.position.x) / 2, (cellScript.gameObject.transform.position.y + cellScript2.transform.position.y) / 2, 0), Quaternion.identity);
            go.GetComponent<Renderer>().material.color = Color.blue;
            CellScript temp = go.GetComponent<CellScript>();
            temp.isTrigger = false;
        }
    }

    public void onMouseExit(int posX, int posY) {
        CellScript cellScript = getCell(posX, posY);
        CellScript cellScript2 = getCell(posX + (verticalPlayer ? 0 : 1), posY + (verticalPlayer ? 1 : 0));

        if(cellScript != null && !cellScript.isClicked) {
            cellScript.material.color = Color.black;
        }

        if(cellScript2 != null && !cellScript2.isClicked) {
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

    int getPossibilities(bool tempVerticalPlayer) {
        int count = 0;
        for(int y = 0; y < 8; ++y) {
            for(int x = 0; x < 8; ++x) {
                if(isValidMove(x, y, tempVerticalPlayer)) {
                    count++;
                }
            }
        }

        return count;
    }

    bool isValidMove(int posX, int posY, bool tempVerticalPlayer) {
        CellScript cellScript = getCell(posX, posY);
        CellScript cellScript2 = getCell(posX + (tempVerticalPlayer ? 0 : 1), posY + (tempVerticalPlayer ? 1 : 0));

        return (cellScript != null && cellScript2 != null && !cellScript.isClicked && !cellScript2.isClicked);
    }

    GameObject move(int posX, int posY, bool tempVerticalPlayer) {
        CellScript cellScript = getCell(posX, posY);
        CellScript cellScript2 = getCell(posX + (verticalPlayer ? 0 : 1), posY + (verticalPlayer ? 1 : 0));

        if(cellScript != null && cellScript2 != null && !cellScript.isClicked && !cellScript2.isClicked) {
            cellScript.material.color = Color.blue;
            cellScript.isClicked = true;
            cellScript2.material.color = Color.blue;
            cellScript2.isClicked = true;

            GameObject go = (GameObject)Instantiate(prefab, new Vector3((cellScript.gameObject.transform.position.x + cellScript2.transform.position.x) / 2, (cellScript.gameObject.transform.position.y + cellScript2.transform.position.y) / 2, 0), Quaternion.identity);
            go.GetComponent<Renderer>().material.color = Color.blue;
            CellScript temp = go.GetComponent<CellScript>();
            temp.isTrigger = false;

            return go;
        }

        return null;
    }

    void undo(int posX, int posY, bool tempVerticalPlayer, GameObject go) {
        CellScript cellScript = getCell(posX, posY);
        CellScript cellScript2 = getCell(posX + (verticalPlayer ? 0 : 1), posY + (verticalPlayer ? 1 : 0));

        cellScript.material.color = Color.black;
        cellScript.isClicked = false;
        cellScript2.material.color = Color.black;
        cellScript2.isClicked = false;

        Destroy(go);
    }
}
