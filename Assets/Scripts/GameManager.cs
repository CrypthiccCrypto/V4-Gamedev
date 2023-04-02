using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

 public enum TURN {
        PLAYER_TURN = 1,
        AI_TURN = -1
}

public class GameManager : MonoBehaviour {
    public int board_size;
    public Dictionary<string, int> board;
    public Grid grid;
    private Board game;
    private MCTSBestMove AI;
    [SerializeField] private int turn;

    void Awake() {
        board = new Dictionary<string, int>();
        game = new Board(board_size);
        AI = new MCTSBestMove(board_size);

        Board.allPossibleCoordinates = new HashSet<string>();

        for(int i = -board_size + 1; i <= board_size - 1; i++) {        // add all possible moves to the set
            for(int j = -board_size + 1; j <= board_size - 1; j++) {
                int k = -(i + j);
                if(Math.Abs(k) <= board_size - 1) {
                    Board.allPossibleCoordinates.Add(Board.GenerateStringFromCoordinates(i, j, k));
                }
            }
        }

        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
    }

    public void UpdateGame(int x, int y, int z) {
        if(board.ContainsKey(Board.GenerateStringFromCoordinates(x, y, z))) {
                Debug.Log("Pawn already placed");
        }
        else {
            int result = game.PlayMove(x, y, z, turn, board);

            //
            // Code to display a tile on the board!
            grid.PlaceTile(x, y, z, turn);
            //

            if(result == (int)TURN.PLAYER_TURN) {
                Debug.Log("Player Won!");
            }
            else if(result == (int)TURN.AI_TURN) {
                Debug.Log("AI Won!");
            }
            else if(result == 2) {
                Debug.Log("Draw!");
            }

            SwitchTurn();
        }
    }

    public void SwitchTurn() {
        turn = -turn;

        if(turn == (int)TURN.AI_TURN) {
            AI.rootNode = new Node(turn, null, board, "");

            AI.FindBestMove(1000);      // parameterize this later according to difficulty and board size

            List<int> coords = Board.GenerateCoordinatesFromString(AI.bestNode.move);           // play the move in the actual game
            Debug.Log("Computer plays: " + coords[0] + " " + coords[1] + " " + coords[2]);
            UpdateGame(coords[0], coords[1], coords[2]);
        }
    }

    public int GetTurn() {
        return turn;
    }

    public int Getboard_size() {
        return board_size;
    }
}