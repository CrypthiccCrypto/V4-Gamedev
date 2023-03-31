using System.Collections;
using System.Collections.Generic;
using System;

public class Node : IComparable {
    public Dictionary<string, int> gameState;
    public Node parent;
    public List<Node> children;
    public int player;        // current player of this node
    public string move;       // the move to be played at this node
    public int numVisits = 0, victory = 0, loss = 0, draw = 0;
    public double UCT = 0;
    public int winner = 0;     // 0 if no winner, +1 or -1 otherwise 

    Node(int player, Node parent, Dictionary<string, int> gameState, string move) {
        this.player = player;
        this.parent = parent;
        this.gameState = gameState;
        this.move = move;
        children = new List<Node>();
    }

    public int CompareTo(object obj) {
        Node other = (Node)obj;

        if(this.UCT >= other.UCT) { return 1; }
        else { return -1; }
    }

    void SetUCT() {
        if(numVisits == 0) UCT = Double.MaxValue;
        else UCT = ((victory + draw/2) / numVisits) + Math.Sqrt(2) * Math.Sqrt(Math.Log(parent.numVisits) / numVisits);
    }
}

class MCTSBestMove {

}