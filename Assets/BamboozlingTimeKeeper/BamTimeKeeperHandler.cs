using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using System.Linq;
using System;

public class BamTimeKeeperHandler : MonoBehaviour {

    public KMBombInfo info;
    public KMBombModule modSelf;
    public KMColorblindMode colorblindMode;
    public KMModSettings ModConfig;
    public KMAudio sound;
    public GameObject buttonL, buttonM, buttonR, buttonGroup, highlightStage1, highlightStage2, door, backing;
    public GameObject animationPointDoorA, animationPointDoorB, animationPointButtonA, animationPointButtonB;
    public TextMesh display;
    public KMSelectable[] buttonsSelectable = new KMSelectable[3], stageSelectables = new KMSelectable[2];

    private List<Color> colorList = new List<Color>()
    {   new Color(1, 0, 0),
        new Color(1, 1, 0),
        new Color(0, 1, 0),
        new Color(0, 1, 1),
        new Color(0, 0, 1),
        new Color(1, 0, 1),
        new Color(1, 1, 1),
        new Color(0, 0, 0)
    }, indcColors = new List<Color>() {
        new Color(0, 0, 0),
        new Color(1, 1, 0),
        new Color(0, 1, 0)
    };
    private List<string> colorString = new string[] {
        "Red",
        "Yellow",
        "Green",
        "Cyan",
        "Blue",
        "Magenta",
        "White",
        "Black"
    }.ToList(), startingPhrases = new string[] {
        "SOME NUMBERS", "THE NUMBERS", "NUMBERS", "TWO NUMBERS", "THREE NUMBERS", "FOUR NUMBERS",
        "SOME NUMBER(S)", "THE NUMBER(S)", "NUMBER(S)", "2 NUMBERS", "3 NUMBERS", "4 NUMBERS",
        "SOME NUMBER", "THE NUMBER", "NUMBER", "ONE NUMBER", "A NUMBER", "1 NUMBER"
    }.ToList(), rowIndex = new string[] {
        "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
        "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
        "20", "30", "40", "50", "60", "70", "80", "90",
        "ZERO", "ONE",  "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE",
        "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN",
        "TWENTY", "THIRTY", "FOURTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
    }.ToList(), colIndex = new string[] {
        "RED",
        "YELLOW",
        "GREEN",
        "CYAN",
        "BLUE",
        "MAGENTA",
        "WHITE",
        "BLACK"
    }.ToList(), KritsyModIDs = new string[] { // An ID list of all of Kritsy's modules uploaded so far
        "KritBlackjack",
        "KritCMDPrompt",
        "KritConnectionDev",
        "KritFlipTheCoin",
        "Krit4CardMonte",
        "KritHoldUps",
        "KritMicroModules",
        "KritHomework",
        "KritRadio",
        "KritScripts"
    }.ToList(), SpeakingModIDs = new string[] { // An ID list of all of Speakingevil's modules uploaded so far
        "14",
        "affineCycle",
        "bamboozledAgain",
        "bamboozlingButton",
        "bamboozlingButtonGrid",
        "bamboozlingTimeKeeper",
        "borderedKeys",
        "caesarCycle",
        "crypticCycle",
        "disorderedKeys",
        "doubleArrows",
        "faultyrgbMaze",
        "forgetMeLater",
        "hillCycle",
        "jumbleCycle",
        "misorderedKeys",
        "orderedKeys",
        "pigpenCycle",
        "playfairCycle",
        "recordedKeys",
        "reorderedKeys",
        "rgbMaze",
        "simonStores",
        "tallorderedKeys",
        "ultimateCycle",
        "UltraStores",
        "unorderedKeys",
        "veryAnnoyingButton"
    }.ToList(), ThreeStageModIDs = new string[] { // A List of 3 Stage Modules on the bomb.
        "3dTunnels",
        "algebra",
        "binaryTree",
        "BookOfMarioModule",
        "challengeAndContact",
        "Color Decoding",
        "EnglishTest",
        "DateFinder",
        "FlavorTextCruel",
        "guitarChords",
        "iceCreamModule",
        "KanjiModule",
        "krazyTalk",
        "logicalButtonsModule",
        "londonUnderground",
        "modernCipher",
        "moduleMovements",
        "monsplodeCards",
        "numberNimbleness",
        "orangeArrowsModule",
        "orderedKeys",
        "partialDerivatives",
        "passportControl",
        "poetry",
        "PrimeChecker",
        "screw",
        "SeaShells",
        "simonSamples",
        "SimonScreamsModule",
        "simonSelectsModule",
        "SimonShrieksModule",
        "SimonSingsModule",
        "simonStops",
        "simonStores",
        "sonic",
        "symbolicCoordinates",
        "ThirdBase",
        "timingIsEverything",
        "UltraStores",
        "Wavetapping",
        "WhosOnFirst",
        "WhosOnFirstTranslated",
        "lgndZoni"
    }.ToList(), RTControlModIDs = new string[] {
        "brushStrokes",
        "burglarAlarm",
        "burgerAlarm",
        "coffeebucks", // Grey spot, can't strike after leaving it for too long though.
        "countdown",
        "cruelCountdown",
        "crystalMaze",
        "fastMath",
        "jackAttack",
        "manometers",
        "KritHomework",
        "numberNimbleness",
        "quizBuzz",
        "simonStops",   // All other Simon modules are in a gray spot due to the fact that these either wait or don't wait to clear the inputs. Simon Stops strikes on waiting for a controlled input for too long.
        "sonicKnuckles", // Grey spot, NOT TP compatible on most streams.
        "stopwatch",
        "wire",
        "ZooModule"
    }.ToList(), RTSensitiveModIDs = new string[] {
        "taxReturns", // Strikes by leaving it sit for too long or by incorrect value.
        "lgndHyperactiveNumbers", // Can't strike by leaving it for too long, resets every now and then.
        "numberCipher", // Can't strike by leaving it for too long, resets every now and then.
        "theSwan", // Strikes by leaving it sit for too long or by incorrect set of presses.
        "veryAnnoyingButton" // Strikes by leaving it sit for too long or by an incorrect press.
    }.ToList(), stage1ButtonColors = new string[3].ToList(), stage2ButtonColors = new string[3].ToList();

    private List<string> stage1Phrases = new List<string>(), stage2Phrases = new List<string>();
    private List<string> stage1PhrClr = new List<string>(), stage2PhrClr = new List<string>();
    private int[] stage1ButtonDigits = new int[3], stage2ButtonDigits = new int[3];

    

    public long startValueA, startValueB;

    private long finalValueA, finalValueB;

    private int scaleFactorA = 0, scaleFactorB = 0;

    private List<long> possibleTimesA = new List<long>(), possibleTimesB = new List<long>();


    private int TodaysDay = DateTime.Today.Day;
    private bool oneTapHolds = false, playingAnim = false, interactable = false, colorBlindActive = false, started = false, specialDay;

    private int curbtnHeld = -1;
    private static int modID = 1;
    public int curModId;
    private bool[] stagesCompleted = new bool[] { true, false };
    private int currentStage = 0;
    // Use this for initialization
    void Awake()
    {
        curModId = modID++;
        oneTapHolds = false;

        try
        {
            colorBlindActive = colorblindMode.ColorblindModeActive;
        }
        catch
        {
            colorBlindActive = false;
        }
    }
    void Start() {

        specialDay = isSpecialDay();
        if (specialDay)
        {
            long[] listValues = new long[] { 2424, 4949 };
            int idxSwap = UnityEngine.Random.Range(0, 2);
            long tempvalue = listValues[idxSwap];
            listValues[idxSwap] = listValues[0];
            listValues[0] = tempvalue;

            startValueA = listValues[0];
            startValueB = listValues[1];
            stage1ButtonColors[0] = "Cyan";
            stage2ButtonColors[0] = "Cyan";
            sound.PlaySoundAtTransform("SigFeverEnter", transform);
        }
        else
        {
            startValueA = UnityEngine.Random.Range(0, 10000);
            startValueB = UnityEngine.Random.Range(0, 10000);
            sound.PlaySoundAtTransform("KefkaLaugh", transform);
        }
        modSelf.OnActivate += delegate
        {
            oneTapHolds = TwitchPlaysEnabled || oneTapHolds;
            GenerateRandomPhrases();
            GenerateRandomButtons();
            currentStage = 1;
            UpdateButtons(1);
            playingAnim = true;
            StartCoroutine(ShowButtons());
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The starting value for stage 1 is {1}", curModId, startValueA);
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The starting value for stage 2 is {1}", curModId, startValueB);
            started = true;
        };
        for (int x = 0; x < stageSelectables.Length; x++)
        {
            int y = x;
            stageSelectables[x].OnInteract += delegate {
                if (!playingAnim && interactable && started)
                {
                    if (currentStage != y + 1)
                    {
                        StartCoroutine(ChangeToStage(y + 1));
                    }
                }
                return false;
            };
        }
        for (int x = 0; x < buttonsSelectable.Length; x++)
        {
            int y = x;
            buttonsSelectable[x].OnInteract += delegate {
                if (!playingAnim && interactable && started)
                {
                    if (oneTapHolds)
                    {
                        if (curbtnHeld != -1)
                        {
                            curbtnHeld = -1;
                        }
                        else
                        {
                            curbtnHeld = y;
                        }
                    }
                    else
                    {
                        sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonPress, transform);
                        curbtnHeld = y;
                    }
                }
                return false;
            };
            buttonsSelectable[x].OnInteractEnded += delegate {
                if (!playingAnim && interactable && started)
                {
                    if (!oneTapHolds)
                    {
                        if (curbtnHeld != -1)
                        {
                            curbtnHeld = -1;
                            sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonRelease,transform);
                        }
                    }
                }
            };
        }

    }

    private readonly List<string> phraseNumList = new List<string>() {
        "0","ZERO",
        "1","ONE",
        "2","TWO",
        "3","THREE",
        "4","FOUR",
        "5","FIVE",
        "6","SIX",
        "7","SEVEN",
        "8","EIGHT",
        "9","NINE",
        "10","TEN",
        "11","ELEVEN",
        "12","TWELVE",
        "13","THIRTEEN",
        "14","FOURTEEN",
        "15","FIFTEEN",
        "16","SIXTEEN",
        "17","SEVENTEEN",
        "18","EIGHTEEN",
        "19","NINETEEN",
        "20","TWENTY",
        "30","THIRTY",
        "40","FOURTY",
        "50","FIFTY",
        "60","SIXTY",
        "70","SEVENTY",
        "80","EIGHTY",
        "90","NINETY"
    };

    void GenerateRandomPhrases()
    {
        // Add a Starting Phrase to each.
        stage1Phrases.Add(startingPhrases[UnityEngine.Random.Range(0, startingPhrases.Count)]);
        stage1PhrClr.Add("White");
        stage2Phrases.Add(startingPhrases[UnityEngine.Random.Range(0, startingPhrases.Count)]);
        stage2PhrClr.Add("White");
        // Add a phrase relating to the starting value.
        // Stage 1
        string startNum1Str = startValueA.ToString("0000");
        if (startValueA > 9)
            for (int x = 0; x < startNum1Str.Length; x++)
            {
                string digitSingle = startNum1Str.Substring(x, 1);
                if (2 * x == startNum1Str.Length && startValueA > 99)
                {
                    stage1Phrases.Add("HUNDRED");
                    stage1PhrClr.Add("White");
                }
                if (!digitSingle.Equals("0"))
                {
                    if (x % 2 == 0)
                    {

                        string detectString = startNum1Str.Substring(x, 2);
                        if (detectString.RegexMatch(@"^\d0$") || detectString.RegexMatch(@"^1\d$"))
                        {
                            stage1Phrases.Add(phraseNumList[phraseNumList.IndexOf(detectString) + UnityEngine.Random.Range(0, 2)]);
                            stage1PhrClr.Add(colorString[UnityEngine.Random.Range(0, colorString.Count)]);
                            x++;
                        }
                        else
                        {
                            stage1Phrases.Add(phraseNumList[phraseNumList.IndexOf(digitSingle + "0") + UnityEngine.Random.Range(0, 2)]);
                            stage1PhrClr.Add(colorString[UnityEngine.Random.Range(0, colorString.Count)]);
                        }
                    }
                    else
                    {
                        stage1Phrases.Add(phraseNumList[phraseNumList.IndexOf(digitSingle) + UnityEngine.Random.Range(0, 2)]);
                        stage1PhrClr.Add(colorString[UnityEngine.Random.Range(0, colorString.Count)]);
                    }
                }
            }
        else
        {
            stage1Phrases.Add(phraseNumList[phraseNumList.IndexOf(startValueA.ToString()) + UnityEngine.Random.Range(0, 2)]);
            stage1PhrClr.Add(colorString[UnityEngine.Random.Range(0, colorString.Count)]);
        }
        // Stage 2
        string startNum2Str = startValueB.ToString("0000");
        if (startValueB > 9)
            for (int x = 0; x < startNum2Str.Length; x++)
            {
                string digitSingle = startNum2Str.Substring(x, 1);
                if (2 * x == startNum2Str.Length && startValueB > 99)
                {
                    stage2Phrases.Add("HUNDRED");
                    stage2PhrClr.Add("White");
                }
                if (!digitSingle.Equals("0"))
                {
                    if (x % 2 == 0)
                    {

                        string detectString = startNum2Str.Substring(x, 2);
                        if (detectString.RegexMatch(@"^\d0$") || detectString.RegexMatch(@"^1\d$"))
                        {
                            stage2Phrases.Add(phraseNumList[phraseNumList.IndexOf(detectString) + UnityEngine.Random.Range(0, 2)]);
                            stage2PhrClr.Add(colorString[UnityEngine.Random.Range(0, colorString.Count)]);
                            x++;
                        }
                        else
                        {
                            stage2Phrases.Add(phraseNumList[phraseNumList.IndexOf(digitSingle + "0") + UnityEngine.Random.Range(0, 2)]);
                            stage2PhrClr.Add(colorString[UnityEngine.Random.Range(0, colorString.Count)]);
                        }
                    }
                    else
                    {
                        stage2Phrases.Add(phraseNumList[phraseNumList.IndexOf(digitSingle) + UnityEngine.Random.Range(0, 2)]);
                        stage2PhrClr.Add(colorString[UnityEngine.Random.Range(0, colorString.Count)]);
                    }
                }
            }
        else
        {
            stage2Phrases.Add(phraseNumList[phraseNumList.IndexOf(startValueA.ToString()) + UnityEngine.Random.Range(0, 2)]);
            stage2PhrClr.Add(colorString[UnityEngine.Random.Range(0, colorString.Count)]);
        }
        // Add Ending Phrases
        stage1Phrases.Add("POINT ZERO");
        stage1PhrClr.Add("White");
        stage2Phrases.Add("POINT ZERO");
        stage2PhrClr.Add("White");
    }
    void GenerateRandomButtons()
    {   // Generate Random Button Colors
        for (int x = 0; x < stage1ButtonColors.Count; x++)
        {
            if (stage1ButtonColors[x] == null)
            {
                stage1ButtonColors[x] = colorString[UnityEngine.Random.Range(0, colorString.Count)];
            }
        }
        for (int x = 0; x < stage2ButtonColors.Count; x++)
        {
            if (stage2ButtonColors[x] == null)
            {
                stage2ButtonColors[x] = colorString[UnityEngine.Random.Range(0, colorString.Count)];
            }
        }
        // Scramble Button Colors
        for (int x = 0; x < stage1ButtonColors.Count; x++)
        {
            int idxSwap = UnityEngine.Random.Range(x, stage1ButtonColors.Count);
            string tempvalue = stage1ButtonColors[idxSwap];
            stage1ButtonColors[idxSwap] = stage1ButtonColors[0];
            stage1ButtonColors[0] = tempvalue;
        }
        for (int x = 0; x < stage2ButtonColors.Count; x++)
        {
            int idxSwap = UnityEngine.Random.Range(x, stage2ButtonColors.Count);
            string tempvalue = stage2ButtonColors[idxSwap];
            stage2ButtonColors[idxSwap] = stage2ButtonColors[0];
            stage2ButtonColors[0] = tempvalue;
        }
        // Generate Random Button Digits
        for (int x = 0; x < stage1ButtonDigits.Length; x++)
        {
            stage1ButtonDigits[x] = UnityEngine.Random.Range(0, 10);
        }
        for (int x = 0; x < stage2ButtonDigits.Length; x++)
        {
            stage2ButtonDigits[x] = UnityEngine.Random.Range(0, 10);
        }
    }
    bool canOverride()
    {
        return stage1ButtonColors.Contains("Cyan") && stage2ButtonColors.Contains("Cyan")
            && ((startValueA == 2424 && startValueB == 4949) || (startValueA == 4949 && startValueB == 2424))
            && info.IsIndicatorPresent(Indicator.SIG);
        // If one of the stage's starting values is 2424, the other stage's starting value is 4949,
        // an indicator labeled SIG is present, and a cyan button is present on both stages...
    }

    bool isSpecialDay()
    {
        return (TodaysDay == 4 && DateTime.Today.Month == 2) || (TodaysDay == 9 && DateTime.Today.Month == 4);
    } // Return true if the module showed up on the 4th of Feb or 9th of April.


    private readonly int AnimTime = 20;
    IEnumerator HideButtons()
    {
        Vector3 localDoorPt1 = animationPointDoorA.transform.localPosition;
        Vector3 localDoorPt2 = animationPointDoorB.transform.localPosition;

        Vector3 buttonGroupPt1 = animationPointButtonA.transform.localPosition;
        Vector3 buttonGroupPt2 = animationPointButtonB.transform.localPosition;

        for (int stp = 0; stp <= AnimTime; stp++)
        {
            Vector3 curlocalPos = new Vector3();
            for (int i = 0; i < AnimTime; i++)
            {
                if (i < stp)
                {
                    curlocalPos += buttonGroupPt2;
                }
                else
                {
                    curlocalPos += buttonGroupPt1;
                }
            }
            curlocalPos /= AnimTime;
            buttonGroup.transform.localPosition = curlocalPos;

            yield return new WaitForSeconds(0);
        }

        for (int stp = 0; stp <= AnimTime; stp++)
        {
            Vector3 curlocalPos = new Vector3();
            for (int i = 0; i < AnimTime; i++)
            {
                if (i < stp)
                {
                    curlocalPos += localDoorPt1;
                }
                else
                {
                    curlocalPos += localDoorPt2;
                }
            }
            curlocalPos /= AnimTime;
            door.transform.localPosition = curlocalPos;

            yield return new WaitForSeconds(0);
        }
        playingAnim = false;
        yield return null;
    }

    IEnumerator ShowButtons()
    {
        Vector3 localDoorPt1 = animationPointDoorA.transform.localPosition;
        Vector3 localDoorPt2 = animationPointDoorB.transform.localPosition;

        Vector3 buttonGroupPt1 = animationPointButtonA.transform.localPosition;
        Vector3 buttonGroupPt2 = animationPointButtonB.transform.localPosition;
        for (int stp = 0; stp <= AnimTime; stp++)
        {
            Vector3 curlocalPos = new Vector3();
            for (int i = 0; i < AnimTime; i++)
            {
                if (i < stp)
                {
                    curlocalPos += localDoorPt2;
                }
                else
                {
                    curlocalPos += localDoorPt1;
                }
            }
            curlocalPos /= AnimTime;
            door.transform.localPosition = curlocalPos;

            yield return new WaitForSeconds(0);
        }
        for (int stp = 0; stp <= AnimTime; stp++)
        {
            Vector3 curlocalPos = new Vector3();
            for (int i = 0; i < AnimTime; i++)
            {
                if (i < stp)
                {
                    curlocalPos += buttonGroupPt1;
                }
                else
                {
                    curlocalPos += buttonGroupPt2;
                }
            }
            curlocalPos /= AnimTime;
            buttonGroup.transform.localPosition = curlocalPos;

            yield return new WaitForSeconds(0);
        }
        playingAnim = false;
        interactable = true;
        currentPart = 0;
        yield return null;
    }

    readonly string[] colorblindInit = new string[] {"R", "Y", "G", "C", "B", "M", "W", "K" };
    void UpdateButtons(int stageNum)
    {
        GameObject[] GameOBJArray = new GameObject[] { buttonL, buttonM, buttonR };
        for (int x = 0; x < GameOBJArray.Length; x++)
        {
            TextMesh curText = GameOBJArray[x].GetComponentInChildren<TextMesh>();
            MeshRenderer curMeshRdr = GameOBJArray[x].GetComponent<MeshRenderer>();
            string detectStr;
            if (stageNum == 1)
            {
                detectStr = stage1ButtonColors[x];
                curText.text = stage1ButtonDigits[x].ToString();
            }
            else if (stageNum == 2)
            {
                detectStr = stage2ButtonColors[x];
                curText.text = stage2ButtonDigits[x].ToString();
            }
            else
            {
                detectStr = "White";
                curText.text = "?";
            }
            int idxDtr = colorString.IndexOf(detectStr);
            curMeshRdr.material.color = colorList[idxDtr];
            if (colorBlindActive)
            {
                curText.text += colorblindInit[idxDtr];
                curText.characterSize = 0.75f;
            }
            else
            {
                curText.characterSize = 1f;
            }
            curText.color = detectStr.Equals("Black") || detectStr.Equals("Blue") ? Color.white : Color.black;
        }
        
    }
    IEnumerator ChangeToStage(int stageNum)
    {
        interactable = false;
        playingAnim = true;
        StartCoroutine(HideButtons());
        highlightStage1.GetComponent<MeshRenderer>().material.color = stagesCompleted[0] ? indcColors[2] : indcColors[0];
        highlightStage2.GetComponent<MeshRenderer>().material.color = stagesCompleted[1] ? indcColors[2] : indcColors[0];
        delay = 90;
        sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.WireSequenceMechanism, transform);
        while (playingAnim)
        {
            display.text = "Transitioning...";
            yield return new WaitForSeconds(0);
        }
        UpdateButtons(stageNum);
        currentStage = stageNum;
        // Reveal Other Stage
        StartCoroutine(ShowButtons());
        display.text = "";
        yield return null;
    }

    // Update is called once per frame
    int delay = 90;
    int currentPart = 0;
    void Update()
    {
        buttonL.transform.localPosition = new Vector3(buttonL.transform.localPosition.x, 0, buttonL.transform.localPosition.z);
        buttonM.transform.localPosition = new Vector3(buttonM.transform.localPosition.x, 0, buttonM.transform.localPosition.z);
        buttonR.transform.localPosition = new Vector3(buttonR.transform.localPosition.x, 0, buttonR.transform.localPosition.z);
        if (curbtnHeld == 0)
        {
            buttonL.transform.localPosition += Vector3.down / 100f;
        }
        else if (curbtnHeld == 1)
        {
            buttonM.transform.localPosition += Vector3.down / 100f;
        }
        else if (curbtnHeld == 2)
        {
            buttonR.transform.localPosition += Vector3.down / 100f;
        }

        if (interactable)
        {
            if (delay >= 45)
            {
                highlightStage1.GetComponent<MeshRenderer>().material.color = currentStage == 1 ? indcColors[1] : stagesCompleted[0] ? indcColors[2] : indcColors[0];
                highlightStage2.GetComponent<MeshRenderer>().material.color = currentStage == 2 ? indcColors[1] : stagesCompleted[1] ? indcColors[2] : indcColors[0];
            }
            else
            {
                highlightStage1.GetComponent<MeshRenderer>().material.color = stagesCompleted[0] ? indcColors[2] : indcColors[0];
                highlightStage2.GetComponent<MeshRenderer>().material.color = stagesCompleted[1] ? indcColors[2] : indcColors[0];
            }
            if (delay <= 0)
            { delay = 90; }
            else
            {
                delay--;
            }
        }
    }

    bool TwitchPlaysEnabled;
    bool ZenModeActive;

    readonly string TwitchHelpMessage = "To hold a given button at a specific time: \"!{0} hold left/middle/right at ##:##\" To tap a given button at a specific time: \"!{0} tap left/middle/right at ##:##\"\n" +
        "To release a button at a specific time based on the display: \"!{0} release display ##:##\" To release a button at a specific time based on the bomb timer: \"!{0} release bombtime ##:##\"\n" +
        "Time format is MM:SS with MM being able to exceed 99 min. To switch between stages: \"!{0} toggle/switch\" You can only switch stages if you are NOT holding a button!";
}
