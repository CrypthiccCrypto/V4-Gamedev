using System.Collections;
using System.Collections.Generic;
using System;

public class Board
{

    Dictionary<string, int> board;
    int board_size;
    static HashSet<string> allPossibleCoordinates;

    static string GenerateStringFromCoordinates(int i, int j, int k) {
        string s = "";
        s += i >= 0 ? "+" + i : i;
        s += j >= 0 ? "+" + j : j;
        s += k >= 0 ? "+" + k : k;

        return s;
    }

    static List<int> GenerateCoordinatesFromString(string s) {

    }

    static List<string> AllPossibleMoves(Dictionary<string, int> gameState) {
        List<string> possibleMoves = new List<string>();

        foreach(string s in allPossibleCoordinates) {
            if(!gameState.ContainsKey(s)) {
                possibleMoves.Add(s);
            }
        }

        return possibleMoves;
    }
    
    public Board(int board_size) { // create a new empty board
        this.board = new Dictionary<string, int>();
        this.board_size = board_size;

        for(int i = -board_size + 1; i <= board_size - 1; i++) {        // add all possible moves to the set
            for(int j = -board_size + 1; j <= board_size - 1; j++) {
                int k = -(i + j);
                allPossibleCoordinates.Add(GenerateStringFromCoordinates(i, j, k));
            }
        }
    }

    // public Board(Dictionary<string, int> board, int board_size) {   // copy a board for the AI to simulate in
    //     this.board = new Dictionary<string, int>(board);
    //     this.board_size = board_size;
    // }

    int PlayMove(int i, int j, int k, int move, Dictionary<string, int> board) {
        string s = Board.GenerateStringFromCoordinates(i, j, k);
        board.Add(s, move);

        return CheckBoard(i, j, k, move, board);
    }

    int CheckBoard(int i, int j, int k, int move, Dictionary<string, int> board) {         
        int dir1 = CheckConsecutive(i, j, k, 0, 1, -1, move);
        int dir2 = CheckConsecutive(i, j, k, 1, -1, 0, move);
        int dir3 = CheckConsecutive(i, j, k, 1, 0, -1, move);

        if(dir1 >= 4 || dir2 >= 4 || dir3 >= 4) return move;        // win
        else if(dir1 == 3 || dir2 == 3 || dir3 == 3) return -move;  // lose
        else return 0;                                              // neither
    }

    int CheckConsecutive(int i, int j, int k, int dir_x, int dir_y, int dir_z, int move) {
        int num_consec = 0;

        int cnt_x = i;
        int cnt_y = j;
        int cnt_z = k;

        while(board.ContainsKey(GenerateStringFromCoordinates(cnt_x, cnt_y, cnt_z))) {
            if(board[GenerateStringFromCoordinates(cnt_x, cnt_y, cnt_z)] == move) {
                num_consec++;

                cnt_x += dir_x;
                cnt_y += dir_y;
                cnt_z += dir_z;
            }
            else {
                break;
            }
        }

        cnt_x = i;
        cnt_y = j;
        cnt_z = k;

        while(board.ContainsKey(GenerateStringFromCoordinates(cnt_x, cnt_y, cnt_z))) {
            if(board[GenerateStringFromCoordinates(cnt_x, cnt_y, cnt_z)] == move) {
                num_consec++;

                cnt_x -= dir_x;
                cnt_y -= dir_y;
                cnt_z -= dir_z;
            }
            else {
                break;
            }
        }
        num_consec--;

        return num_consec;        
    }

    int SimulateFromLeafNode(Node leaf) {
        if(leaf.winner != 0) { return leaf.winner; } // game has ended, return results
        int player = -leaf.player;
        Dictionary<string, int> gameState = new Dictionary<string, int>(leaf.gameState);
        Random rd = new Random();

        while(true) {
            List<string> possibleMoves = Board.AllPossibleMoves(leaf.gameState);
            int randomMove = rd.Next(0, possibleMoves.Count);
            // int result = PlayMove();
        }
        return 0;
    }
}
