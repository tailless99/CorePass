using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public struct PlaySoundData {
    public AudioClip clip;
    public float volume;
    public float pitch;
    public bool isLoop;

    // �����ڸ� ���� �⺻�� ����
    public PlaySoundData(AudioClip clip, float volume = 1f, float pitch = 1f, bool isLoop = false) {
        this.clip = clip;
        this.volume = volume;
        this.pitch = pitch;
        this.isLoop = isLoop;
    }
}
public class SoundManager : Singleton<SoundManager> {
    [Header("Settings")]
    [SerializeField] private int numberOfSources = 10; // ���� ��� ������ ����� ����
    [SerializeField] private GameObject audioSourcePrefab; // ����� �ҽ� ������Ʈ�� ������ ������Ʈ ������
    [SerializeField] private Transform spawnTransform; // �ν��Ͻ� ��, ������ �θ� ��ġ

    [Header("Static Use Audio Clip List")]
    /// <summary>
    /// ���� ����ϴ� ����� Ŭ���� ���������� ������ �ְ� �ϱ� ���� ����
    /// 0 : �÷��̾� ����
    /// 1 : ���ӿ��� �����
    /// 2 : UI ��ư Ŭ��
    /// 3 : Game BGM
    /// 4 : Game Clear Sound
    /// </summary>
    [SerializeField] private List<AudioClip> commonAudioClipsList;


    // ����� �ҽ� ������Ʈ ����
    private List<AudioSource> availableSources;
    private AudioSource bgmAudioSource;

    // ���� ����
    private bool isGameOver;


    protected override void Awake() {
        base.Awake();
        availableSources = new List<AudioSource>();

        // �̸� ������ ����ŭ ����� �ҽ��� ����
        AddAudioSource();
    }

    private void Start() {
        // BGM ���
        PlayBGM();

        #region �̺�Ʈ ����
        // BGM ����
        EventBusManager.Instance.Subscribe<ChangedBGMVolumeEvent>(e => OnChangedBGMVolume(e.Volum));

        // ���� Ŭ���� ���� 
        EventBusManager.Instance.Subscribe<GameOver_ResetSoundEvent>(_ => GameClearEventSubscribe());
        EventBusManager.Instance.Subscribe<GameClearEvent>(_ => GameClear());

        // ���� �Ŵ��� �Լ��� �̺�Ʈ�� ���
        EventBusManager.Instance.Subscribe<PlaySoundEvent>(e => OnPlaySoundEvent(e.Data));
        EventBusManager.Instance.Subscribe<PlayOneShotEvent>(e => OnPlayOneShotEvent(e.Data));
        #endregion
    }

    // ����� �ҽ��� �Ҵ�� ������ ����ϴ� ���
    public void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f, bool isLoop = false, bool isBGM = false) {
        AudioSource source = GetAvailableSource();
        if (source != null && clip != null) {
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.loop = isLoop;
            source.Play();
        }

        // BGM ������ Audiosource ����
        if (isBGM) bgmAudioSource = source;
    }

    // ���� ��� ���� Ŭ���� ���ļ� ������ Ŭ���� �ѹ� �� ���. 1���� ����Ǵ� ���
    public void PlayOneShotSound(AudioClip clip, float volume = 1f, float pitch = 1f) {
        AudioSource source = GetAvailableSource();
        if (source != null && clip != null) {
            source.pitch = pitch;
            source.PlayOneShot(clip, volume);
            source.pitch = 1f;
        }
    }

    // ���� ��� ������ ����� �ҽ��� ��ȯ�޴� ���
    private AudioSource GetAvailableSource() {
        // 1. ���� ��� ������ �ҽ��� �ִ��� Ȯ�� �� ��ȯ
        foreach (AudioSource source in availableSources)
            if (!source.isPlaying) return source;

        // 2. ���� ���� ��� ������ �ҽ��� ���ٸ� �߰� ���� �� �Ҵ�
        AddAudioSource(); // �߰� ����
        foreach (AudioSource source in availableSources) // �Ҵ�
            if (!source.isPlaying) return source;

        return null;
    }

    // ����� �ҽ��� �����ϴ� ���
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

        isGameOver = true; // �̺�Ʈ �÷���
        PlaySound(commonAudioClipsList[0], .25f);
    }

    // ��� ������� ���� ����
    public void AllAudioStop() {
        // ��� ������� ������ �����ϴ� ���
        foreach (AudioSource source in availableSources)
            if (source.isPlaying) source.Stop();
    }

    private void Update() {
        // ���� ���� ��, �÷��̾��� ���� ����Ʈ ���带 Ʋ��, ���޾� ���ӿ��� ����� ư��.
        if (isGameOver) {
            if (!availableSources[0].isPlaying && availableSources[0].clip == commonAudioClipsList[0]) {
                availableSources[0].clip = commonAudioClipsList[1];
                availableSources[0].volume = .15f;
                availableSources[0].Play();
            }
        }
    }

    // ��ư Ŭ���� ���� ���
    public void UIBtnClick() {
        PlayOneShotSound(commonAudioClipsList[2], .5f);
    }

    private void PlayBGM() {
        PlaySound(commonAudioClipsList[3], 0.5f, 1f, true, true);
    }

    private void GameClear() {
        PlaySound(commonAudioClipsList[4], 0.18f, 1f);
    }



    #region �̺�Ʈ ������ �����ϱ� ���� ��Ű¡ �� �Լ���
    // �̺�Ʈ ������ ������ �Լ��� ������ ����ޱ� ���� �ϳ��� ���� �Լ�
    private void GameClearEventSubscribe() {
        AllAudioStop();
        PlaySound(commonAudioClipsList[3], 0.18f, 1f, true);
    }

    private void OnPlaySoundEvent(PlaySoundData evt) {
        // ����ü���� ���� ������ �޼��� �Ű������� ����
        PlaySound(evt.clip, evt.volume, evt.pitch, evt.isLoop);
    }

    private void OnPlayOneShotEvent(PlaySoundData evt) {
        // ����ü���� ���� ������ �޼��� �Ű������� ����
        PlayOneShotSound(evt.clip, evt.volume, evt.pitch);
    }
    #endregion

    public void PauseAudio() {
        Debug.Log("[Debug] PauseAudio called from JavaScript."); // �α� �߰�
        AudioListener.pause = true;
    }
    
    public void ResumeAudio() {
        Debug.Log("[Debug] ResumeAudio called from JavaScript."); // �α� �߰�
        AudioListener.pause = false;
    }

    private void OnChangedBGMVolume(float newVolume) {
        bgmAudioSource.volume = newVolume;
    }
}
