using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Node {
    public Node parent;
    public List<Node> children;
    public int player;        // current player of this node
    public int move;       // the move to be played at this node
    public int numVisits = 0, victory = 0, loss = 0, draw = 0;
    public double UCT = 0;
    public int winner = 0;     // 0 if no winner, +1, -1, 2 otherwise 

    public Node(int player, Node parent, int move) {
        this.player = player;
        this.parent = parent;
        this.move = move;
        children = new List<Node>();
    }

    public void SetUCT() {
        if(numVisits == 0) UCT = Double.MaxValue;       // if a node has not been visited then explore it
        else UCT = ((victory + (draw/2)) / numVisits) + Math.Sqrt(2) * Math.Sqrt(Math.Log(parent.numVisits) / numVisits);
    }
}

public class MCTSBestMove {
    public Game simulator;
    public Node rootNode;
    public Node bestNode;

    
    // MINIMAX USED ONLY
    public int max_depth = 5;
    public int best_minimax_move;

    public MCTSBestMove(int boardsize) {
        simulator = new Game(boardsize);   

        rootNode = null;    // set later from main
        bestNode = null;    // set from findbestmove function
    }

    // simulate 1 round
    public Node SelectNodeForRollout() {       // selection phase
        Node currentNode = rootNode;

        while(true) {
            if(currentNode.winner != 0) {       // game over
                return currentNode;
            } 
            if (currentNode.children.Count == 0) {
                GenerateChildren(currentNode);
                
                currentNode = currentNode.children[0];
                
                int[] coords = simulator.IndexToCubic(currentNode.move);
                simulator.PlayMove(coords[0], coords[1], coords[2], currentNode.player);
            } 
            else {
                foreach(Node child in currentNode.children) {
                    child.SetUCT();
                }
                currentNode.children.Sort((y, x) => x.UCT.CompareTo(y.UCT));
                
                currentNode = currentNode.children[0];
                
                int[] coords = simulator.IndexToCubic(currentNode.move);
                simulator.PlayMove(coords[0], coords[1], coords[2], currentNode.player);
            }
        }
    }

    public void GenerateChildren(Node n) {     // expansion phase
        List<int> possibleMoves = Board.AllPossibleMoves(simulator);
        int player = -n.player;

        foreach(int idx in possibleMoves) {
            int[] coords = simulator.IndexToCubic(idx);            
            simulator.PlayMove(coords[0], coords[1], coords[2], player);
            
            int result = Board.CheckBoard(coords[0], coords[1], coords[2], player, simulator);
            Node child = new Node(player, n, idx);
            child.winner = result;
            n.children.Add(child);

            simulator.UnplayMove(coords[0], coords[1], coords[2], player);
        }
    }

    public void BackPropagate(Node n, int won) {
        Node cnt = n;
        while(cnt != null) {
            int[] coords = simulator.IndexToCubic(cnt.move);
            if (cnt != rootNode) simulator.UnplayMove(coords[0], coords[1], coords[2], cnt.player);
            
            cnt.numVisits++;
            if(won == 2) {
                cnt.draw++;
            }
            else if(cnt.player == won) {
                cnt.victory++;
            }
            else {
                cnt.loss++;
            }

            cnt = cnt.parent;
        }
    }

    public void FindBestMove(int iterations) {
        for(int i = 0; i < iterations; i++) {
            Node leafToSimulateFrom = SelectNodeForRollout();
            int won = leafToSimulateFrom.winner;
            BackPropagate(leafToSimulateFrom, won);
        }

        int mostVisits = 0;
        foreach(Node child in rootNode.children) {
            if(child.numVisits > mostVisits) {
                mostVisits = child.numVisits;
                bestNode = child;
            }
        }
    }

    public int MiniMax(int turn, bool root, int beta, int alpha, int height) {
        if(height < 0) { return 0; }
        bool max_node = (turn == (int)TURN.PLAYER_TURN);    // Player is MAXNODE, AI is MINNODE

        int best_score = max_node ? Int32.MinValue : Int32.MaxValue;

        List<int> possibleMoves = Board.AllPossibleMoves(simulator);

        foreach (int idx in possibleMoves) {
            int[] coords = simulator.IndexToCubic(idx);
            simulator.PlayMove(coords[0], coords[1], coords[2], turn);
            int result = Board.CheckBoard(coords[0], coords[1], coords[2], turn, simulator);

            if (result != 0) {
                simulator.UnplayMove(coords[0], coords[1], coords[2], turn);

                if (result == 2) { result = 0; }
                else if (result == 1) {
                    if (max_node) { return result; }
                } else if (result == -1) {
                    if (!max_node) {
                        if (root) { best_minimax_move = idx; }
                        return result;
                    }
                }
            } else {
                result = MiniMax(-turn, false, beta, alpha, height - 1);
                simulator.UnplayMove(coords[0], coords[1], coords[2], turn);
            }

            if (max_node && result > best_score) {
                if (result >= beta) { return beta; }
                best_score = result;
                alpha = Math.Max(alpha, result);
            } else if (!max_node && result < best_score) {
                if (root) { best_minimax_move = idx; }
                else if (result <= alpha) { return alpha; }
                best_score = result;
                beta = Math.Min(beta, result);
            }
        }

        return best_score;
    }
}
