using Events;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField] private Button button;
    private MainScreens mainScreens;
    private int id;
    
    public void Init(int id, MainScreens mainScreens)
    {
        this.id = id;
        this.mainScreens = mainScreens;
        button.OnPointerClickAsObservable().Subscribe(_ => OpenTimer()).AddTo(this);
    }
    
    private void OpenTimer()
    {
        mainScreens.EventDispatcher.DispatchEvent(GameEvent.CLICK_BUTTON, id);
    }
}
