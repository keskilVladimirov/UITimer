using System;
using TMPro;
using UniRx;
using UnityEngine;

public class CountDownTextComponent : MonoBehaviour
{
    private const string FORMAT = "hh':'mm':'ss";
    
    private TextMeshProUGUI text;
    private IConnectableObservable<int> countDownObservable;
    private int countTimeNow;
    private int id;
    private CompositeDisposable disposables;
    
    private void Start()
    {
        countTimeNow = 0;
        text = GetComponent<TextMeshProUGUI>();
    }

    public void StartTimer(int id, int countTime, Action<int> onComplete)
    {
        this.id = id;
        if (countTimeNow == 0)
        {
            countTimeNow = countTime;
        }
        Run(countTimeNow, onComplete);
    }
    
    private void Run(int countTime, Action<int> onComplete)
    {
        disposables ??= new CompositeDisposable();
        countDownObservable = CreateCountDownObservable(countTime).Publish();
        countDownObservable.Connect();
        countDownObservable
            .Subscribe(time =>
            {
                countTimeNow = time;
                FormatTime(countTimeNow);
            }, () =>
            {
                FormatTime(0);
                onComplete?.Invoke(id);
            }).AddTo(disposables);
    }

    private IObservable<int> CreateCountDownObservable(int countTime)
    {
        return Observable
            .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))
            .Select(x => (int) (countTime - x))
            .TakeWhile(x => x > 0);
    }

    private void FormatTime(int countTime)
    {
        var result = TimeSpan.FromSeconds(countTime);
        text.text = result.ToString(FORMAT);
    }

    public void EditTime(int countTime, bool minus)
    {
        OnDisposables();
        if (minus)
        {
            if (countTimeNow <= 0)
                countTimeNow = 0;
            else
                countTimeNow -= countTime;
        }
        else
            countTimeNow += countTime;
        FormatTime(countTimeNow);
    }
    
    private void OnDisposables()
    {
        if (disposables == null) return;
        disposables.Dispose();
        disposables = null;
    }
}