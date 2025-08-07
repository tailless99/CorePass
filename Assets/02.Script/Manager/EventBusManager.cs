using System;
using UnityEngine;
using UnityEngine.Events;

public class EventBusManager : Singleton<EventBusManager>
{
    // 게임 오버 이벤트
    public UnityEvent onPlayerDeathEvent; // 플레이어가 죽을 때 실행되는 이벤트
    public UnityEvent onGameOver_ResetUI; // 점수, 피버, UI 등 초기화 이벤트
    public UnityEvent onGameOver_ResetSound; // 사운드 초기화 이벤트

    // 게임 클리어 이벤트
    public UnityEvent onGameClearEvent;

    // 재시작 연출 종료 이벤트
    public UnityEvent onRestartAnimationStarted;
    public UnityEvent onRestartAnimationFinished;

    // 피버 시작 이벤트
    public UnityEvent onFeverTimeStarted;
    public UnityEvent onFeverTimeFinished;
    

    #region 이벤트 구독 함수들
    public void SubscribeOnPlayerDeathEvent(UnityAction action) => onPlayerDeathEvent.AddListener(action);

    public void SubscribeOnGameOver_ResetUI(UnityAction action) => onGameOver_ResetUI.AddListener(action);

    public void SubscribeOnGameOver_ResetSound(UnityAction action) => onGameOver_ResetSound.AddListener(action);

    public void SubscribeOnGameClearEvent(UnityAction action) => onGameClearEvent.AddListener(action);
    
    public void SubscribeOnRestartAnimationStarted(UnityAction action) => onRestartAnimationStarted.AddListener(action);
    
    public void SubscribeOnRestartAnimationFinished(UnityAction action) => onRestartAnimationFinished.AddListener(action);
    
    public void SubscribeOnFeverTimeStarted(UnityAction action) => onFeverTimeStarted.AddListener(action);
    
    public void SubscribeOnFeverTimeFinished(UnityAction action) => onFeverTimeFinished.AddListener(action);
    #endregion


    #region 이벤트 실행 함수들
    // 게임 오버시, UI 요소 초기화 이벤트 실행
    public void StartEvent_PlayerDeathEvent() => onPlayerDeathEvent.Invoke();

    // 게임 오버시, UI 요소 초기화 이벤트 실행
    public void StartEvent_GameOverResetUI() => onGameOver_ResetUI.Invoke();
    
    // 게임 오버시, Sound 요소 초기화 이벤트 실행
    public void StartEvent_GameOverResetSound() => onGameOver_ResetSound.Invoke();

    // 게임 클리어 이벤트 실행
    public void StartEvent_GameClear() => onGameClearEvent.Invoke();

    // 재시작 연출 종료 이벤트 실행
    public void StartEvent_StartRestartAnimation() => onRestartAnimationStarted.Invoke();

    // 재시작 연출 종료 이벤트 실행
    public void StartEvent_EndRestartAnimation() => onRestartAnimationFinished.Invoke();

    // 피버 시작 이벤트 실행
    public void StartEvent_StartFeverTime() => onFeverTimeStarted.Invoke();

    // 피버 시작 이벤트 실행
    public void StartEvent_EndFeverTime() => onFeverTimeFinished.Invoke();
    #endregion
}
