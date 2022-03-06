using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Events;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using EventArgs = Events.EventArgs;

public class MainScreens : MonoBehaviour
{
    public EventDispatcher EventDispatcher;
    
    [SerializeField] private Transform content;
    [SerializeField] private Transform contentTimer;
    [SerializeField] private GameObject timerPrefab;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button minusButton;
    
    private CanvasGroup canvasGroup;
    private List<ItemButton> listButtonPrefb;
    private List<Timer> listTimerPrefb;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        EventDispatcher = new EventDispatcher();
        listButtonPrefb = new List<ItemButton>();
        listTimerPrefb = new List<Timer>();
        plusButton.OnPointerClickAsObservable().Subscribe(_ => PlusButton()).AddTo(this);
        minusButton.OnPointerClickAsObservable().Subscribe(_ => MinusButton()).AddTo(this);
        EventDispatcher.AddListener(GameEvent.CLICK_BUTTON, OpenTimerScreens);

        for (int i = 0; i < 3; i++)
        {
            CreatButton(i);
        }

        StartCoroutine(Coroutine());
    }
    
    IEnumerator Coroutine()
    {
        yield return new WaitForEndOfFrame();
        canvasGroup.alpha = 1;
        Animations.OpenMainScreens(listButtonPrefb);
    }
    
    private void OpenTimerScreens(EventArgs eventargs)
    {
        var id = eventargs.args[0] as int? ?? 0;

        foreach (var item in listTimerPrefb)
        {
            if (item.Id == id)
            {
                item.gameObject.SetActive(true);
                return;
            }
        }
        var timerGO = Instantiate(timerPrefab, Vector3.zero, Quaternion.identity);
        timerGO.gameObject.transform.SetParent(contentTimer, false);
        var timer = timerGO.GetComponent<Timer>();
        timer.Init(id, OnComplete);
        listTimerPrefb.Add(timer);
    }
    
    private void PlusButton()
    {
        var index = listButtonPrefb.Count;
        CreatButton(index);
    }
    
    private void MinusButton()
    {
        if (listButtonPrefb.Any())
        {
            var temp = listButtonPrefb[listButtonPrefb.Count - 1];
            listButtonPrefb.Remove(temp);
            Destroy(temp.gameObject);
        }
    }
    
    private void CreatButton(int index)
    {
        var button = Instantiate(buttonPrefab, content, false);
        var itemButton = button.GetComponent<ItemButton>();
        itemButton.Init(index,this);
        listButtonPrefb.Add(itemButton);
        var textName = button.GetComponentInChildren<TextMeshProUGUI>();
        textName.text =  "Button " + index;
          
    }
    
    private void OnComplete(int index)
    {
        if (listButtonPrefb.Count > index)
        {
            var button = listButtonPrefb[index];
            var anim = Animations.TimerEnd(button.gameObject.transform);
            anim.Play();
        }
    }
}
