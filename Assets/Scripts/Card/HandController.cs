using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public static HandController handController;

    public int maxHandSize;
    public int startingHandSizePerPlayer;
    public int playerNumber;

    public float cardStartingHeight;
    public float cardHighlightHeight;
    public float cardStartingSize;
    public float cardHighlightSize;
    public float cardHighlightXBoarder;
    public float cardSpacing;

    public DeckController deck;
    public GameObject cardTemplate;

    public Color redCasterColor;
    public Color blueCasterColor;
    public Color greenCasterColor;
    public Color enemyCasterColor;

    private List< CardController> hand;
    // Start is called before the first frame update
    void Start()
    {
        if (HandController.handController == null)
            HandController.handController = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        hand = new List<CardController>();
        DrawFullHand();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    //Returns a random card from the deck from ONE color
    private CardController GetCard(int index)
    {
        CardController card = Instantiate(cardTemplate).GetComponent<CardController>();
        card.transform.SetParent(CanvasController.canvasController.uiCanvas.transform);
        card.SetCard(deck.DrawCard(index));

        return card;
    }

    //Adds a new card to the hand and reorders card positions from ONE color
    private void DrawCard(int index)
    {
        hand.Add(GetCard(index));
        ResetCardPositions();
    }
    */

    //Returns a random card from the deck fron ANY color
    private CardController GetAnyCard()
    {
        CardController card = Instantiate(cardTemplate).GetComponent<CardController>();
        card.transform.SetParent(CanvasController.canvasController.uiCanvas.transform);
        card.SetCard(deck.DrawAnyCard());

        return card;
    }

    //Adds a new card to the hand and reorders card positions from ANY color
    private void DrawAnyCard()
    {
        hand.Add(GetAnyCard());
        ResetCardPositions();
    }

    //Redraw all card positions so they're centered
    public void ResetCardPositions()
    {
        //Odd number of cards
        if (hand.Count % 2 == 1)
        {
            for (int i = 0; i < hand.Count; i++)
            {
                Vector2 cardLocation = new Vector2((i - hand.Count / 2) * cardSpacing, cardStartingHeight);
                hand[i].SetLocation(cardLocation);
                hand[i].transform.localScale = new Vector2(cardStartingSize,cardStartingSize);
            }
        }
        //Even number of cards
        else
        {
            for (int i = 0; i < hand.Count; i++)
            {
                Vector2 cardLocation = new Vector2((i - hand.Count / 2 + 0.5f) * cardSpacing, cardStartingHeight);
                hand[i].SetLocation(cardLocation);
                hand[i].transform.localScale = new Vector2(cardStartingSize, cardStartingSize);
            }
        }
    }

    //Removes the card from the hand and disable movement for the caster of the card
    public void RemoveCard(CardController removedCard)
    {
        hand.Remove(removedCard);
        ResetCardPositions();

        //Disables movement of all players with the removed card casterColor
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject caster = players[0];
        foreach (GameObject player in players)
            if (player.GetComponent<PlayerController>().GetColorTag() == removedCard.GetCard().casterColor)
                player.GetComponent<PlayerMoveController>().CommitMove();
    }

    //Draw cards untill hand is full
    public void DrawFullHand()
    {
        /*
        for (int i = 0; i < playerNumber; i++)
        {
            for (int j = 0; j < startingHandSizePerPlayer; j++)
            {
                if (deck.GetCurrentDeckSize(i) == 0)
                    deck.ResetDeck(i);
                DrawCard(i);
            }
        }*/
        for (int i = 0; i < maxHandSize; i++)
            DrawAnyCard();
        ResetCardPlayability(TurnController.turnController.GetCurrentMana());
    }

    public void ClearHand()
    {
        Debug.Log("trace");
        foreach (CardController card in hand)
        {
            DeckController.deckController.ReportUsedCard(card.GetCard());
            Destroy(card.gameObject);
        }
        hand = new List<CardController>();
        ResetCardPositions();
    }

    //Called from TurnController
    public void ResetCardPlayability (int mana)
    {
        foreach (CardController card in hand)
        {
            card.ResetPlayability(mana);
        }
    }

    public Color GetCasterColor(Card.CasterColor color)
    {
        switch(color)
        {
            case Card.CasterColor.Red:
                return redCasterColor;
            case Card.CasterColor.Blue:
                return blueCasterColor;
            case Card.CasterColor.Green:
                return greenCasterColor;
            case Card.CasterColor.Enemy:
                return enemyCasterColor;
            default:
                return enemyCasterColor;
        }
    }
}
