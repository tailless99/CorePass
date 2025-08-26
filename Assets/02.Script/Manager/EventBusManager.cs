using System;
using UnityEngine;
using UnityEngine.Events;

public class EventBusManager : Singleton<EventBusManager>
{
    // ���� ���� �̺�Ʈ
    public UnityEvent onPlayerDeathEvent; // �÷��̾ ���� �� ����Ǵ� �̺�Ʈ
    public UnityEvent onGameOver_ResetUI; // ����, �ǹ�, UI �� �ʱ�ȭ �̺�Ʈ
    public UnityEvent onGameOver_ResetSound; // ���� �ʱ�ȭ �̺�Ʈ

    // ���� Ŭ���� �̺�Ʈ
    public UnityEvent onGameClearEvent;

    // ����� ���� ���� �̺�Ʈ
    public UnityEvent onRestartAnimationStarted;
    public UnityEvent onRestartAnimationFinished;

    // �ǹ� ���� �̺�Ʈ
    public UnityEvent onFeverTimeStarted;
    public UnityEvent onFeverTimeFinished;

    // ���� ȹ�� �̺�Ʈ
    public UnityEvent<int> onAddScore;
    public UnityEvent<int> onAddFeverPoint;

    // ���� �̺�Ʈ
    public UnityEvent<PlaySoundEvent> onPlaySound;
    public UnityEvent<PlaySoundEvent> onPlayOneShot;
    public UnityEvent<float> onChangedBGMVolume;


    #region �̺�Ʈ ���� �Լ���
    public void SubscribeOnPlayerDeathEvent(UnityAction action) => onPlayerDeathEvent.AddListener(action);

    public void SubscribeOnGameOver_ResetUI(UnityAction action) => onGameOver_ResetUI.AddListener(action);

    public void SubscribeOnGameOver_ResetSound(UnityAction action) => onGameOver_ResetSound.AddListener(action);

    public void SubscribeOnGameClearEvent(UnityAction action) => onGameClearEvent.AddListener(action);
    
    public void SubscribeOnRestartAnimationStarted(UnityAction action) => onRestartAnimationStarted.AddListener(action);
    
    public void SubscribeOnRestartAnimationFinished(UnityAction action) => onRestartAnimationFinished.AddListener(action);
    
    public void SubscribeOnFeverTimeStarted(UnityAction action) => onFeverTimeStarted.AddListener(action);
    
    public void SubscribeOnFeverTimeFinished(UnityAction action) => onFeverTimeFinished.AddListener(action);
    
    public void SubscribeOnAddScore(UnityAction<int> action) => onAddScore.AddListener(action);
    
    public void SubscribeOnAddFeverPoint(UnityAction<int> action) => onAddFeverPoint.AddListener(action);

    public void SubscribeOnPlaySound(UnityAction<PlaySoundEvent> action) => onPlaySound.AddListener(action);

    public void SubscribeOnPlayOneShot(UnityAction<PlaySoundEvent> action) => onPlayOneShot.AddListener(action);
    
    public void SubscribeOnChangedBGMVolume(UnityAction<float> action) => onChangedBGMVolume.AddListener(action);
    #endregion


    #region �̺�Ʈ ���� �Լ���
    // ���� ������, UI ��� �ʱ�ȭ �̺�Ʈ ����
    public void StartEvent_PlayerDeathEvent() => onPlayerDeathEvent.Invoke();

    // ���� ������, UI ��� �ʱ�ȭ �̺�Ʈ ����
    public void StartEvent_GameOverResetUI() => onGameOver_ResetUI.Invoke();
    
    // ���� ������, Sound ��� �ʱ�ȭ �̺�Ʈ ����
    public void StartEvent_GameOverResetSound() => onGameOver_ResetSound.Invoke();

    // ���� Ŭ���� �̺�Ʈ ����
    public void StartEvent_GameClear() => onGameClearEvent.Invoke();

    // ����� ���� ���� �̺�Ʈ ����
    public void StartEvent_StartRestartAnimation() => onRestartAnimationStarted.Invoke();

    // ����� ���� ���� �̺�Ʈ ����
    public void StartEvent_EndRestartAnimation() => onRestartAnimationFinished.Invoke();

    // �ǹ� ���� �̺�Ʈ ����
    public void StartEvent_StartFeverTime() => onFeverTimeStarted.Invoke();

    // �ǹ� ���� �̺�Ʈ ����
    public void StartEvent_EndFeverTime() => onFeverTimeFinished.Invoke();

    // ���� �߰� �̺�Ʈ ����
    public void StartEvent_AddScore(int addScore) => onAddScore.Invoke(addScore);

    // ���� �߰� �̺�Ʈ ����
    public void StartEvent_AddFeverPoint(int addFeverPoint) => onAddFeverPoint.Invoke(addFeverPoint);

    // Play Sound �̺�Ʈ ����
    public void StartEvent_PlaySound(PlaySoundEvent args) => onPlaySound.Invoke(args);

    // Play One Shot �̺�Ʈ ����
    public void StartEvent_PlayOneShot(PlaySoundEvent args) => onPlayOneShot.Invoke(args);

    // BGM ���� ���� �̺�Ʈ ����
    public void StartEvent_ChangeBGMVolume(float volume) => onChangedBGMVolume.Invoke(volume);
    #endregion
}
