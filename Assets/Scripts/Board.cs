using System.Collections;
using System.Collections.Generic;
using System;

public class Board      // only encodes the rules, does not contain an actual board
{
    int board_size;
    static HashSet<string> allPossibleCoordinates;

    public static string GenerateStringFromCoordinates(int i, int j, int k) {
        string s = "";
        s += i >= 0 ? "+" + i : i;
        s += j >= 0 ? "+" + j : j;
        s += k >= 0 ? "+" + k : k;

        return s;
    }

    public static List<int> GenerateCoordinatesFromString(string s) {
        int i=0,a=0,b=0,c=0;
        char[] c_arr = s.ToCharArray();
        
        foreach(char ch in c_arr){
            if(ch == '-' || ch == '+'){
                if(a==0) a++;
                else if(a==1){
                    a++;
                    b=i;
                }else{
                    c=i;
                }
            }
            i++;
        }
        string a1 = s.Substring(c+1);
        string a2 = s.Substring(b+1, c-(b+1));
        string a3 = s.Substring(1, b-1);

        
        List<int> list = new List<int>(); 
        list.Add(int.Parse(a3));  
        list.Add(int.Parse(a2)); 
        list.Add(int.Parse(a1));

        if(s[0]=='-') list[0]= -list[0];
        if(s[b]=='-') list[1]= -list[1];
        if(s[c]=='-') list[2]= -list[2];
        
        // Console.WriteLine(list[0] + " " + list[1] + " " + list[2]);
        
        return list;
    }

    public static List<string> AllPossibleMoves(Dictionary<string, int> gameState) {
        List<string> possibleMoves = new List<string>();
        // Console.WriteLine("__________________________________");
        foreach(string s in allPossibleCoordinates) {
            if(!gameState.ContainsKey(s)) {
                possibleMoves.Add(s);
                // Console.WriteLine(s);
            }
        }
        // Console.WriteLine("__________________________________");

        return possibleMoves;
    }
    
    public Board(int board_size) { // create a new empty board
        this.board_size = board_size;
        allPossibleCoordinates = new HashSet<string>();

        for(int i = -board_size + 1; i <= board_size - 1; i++) {        // add all possible moves to the set
            for(int j = -board_size + 1; j <= board_size - 1; j++) {
                int k = -(i + j);
                if(Math.Abs(k) <= board_size - 1) allPossibleCoordinates.Add(GenerateStringFromCoordinates(i, j, k));
            }
        }
    }

    public int PlayMove(int i, int j, int k, int move, Dictionary<string, int> board) {
        string s = Board.GenerateStringFromCoordinates(i, j, k);
        board.Add(s, move);

        return CheckBoard(i, j, k, move, board);
    }

    public void UnplayMove(int i, int j, int k, Dictionary<string, int> board) {
        string move = Board.GenerateStringFromCoordinates(i, j, k);

        if(board.ContainsKey(move)) {
            board.Remove(move);
        }
        else {
            Console.WriteLine("Unknown Error! Trying to remove unplayed move");
        }
    }

    public int CheckBoard(int i, int j, int k, int move, Dictionary<string, int> board) {         
        int dir1 = CheckConsecutive(i, j, k, 0, 1, -1, move, board);
        int dir2 = CheckConsecutive(i, j, k, 1, -1, 0, move, board);
        int dir3 = CheckConsecutive(i, j, k, 1, 0, -1, move, board);

        if(dir1 >= 4 || dir2 >= 4 || dir3 >= 4) return move;        // win
        else if(dir1 == 3 || dir2 == 3 || dir3 == 3) return -move;  // lose
        else if(board.Count == 3*board_size*board_size - 3*board_size + 1) {
            return 2;                                               // draw is assigned value 2     // optimize this
        }
        else return 0;                                              // neither
    }

    public int CheckConsecutive(int i, int j, int k, int dir_x, int dir_y, int dir_z, int move, Dictionary<string, int> board) {
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

    public int SimulateFromLeafNode(Node leaf) {
        if(leaf.winner != 0) { return leaf.winner; } // game has ended, return results

        int player = -leaf.player;
        Dictionary<string, int> gameState = new Dictionary<string, int>(leaf.gameState);
        Random rd = new Random();
        List<string> possibleMoves = Board.AllPossibleMoves(gameState);

        while(true) {
            int randomMove = rd.Next(0, possibleMoves.Count);
            List<int> coords = Board.GenerateCoordinatesFromString(possibleMoves[randomMove]);
            int result = PlayMove(coords[0], coords[1], coords[2], player, gameState);
            if(result != 0) return result;         // there has been a win/loss/draw

            player = -player;
            possibleMoves.RemoveAt(randomMove);    // this move can no longer be played
        }
    }
}