using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class Game
{
    public int plays;
    public int board_size;
    public bool [,] board;

    public Game(int board_size) {
        this.board_size = board_size;
        board = new bool[2, 8 * board_size * board_size * board_size + 4 * board_size * board_size + 3 * board_size];
        this.plays = 0;
    }

    public int CubicToIndex(int i, int j, int k) {
        int tmp = 2*board_size - 1;
        if(Math.Abs(i) >= board_size || Math.Abs(j) >= board_size || Math.Abs(k) >= board_size) {
            return board.GetLength(1) - 1;
        }
        return (i+board_size - 1)*tmp*tmp +(j+board_size - 1)*tmp + (k+board_size - 1);
    }

    public int[] IndexToCubic(int idx) {
        int tmp = 2*board_size - 1;
        int k = (idx % tmp) - board_size + 1;
        idx /= tmp;
        int j = (idx % tmp) - board_size + 1;
        idx /= tmp;
        int i = idx - board_size + 1;

        int[] coords = {i, j, k};
        return coords;
    }

    public void PlayMove(int i, int j, int k, int player) {
        int idx = CubicToIndex(i, j, k);
        board[((player + 1) >> 1), idx] = true;
        plays += 1;
    }

    public void UnplayMove(int i, int j, int k, int player) {
        int idx = CubicToIndex(i, j, k);
        board[((player + 1) >> 1), idx] = false;
        plays -= 1;
    }

    public bool Draw() {
        return plays == (3*board_size*board_size - 3*board_size + 1);
    }
}

public static class Board      // only encodes the rules, does not contain an actual board
{
    public static HashSet<int> allPossibleIndices;

    public static int CheckBoard(int i, int j, int k, int player, Game game) {
        int dir1 = CheckConsecutive(i, j, k, 0, 1, -1, player, game);
        int dir2 = CheckConsecutive(i, j, k, 1, -1, 0, player, game);
        int dir3 = CheckConsecutive(i, j, k, 1, 0, -1, player, game);

        if (dir1 >= 4 || dir2 >= 4 || dir3 >= 4) return player;
        else if (dir1 == 3 || dir2 == 3 || dir3 == 3) return -player;
        else if (game.Draw()) return 2;
        else return 0;
    }

    public static int CheckConsecutive(int i, int j, int k, int dir_x, int dir_y, int dir_z, int player, Game game) {
        int num_consec = 0;

        int cnt_x = i;
        int cnt_y = j;
        int cnt_z = k;

        int player_id = ((player + 1) >> 1); 

        while(game.board[player_id, game.CubicToIndex(cnt_x, cnt_y, cnt_z)]) {
            num_consec++;

            cnt_x += dir_x;
            cnt_y += dir_y;
            cnt_z += dir_z;
        }
        
        cnt_x = i;
        cnt_y = j;
        cnt_z = k;
        
        while(game.board[player_id, game.CubicToIndex(cnt_x, cnt_y, cnt_z)]) {
            num_consec++;

            cnt_x -= dir_x;
            cnt_y -= dir_y;
            cnt_z -= dir_z;
        }

        num_consec--;
        return num_consec;
    }

    public static List<int> AllPossibleMoves(Game game) {
        List<int> possibleMoves = new List<int>();

        foreach(int idx in allPossibleIndices) {
            if(game.board[0, idx] || game.board[1, idx]) continue;

            possibleMoves.Add(idx);
        }

        return possibleMoves;
    }
}