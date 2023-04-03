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
    public Game game;
    public const int applyMCTSLimit = 10000;
    private MCTSBestMove AI;
    [SerializeField] private int turn;

    void Awake() {
        game = new Game(board_size);
        AI = new MCTSBestMove(board_size);

        Board.allPossibleIndices = new HashSet<int>();

        for(int i = -board_size + 1; i <= board_size - 1; i++) {        // add all possible moves to the set
            for(int j = -board_size + 1; j <= board_size - 1; j++) {
                int k = -(i + j);
                if(Math.Abs(k) <= board_size - 1) {
                    Board.allPossibleIndices.Add(game.CubicToIndex(i, j, k));
                }
            }
        }

        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
    }

    public void UpdateGame(int x, int y, int z) {
        int idx = game.CubicToIndex(x, y, z);
        if(game.board[0, idx] || game.board[1, idx]) {
                Debug.Log("Don't place on the same tile");
        }
        else {
            game.PlayMove(x, y, z, turn);
            int result = Board.CheckBoard(x, y, z, turn, game);

            // Code to display a tile on the board!
            grid.PlaceTile(x, y, z, turn);
            

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
            AI.simulator = game;
            int[] coords = null;

            if(game.plays < applyMCTSLimit) {   // minimax logic, only logic used for now
                AI.MiniMax(turn, true, 2, -2, 0);
                coords = game.IndexToCubic(AI.best_minimax_move);
            }
            else {
                AI.rootNode = new Node(-turn, null, -1); // since player turn has just been played

                AI.FindBestMove(10000);                 
                coords = game.IndexToCubic(AI.bestNode.move);           // play the move in the actual game
            }

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