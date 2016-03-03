using UnityEngine;
using System.Collections;

public class Move {

    int posX, posY;
    bool tempVerticalPlayer;

    public Move(int posX, int posY, bool tempVerticalPlayer) {
        this.posX = posX;
        this.posY = posY;
        this.tempVerticalPlayer = tempVerticalPlayer;
    }

    public int getPosX() {
        return posX;
    }

    public int getPosY() {
        return posY;
    }

    public bool getTempVerticalPlayer() {
        return tempVerticalPlayer;
    }
}
