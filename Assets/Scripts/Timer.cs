using System;
using System.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private CountDownTextComponent timer;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button plusButton;
    private Animator animator;
    private Action<int> onComplete;
    private CompositeDisposable disposables;
    private int id;
    public int Id => id;

    private void Start()
    {
        animator = GetComponent<Animator>();
        minusButton.OnPointerDownAsObservable().Subscribe(_ => { ButtonClick(minusButton, true); }).AddTo(this);
        playButton.OnPointerClickAsObservable().Subscribe(_ => { PlayButtonClick(); }).AddTo(this);
        plusButton.OnPointerDownAsObservable().Subscribe(_ => { ButtonClick(plusButton, false); }).AddTo(this);
    }
    
    public void Init(int id, Action<int> onComplete)
    {
        this.id = id;
        this.onComplete = onComplete;
        
    }
    
    private async void PlayButtonClick()
    {
        timer.StartTimer(id,30, onComplete);
        animator.SetTrigger("Closed");
        await Task.Delay(300);
        gameObject.SetActive(false);
    }
    
    private void ButtonClick(UIBehaviour tempButton, bool minus)
    {
        disposables ??= new CompositeDisposable();
        var buttonValueStream = Observable.Interval(TimeSpan.FromMilliseconds(300));
        buttonValueStream.Subscribe(data => timer.EditTime(1, minus)).AddTo(disposables);
        tempButton.OnPointerUpAsObservable().Subscribe(_ =>
        {
            timer.EditTime(1, minus);
            OnDisposables();
        }).AddTo(disposables);
    }
    
    private void OnDisposables()
    {
        if (disposables == null) return;
        disposables.Dispose();
        disposables = null;
    }
}
