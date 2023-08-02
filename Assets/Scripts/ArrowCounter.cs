using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArrowCounter : MonoBehaviour
{
    public TextMeshProUGUI arrowCounter;

    // Start is called before the first frame update
    void Start()
    {
        arrowCounter.text = GlobalState.Instance.arrowCount.ToString();
    }

    public void SetArrowCounter(int value)
    {
       arrowCounter.text = value.ToString(); 
    }
}
