using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
public class BamTimeKeeperHandler : MonoBehaviour {

    public KMBombInfo info;
    public KMColorblindMode colorblindMode;
    public KMAudio sound;
    public GameObject buttonL, buttonM, buttonR, highlightStage1, hightlightStage2;
    public GameObject animationPointDoorA, animationPointDoorB, animationPointButtonA, animationPointButtonB;
    public TextMesh display;
    public Material[] buttonColors = new Material[8];

    public Material[] indicatorStatus = new Material[3];

    private ulong finalValueA;
    private ulong finalValueB;
    private List<ulong> possibleTimesA = new List<ulong>();
    private List<ulong> possibleTimesB = new List<ulong>();

    // Use this for initialization
    void Start () {
        sound.PlaySoundAtTransform("KefkaLaugh", transform);
    }

	// Update is called once per frame
	void Update () {

	}
    IEnumerator HideButtons()
    {
        yield return null;
    }
}
