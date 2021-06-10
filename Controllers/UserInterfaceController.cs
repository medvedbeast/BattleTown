using Enumerations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserInterfaceController : MonoBehaviour
{
    [SerializeField]
    private View current;
    public View Current
    {
        get => current;
        set
        {
            current = value;
            Show();
        }
    }

    public List<View> views = new List<View>();

    public void Start()
    {
    }

    public void Initialize()
    {
        Game.Events.GameStateChange += OnGameStateChanged;

        var inGame = views.Where(x => x is InGame).First() as InGame;
        Game.Events.UnitSpawn += inGame.OnUnitSpawn;
        Game.Events.UnitDestruction += inGame.OnUnitDestruction;
        Game.Events.BaseHealthChange += inGame.OnBaseHealthChange;
        Game.Events.UnitEquipmentChange += inGame.OnUnitEquipmentChange;
        Game.Events.UnitHealthChange += inGame.OnUnitHealthChange;
        Game.Events.UnitInteractionStart += inGame.OnInteractionStart;
        Game.Events.UnitInteractionEnd += inGame.OnInteractionEnd;
    }

    public void OnGameStateChanged()
    {
        switch (Game.State)
        {
            case GAME_STATE.MAIN_MENU:
                {
                    Current = views.Where(x => x is MainMenu).First();
                    break;
                }
            case GAME_STATE.PRE_GAME:
                {

                    break;
                }
            case GAME_STATE.IN_GAME:
                {
                    Current = views.Where(x => x is InGame).First();
                    break;
                }
            case GAME_STATE.POST_GAME:
                {
                    break;

                }
            case GAME_STATE.GAME_LOST:
                {
                    Current = views.Where(x => x is GameLost).First();
                    break;
                }
            case GAME_STATE.GAME_WON:
                {
                    Current = views.Where(x => x is GameWon).First();
                    break;
                }
        }
    }

    private void Show()
    {
        foreach (var v in views)
        {
            if (v != current)
            {
                v.Hide();
            }
            else
            {
                v.Show();
            }
        }
    }
}
