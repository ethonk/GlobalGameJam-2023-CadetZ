using System;
using UnityEngine;

public class TransformationHandler : MonoBehaviour
{
    private enum TransformationState { Chimpanzee, Caveman };
    [SerializeField] private TransformationState transformationState;

    [Header("Chimpanzee")]
    [SerializeField] private float chimpanzeeCurr;
    [SerializeField] private float chimpanzeeMax = 5f;
    
    [Header("Caveman")]
    [SerializeField] private float cavemanCurr;
    [SerializeField] private float cavemanMax = 100f;
    [SerializeField] private float cavemanDecayCurr;
    [SerializeField] private float cavemanDecayRate = 0.01f;
    [SerializeField] private float cavemanRestoreRate = 2f;

    [Header("Player Prefabs")]
    [SerializeField] private Transform plrOrangutan;
    [SerializeField] private Transform plrCaveman;

    private void Start()
    {
        cavemanDecayCurr = cavemanDecayRate;
    }

    private void Update()
    {
        // if not a caveman don't run
        if (transformationState != TransformationState.Caveman) return;
        
        // decrement the caveman
        cavemanCurr -= cavemanDecayCurr * Time.deltaTime;
        cavemanDecayCurr *= cavemanDecayRate;
        
        // return state if caveman is fully depleted
        if (cavemanCurr <= 0)
        {
            cavemanCurr = 0;
            transformationState = TransformationState.Chimpanzee;

            cavemanDecayCurr = cavemanDecayRate;
            
            // spawn orangutan at caveman
            var origPlayer = FindObjectOfType<PlayerDetails>().transform;
            var newOrangutan = Instantiate(plrOrangutan, null);
            newOrangutan.position = origPlayer.position;
            // delete existing caveman
            Destroy(origPlayer.gameObject);
        }
    }

    /// <summary>
    /// Increment if a kill is received.
    /// </summary>
    public void Increment()
    {
        // increment accordingly
        switch (transformationState)
        {
            case TransformationState.Chimpanzee:
                chimpanzeeCurr++;
                if (chimpanzeeCurr > chimpanzeeMax) chimpanzeeCurr = chimpanzeeMax;
                break;
            
            case TransformationState.Caveman:
                cavemanCurr += cavemanRestoreRate;
                if (cavemanCurr > cavemanMax) cavemanCurr = cavemanMax;
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        // change to caveman if right condition
        if (transformationState == TransformationState.Chimpanzee && chimpanzeeCurr >= chimpanzeeMax)
        {
            chimpanzeeCurr = 0;
            cavemanCurr = cavemanMax;
            
            transformationState = TransformationState.Caveman;
            cavemanDecayCurr = cavemanDecayRate;
            
            // spawn caveman at orangutan
            var origPlayer = FindObjectOfType<PlayerDetails>().transform;
            var newCaveman = Instantiate(plrCaveman, null);
            newCaveman.position = origPlayer.position;
            // delete existing orangutan
            Destroy(origPlayer.gameObject);
        }
    }

    public float GetBarPercentage()
    {
        switch (transformationState)
        {
            case TransformationState.Chimpanzee:
                return chimpanzeeCurr / chimpanzeeMax;
            case TransformationState.Caveman:
                return cavemanCurr / cavemanMax;
            default:
                return 0;
        }
    }
}
