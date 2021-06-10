using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class MainMenu : View
{
    public void OnMenuItemMouseOver(GameObject e)
    {
        var image = e.transform.Find("Image").gameObject;
        image.SetActive(true);
    }

    public void OnMenuItemMouseOut(GameObject e)
    {
        var image = e.transform.Find("Image").gameObject;
        image.SetActive(false);
    }

    public void OnStartGameClick()
    {
        Game.SetState(Enumerations.GAME_STATE.PRE_GAME);
    }
}
