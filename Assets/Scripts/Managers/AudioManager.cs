using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip tapClip;
    [SerializeField] private AudioClip scoreClip;
    [SerializeField] private AudioClip gameOverClip;

    private AudioSource _audioSource;
    private Action _onTap;
    private Action<int> _onScoreUpdated;
    private Action _onGameOver;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 0f;

        _onTap = HandleTap;
        _onScoreUpdated = HandleScoreUpdated;
        _onGameOver = HandleGameOver;

        GameEvents.OnTap += _onTap;
        GameEvents.OnScoreUpdated += _onScoreUpdated;
        GameEvents.OnGameOver += _onGameOver;
    }

    private void OnDestroy()
    {
        if (_onTap != null)
        {
            GameEvents.OnTap -= _onTap;
        }

        if (_onScoreUpdated != null)
        {
            GameEvents.OnScoreUpdated -= _onScoreUpdated;
        }

        if (_onGameOver != null)
        {
            GameEvents.OnGameOver -= _onGameOver;
        }
    }

    private void HandleTap()
    {
        PlayClip(tapClip);
    }

    private void HandleScoreUpdated(int score)
    {
        if (score <= 0)
        {
            return;
        }

        PlayClip(scoreClip);
    }

    private void HandleGameOver()
    {
        PlayClip(gameOverClip);
    }

    private void PlayClip(AudioClip clip)
    {
        if (_audioSource == null || clip == null)
        {
            return;
        }

        _audioSource.PlayOneShot(clip);
    }
}
