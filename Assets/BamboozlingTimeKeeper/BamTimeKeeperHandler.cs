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
    }.ToList(), ThreeStageModIDs = new string[] { // An ID List of 3 Stage Modules on the bomb.
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
    private string[,] ManualOperationsList = new string[,] {
        { "STOP", "+1", "-10", "B1", "B3", "+559", "*3", "B1", "-65", "B1", "*10", "+84", "/4", "+485", "+459", "+45", "*2", "+456", "+45", "B3", "STOP", "+45", "-512", "*5", "*5", "*2", "*2", "B1", "/5", "-1", "+10", "B3", "*4", "-559", "+3", "STOP", "+65", "B2", "/10", "-84", "+4", "-485", "-459", "-45", "/2", "-456", "-45", "+0", "*2", "+330", "+512", "/5", "/5", "/2", "/2", "STOP" },
        { "*5", "STOP", "+25", "/2", "B2", "-58", "/3", "+158", "+46", "B1", "/10", "-46", "*3", "+54", "-485", "-90", "+256", "-98", "*5", "B3", "-59", "STOP", "+128", "/5", "*12", "*5", "/2", "B2", "-50", "+1", "-25", "*2", "/4", "+58", "STOP", "B3", "-46", "B2", "*10", "+46", "-3", "-84", "+485", "+90", "-256", "+98", "/5", "-25", "/2", "-330", "-126", "*5", "/12", "/5", "STOP", "B1"},
        { "+50", "-1", "STOP", "*2", "B1", "+25", "*6", "B2", "-415", "B1", "*10", "+94", "/3", "+95", "-458", "+30", "-32", "-485", "/9", "B3", "/3", "-89", "STOP", "*10", "-120", "*7", "*2", "B3", "*2", "B3", "+25", "/2", "+4", "STOP", "/3", "-158", "+415", "B2", "/10", "-94", "+3", "-95", "+458", "-30", "+32", "+485", "*9", "+25", "*5", "+90", "/2", "/10", "+120", "STOP", "*2", "B3"},
        { "/2", "B2", "-25", "STOP", "B2", "-48", "/6", "-264", "+48", "B1", "/10", "+54", "*4", "+24", "+478", "+120", "+64", "+741", "+45", "B3", "+55", "*7", "*2", "STOP", "-60", "+21", "/2", "B3", "+25", "B2", "-66", "B1", "STOP", "-25", "*3", "B1", "-48", "B2", "*10", "-54", "-4", "-24", "-478", "-120", "-64", "-741", "-45", "-25", "/5", "-90", "*5", "*10", "STOP", "/7", "/2", "B2"},
        { "-25", "B1", "+66", "B2", "STOP", "+99", "+9", "*56", "-56", "B1", "*10", "+49", "/2", "-48", "+0", "-150", "-64", "-46", "-5", "B3", "-20", "/5", "/5", "/10", "STOP", "/2", "*2", "B2", "B1", "/3", "+512", "STOP", "-4", "+48", "/6", "+264", "+56", "B2", "/10", "-49", "+2", "+48", "+0", "+150", "+64", "+46", "+5", "+25", "*10", "/3", "/5", "STOP", "+60", "-21", "*2", "B1"},
        { "B3", "*3", "-512", "/9", "B3", "STOP", "-6", "/7", "-99", "B1", "/10", "+21", "*2", "-78", "-45", "+15", "+32", "+155", "+46", "B3", "+23", "+48", "*5", "+1", "+180", "STOP", "/2", "B3", "B2", "*3", "STOP", "*9", "*4", "-99", "*6", "/56", "+99", "B2", "*10", "-21", "-2", "+78", "+45", "-15", "-32", "-155", "-46", "-25", "/10", "*3", "STOP", "-1", "-180", "*2", "/2", "B3"},
        { "B1", "/3", "+12", "*6", "B1", "-454", "STOP", "B3", "+284", "B1", "-100", "+86", "/6", "-459", "*5", "+15", "-256", "-45", "+75", "B3", "+59", "+89", "/2", "-151", "/5", "/5", "STOP", "B1", "B3", "STOP", "-12", "/6", "/4", "+454", "-9", "*7", "-284", "B2", "+100", "-86", "-6", "+459", "/5", "-15", "+256", "+45", "-75", "+25", "+20", "STOP", "*2", "+151", "*5", "*5", "*2", "B2"},
        { "B2", "B3", "/5", "B3", "B2", "*5", "-3", "STOP", "-54", "B1", "-100", "*11", "*6", "-101", "/5", "*15", "/2", "+485", "-87", "B3", "-41", "-78", "+155", "+150", "/12", "/7", "+40", "STOP", "STOP", "B1", "*5", "B2", "+0", "/5", "+6", "B2", "+54", "B2", "+100", "/11", "+6", "+101", "*5", "-15", "*2", "-485", "+87", "+0", "STOP", "/10", "-155", "-150", "*12", "*7", "-40", "B1"}
    };// Current Format is (Color,PhraseText)
    private List<string> stage1Phrases = new List<string>(), stage2Phrases = new List<string>();
    private List<string> stage1PhrClr = new List<string>(), stage2PhrClr = new List<string>();
    private int[] stage1ButtonDigits = new int[3], stage2ButtonDigits = new int[3];

    public long startValueA, startValueB;
    private long finalValueA, finalValueB;
    private int scaleFactorA = 0, scaleFactorB = 0;
    private int? crtBtnIdxStg1, crtBtnIdxStg2;
    private List<long> possibleTimesA = new List<long>(), possibleTimesB = new List<long>();

    private int TodaysDay = DateTime.Today.Day;
    private int TodaysMonth = DateTime.Today.Month;
    private bool oneTapHolds = false, playingAnim = false, interactable = false, colorBlindActive = false, started = false, specialDay, holdCorStg1, holdCorStg2, isHeld = false;
    

    private int curbtnHeld = -1, currentStage = 0, timeHeldSec = 0;
    private static int modID = 1;
    public int curModId;
    private bool[] stagesCompleted = new bool[] { false, false };

    private string curSerNo;
    private List<string> idModsonBomb = new List<string>(), nameModsonBomb = new List<string>();
    private int batteryCount, batteryholdCount, startTime;

    private readonly string[] specialDayStartPhrases = new string[] {
        "Iku zo.",
        "Showtime.",
        "Are we starting soon?",
        "Let's do this.",
        "Yaru no ka."
    }, specialDayStrikePhrases = new string[] {
        "Taihen da.",
        "Oh boy.",
        "Wait, what?",
        "Oof.",
        "Huh, too bad.",
        "Aw...",
    }, awakePhrases = new string[] { 
        "YOU WON'T WIN",
        "YOU WILL SUFFER",
        "NOTHING SPARED",
        "YOU'RE NOT SAFE",
        "NOTHING'S SAFE",
        "THIS BLOCKS YOU",
        "BLOCKED BY HATE"
    }, strikePhrases = new string[] {
        "THAT'S A STRIKE",
        "FAILED AGAIN",
        "NOT YET",
        "TRY AGAIN",
        "THAT'S NOT RIGHT",
        "THAT'S WRONG",
        "THAT IS WRONG",
        "NO U",
        "GET BAMBOOZLED",
        "STILL WRONG"
    };

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
            sound.PlaySoundAtTransform("SigFeverEnter", transform);
            long[] listValues = new long[] { 2424, 4949 };
            int idxSwap = UnityEngine.Random.Range(0, 2);
            long tempvalue = listValues[idxSwap];
            listValues[idxSwap] = listValues[0];
            listValues[0] = tempvalue;

            startValueA = listValues[0];
            startValueB = listValues[1];
            stage1ButtonColors[0] = "Cyan";
            stage2ButtonColors[0] = "Cyan";
            display.text = specialDayStartPhrases[UnityEngine.Random.Range(0, specialDayStartPhrases.Length)];
        }
        else
        {
            sound.PlaySoundAtTransform("KefkaLaugh", transform);
            startValueA = UnityEngine.Random.Range(0, 10000);
            startValueB = UnityEngine.Random.Range(0, 10000);
            display.text = awakePhrases[UnityEngine.Random.Range(0, awakePhrases.Length)];
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

            curSerNo = info.GetSerialNumber();
            startTime = Mathf.RoundToInt(info.GetTime());
            idModsonBomb = info.GetModuleIDs();
            nameModsonBomb = info.GetModuleNames();
            batteryCount = info.GetBatteryCount();
            batteryholdCount = info.GetBatteryHolderCount();
            //Redirect to Calculation Methods
            CalculateStage1();
            while (finalValueA < 10)
                finalValueA += 15;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The final value for stage 1 is {1}", curModId, finalValueA);
            

            CalculateStage2();
            while (finalValueB < 10)
                finalValueB += 15;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The final value for stage 2 is {1}", curModId, finalValueB);
            CalcScaleFactors();

            CalculateAllPossibleTimes(possibleTimesA, finalValueA, scaleFactorA);
            CalculateAllPossibleTimes(possibleTimesB, finalValueB, scaleFactorB);

            DetermineCorrectButton(1);
            DetermineCorrectButton(2);
            //End Redirect to Calculation Methods
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 1 FINAL VALUE: {1}", curModId, finalValueA);
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 1 SCALE FACTOR: {1}", curModId, scaleFactorA);
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 1 POSSIBLE TIMES:\n {1}", curModId, FormatDebugList(possibleTimesA.Where(a => a < int.MaxValue).ToList()));
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 1 CORRECT BUTTON: {1}", curModId, buttonPos[(int)crtBtnIdxStg1]);

            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 2 FINAL VALUE: {1}", curModId, finalValueB);
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 2 SCALE FACTOR: {1}", curModId, scaleFactorB);
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 2 POSSIBLE TIMES:\n {1}", curModId, FormatDebugList(possibleTimesB.Where(a => a < int.MaxValue).ToList()));
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 2 CORRECT BUTTON: {1}", curModId, buttonPos[(int)crtBtnIdxStg2]);
            started = true;
        };
        for (int x = 0; x < stageSelectables.Length; x++)
        {
            int y = x;
            stageSelectables[x].OnInteract += delegate {
                sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
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
                        if (curbtnHeld == y)
                        {
                            curbtnHeld = -1;
                            sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonRelease, transform);
                        }
                        else if (curbtnHeld == -1)
                        {
                            sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonPress, transform);
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
        return (TodaysDay == 4 && TodaysMonth == 2) || (TodaysDay == 9 && TodaysMonth == 4);
    } // Return true if the module showed up on the 4th of Feb or 9th of April.

    int GetValueofBase36Digit(char oneDigit)
    {
        switch (oneDigit.ToString().ToLower().ToCharArray()[0])// Ensure the value is a lowercase value. In case it wasn't set to that already.
        {
            case 'z':
                return 35;
            case 'y':
                return 34;
            case 'x':
                return 33;
            case 'w':
                return 32;
            case 'v':
                return 31;
            case 'u':
                return 30;
            case 't':
                return 29;
            case 's':
                return 28;
            case 'r':
                return 27;
            case 'q':
                return 26;
            case 'p':
                return 24;
            case 'o':
                return 24;
            case 'n':
                return 23;
            case 'm':
                return 22;
            case 'l':
                return 21;
            case 'k':
                return 20;
            case 'j':
                return 19;
            case 'i':
                return 18;
            case 'h':
                return 17;
            case 'g':
                return 16;
            case 'f':
                return 15;
            case 'e':
                return 14;
            case 'd':
                return 13;
            case 'c':
                return 12;
            case 'b':
                return 11;
            case 'a':
                return 10;
            case '9':
                return 9;
            case '8':
                return 8;
            case '7':
                return 7;
            case '6':
                return 6;
            case '5':
                return 5;
            case '4':
                return 4;
            case '3':
                return 3;
            case '2':
                return 2;
            case '1':
                return 1;
            case '0':
            default:
                return 0;
        }
    }

    string FormatDebugList(List<string> listString)
    {
        string output = "";
        for (int x = 0; x < listString.Count;x++)
        {
            if (x != 0)
                output += ", ";
            output += listString[x];
        }
        return output;
    }
    string FormatDebugList(List<int> listString)
    {
        string output = "";
        for (int x = 0; x < listString.Count; x++)
        {
            if (x != 0)
                output += ", ";
            output += listString[x];
        }
        return output;
    }
    string FormatDebugList(List<long> listString)
    {
        string output = "";
        for (int x = 0; x < listString.Count; x++)
        {
            if (x != 0)
                output += ", ";
            output += listString[x];
        }
        return output;
    }
    string GrabCellFromManualTable(int xidx, int yidx)
    {
        //print(xidx + "," + yidx);
        return xidx >= 0 && xidx < ManualOperationsList.GetLength(0) && yidx >= 0 && yidx < ManualOperationsList.GetLength(1) ? ManualOperationsList[xidx, yidx] : "";
    }// Grab the operation from the operation list.
    readonly string[] buttonPos = new string[] { "LEFT", "MIDDLE", "RIGHT" };
    readonly string[] vanillaInds = new string[] { "SND", "CLR", "CAR", "IND", "FRQ", "SIG", "NSA", "MSA", "TRN", "BOB", "FRK" };
    readonly string[] vanillaPorts = new string[] { "DVI", "Parallel", "PS/2", "RJ45", "Serial", "StereoRCA" };
    readonly string[] definablePhrases = new string[] { "ONE NUMBER", "TWO NUMBERS", "THREE NUMBERS", "FOUR NUMBERS", "1 NUMBER", "2 NUMBERS", "3 NUMBERS", "4 NUMBERS", "A NUMBER" };
    int stageRuleApplied = 0;
    void CalculateStage1()
    {
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 1 CALCULATIONS:", curModId);
        if (canOverride())
        {
            finalValueA = 15;
            scaleFactorA = 2;
            crtBtnIdxStg1 = stage1ButtonColors.IndexOf("Cyan");
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]:"+" Override present! Final Value for this stage is actually 15 seconds, with a scale factor of 2, with the correct button being the {1} button!".ToUpper(), curModId, buttonPos[(int)crtBtnIdxStg1]);
            return; 
        }
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The starting value for stage 1 is {1}", curModId, startValueA);
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The phrases for stage 1 are {1}", curModId, FormatDebugList(stage1Phrases));
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The color of the phrases for stage 1 are {1}", curModId, FormatDebugList(stage1PhrClr));
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The button colors for stage 1 are {1}", curModId, FormatDebugList(stage1ButtonColors));
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The digits on the buttons for stage 1 are {1}", curModId, FormatDebugList(stage1ButtonDigits.ToList()));
        finalValueA = startValueA;
        // Step 2
        string firstSerNos = curSerNo.Substring(0, curSerNo.Length / 2);
        foreach (char srlchr in firstSerNos.ToCharArray())
        { finalValueA -= GetValueofBase36Digit(srlchr); }
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 2 applied.", curModId);
        // Step 3
        finalValueA += 4 * (info.GetPortCount() + info.GetPortPlateCount());
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 3 applied.", curModId);
        // Step 4
        int speakingCount = 0;
        int forgetCount = 0;
        foreach (string modids in idModsonBomb.Where(a => SpeakingModIDs.Contains(a)))
        {
            if (modids.Equals("simonStores"))
                speakingCount += 2;
            else if (modids.Equals("UltraStores"))
                speakingCount += 5;
            else
                speakingCount++;
        }
        speakingCount = Math.Max(1, speakingCount);
        foreach (string modName in nameModsonBomb.Where(a => a.Contains("Forget")))
        {
            if (modName.Equals("Forget Them All"))
                forgetCount += 5;
            else if (modName.Equals("Forget Me Now"))
                forgetCount += 2;
            else
                forgetCount++;
        }
        forgetCount = Math.Max(1, speakingCount);
        int positiveDifference = Math.Abs(speakingCount - forgetCount);
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 4: The positive difference between the number of modules on the bomb with the word \"Forget\" in its name and the number of modules made by SpeakingEvil on the bomb is {1}", curModId, positiveDifference);
        finalValueA *= positiveDifference;
        // Step 5
        if (nameModsonBomb.Contains("Simon Stops"))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 5: False", curModId);
            finalValueA = finalValueA * 3 / 2;
        }
        else
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 5: True", curModId);
            finalValueA /= 2;
        }
        // Step 6
        if (stage1ButtonColors.Distinct().Count() == 1)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 6: True", curModId);
            crtBtnIdxStg1 = 0;
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 6: False", curModId); 
        // Step 7
        if (!stage1PhrClr[1].Equals("Red") && stage1ButtonColors.Contains("Red"))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 7: True", curModId);
            finalValueA += startValueB;
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 7: False", curModId);
        // Step 8
        finalValueA += idModsonBomb.Where(a => RTControlModIDs.Contains(a)).Count() * batteryCount;
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 8 applied.", curModId);
        // Step 9
        if (stage1ButtonColors[1].Equals("Green") || stage1ButtonColors[1].Equals("Magenta"))
        {
            crtBtnIdxStg1 = 1;
            if (stageRuleApplied == 0)
                stageRuleApplied = 1;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 9: True", curModId);
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 9: False", curModId);
        // Step 10
        if (finalValueA % 1176 <= 5 || finalValueA % 1176 >= 1171)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 10: True", curModId);
            return;
        }
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 10: False", curModId);
        // Step 11
        finalValueA += TodaysDay;
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 11 applied.", curModId);
        // Step 12
        int vanillaIndOnCnt = 0;
        int vanillaIndOffCnt = 0;
        foreach (string unlitIndc in info.GetOffIndicators())
        {
            if (vanillaInds.Contains(unlitIndc))
            {
                vanillaIndOffCnt++;
            }
        }
        foreach (string litIndc in info.GetOnIndicators())
        {
            if (vanillaInds.Contains(litIndc))
            {
                vanillaIndOnCnt++;
            }
        }
        finalValueA += 20 * (vanillaIndOnCnt - vanillaIndOffCnt);
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 12 applied.", curModId);
        // Step 13
        int ThreeStgModsCnt = Math.Max(idModsonBomb.Where(a => ThreeStageModIDs.Contains(a)).Count(), 1);
        if (startValueA < 5000)
        {
            finalValueA *= ThreeStgModsCnt;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 13: Stage 1's starting value is < 5000", curModId);
        }
        else {
            finalValueA += ThreeStgModsCnt;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 13: Stage 1's starting value is >= 5000", curModId);
        }
        // Step 14
        int modPortCnt = 0;
        foreach (string portName in info.GetPorts())
        {
            if (!vanillaPorts.Contains(portName))
                modPortCnt++;
        }
        finalValueA -= modPortCnt * 6;
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 14 applied. Going to Process for Stage 1", curModId);
        // Step 17
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Begin Step 17", curModId);
        bool canStop = false;
        for (int x = 0; x < stage1Phrases.Count && !canStop; x++)
        {
            string curCell = GrabCellFromManualTable(colIndex.IndexOf(stage1PhrClr[x].ToUpper()), rowIndex.IndexOf(stage1Phrases[x].ToUpper()));
            if (curCell.Length != 0) Debug.LogFormat("[Bamboozling Time Keeper #{0}]: \"{1}\" is a valid cell from the table.", curModId, curCell);
            if (curCell.Equals("STOP"))
            {
                canStop = true;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This ends step 17.", curModId);
            }
            else if (curCell.RegexMatch(@"^B\d$"))
            {
                crtBtnIdxStg1 = int.Parse(curCell.Substring(1, 1)) - 1;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This changed the correct button to be on the {1}", curModId, buttonPos[(int)crtBtnIdxStg1]);
            }
            else if (curCell.RegexMatch(@"^\+\d+"))
            {
                int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                finalValueA += modifiedValue;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This added the current value by {1}", curModId, modifiedValue);
            }
            else if (curCell.RegexMatch(@"^\-\d+"))
            {
                int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                finalValueA -= modifiedValue;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This subtracted the current value by {1}", curModId, modifiedValue);
            }
            else if (curCell.RegexMatch(@"^\*\d+"))
            {
                int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                finalValueA *= modifiedValue;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This multiplied the current value by {1}", curModId, modifiedValue);
            }
            else if (curCell.RegexMatch(@"^\/\d+"))
            {
                int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                finalValueA /= modifiedValue;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This divided the current value by {1}", curModId, modifiedValue);
            }
        }
        // Step 18
        if (definablePhrases.Contains(stage1Phrases[0]))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 18: True", curModId);
            foreach (string unlitInd in info.GetOffIndicators())
                finalValueA += GetValueofBase36Digit(unlitInd.ToCharArray()[0]);
        }
        else
        { Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 18: False", curModId); }
        // Step 19
        if (stage1Phrases.Contains("HUNDRED"))
        {
            finalValueA /= 100;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 19: True", curModId);
        }
        else { Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 19: False", curModId); }
        // Step 20
        if (!nameModsonBomb.Contains("Simon's Stages") && nameModsonBomb.Contains("Übermodule"))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 20: True", curModId);
            return;
        }
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 20: False", curModId);
        // Step 21
        int needyCount = nameModsonBomb.Count - info.GetSolvableModuleNames().Count;
        finalValueA += 99 * needyCount;
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 21: There are this many needies on the bomb: {1}", curModId,needyCount);
        // Step 22
        if (stage1Phrases[0].Contains("(") || stage1Phrases[0].RegexMatch(@"^\d\sNUMBERS?"))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 22: True, processing like Step 17", curModId);
            canStop = false;
            for (int x = 0; x < stage1ButtonColors.Count && !canStop; x++)
            {
                string curCell = GrabCellFromManualTable(colIndex.IndexOf(stage1ButtonColors[x].ToUpper()), rowIndex.IndexOf(stage1ButtonDigits[x].ToString()));
                if (curCell.Length != 0) Debug.LogFormat("[Bamboozling Time Keeper #{0}]: \"{1}\" is a valid cell from the table.", curModId, curCell);
                if (curCell.Equals("STOP"))
                {
                    canStop = true;
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This ends step 22.", curModId);
                }
                else if (curCell.RegexMatch(@"^B\d$"))
                {
                    crtBtnIdxStg1 = int.Parse(curCell.Substring(1, 1)) - 1;
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This changed the correct button to be on the {1}", curModId, buttonPos[(int)crtBtnIdxStg1]);
                }
                else if (curCell.RegexMatch(@"^\+\d+"))
                {
                    int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                    finalValueA += modifiedValue;
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This added the current value by {1}", curModId, modifiedValue);
                }
                else if (curCell.RegexMatch(@"^\-\d+"))
                {
                    int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                    finalValueA -= modifiedValue;
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This subtracted the current value by {1}", curModId, modifiedValue);
                }
                else if (curCell.RegexMatch(@"^\*\d+"))
                {
                    int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                    finalValueA *= modifiedValue;
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This multiplied the current value by {1}", curModId, modifiedValue);
                }
                else if (curCell.RegexMatch(@"^\/\d+"))
                {
                    int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                    finalValueA /= modifiedValue;
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This divided the current value by {1}", curModId, modifiedValue);
                }
            }
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 22: False", curModId);
        if (finalValueA < 0)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 23: True", curModId);
            finalValueA *= -6;
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 23: False", curModId);
        string last3SerDigits = curSerNo.Substring(curSerNo.Length / 2, curSerNo.Length - (curSerNo.Length / 2));
        foreach (char serchar in last3SerDigits.ToCharArray())
        {
            finalValueA += GetValueofBase36Digit(serchar);
        }
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 24 applied.", curModId);
    }
    void CalculateStage2()
    {
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 2 CALCULATIONS:", curModId);
        if (canOverride())
        {
            finalValueB = 15;
            scaleFactorB = 2;
            crtBtnIdxStg2 = stage2ButtonColors.IndexOf("Cyan");
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]:"+" Override present! Final Value for this stage is actually 15 seconds, with a scale factor of 2, with the correct button being the {1} button!".ToUpper(), curModId, buttonPos[(int)crtBtnIdxStg2]);
            return;
        }
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The starting value for stage 2 is {1}", curModId, startValueB);
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The phrases for stage 2 are {1}", curModId, FormatDebugList(stage2Phrases));
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The color of the phrases for stage 2 are {1}", curModId, FormatDebugList(stage2PhrClr));
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The button colors for stage 2 are {1}", curModId, FormatDebugList(stage2ButtonColors));
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The digits on the buttons for stage 2 are {1}", curModId, FormatDebugList(stage2ButtonDigits.ToList()));
        finalValueB = startValueB;
        // Step 2
        string firstSerNos = curSerNo.Substring(0, curSerNo.Length / 2);
        foreach (char srlchr in firstSerNos.ToCharArray())
        { finalValueB -= GetValueofBase36Digit(srlchr); }
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 2 applied.", curModId);
        // Step 3
        finalValueB += 4 * (info.GetPortCount() + info.GetPortPlateCount());
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 3 applied.", curModId);
        // Step 4
        int speakingCount = 0;
        int forgetCount = 0;
        foreach (string modids in idModsonBomb.Where(a => SpeakingModIDs.Contains(a)))
        {
            if (modids.Equals("simonStores"))
                speakingCount += 2;
            else if (modids.Equals("UltraStores"))
                speakingCount += 5;
            else
                speakingCount++;
        }
        speakingCount = Math.Max(1, speakingCount);
        foreach (string modName in nameModsonBomb.Where(a => a.Contains("Forget")))
        {
            if (modName.Equals("Forget Them All"))
                forgetCount += 5;
            else if (modName.Equals("Forget Me Now"))
                forgetCount += 2;
            else
                forgetCount++;
        }
        forgetCount = Math.Max(1, speakingCount);
        int positiveDifference = Math.Abs(speakingCount - forgetCount);
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 4: The positive difference between the number of modules on the bomb with the word \"Forget\" in its name and the number of modules made by SpeakingEvil on the bomb is {1}", curModId, positiveDifference);
        finalValueB *= positiveDifference;
        // Step 5
        if (nameModsonBomb.Contains("Simon Stops"))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 5: True", curModId);
            finalValueB = finalValueB * 3 / 2;
        }
        else
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 5: False", curModId);
            finalValueB /= 2;
        }
        // Step 6
        if (stage2ButtonColors.Distinct().Count() == 1)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 6: True", curModId);
            crtBtnIdxStg2 = 1;
        }
        else
        { Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 6: False", curModId); }
        // Step 7
        if (!stage2PhrClr[1].Equals("Red") && stage2ButtonColors.Contains("Red"))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 7: True", curModId);
            finalValueB += startValueA;
        }
        else
        { Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 7: False", curModId); }
        // Step 8
        finalValueB += idModsonBomb.Where(a => RTControlModIDs.Contains(a)).Count() * batteryCount;
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 8 applied.", curModId);
        // Step 9
        if (stage2ButtonColors[1].Equals("Green") || stage2ButtonColors[1].Equals("Magenta"))
        {
            crtBtnIdxStg2 = 1;
            if (stageRuleApplied == 0)
                stageRuleApplied = 2;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 9: True", curModId);
        }
        else
        { Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 9: False", curModId); }
        // Step 10
        if (finalValueB % 1176 <= 5 || finalValueB % 1176 >= 1171)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 10: True", curModId);
            return;
        }
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 10: False", curModId);
        // Step 11
        finalValueB += TodaysDay;
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 11 applied.", curModId);
        // Step 12
        int vanillaIndOnCnt = 0;
        int vanillaIndOffCnt = 0;
        foreach (string unlitIndc in info.GetOffIndicators())
        {
            if (vanillaInds.Contains(unlitIndc))
            {
                vanillaIndOffCnt++;
            }
        }
        foreach (string litIndc in info.GetOnIndicators())
        {
            if (vanillaInds.Contains(litIndc))
            {
                vanillaIndOnCnt++;
            }
        }
        finalValueB += 20 * (vanillaIndOnCnt - vanillaIndOffCnt);
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 12 applied.", curModId);
        // Step 13
        int ThreeStgModsCnt = Math.Max(idModsonBomb.Where(a => ThreeStageModIDs.Contains(a)).Count(), 1);
        if (startValueB < 5000)
        {
            finalValueB *= ThreeStgModsCnt;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 13: Stage 2's starting value is < 5000", curModId);
        }
        else
        {
            finalValueB += ThreeStgModsCnt;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 13: Stage 2's starting value is >= 5000", curModId);
        }
        // Step 14
        int modPortCnt = 0;
        foreach (string portName in info.GetPorts())
        {
            if (!vanillaPorts.Contains(portName))
                modPortCnt++;
        }
        finalValueB -= modPortCnt * 6;
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 14 applied. Going to Process for Stage 2...", curModId);
        // Step 18
        if (definablePhrases.Contains(stage2Phrases[0]))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 18: True", curModId);
            string curCell = GrabCellFromManualTable(colIndex.IndexOf(stage2PhrClr[1].ToUpper()), rowIndex.IndexOf(stage2Phrases[1].ToUpper()));
            if (curCell.Length != 0) Debug.LogFormat("[Bamboozling Time Keeper #{0}]: \"{1}\" is a valid cell from the table.", curModId, curCell);
            if (curCell.Equals("STOP"))
            {
                finalValueB /= 17;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This divides the current value by 17", curModId);
            }
            else if (curCell.RegexMatch(@"^B\d$"))
            {
                int multiplier = int.Parse(curCell.Substring(1, 1)) + 1;
                finalValueB *= multiplier;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This multiplied the value by {1}", curModId, multiplier);
            }
            else if (curCell.RegexMatch(@"^\+\d+"))
            {
                int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                finalValueB += modifiedValue;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This added the current value by {1}", curModId, modifiedValue);
            }
            else if (curCell.RegexMatch(@"^\-\d+"))
            {
                int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                finalValueB -= modifiedValue;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This subtracted the current value by {1}", curModId, modifiedValue);
            }
            else if (curCell.RegexMatch(@"^\*\d+"))
            {
                int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                finalValueB *= modifiedValue;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This multiplied the current value by {1}", curModId, modifiedValue);
            }
            else if (curCell.RegexMatch(@"^\/\d+"))
            {
                int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                finalValueB /= modifiedValue;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This divided the current value by {1}", curModId, modifiedValue);
            }
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 18: False", curModId);
        // Step 19
        if (startingPhrases.IndexOf(stage2Phrases[0]) >= stage2Phrases.Count * 2 / 3)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 19: True, performing like Step 18", curModId);
            if (!stage2Phrases[stage2Phrases.Count - 2].Equals("HUNDRED"))
            {
                string curCell = GrabCellFromManualTable(colIndex.IndexOf(stage2PhrClr[stage2PhrClr.Count - 2].ToUpper()), rowIndex.IndexOf(stage2Phrases[stage2Phrases.Count - 2].ToUpper()));
                if (curCell.Length != 0) Debug.LogFormat("[Bamboozling Time Keeper #{0}]: \"{1}\" is a valid cell from the table.", curModId, curCell);
                if (curCell.Equals("STOP"))
                {
                    finalValueB /= 17;
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This divides the current value by 17", curModId);
                }
                else if (curCell.RegexMatch(@"^B\d$"))
                {
                    int multiplier = int.Parse(curCell.Substring(1, 1)) + 1;
                    finalValueB *= multiplier;
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This multiplied the value by {1}", curModId, multiplier);
                }
                else if (curCell.RegexMatch(@"^\+\d+"))
                {
                    int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                    finalValueB += modifiedValue;
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This added the current value by {1}", curModId, modifiedValue);
                }
                else if (curCell.RegexMatch(@"^\-\d+"))
                {
                    int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                    finalValueB -= modifiedValue;
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This subtracted the current value by {1}", curModId, modifiedValue);
                }
                else if (curCell.RegexMatch(@"^\*\d+"))
                {
                    int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                    finalValueB *= modifiedValue;
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This multiplied the current value by {1}", curModId, modifiedValue);
                }
                else if (curCell.RegexMatch(@"^\/\d+"))
                {
                    int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                    finalValueB /= modifiedValue;
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This divided the current value by {1}", curModId, modifiedValue);
                }
            }
            else
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The 2nd to last phrase is actually \"HUNDRED\"", curModId);
                finalValueB *= 100;
            }
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 19: False", curModId);
        // Step 20,21,22
        if (stage1Phrases[0].Contains("(") || stage1Phrases[0].RegexMatch(@"^\d\sNUMBERS?"))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 20: True", curModId);
            // Step 21
            string curCell = GrabCellFromManualTable(7 - colIndex.IndexOf(stage2ButtonColors[1].ToUpper()), (rowIndex.IndexOf(stage2Phrases[1].ToUpper()) + rowIndex.IndexOf(stage1Phrases[1].ToUpper())) % 56);
            if (curCell.Length != 0) Debug.LogFormat("[Bamboozling Time Keeper #{0}]: \"{1}\" is a valid cell from the table.", curModId, curCell);
            if (curCell.Equals("STOP"))
            {
                finalValueB /= 17;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This divides the current value by 17", curModId);
            }
            else if (curCell.RegexMatch(@"^B\d$"))
            {
                int multiplier = int.Parse(curCell.Substring(1, 1)) + 1;
                finalValueB *= multiplier;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This multiplied the value by {1}", curModId, multiplier);
            }
            else if (curCell.RegexMatch(@"^\+\d+"))
            {
                int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                finalValueB += modifiedValue;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This added the current value by {1}", curModId, modifiedValue);
            }
            else if (curCell.RegexMatch(@"^\-\d+"))
            {
                int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                finalValueB -= modifiedValue;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This subtracted the current value by {1}", curModId, modifiedValue);
            }
            else if (curCell.RegexMatch(@"^\*\d+"))
            {
                int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                finalValueB *= modifiedValue;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This multiplied the current value by {1}", curModId, modifiedValue);
            }
            else if (curCell.RegexMatch(@"^\/\d+"))
            {
                int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                finalValueB /= modifiedValue;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This divided the current value by {1}", curModId, modifiedValue);
            }
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: End of Step 21", curModId);
            // Step 22
            curCell = GrabCellFromManualTable((colIndex.IndexOf(stage2ButtonColors[0].ToUpper()) + colIndex.IndexOf(stage2ButtonColors[2].ToUpper()))%8, Math.Abs(rowIndex.IndexOf(stage1Phrases[1].ToUpper()) - rowIndex.IndexOf(stage2Phrases[1].ToUpper())));
            if (curCell.Length != 0) Debug.LogFormat("[Bamboozling Time Keeper #{0}]: \"{1}\" is a valid cell from the table.", curModId, curCell);
            if (curCell.Equals("STOP"))
            {
                finalValueB /= 17;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This divides the current value by 17", curModId);
            }
            else if (curCell.RegexMatch(@"^B\d$"))
            {
                int multiplier = int.Parse(curCell.Substring(1, 1)) + 1;
                finalValueB *= multiplier;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This multiplied the value by {1}", curModId, multiplier);
            }
            else if (curCell.RegexMatch(@"^\+\d+"))
            {
                int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                finalValueB += modifiedValue;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This added the current value by {1}", curModId, modifiedValue);
            }
            else if (curCell.RegexMatch(@"^\-\d+"))
            {
                int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                finalValueB -= modifiedValue;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This subtracted the current value by {1}", curModId, modifiedValue);
            }
            else if (curCell.RegexMatch(@"^\*\d+"))
            {
                int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                finalValueB *= modifiedValue;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This multiplied the current value by {1}", curModId, modifiedValue);
            }
            else if (curCell.RegexMatch(@"^\/\d+"))
            {
                int modifiedValue = int.Parse(curCell.Substring(1, curCell.Length - 1));
                finalValueB /= modifiedValue;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This divided the current value by {1}", curModId, modifiedValue);
            }
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: End of Step 22", curModId);
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 20: False, skipping to Step 23", curModId);
        // Step 23
        if (stage2ButtonColors[0].Equals(stage2ButtonColors[2]) && stage2ButtonColors[2].Equals(stage2PhrClr[1]) && !stage2ButtonColors[1].Equals(stage2PhrClr[1]))
        {
            crtBtnIdxStg2 = 1;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 23: True", curModId);
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 23: False", curModId);
        // Step 24
        if (stageRuleApplied == 1)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 24: Step 9 was applied to Stage 1", curModId);
            finalValueB += 25;
        }
        else if (stageRuleApplied == 2)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 24: Step 9 was applied to Stage 2", curModId);
            finalValueB += 150;
        }
        else
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 24: Step 9 was not applied to either stages", curModId);
            finalValueB -= 250;
        }
        // Step 25
        finalValueB -= 10 * idModsonBomb.Where(a => KritsyModIDs.Contains(a)).Count();
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 25 applied.", curModId);
        // Step 26
        if (finalValueB < 0)
        {
            finalValueB *= -65;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 26: True", curModId);
            return;
        }
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 26: False", curModId);
        // Step 27
        if (stage2ButtonColors[0].Length + stage2ButtonColors[1].Length + stage2ButtonColors[2].Length > 12)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 27: True", curModId);
            int valuetoDivide = 0;
            foreach (string color in stage2PhrClr.Where(a => !a.Equals("White")))
            {
                valuetoDivide += color.Length;
            }
            if (valuetoDivide != 0)
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Value to divide is {1}", curModId, valuetoDivide);
                finalValueB /= valuetoDivide;
            }
            else
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Value to divide is 0, doubling instead.", curModId);
                finalValueB *= 2;
            }
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 27: False", curModId);
        // Step 28
        int tfc = info.GetTwoFactorCounts();
        if (tfc > 0)
        {
            finalValueB += tfc * 50;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 28: True", curModId);
            return;
        }
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 28: False", curModId);
        // Step 29
        if (stage2Phrases.Count == 7)
        {
            finalValueB /= 7;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 29: True", curModId);
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 29: False", curModId);
        // Step 30
        if (info.GetBatteryCount(1) == 0)// Grab the number of D batteries on the bomb, I hope.
        {
            if (stage2ButtonColors[0].Length > stage2ButtonColors[1].Length && stage2ButtonColors[0].Length > stage2ButtonColors[2].Length)
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 30: The left button has the most characters in its color name.", curModId);
                crtBtnIdxStg2 = 0;
            }
            else if (stage2ButtonColors[1].Length > stage2ButtonColors[2].Length && stage2ButtonColors[1].Length > stage2ButtonColors[0].Length)
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 30: The middle button has the most characters in its color name.", curModId);
                crtBtnIdxStg2 = 1;
            }
            else if (stage2ButtonColors[2].Length > stage2ButtonColors[1].Length && stage2ButtonColors[2].Length > stage2ButtonColors[0].Length)
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 30: The right button has the most characters in its color name.", curModId);
                crtBtnIdxStg2 = 2;
            }
            else
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 30: There is a tie for the most characters in each of the button's name of the color.", curModId);
            }
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 30: A D battery is present.", curModId);
        // Step 31
        finalValueB /= 3;
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 31 applied.", curModId);
        // Step 32
        foreach (string idc in info.GetIndicators())
        {
            bool toAdd = false;
            char[] idcletters = idc.ToCharArray();
            for (int x = 0; x < idcletters.Length; x++)
            {
                if ("SPEAKINGEVIL".Contains(idcletters[0]))
                {
                    toAdd = true;
                }
            }
            if (toAdd)
                finalValueB += 10;
        }
    }
    void CalcScaleFactors()
    {
        if (startValueA < 5000)
            scaleFactorA += 1;
        if (startValueB < 5000)
            scaleFactorB += 1;

        scaleFactorA -= batteryCount / 2;
        scaleFactorB -= batteryholdCount;

        if (idModsonBomb.Contains("forgetThis"))
            scaleFactorA += 1;
        if (idModsonBomb.Contains("forgetEnigma"))
            scaleFactorB += 1;

        if (info.GetSerialNumberNumbers().Count() >= 4)// In a world where symbols can show up in the serial number.
            scaleFactorA += 3;
        if (info.GetSerialNumberLetters().Count() >= 4)// In a world where symbols can show up in the serial number.
            scaleFactorB += 3;

        if (idModsonBomb.Contains("timeKeeper"))
            scaleFactorA += 2;
        if (idModsonBomb.Contains("TurnTheKey"))
            scaleFactorB += 2;

        if (!definablePhrases.Contains(stage1Phrases[0]))
            scaleFactorA += 2;
        if (definablePhrases.Contains(stage2Phrases[0]))
            scaleFactorB += 2;

        if (startTime >= 1800)
        {
            scaleFactorA += 2;
            scaleFactorB += 2;
        }
        
        if (info.GetSerialNumberNumbers().Count() == 3)// In a world where symbols can show up in the serial number.
            scaleFactorA += 2;
        if (info.GetSerialNumberLetters().Count() == 3)// In a world where symbols can show up in the serial number.
            scaleFactorB += 2;

        if (idModsonBomb.Contains("MemoryV2"))
            scaleFactorA += 1;
        if (!idModsonBomb.Contains("forgetItNot"))
            scaleFactorB += 1;

        if (finalValueA >= 10000)
            scaleFactorA /= 2;
        if (finalValueB >= 10000)
            scaleFactorB /= 2;

        if (info.IsIndicatorOn(Indicator.BOB) && info.IsIndicatorOn(Indicator.FRK) && batteryCount == 4 && batteryholdCount == 2)
        { 
            scaleFactorA = 2;
            scaleFactorB = 2; 
        }

        scaleFactorA = Math.Max(Math.Min(scaleFactorA, 5), 2);
        scaleFactorB = Math.Max(Math.Min(scaleFactorB, 5), 2);
    }
    void DetermineCorrectButton(int stageNum)
    {
        if (stageNum == 1)
        {
            int presentInStage = 0;
            int idxCon6 = -1;
            for (int x = 0; x < stage1ButtonDigits.Length; x++)
            {
                if (stage1Phrases.Contains(phraseNumList[phraseNumList.IndexOf(stage1ButtonDigits[x].ToString())]) || stage1Phrases.Contains(phraseNumList[phraseNumList.IndexOf(stage1ButtonDigits[x].ToString()) + 1]))
                    presentInStage++;
                else
                    idxCon6 = x;
            }

            if (crtBtnIdxStg1 != null) return;
            if (finalValueA >= 0 && finalValueA <= 9999) crtBtnIdxStg1 = 0;
            else if (info.IsPortPresent(Port.Parallel) && stage2ButtonColors.Where(a => a.Equals("Magenta")).Count() == 1)
                crtBtnIdxStg1 = stage1ButtonColors.IndexOf("Magenta");
            else if (info.IsPortPresent(Port.StereoRCA) && stage1ButtonColors.Where(a => a.Equals("Red")).Count() == 1 && stage1ButtonColors.Where(a => a.Equals("White")).Count() == 1)
            {
                for (int x = 0; x < stage1ButtonColors.Count; x++)
                    if (!stage1ButtonColors[x].Equals("Red") && !stage1ButtonColors[x].Equals("White"))
                    {
                        crtBtnIdxStg1 = x;
                        return;
                    }
            }
            else if (!stage1PhrClr.Contains("Black") && stage1ButtonColors.Where(a => a.Equals("Black")).Count() == 1) crtBtnIdxStg1 = stage1ButtonColors.IndexOf("Black");
            else if (stage1ButtonColors.Where(a => !a.Equals("White")).ToList().Intersect(stage1PhrClr.Where(a => !a.Equals("White")).ToList()).Count() == 0) crtBtnIdxStg1 = 2;
            else if (stage1PhrClr.Where(a => a.Equals("White")).Count() == stage1PhrClr.Count() && presentInStage == 2) crtBtnIdxStg1 = idxCon6;
            else if (stage1ButtonColors.Where(a => a.Equals("Blue")).Count() == 1 && stage1ButtonColors.Where(a => a.Equals("Green")).Count() == 1 && stage1ButtonColors.Where(a => a.Equals("Red")).Count() == 1) crtBtnIdxStg1 = stage1ButtonColors.IndexOf("Blue");
            else if (!stage1ButtonColors.Contains("Cyan") && !stage1ButtonColors.Contains("Magenta")) crtBtnIdxStg2 = 1;
            else crtBtnIdxStg2 = (int?)(finalValueB % 3);
        }
        else if (stageNum == 2)
        {
            int presentInStage = 0;
            int idxCon6 = -1;
            for (int x = 0; x < stage2ButtonDigits.Length; x++)
            {
                if (stage2Phrases.Contains(phraseNumList[phraseNumList.IndexOf(stage2ButtonDigits[x].ToString())]) || stage2Phrases.Contains(phraseNumList[phraseNumList.IndexOf(stage2ButtonDigits[x].ToString()) + 1]))
                    presentInStage++;
                else
                    idxCon6 = x;
            }
            if (crtBtnIdxStg2 != null) return;
            if (finalValueB >= 0 && finalValueB <= 9999) crtBtnIdxStg2 = 0;
            else if (info.IsPortPresent(Port.Parallel) && stage2ButtonColors.Where(a => a.Equals("Magenta")).Count() == 1)
                crtBtnIdxStg2 = stage2ButtonColors.IndexOf("Magenta");
            else if (info.IsPortPresent(Port.StereoRCA) && stage2ButtonColors.Where(a => a.Equals("Red")).Count() == 1 && stage2ButtonColors.Where(a => a.Equals("White")).Count() == 1)
            {
                for (int x = 0; x < stage2ButtonColors.Count; x++)
                    if (!stage2ButtonColors[x].Equals("Red") && !stage2ButtonColors[x].Equals("White"))
                    {
                        crtBtnIdxStg2 = x;
                        return;
                    }
            }
            else if (!stage2PhrClr.Contains("Black") && stage2ButtonColors.Where(a => a.Equals("Black")).Count() == 1) crtBtnIdxStg2 = stage2ButtonColors.IndexOf("Black");
            else if (stage2ButtonColors.Where(a => !a.Equals("White")).ToList().Intersect(stage2PhrClr.Where(a => !a.Equals("White")).ToList()).Count() == 0) crtBtnIdxStg2 = 2;
            else if (stage2PhrClr.Where(a => a.Equals("White")).Count() == stage2PhrClr.Count() && presentInStage == 2) crtBtnIdxStg1 = idxCon6;
            else if (stage2ButtonColors.Where(a => a.Equals("Blue")).Count() == 1 && stage2ButtonColors.Where(a => a.Equals("Green")).Count() == 1 && stage2ButtonColors.Where(a => a.Equals("Red")).Count() == 1) crtBtnIdxStg2 = stage2ButtonColors.IndexOf("Blue");
            else if (!stage2ButtonColors.Contains("Cyan") && !stage2ButtonColors.Contains("Magenta")) crtBtnIdxStg2 = 1;
            else crtBtnIdxStg2 = 2 - (int)(finalValueB % 3);
        }
    }
    void CalculateAllPossibleTimes(List<long> destination, long finalValue, int scaleFactor)
    {
        if (scaleFactor <= 0 || scaleFactor == 1)
        {
            throw new FormatException("Attempted to get all possible times with an invalid scale factor! This is not allowed.");
        }
        destination.Add(finalValue);
        long currentValue = finalValue;
        while (currentValue * scaleFactor > 0)
        {
            currentValue *= scaleFactor;
            destination.Add(currentValue);
        }
        currentValue = finalValue;
        while (currentValue / scaleFactor > 0)
        {
            currentValue /= scaleFactor;
            destination.Add(currentValue);
        }
        destination.Sort();
    }

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
        StartCoroutine(UpdateDisplay(currentStage));
        yield return null;
    }

    IEnumerator UpdateDisplay(int stage)
    {
        currentPart = -1;
        while (!interactable)
        {
            yield return new WaitForSeconds(0);
        }
        while (currentStage == stage && curbtnHeld == -1 && interactable)
        {
            currentPart++;
            if (stage == 1)
            {
                if (currentPart < stage1Phrases.Count() && currentPart >= 0)
                {
                    display.text = stage1Phrases[currentPart];
                    display.color = colorList[colorString.IndexOf(stage1PhrClr[currentPart])];
                    if (colorBlindActive && !startingPhrases.Contains(display.text) && !display.text.Equals("HUNDRED") && !display.text.Equals("POINT ZERO"))
                    {
                        yield return new WaitForSeconds(0.75f);
                        if (currentStage != stage || curbtnHeld != -1 || !interactable) break;
                        display.text = "IN " + stage1PhrClr[currentPart].ToUpper();
                        display.color = Color.white;
                    }
                }
                else
                {
                    display.text = "";
                    display.color = Color.white;
                    currentPart = -1;
                }
            }
            else if (stage == 2)
            {
                if (currentPart < stage2Phrases.Count() && currentPart >= 0)
                {
                    display.text = stage2Phrases[currentPart];
                    display.color = colorList[colorString.IndexOf(stage2PhrClr[currentPart])];
                    if (colorBlindActive && !startingPhrases.Contains(display.text) && !display.text.Equals("HUNDRED") && !display.text.Equals("POINT ZERO"))
                    {
                        yield return new WaitForSeconds(0.75f);
                        if (currentStage != stage || curbtnHeld != -1 || !interactable) break;
                        display.text = "IN " + stage2PhrClr[currentPart].ToUpper();
                        display.color = Color.white;
                    }
                }
                else
                {
                    display.text = "";
                    display.color = Color.white;
                    currentPart = -1;
                }
            }
            if (display.color.Equals(Color.black))
            {
                backing.GetComponent<MeshRenderer>().material.color = Color.white;
            }
            else
            {
                backing.GetComponent<MeshRenderer>().material.color = Color.black;
            }
            yield return new WaitForSeconds(0.75f);
        }
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
        int cnt = 0;
        while (playingAnim)
        {
            display.text = "SWITCHING";
            display.color = Color.white;
            for (int x = 0; x < cnt; x++)
                display.text += ".";
            cnt = (cnt + 1) % 4;
            yield return new WaitForSeconds(0.1f);
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
