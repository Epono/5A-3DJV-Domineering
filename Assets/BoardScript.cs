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

    Move move;

    public enum algos { Minimax, Negamax, AlphaBetaMinimax, AlphaBetaNegamax };

    [SerializeField]
    algos algo;

    [SerializeField]
    int depth = 1, alpha = 1, beta = 1;

    CellScript[][] cellScriptsTable = new CellScript[8][];

    CellScript getCellOld(int posX, int posY) {
        foreach(CellScript cellScript in cellScripts) {
            if(cellScript.posX == posX && cellScript.posY == posY) {
                return cellScript;
            }
        }

        return null;
    }

    CellScript getCell(int posX, int posY) {
        if(posX < 8 && posY < 8) {
            return cellScriptsTable[posX][posY];
        } else {
            return null;
        }
    }

    void Start() {

    }

    void Init() {
        for(int x = 0; x < 8; ++x) {
            cellScriptsTable[x] = new CellScript[8];
            for(int y = 0; y < 8; ++y) {
                cellScriptsTable[x][y] = getCellOld(x, y);
            }
        }

        for(int x = 0; x < 8; ++x) {
            for(int y = 0; y < 8; ++y) {
                Debug.Log(cellScriptsTable[x][y].posX + ", " + cellScriptsTable[x][y].posY);
            }
        }
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Return)) {
            switch(algo) {
                case algos.Minimax:
                    MinimaxMax(depth);
                    break;
                case algos.Negamax:
                    Negamax(depth);
                    break;
                case algos.AlphaBetaMinimax:
                    AlphaBetaMax(depth, alpha, beta);
                    break;
                case algos.AlphaBetaNegamax:
                    AlphaBetaNegamax(depth, alpha, beta);
                    break;
            }

            onMouseDown(move.getPosX(), move.getPosY());

            if(getPossibilitiesCount(verticalPlayer) == 0) {

            }
        } else if(Input.GetKeyDown(KeyCode.A)) {
            Init();
        }
        text.text = "Possibilités joueur vertical : " + getPossibilitiesCount(true) + "\n" + "Possibilités joueur horizontal : " + getPossibilitiesCount(false);
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

    List<Move> getPossibilities(bool tempVerticalPlayer) {
        List<Move> moves = new List<Move>();
        for(int y = 0; y < 8; ++y) {
            for(int x = 0; x < 8; ++x) {
                Move move = new Move(x, y, tempVerticalPlayer);
                if(isValidMove(move)) {
                    moves.Add(move);
                }
            }
        }

        return moves;
    }
    int getPossibilitiesCount(bool tempVerticalPlayer) {
        int count = 0;
        for(int y = 0; y < 8; ++y) {
            for(int x = 0; x < 8; ++x) {
                Move move = new Move(x, y, tempVerticalPlayer);
                if(isValidMove(move)) {
                    count++;
                }
            }
        }

        return count;
    }

    bool isValidMove(Move move) {
        int posX = move.getPosX();
        int posY = move.getPosY();
        bool tempVerticalPlayer = move.getTempVerticalPlayer();

        CellScript cellScript = getCell(posX, posY);
        CellScript cellScript2 = getCell(posX + (tempVerticalPlayer ? 0 : 1), posY + (tempVerticalPlayer ? 1 : 0));

        return (cellScript != null && cellScript2 != null && !cellScript.isClicked && !cellScript2.isClicked);
    }

    void play(Move move) {
        int posX = move.getPosX();
        int posY = move.getPosY();
        bool tempVerticalPlayer = move.getTempVerticalPlayer();

        CellScript cellScript = getCell(posX, posY);
        CellScript cellScript2 = getCell(posX + (verticalPlayer ? 0 : 1), posY + (verticalPlayer ? 1 : 0));

        cellScript.isClicked = true;
        cellScript2.isClicked = true;
    }

    void undo(Move move) {
        int posX = move.getPosX();
        int posY = move.getPosY();
        bool tempVerticalPlayer = move.getTempVerticalPlayer();

        CellScript cellScript = getCell(posX, posY);
        CellScript cellScript2 = getCell(posX + (verticalPlayer ? 0 : 1), posY + (verticalPlayer ? 1 : 0));

        cellScript.isClicked = false;
        cellScript2.isClicked = false;
    }

    int evaluate(bool tempVerticalPlayer) {
        int possibilitiesPlayer = getPossibilitiesCount(tempVerticalPlayer);
        int possibilitiesAdversary = getPossibilitiesCount(!tempVerticalPlayer);

        return possibilitiesPlayer - possibilitiesAdversary;
    }

    //////////////////////////////////////////////////////////////////////////// Minimax
    int MinimaxMax(int depth) {
        if(depth == 0) {
            return evaluate(verticalPlayer);
        }
        int eval = int.MinValue;
        List<Move> moves = getPossibilities(verticalPlayer);
        foreach(Move m in moves) {
            play(m);
            int e = MinimaxMin(depth - 1);
            undo(m);
            if(e > eval) {
                eval = e;
                move = m;
            }
        }

        return eval;
    }

    int MinimaxMin(int depth) {
        if(depth == 0) {
            return evaluate(verticalPlayer);
        }
        int eval = int.MaxValue;
        List<Move> moves = getPossibilities(verticalPlayer);
        foreach(Move m in moves) {
            play(m);
            int e = MinimaxMax(depth - 1);
            undo(m);
            if(e < eval) {
                eval = e;
                move = m;
            }
        }

        return eval;
    }

    //////////////////////////////////////////////////////////////////////////// Negamax
    int Negamax(int depth) {
        if(depth == 0) {
            return evaluate(verticalPlayer);
        }
        int eval = int.MinValue;
        List<Move> moves = getPossibilities(verticalPlayer);
        foreach(Move m in moves) {
            play(m);
            int e = -Negamax(depth - 1);
            undo(m);
            if(e > eval) {
                eval = e;
                move = m;
            }
        }

        return eval;
    }

    //////////////////////////////////////////////////////////////////////////// Alpha-Beta Minimax
    int AlphaBetaMax(int depth, int alpha, int beta) {
        if(depth == 0) {
            return evaluate(verticalPlayer);
        }
        List<Move> moves = getPossibilities(verticalPlayer);
        foreach(Move m in moves) {
            play(m);
            int e = AlphaBetaMin(depth - 1, alpha, beta);
            undo(m);
            if(e > alpha) {
                alpha = e;
                move = m;
                if(alpha >= beta) {
                    // ?
                    //move = m;
                    return beta;
                }
            }
        }

        return alpha;
    }

    int AlphaBetaMin(int depth, int alpha, int beta) {
        if(depth == 0) {
            return evaluate(verticalPlayer);
        }
        List<Move> moves = getPossibilities(verticalPlayer);
        foreach(Move m in moves) {
            play(m);
            int e = AlphaBetaMax(depth - 1, alpha, beta);
            undo(m);
            if(e < beta) {
                beta = e;
                move = m;
                if(alpha >= beta) {
                    // ?
                    //move = m;
                    return alpha;
                }
            }
        }

        return beta;
    }

    //////////////////////////////////////////////////////////////////////////// Alpha-Beta Negamax
    int AlphaBetaNegamax(int depth, int alpha, int beta) {
        if(depth == 0) {
            return evaluate(verticalPlayer);
        }
        List<Move> moves = getPossibilities(verticalPlayer);
        foreach(Move m in moves) {
            play(m);
            int e = -AlphaBetaNegamax(depth - 1, -beta, -alpha);
            undo(m);
            if(e > alpha) {
                alpha = e;
                move = m;
                if(alpha >= beta) {
                    // ?
                    //move = m;
                    return beta;
                }
            }
        }

        return alpha;
    }
}
