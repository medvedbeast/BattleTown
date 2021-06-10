using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWon : View
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

    public void OnQuitGameClick()
    {
        Game.SetState(Enumerations.GAME_STATE.QUIT);
    }
}
