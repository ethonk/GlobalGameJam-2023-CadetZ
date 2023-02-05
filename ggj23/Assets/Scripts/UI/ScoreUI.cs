using Managers;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Animator scoreAnimator;

    private void Update()
    {
        scoreText.text = GameManager.Instance.score.ToString();
    }

    public void AddScore()
    {
        scoreAnimator.SetTrigger("triggerScore");
    }
}
