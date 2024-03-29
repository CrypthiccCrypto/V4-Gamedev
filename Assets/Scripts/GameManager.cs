using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TURN {
    PLAYER_TURN = 1,
    AI_TURN = -1,
    NO_TURN = 0 // game end
}

public enum DIFFICULTY {
    EASY = 1,
    MEDIUM = 2,
    HARD = 3
}

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public int board_size = 5;

    public GameObject pauseMenu;
    public GameObject WonUI;
    public GameObject LoseUI;
    public Dictionary<string, int> board;
    public Grid grid;
    public Game game;
    [SerializeField] public DIFFICULTY difficulty;
    public const int applyMCTSLimit = 10000;
    private MCTSBestMove AI;
    private bool aiFirst;
    [SerializeField] private int turn;
    private bool gameStarted = false;

    int currTurn;

    void Awake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if(gameStarted && Input.GetButtonDown("Cancel"))
        {
            currTurn = turn;
            turn = (int)TURN.NO_TURN;
            pauseMenu.SetActive(true);
        }
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        gameStarted = false;

        if(!string.Equals(scene.name, "GameScene")) return;

        gameStarted = true;
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
        pauseMenu = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
        WonUI = GameObject.Find("Canvas").transform.GetChild(2).gameObject;
        LoseUI = GameObject.Find("Canvas").transform.GetChild(3).gameObject;

        if(aiFirst) {
            turn = (int)TURN.PLAYER_TURN;
        }
        else {
            turn = (int)TURN.AI_TURN;
        }

        SwitchTurn();   // Set the game into motion
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
                WonUI.SetActive(true);
                turn = (int)TURN.NO_TURN;
            }
            else if(result == (int)TURN.AI_TURN) {
                LoseUI.SetActive(true);
                turn = (int)TURN.NO_TURN;
            }
            else if(result == 2) {
                turn = (int)TURN.NO_TURN;
            }
            else {
                SwitchTurn();
            }
        }
    }

    public void SwitchTurn() {
        turn = -turn;

        if(turn == (int)TURN.AI_TURN) {
            AI.simulator = game;
            int[] coords = null;

            if(game.plays < applyMCTSLimit) {   // minimax logic, only logic used for now
                int depth = GetDepth();
                AI.MiniMax(turn, true, 2, -2, depth);
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

    public int GetDepth() {
        float f = UnityEngine.Random.Range(0f, 1f);
        switch(difficulty) {
            case DIFFICULTY.EASY:
            if(f <= 0.5) {
                return 0;
            }
            else {
                return 1;
            }
            case DIFFICULTY.MEDIUM:
            if(f <= 0.6) {
                return 1;
            }
            else if(f <= 0.9) {
                return 2;
            }
            else {
                return 3;
            }

            case DIFFICULTY.HARD:
            return 4;
        }
        return 0;
    }

    public int GetTurn() {
        return turn;
    }

    public int GetBoardSize() {
        return board_size;
    }

    public void SetDifficulty(DIFFICULTY difficulty) {
        this.difficulty = difficulty;
    }

    public void SetBoardSize(int board_size) {
        this.board_size = board_size;
    }

    public void SetAiFirst(bool aiFirst) {
        this.aiFirst = aiFirst;
    }

    public void Continue()
    {
        turn = currTurn;
        pauseMenu.SetActive(false);
    }
}