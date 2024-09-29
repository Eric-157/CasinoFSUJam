using System;
using System.Collections;
using System.Linq;
using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public AudioSource audioSource;

    private Deck deck = new();

    private int slotIndex;

    public override void Spawned()
    {
        slotIndex = Array.FindIndex(Slot.slots, s => !s.active);
        Slot.slots[slotIndex].active = true;

        RenderPlayer();
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        Slot.slots[slotIndex].active = false;
        Slot.slots[slotIndex].text.text = "(no player)";
    }

    public enum Action
    {
        None,
        Hit,
        Stand,
    }

    public PlayerInputs inputs;

    public override void FixedUpdateNetwork()
    {
        if (handLength == 0)
        {
            deck.Reset();
            deck.Shuffle();
            addToHand(deck.DrawBasic());
            addToHand(deck.DrawBasic());
        }

        if (inputs.Action is Action.Stand && roundStatus is RoundStatus.Normal)
        {
            playerStatus = PlayerStatus.Stand;
            inputs.Action = Action.None;
        }

        if (inputs.Action is Action.Hit && roundStatus is RoundStatus.Normal)
        {
            addToHand(deck.DrawAny());

            //im so sorry.
            StartCoroutine(Ability());


            

            inputs.Action = Action.None;
        }

        if (roundStatus is RoundStatus.Ready &&
            FindObjectsByType<Player>(FindObjectsSortMode.None)
            .All(status => status.roundStatus is RoundStatus.Ready or RoundStatus.Resetting))
        {
            roundStatus = RoundStatus.Resetting;
            playerStatus = PlayerStatus.Playing;
            inputs.Action = Action.None;
            handLength = 0;
            handTotal = 0;

            Debug.Log("Resetting!");
        }

        if (roundStatus is RoundStatus.Resetting &&
            FindObjectsByType<Player>(FindObjectsSortMode.None)
            .All(status => status.roundStatus is RoundStatus.Resetting or RoundStatus.Normal))
        {
            roundStatus = RoundStatus.Normal;
            Debug.Log("Round done!");
        }

        if (roundStatus is RoundStatus.Normal && playerStatus is not PlayerStatus.Playing)
        {
            roundStatus = RoundStatus.Ready;
            Debug.Log("Ready!");
        }
    }

    public enum PlayerStatus
    {
        Playing,
        Bust,
        Stand,
    }

    [Networked, OnChangedRender(nameof(RenderPlayer))]
    public PlayerStatus playerStatus { get; set; }

    private void addToHand(Card card)
    {
        hand.Set(handLength, card);
        handLength++;
        handTotal += card.Value();
        var clip = Resources.Load<AudioClip>($"CardDrawSounds/{card.SoundEffect()}");
        audioSource.PlayOneShot(clip, 1);
        if (handTotal > 21)
            {
                playerStatus = PlayerStatus.Bust;
            }
    }

    //prototyping discarding
    private void removeFromHand(int index)
    {
        handTotal -= hand.Get(index).Value();
        hand.Set(index, Card.Empty);
    }

    [Networked, Capacity(30)]
    public NetworkArray<Card> hand { get; }

    [Networked, OnChangedRender(nameof(RenderPlayer))]
    public int handLength { get; set; }

    [Networked]
    public int handTotal { get; set; }

    void RenderPlayer()
    {
        var slot = Slot.slots[slotIndex];

        slot.text.text = handTotal + " " + playerStatus switch
        {
            PlayerStatus.Bust => "Bust!",
            PlayerStatus.Stand => "Stand.",
            _ => "",
        };

        for (int i = 0; i < 8; i++)
        {
            var child = slot.transform.GetChild(i);

            if (i >= handLength)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
                child.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"CardArt/{hand[i]}");
            }
        }
    }

    public enum RoundStatus
    {
        Normal,
        Ready,
        Resetting,
    }

    public void AnonomlyEffect(Card drawnCard)
    {
        if (drawnCard == Card.Uno2)
        {
            removeFromHand(handLength - 2);
            removeFromHand(handLength - 3);

        }
        if (drawnCard == Card.Uno4)
        {
            for (int i = 0; i < 4; i++)
            {
                if (handLength < 8)
                {
                    addToHand(deck.DrawBasic());
                }
            }
        }
        if (drawnCard == Card.Pot)
        {
            for (int i = 0; i < 2; i++)
            {
                if (handLength < 8)
                {
                    addToHand(deck.DrawBasic());
                }
            }
        }
          if(hand[handLength-1] == Card.Trainer)
        {
            for(int i = 0; i < handLength;i++)
            {
                removeFromHand(handLength-i-1);
            }
            addToHand(deck.DrawBasic());
            addToHand(deck.DrawBasic());
        }
        if(hand[handLength-1] == Card.Energy)
        {

        }
        if(hand[handLength-1] == Card.Monster)
        {

        }
        if(hand[handLength-1] == Card.Jonkler)
        {
            int temp = UnityEngine.Random.Range(0,2);
            if (temp == 0)
            {
                removeFromHand(handLength-2);
                
            }
            if (temp == 1)
            {
                addToHand(deck.DrawBasic());
            }
            
        }
        if(hand[handLength-1] == Card.Defuse)
        {
            removeFromHand(handLength-2);
        }
        if(hand[handLength-1] == Card.Explode)
        {
            for (int i = 0; i < 8; i++)
            {
                if (handLength < 8)
                {
                    addToHand(deck.DrawBasic());
                }
            }
        }

    }

    private IEnumerator Ability()
    {
        yield return new WaitForSeconds(1);
        AnonomlyEffect(hand[handLength - 1]);
    }


    [Networked]
    public RoundStatus roundStatus { get; set; }

    
}
