using System.Diagnostics;
using UnityEngine;

public class TemporaryBall : MonoBehaviour
{

    private Stopwatch ballTimer = new Stopwatch();

    public int timer = 15;

	public GameObject indicator;

    void OnEnable()
    {
        ballTimer.Start();
    }
    
    void Update()
    {
        if (ballTimer.Elapsed.Seconds == timer)
        {
            ballTimer.Stop();
			indicator.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
		EventMaster.eMInst.WaitForNewEvent();
            ballTimer.Reset();        
    }
}
