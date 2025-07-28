using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
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
    /// </summary>
    [SerializeField] private List<AudioClip> commonAudioClipsList;


    // ����� �ҽ� ������Ʈ ����
    private List<AudioSource> availableSources;

    // ���� ����
    private bool isGameOver;


    protected override void Awake() {
        base.Awake();
        availableSources = new List<AudioSource>();

        // �̸� ������ ����ŭ ����� �ҽ��� ����
        AddAudioSource();
    }

    // ����� �ҽ��� �Ҵ�� ������ ����ϴ� ���
    public void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f, bool isLoop = false) {
        AudioSource source = GetAvailableSource();
        if (source != null && clip != null) {
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.loop = isLoop;
            source.Play();
        }
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
}
