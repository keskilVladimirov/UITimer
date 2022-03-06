using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public static class Animations
{
    private const int SHAKE_DURATION = 3;
    private const int SHAKE_STRENGTH = 25;
    private const int START_VALUE = -540;
    private const int END_VALUE = 540;
    private const int MOVE_DURATION = 1;

    public static Sequence TimerEnd(Transform transform)
    {
        var timeLine = DOTween.Sequence();
        timeLine.Append(transform.DOShakePosition(SHAKE_DURATION, SHAKE_STRENGTH));
        return timeLine;
    }

    public static async void OpenMainScreens(List<ItemButton> listPrefabs)
    {
        foreach (var t in listPrefabs)
        {
            var pos = t.gameObject.transform.position;
            t.gameObject.transform.position = new Vector3(START_VALUE, pos.y, pos.z);
        }
        foreach (var t in listPrefabs)
        {
            t.gameObject.transform.DOMoveX(END_VALUE, MOVE_DURATION);
            await Task.Delay(END_VALUE);
        }
    }
}