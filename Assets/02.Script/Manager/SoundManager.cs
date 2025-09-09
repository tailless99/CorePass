using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public struct PlaySoundData {
    public AudioClip clip;
    public float volume;
    public float pitch;
    public bool isLoop;

    // 생성자를 통해 기본값 설정
    public PlaySoundData(AudioClip clip, float volume = 1f, float pitch = 1f, bool isLoop = false) {
        this.clip = clip;
        this.volume = volume;
        this.pitch = pitch;
        this.isLoop = isLoop;
    }
}
public class SoundManager : Singleton<SoundManager> {
    [Header("Settings")]
    [SerializeField] private int numberOfSources = 10; // 동시 재생 가능한 오디오 숫자
    [SerializeField] private GameObject audioSourcePrefab; // 오디오 소스 컴포넌트를 생성할 오브젝트 프리팹
    [SerializeField] private Transform spawnTransform; // 인스턴스 후, 생성할 부모 위치

    [Header("Static Use Audio Clip List")]
    /// <summary>
    /// 자주 사용하는 오디오 클립을 자제적으로 가지고 있게 하기 위한 변수
    /// 0 : 플레이어 죽음
    /// 1 : 게임오버 오디오
    /// 2 : UI 버튼 클릭
    /// 3 : Game BGM
    /// 4 : Game Clear Sound
    /// </summary>
    [SerializeField] private List<AudioClip> commonAudioClipsList;


    // 오디오 소스 컴포넌트 모음
    private List<AudioSource> availableSources;
    private AudioSource bgmAudioSource;

    // 변수 모음
    private bool isGameOver;


    protected override void Awake() {
        base.Awake();
        availableSources = new List<AudioSource>();

        // 미리 지정한 수만큼 오디오 소스를 생성
        AddAudioSource();
    }

    private void Start() {
        // BGM 재생
        PlayBGM();

        #region 이벤트 구독
        // BGM 조절
        EventBusManager.Instance.Subscribe<ChangedBGMVolumeEvent>(e => OnChangedBGMVolume(e.Volum));

        // 게임 클리어 사운드 
        EventBusManager.Instance.Subscribe<GameOver_ResetSoundEvent>(_ => GameClearEventSubscribe());
        EventBusManager.Instance.Subscribe<GameClearEvent>(_ => GameClear());

        // 사운드 매니저 함수를 이벤트로 등록
        EventBusManager.Instance.Subscribe<PlaySoundEvent>(e => OnPlaySoundEvent(e.Data));
        EventBusManager.Instance.Subscribe<PlayOneShotEvent>(e => OnPlayOneShotEvent(e.Data));
        #endregion
    }

    // 오디오 소스에 할당된 음원을 재생하는 기능
    public void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f, bool isLoop = false, bool isBGM = false) {
        AudioSource source = GetAvailableSource();
        if (source != null && clip != null) {
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.loop = isLoop;
            source.Play();
        }

        // BGM 조절용 Audiosource 저장
        if (isBGM) bgmAudioSource = source;
    }

    // 현재 재생 중인 클립에 겹쳐서 지정된 클립을 한번 더 재생. 1번만 재생되는 기능
    public void PlayOneShotSound(AudioClip clip, float volume = 1f, float pitch = 1f) {
        AudioSource source = GetAvailableSource();
        if (source != null && clip != null) {
            source.pitch = pitch;
            source.PlayOneShot(clip, volume);
            source.pitch = 1f;
        }
    }

    // 현재 사용 가능한 오디오 소스를 반환받는 기능
    private AudioSource GetAvailableSource() {
        // 1. 현재 사용 가능한 소스가 있는지 확인 및 반환
        foreach (AudioSource source in availableSources)
            if (!source.isPlaying) return source;

        // 2. 만약 현재 사용 가능한 소스가 없다면 추가 증설 및 할당
        AddAudioSource(); // 추가 증설
        foreach (AudioSource source in availableSources) // 할당
            if (!source.isPlaying) return source;

        return null;
    }

    // 오디오 소스를 생성하는 기능
    private void AddAudioSource() {
        for (int i = 0; i < numberOfSources; i++) {
            GameObject go = Instantiate(audioSourcePrefab, spawnTransform);
            go.name = "AudioSource_" + i;
            availableSources.Add(go.GetComponent<AudioSource>());
            go.SetActive(true);
        }
    }

    public void GameOver() {
        AllAudioStop();

        isGameOver = true; // 이벤트 플래그
        PlaySound(commonAudioClipsList[0], .25f);
    }

    // 모든 재생중인 음악 정지
    public void AllAudioStop() {
        // 모든 재생중인 음악을 정지하는 기능
        foreach (AudioSource source in availableSources)
            if (source.isPlaying) source.Stop();
    }

    private void Update() {
        // 게임 오버 시, 플레이어의 죽음 이펙트 사운드를 틀고, 연달아 게임오버 브금을 튼다.
        if (isGameOver) {
            if (!availableSources[0].isPlaying && availableSources[0].clip == commonAudioClipsList[0]) {
                availableSources[0].clip = commonAudioClipsList[1];
                availableSources[0].volume = .15f;
                availableSources[0].Play();
            }
        }
    }

    // 버튼 클릭시 사운드 출력
    public void UIBtnClick() {
        PlayOneShotSound(commonAudioClipsList[2], .5f);
    }

    private void PlayBGM() {
        PlaySound(commonAudioClipsList[3], 0.5f, 1f, true, true);
    }

    private void GameClear() {
        PlaySound(commonAudioClipsList[4], 0.18f, 1f);
    }



    #region 이벤트 버스에 구독하기 위해 패키징 한 함수들
    // 이벤트 버스에 구독될 함수의 순서를 보장받기 위해 하나로 묶은 함수
    private void GameClearEventSubscribe() {
        AllAudioStop();
        PlaySound(commonAudioClipsList[3], 0.18f, 1f, true);
    }

    private void OnPlaySoundEvent(PlaySoundData evt) {
        // 구조체에서 받은 값들을 메서드 매개변수로 전달
        PlaySound(evt.clip, evt.volume, evt.pitch, evt.isLoop);
    }

    private void OnPlayOneShotEvent(PlaySoundData evt) {
        // 구조체에서 받은 값들을 메서드 매개변수로 전달
        PlayOneShotSound(evt.clip, evt.volume, evt.pitch);
    }
    #endregion

    public void PauseAudio() {
        Debug.Log("[Debug] PauseAudio called from JavaScript."); // 로그 추가
        AudioListener.pause = true;
    }
    
    public void ResumeAudio() {
        Debug.Log("[Debug] ResumeAudio called from JavaScript."); // 로그 추가
        AudioListener.pause = false;
    }

    private void OnChangedBGMVolume(float newVolume) {
        bgmAudioSource.volume = newVolume;
    }
}
