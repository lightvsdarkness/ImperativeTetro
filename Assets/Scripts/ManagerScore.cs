using System;
using System.Collections;
using System.Collections.Generic;
using General;
using UnityEngine;

public class ManagerScore : SingletonManager<ManagerScore> {
    [Space]
    public DataScore ScoreData;

    [Space]
    public DataScoreLinesCleared DefaultScoreLinesCleared;
    public int DefaultValue = 200;

    [Space]
    public List<GameObject> UIObjectsCurrentScore = new List<GameObject>();
    public List<IIndicator> UIElementsCurrentScore = new List<IIndicator>();
    [Space]
    public List<GameObject> UIObjectsHighScore = new List<GameObject>();
    public List<IIndicator> UIElementsHighScore = new List<IIndicator>();

    private int _rowsClearedThisTurn;

    private ManagerTetroGame _game;

    public event Action<int> ScoreUpdated;

    /// <summary>
    /// I can into DI, Senpai!
    /// </summary>
    public void Initialize(ManagerTetroGame game) {
        _game = game;
        _game.RowCleared += UpdateScore;

        ScoreData.CurrentScore = 0;

        UIElementsCurrentScore.Clear();
        foreach (GameObject gObject in UIObjectsCurrentScore) {
            UIElementsCurrentScore.Add(gObject.GetComponent<IIndicator>());
        }

        UIElementsHighScore.Clear();
        foreach (GameObject gObject in UIObjectsHighScore) {
            UIElementsHighScore.Add(gObject.GetComponent<IIndicator>());
        }

        UpdateUI(UIElementsHighScore, ScoreData.HighScore.ToString());
    }

    public void UpdateScore() {
        if (DefaultScoreLinesCleared != null && DefaultScoreLinesCleared.LinesCleared.Count > 0
            && DefaultScoreLinesCleared.LinesCleared.Count >= _rowsClearedThisTurn - 1)
            ScoreData.CurrentScore += DefaultScoreLinesCleared.LinesCleared[_rowsClearedThisTurn - 1];
        else
            ScoreData.CurrentScore += DefaultValue;

        UpdateUI(UIElementsCurrentScore, ScoreData.CurrentScore.ToString());

        CheckHighScore();
    }

    private void CheckHighScore() {
        if (ScoreData.HighScore < ScoreData.CurrentScore) {
            ScoreData.HighScore = ScoreData.CurrentScore;

            UpdateUI(UIElementsHighScore, ScoreData.HighScore.ToString());
        }
    }

    private void UpdateUI(List<IIndicator> uiIndicators, string text) {
        foreach (var ui in uiIndicators) {
            ui.IndicateValue(text);
        }
    }

}