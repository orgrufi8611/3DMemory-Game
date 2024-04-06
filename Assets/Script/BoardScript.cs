using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Shapes
{
    BALL,CUBE,LINE,CAPSUL,PLANE,count
}

public class BoardScript : MonoBehaviour
{
    public static BoardScript Instance { get; private set; }

    [SerializeField] TMP_Text timeIndicator;
    float timePlayed;

    [SerializeField] TMP_Text missesIndicator;
    int missed;
    
    CardScript[,] board = new CardScript[5, 3];
    [SerializeField] CardScript cardsPrefub;
    [SerializeField] GameObject[] shapes = new GameObject[5];
    [SerializeField] Shapes[,] boardShapes = new Shapes[5,3];

    [SerializeField] Vector2[] highlighted = new Vector2[3];
    [SerializeField] int highlightedCount;


    public bool playable;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        missesIndicator.text = "Wrong Guesses" + missed.ToString();
        highlightedCount = 0;
        playable = true;
        timePlayed = 0;
        missed = 0;
        for (int i = 0; i <board.GetLength(0); i++)
        {
            for (int j = 0; j <board.GetLength(1) ; j++)
            {
                Vector3 pos = new Vector3(-5f + i * 2.5f, 0, -3.5f + j * 3.5f);
                board[i, j] = Instantiate(cardsPrefub,pos,Quaternion.identity).GetComponent<CardScript>();
                board[i, j].SetLocation(i, j);
                boardShapes[i,j] = Shapes.count;
            }
        }
        SetShapesOnCards();
        /* Set Shapes with while
        //int setted;
        //for(int i  = 0; i < (int)Shapes.count; i++)
        //{
        //    setted = 0;
        //    while(setted < 3)
        //    {
        //        int r = Random.Range(0, 4);
        //        int c = Random.Range(0, 2);
        //        if(boardShapes[r,c] == Shapes.count)
        //        {
        //            boardShapes[r, c] = (Shapes)i;
        //            board[r, c].shape = shapes[i];
        //            setted++;
        //        }
        //    }
        //}*/
    }

    void SetShapesOnCards() 
    {
        List<CardScript> cards = new List<CardScript>();
        foreach(CardScript card in board) 
        {
            cards.Add(card);
        }
        for(int i = 0; i < (int)Shapes.count; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                int r = Random.Range(0, cards.Count);
                cards[r].SetShape(shapes[i],i);
                boardShapes[cards[r].GetRow(),cards[r].GetCol()] = (Shapes)i;
                cards.RemoveAt(r);
            }

        }
    }

    void Update()
    {
        timePlayed += Time.deltaTime;
        timeIndicator.text = "Time: "+((int)timePlayed).ToString();
    }
    
    public void HighLightCard(int r,int c)
    {
        if(highlightedCount < 3)
        {
            highlighted[highlightedCount] = new Vector2(r, c);
            highlightedCount++;
        }
        if(highlightedCount == 3)
        {
            FlipCards();
        }
    }

    void FlipCards()
    {
        foreach(Vector2 card in highlighted)
        {
            board[(int)card.x,(int)card.y].Flip();
        }
        CheckCorrect();
    }

    void CheckCorrect()
    {
        Shapes correct = boardShapes[(int)highlighted[0].x, (int)highlighted[0].y];
        int correctCount = 0;
        foreach(Vector2 card in highlighted)
        {
            print("comparing " + boardShapes[(int)card.x, (int)card.y].ToString() + " To " + correct.ToString());
            if (boardShapes[(int)card.x, (int)card.y] != correct)
            {
                Invoke(nameof(Wrong), 4);
                break;
            }
            correctCount++;
        }
        if(correctCount == 3)
        {
            Invoke(nameof(Correct), 4);
        }
    }

    void Correct()
    {
        foreach (Vector2 card in highlighted)
        {
            board[(int)card.x, (int)card.y].RemoveFromBoard();
        }
        playable = true;
        ResetHighlighted();
        CheckBoard();
    }

    void ResetHighlighted()
    {
        for(int i = 0;i<highlighted.Length; i++)
        {
            highlighted[i] = Vector2.zero;
        }
        highlightedCount = 0;
    }

    void CheckBoard()
    {
        int empty = 0;
        foreach(CardScript card in board)
        {
            if (card == null)
            {
                empty++;
            }
        }
        if(empty == board.GetLength(0) * board.GetLength(1))
        {
            Invoke(nameof(GameOver),1);
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void Wrong()
    {
        missed++;
        missesIndicator.text = "Wrong Guesses: " + missed.ToString();
        foreach(Vector2 card in highlighted)
        {
            board[(int)card.x, (int)card.y].SetBack();
        }
        playable = true;
        ResetHighlighted();
    }
}
