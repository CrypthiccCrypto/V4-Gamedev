using System.Collections;
using System.Collections.Generic;
using System;

public class Node {
    public Dictionary<string, int> gameState;
    public Node parent;
    public List<Node> children;
    public int player;        // current player of this node
    public string move;       // the move to be played at this node
    public int numVisits = 0, victory = 0, loss = 0, draw = 0;
    public double UCT = 0;
    public int winner = 0;     // 0 if no winner, +1 or -1 otherwise 

    public Node(int player, Node parent, Dictionary<string, int> gameState, string move) {
        this.player = player;
        this.parent = parent;
        this.gameState = gameState;
        this.move = move;
        children = new List<Node>();
    }

    // public int CompareTo(object obj) {
    //     Node other = (Node)obj;

    //     if(this.UCT >= other.UCT) { return 1; }
    //     else { return -1; }
    // }

    public void SetUCT() {
        if(numVisits == 0) UCT = Double.MaxValue;       // if a node has not been visited then explore it
        else UCT = ((victory + draw/2) / numVisits) + Math.Sqrt(2) * Math.Sqrt(Math.Log(parent.numVisits) / numVisits);
    }
}

public class MCTSBestMove {
    public Board simulator;
    public Node rootNode;
    public Node bestNode;

    public MCTSBestMove(int boardsize) {
        // OBTAIN ACTUAL VALUE FROM GAME MANAGER// OBTAIN ACTUAL VALUE FROM GAME MANAGER// OBTAIN ACTUAL VALUE FROM GAME MANAGER// OBTAIN ACTUAL VALUE FROM GAME MANAGER
        simulator = new Board(boardsize);   
        // OBTAIN ACTUAL VALUE FROM GAME MANAGER// OBTAIN ACTUAL VALUE FROM GAME MANAGER// OBTAIN ACTUAL VALUE FROM GAME MANAGER// OBTAIN ACTUAL VALUE FROM GAME MANAGER// OBTAIN ACTUAL VALUE FROM GAME MANAGER// OBTAIN ACTUAL VALUE FROM GAME MANAGER

        rootNode = null;    // set later from main
        bestNode = null;    // set from findbestmove function
    }

    public Node SelectNodeForRollout() {
        Node currentNode = rootNode;

        while(true) {
            if(currentNode.winner != 0) {       // game over
                return currentNode;
            } 
            if (currentNode.children.Count == 0) {
                GenerateChildren(currentNode);
                return currentNode.children[0];
            } 
            else {
                foreach(Node child in currentNode.children) {
                    child.SetUCT();
                }
                currentNode.children.Sort((y, x) => x.UCT.CompareTo(y.UCT));
                
                // if(currentNode.children.Count >= 1) {
                //     Console.WriteLine(currentNode.children[0].UCT + " " + currentNode.children[1].UCT);
                // }
                currentNode = currentNode.children[0];
                if(currentNode.numVisits == 0) {
                    return currentNode;
                }
            }
        }
    }

    public void GenerateChildren(Node n) {     // expansion phase
        List<string> possibleMoves = Board.AllPossibleMoves(n.gameState);
        int player = -n.player;

        foreach(string s in possibleMoves) {
            List<int> coords = Board.GenerateCoordinatesFromString(s);

            int result = simulator.PlayMove(coords[0], coords[1], coords[2], player, n.gameState);
            
            Node child = new Node(player, n, n.gameState, s);
            child.winner = result;
            n.children.Add(child);

            simulator.UnplayMove(coords[0], coords[1], coords[2], n.gameState);
        }
    }

    public void BackPropagate(Node n, int won) {
        Node cnt = n;
        while(cnt != null) {
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

    public void FindBestMove(int iterations) {     // replace with time later
        for(int i = 0; i < iterations; i++) {
            Node leafToSimulateFrom = SelectNodeForRollout();
            int won = simulator.SimulateFromLeafNode(leafToSimulateFrom);
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
}
