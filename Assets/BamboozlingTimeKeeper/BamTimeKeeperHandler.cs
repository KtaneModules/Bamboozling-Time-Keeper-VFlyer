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
    public KMAudio sound;
    public KMAudio.KMAudioRef soundwithRef;
    public GameObject buttonL, buttonM, buttonR, buttonGroup, highlightStage1, highlightStage2, door, backing;
    public GameObject animationPointDoorA, animationPointDoorB, animationPointButtonA, animationPointButtonB;
    public TextMesh display;
    public KMSelectable[] buttonsSelectable = new KMSelectable[3], stageSelectables = new KMSelectable[2];
    public Material[] materialList = new Material[2];
    public KMModSettings ModSettings;

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
    private List<string> colorString = new List<string>() {
        "Red",
        "Yellow",
        "Green",
        "Cyan",
        "Blue",
        "Magenta",
        "White",
        "Black"
    }, startingPhrases = new List<string>() {
        "SOME NUMBERS", "THE NUMBERS", "NUMBERS", "TWO NUMBERS", "THREE NUMBERS", "FOUR NUMBERS",
        "SOME NUMBER(S)", "THE NUMBER(S)", "NUMBER(S)", "2 NUMBERS", "3 NUMBERS", "4 NUMBERS",
        "SOME NUMBER", "THE NUMBER", "NUMBER", "ONE NUMBER", "A NUMBER", "1 NUMBER"
    }, rowIndex = new List<string>() {
        "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
        "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
        "20", "30", "40", "50", "60", "70", "80", "90",
        "ZERO", "ONE",  "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE",
        "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN",
        "TWENTY", "THIRTY", "FOURTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
    }, colIndex = new List<string>() {
        "RED",
        "YELLOW",
        "GREEN",
        "CYAN",
        "BLUE",
        "MAGENTA",
        "WHITE",
        "BLACK"
    }, KritsyModIDs = new List<string>() { // An ID list of all of Kritsy's modules uploaded so far
        "KritBlackjack",
        "KritCMDPrompt",
        "KritConnectionDev",
        "KritFlipTheCoin",
        "Krit4CardMonte", // Module now has a different maintainer however still counts as Kritsy's module.
        "KritHoldUps",
        "KritLockpickMaze", // Test Build ATM
        "KritMicroModules",
        "KritHomework",
        "KritRadio",
        "KritScripts"
    }, SpeakingModIDs = new List<string>() { // An ID list of all of Speakingevil's modules uploaded so far
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
        "silhouettes",
        "tallorderedKeys",
        "ultimateCycle",
        "UltraStores",
        "unorderedKeys",
        "veryAnnoyingButton"
    }, ThreeStageModIDs = new List<string>() { // An ID List of 3 Stage Modules on the bomb.
        "3dTunnels",
        "algebra",
        "alphabeticalRuling",
        "binaryGrid",
        "binaryTree",
        "BookOfMarioModule",
        "challengeAndContact",
        "Color Decoding",
        "CruelKeypads",
        "divisibleNumbers",
        "EdgeworkModule",
        "EncryptedDice",
        "EnglishTest",
        "DateFinder",
        "elderFuthark",
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
        "revPolNot",
        "screw",
        "SeaShells",
        "simonSamples",
        "SimonScreamsModule",
        "simonSelectsModule",
        "SimonShrieksModule",
        "SimonSingsModule",
        "simonsOnFirst",
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
    }, RTControlModIDs = new List<string>() {
        "brushStrokes",
        "burglarAlarm",
        "burgerAlarm",
        "coffeebucks", // Grey spot, can't strike after leaving it for too long though.
        "countdown",
        "cruelCountdown",
        "crystalMaze",
        "etterna",  // Thanks Rhythms
        "fastMath",
        "GoingBackwardsModule",
        "jackAttack",
        "KritLockpickMaze", // May be changed in the final build
        "manometers",
        "KritHomework",
        "necronomicon",
        "NotMaze",      // 10 second delay before the module resets
        "numberNimbleness",
        "PointOfOrderModule", // 6 second delay before the module strikes
        "quizBuzz",
        "shellGame",
        "simonStops",   // All other Simon modules are in a gray spot due to the fact that these either wait or don't wait to clear the inputs. Simon Stops strikes on waiting for a controlled input for too long.
        "snowflakes",
        "sonicKnuckles",
        "stopwatch",
        "valves",   // Brush Strokes is considered a RT Controlled Module, why not Valves? - Asew
        "wire",
        "ZooModule"
    }, RTSensitiveModIDs = new List<string>() {
        "blinkstopModule", // Can't strike by leaving it for too long, new sequence is given at the end of the previous set of flashes, starts flashing the sequence as soon as the bomb starts.
        "kataCheatCheckout", // Source Code suggests RT sensitivity mainly for trying to check on each item quickly before the timer runs out randomly. Can only be delayed by correct interactions on the module or solving it.
        "lgndHyperactiveNumbers", // Can't strike by leaving it for too long, resets every now and then.
        "lunchtime", // Can detonate a bomb by leaving it for too long, otherwise strikable by incorrect selection and/or timing
        "numberCipher", // Can't strike by leaving it for too long, resets every now and then.
        "RAM", // Or Random Access Memory; Can strike by leaving it sit for too long.
        "theSwan", // Strikes by leaving it sit for too long or by incorrect set of presses.
        "taxReturns", // Strikes by leaving it sit for too long or by incorrect value.
        "veryAnnoyingButton", // Or The Very Annoying Button; Strikes by leaving it sit for too long or by an incorrect press.
        "qkUCN" // Or Ultimate Custom Night; Strikes by leaving it sit for too long in an incorrect setup, or by solves.
    }, stage1ButtonColors = new List<string>(), stage2ButtonColors = new List<string>();
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

    private long startValueA, startValueB, finalValueA, finalValueB, bTimeOnHold, bTimeEndHold;
    private int scaleFactorA = 0, scaleFactorB = 0;
    private int? crtBtnIdxStg1, crtBtnIdxStg2;
    private List<long> possibleTimesA = new List<long>(), possibleTimesB = new List<long>();

    private int TodaysDay = DateTime.Today.Day;
    private int TodaysMonth = DateTime.Today.Month;
    private bool oneTapHolds = false, playingAnim = false, interactable = false, colorBlindActive = false, started = false, specialDay, holdCorStg1, holdCorStg2, isHeld = false, isLeftFlashingConsistent, isRightFlashingConsistent,zenModeDetected = false,forcedSolve=false;

    private List<string> leftHoldColors = new List<string>(), rightHoldColors = new List<string>();
    private string inconsistMorseLetterL, inconsistMorseLetterR;

    private int curbtnHeld = -1, currentStage = 0, timeHeldSec = 0, leftmostIdxFlashable, rightmostIdxFlashable;
    private static int modID = 1;
    public int curModId;
    private bool[] stagesCompleted = new bool[] { false, false };

    private string curSerNo;
    private List<string> idModsonBomb, nameModsonBomb;
    private int batteryCount, batteryholdCount, startTime;

    private readonly string[] specialDayStartPhrases = {
        "Iku zo.",
        "Showtime.",
        "Are we starting\nsoon?",
        "Let's do this.",
        "Yaru no ka."
    }, specialDayStrikePhrases = {
        "Taihen da.",
        "Oh boy.",
        "Wait, what?",
        "Oof.",
        "Huh, too bad.",
        "Aw..."
    }, awakePhrases = {
        "YOU WON'T WIN",
        "YOU WILL SUFFER",
        "NOTHING SPARED",
        "YOU'RE NOT SAFE",
        "NOTHING'S SAFE",
        "NO CHANCES LEFT",
        "DON'T LEAVE\nTHIS BEHIND",
        "DO YOU FACE IT?"
    }, strikePhrases = {
        "THAT'S A STRIKE",
        "THAT IS A STRIKE",
        "FAILED AGAIN",
        "NOT YET",
        "TRY AGAIN",
        "THAT'S NOT RIGHT",
        "THAT'S WRONG",
        "THAT IS WRONG",
        "NO U",
        "GET BAMBOOZLED",
        "STILL WRONG"
    }, actionPhrases = {
        "HOLD",
        "TAP"
    }, disarmPhrases = {
        "EXCELLENT", "AWESOME", "AMAZING", "STUPENDOUS", "REMARKABLE", "MAGNIFICENT", "IMPRESSIVE", "PHENOMENAL", "ASTOUNDING", "EXTRAORDINARY", "WHAT A PRO", "YOU DEFEATED", "GODLIKE"
    }, disarmSpecialPhrases = {
        "So I won huh?","Katta no ka.","Katteta.", "Oh, hey I won?", "I won?"
    }, letters = {
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
    }, specialSoundStrikes = { "SigLight", "SigHeavy", "SigLose" }, specialSoundCorrect = { "Sig20Plus", "Sig20PlusALT" };


    private BamTimeKeeperSettings Settings = new BamTimeKeeperSettings();

    private static int countSoundsPlayed = 0;
    // Use this for initialization
    void Awake()
    {
        curModId = modID++;
        try
        {
            ModConfig<BamTimeKeeperSettings> modconfig = new ModConfig<BamTimeKeeperSettings>("BamTimeKeeperSettings");
            Settings = modconfig.Settings;


            ModSettings.RefreshSettings();
            
            oneTapHolds = Settings.OneTapHolds;


        }
        catch
        {
            Debug.LogWarningFormat("[Bamboozling Time Keeper #{0}]: WARNING! Config does not exist for Bamboozling Time Keeper, using default settings.", curModId);
            oneTapHolds = false;
        }
        finally
        {
            try
            {
                colorBlindActive = colorblindMode.ColorblindModeActive;
            }
            catch
            {
                colorBlindActive = false;
            }
        }
    }
    void Start() {
        specialDay = isSpecialDay();
        IEnumerator currentlyRunning = null;
        if (specialDay)
        {
            long[] listValues = new long[] { 2424, 4949 };
            int idxSwap = UnityEngine.Random.Range(0, 2);
            long tempvalue = listValues[idxSwap];
            listValues[idxSwap] = listValues[0];
            listValues[0] = tempvalue;

            startValueA = listValues[0];
            startValueB = listValues[1];
            stage1ButtonColors.Add("Cyan");
            stage2ButtonColors.Add("Cyan");
            currentlyRunning = TypeText(specialDayStartPhrases[UnityEngine.Random.Range(0, specialDayStartPhrases.Length)].ToUpper(), false);
        }
        else
        {
            startValueA = UnityEngine.Random.Range(0, 10000);
            startValueB = UnityEngine.Random.Range(0, 10000);
            currentlyRunning = TypeText(awakePhrases[UnityEngine.Random.Range(0, awakePhrases.Length)].ToUpper(), false);
        }
        modSelf.OnActivate += delegate
        {
            oneTapHolds = TwitchPlaysActive || oneTapHolds;
            zenModeDetected = ZenModeActive;
            StopCoroutine(currentlyRunning);
            display.text = "BEGIN";
            if (countSoundsPlayed < 1)
            {
                if (specialDay)
                {
                    sound.PlaySoundAtTransform("SigFeverEnter", transform);
                }
                else
                {
                    sound.PlaySoundAtTransform("KefkaLaugh", transform);
                }
                countSoundsPlayed++;
            }
            GenerateRandomPhrases();
            GenerateRandomButtons();

            currentStage = 1;
            UpdateButtons(currentStage);
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
                finalValueA += 16;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The final value for stage 1 is {1}", curModId, finalValueA);


            CalculateStage2();
            while (finalValueB < 10)
                finalValueB += 16;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The final value for stage 2 is {1}", curModId, finalValueB);
            CalcScaleFactors();

            CalculateAllPossibleTimes(possibleTimesA, finalValueA, scaleFactorA);
            CalculateAllPossibleTimes(possibleTimesB, finalValueB, scaleFactorB);

            DetermineCorrectButton();

            DetermineStateConditions();

            //End Redirect to Calculation Methods
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 1 FINAL VALUE: {1}", curModId, finalValueA);
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 1 SCALE FACTOR: {1}", curModId, scaleFactorA);
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 1 POSSIBLE TIMES: {1}", curModId, FormatDebugList(possibleTimesA.Where(a => a < int.MaxValue).ToList()));
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 1 CORRECT BUTTON: {1}", curModId, buttonPos[(int)crtBtnIdxStg1]);
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 1 CORRECT ACTION: {1}", curModId, holdCorStg1 ? actionPhrases[0] : actionPhrases[1]);

            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 2 FINAL VALUE: {1}", curModId, finalValueB);
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 2 SCALE FACTOR: {1}", curModId, scaleFactorB);
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 2 POSSIBLE TIMES: {1}", curModId, FormatDebugList(possibleTimesB.Where(a => a < int.MaxValue).ToList()));
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 2 CORRECT BUTTON: {1}", curModId, buttonPos[(int)crtBtnIdxStg2]);
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 2 CORRECT ACTION: {1}", curModId, holdCorStg2 ? actionPhrases[0] : actionPhrases[1]);
            started = true;
            StartCoroutine(HandleSounds());
        };
        for (int x = 0; x < stageSelectables.Length; x++)
        {
            int y = x;
            stageSelectables[x].OnInteract += delegate {
                sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
                if (!playingAnim && interactable && started && curbtnHeld == -1)
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
                            bTimeEndHold = Mathf.FloorToInt(info.GetTime());
                            if (soundwithRef != null)
                                soundwithRef.StopSound();
                            StopCoroutine(HandleExtendedHold());
                            HandleRelease(y);
                            curbtnHeld = -1;
                            sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonRelease, transform);
                        }
                        else if (curbtnHeld == -1)
                        {
                            bTimeOnHold = Mathf.FloorToInt(info.GetTime());
                            sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonPress, transform);
                            curbtnHeld = y;
                            StartCoroutine(HandleExtendedHold());
                        }
                    }
                    else
                    {
                        bTimeOnHold = Mathf.FloorToInt(info.GetTime());
                        sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonPress, transform);
                        curbtnHeld = y;
                        StartCoroutine(HandleExtendedHold());
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
                            bTimeEndHold = Mathf.FloorToInt(info.GetTime());
                            if (soundwithRef != null)
                                soundwithRef.StopSound();
                            HandleRelease(y);
                            curbtnHeld = -1;
                            sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonRelease, transform);
                            StopCoroutine(HandleExtendedHold());
                        }
                    }
                }
            };
            info.OnBombExploded += delegate
            {
                if (soundwithRef != null)
                    soundwithRef.StopSound();
            };
        }
        StartCoroutine(currentlyRunning);
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
        while (stage1ButtonColors.Count < 3)
        {
            stage1ButtonColors.Add(colorString[UnityEngine.Random.Range(0, colorString.Count)]);
        }
        while (stage2ButtonColors.Count < 3)
        {
            stage2ButtonColors.Add(colorString[UnityEngine.Random.Range(0, colorString.Count)]);
        }
        // Scramble Button Colors
        stage1ButtonColors.Shuffle();
        stage2ButtonColors.Shuffle();
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
        return (TodaysDay == 4 && TodaysMonth == 2) || (TodaysDay == 9 && TodaysMonth == 4) || (TodaysDay == 16 && TodaysMonth == 6);
    } // Return true if the module showed up on the 4th of Feb, 9th of April, or 16th of June.
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
        for (int x = 0; x < listString.Count; x++)
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
    long PerformMultipleOperations(long curValue, string[] operation, bool isStage2)
    {// Perform the said operations from ther operation list.
        for (int x = 0; x < operation.Length; x++)
        {
            if (operation[x].Length != 0) Debug.LogFormat("[Bamboozling Time Keeper #{0}]: \"{1}\" is a valid cell from the table.", curModId, operation[x]);
            if (operation[x].Equals("STOP"))
            {
                if (!isStage2)
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This ends step 17 for stage 1.", curModId);
                    return curValue;
                }
                else
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This divides the current value by 17", curModId);
                    curValue /= 17;
                }
            }
            else if (operation[x].RegexMatch(@"^B\d$"))
            {
                if (!isStage2)
                {
                    crtBtnIdxStg1 = int.Parse(operation[x].Substring(1, 1)) - 1;
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This changed the correct button to be on the {1}", curModId, buttonPos[(int)crtBtnIdxStg1]);
                }
                else
                {
                    int multiplier = int.Parse(operation[x].Substring(1, 1)) + 1;
                    curValue *= multiplier;
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This multiplied the value by {1}", curModId, multiplier);
                }
            }
            else if (operation[x].RegexMatch(@"^\+\d+"))
            {
                int modifiedValue = int.Parse(operation[x].Substring(1));
                curValue += modifiedValue;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This added the current value by {1}", curModId, modifiedValue);
            }
            else if (operation[x].RegexMatch(@"^\-\d+"))
            {
                int modifiedValue = int.Parse(operation[x].Substring(1));
                curValue -= modifiedValue;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This subtracted the current value by {1}", curModId, modifiedValue);
            }
            else if (operation[x].RegexMatch(@"^\*\d+"))
            {
                int modifiedValue = int.Parse(operation[x].Substring(1));
                curValue *= modifiedValue;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This multiplied the current value by {1}", curModId, modifiedValue);
            }
            else if (operation[x].RegexMatch(@"^\/\d+"))
            {
                int modifiedValue = int.Parse(operation[x].Substring(1));
                curValue /= modifiedValue;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: This divided the current value by {1}", curModId, modifiedValue);
            }
        }
        return curValue;
    }
    readonly string[] buttonPos = new string[] { "LEFT", "MIDDLE", "RIGHT" },
        vanillaInds = new string[] { "SND", "CLR", "CAR", "IND", "FRQ", "SIG", "NSA", "MSA", "TRN", "BOB", "FRK" },
        vanillaPorts = new string[] { "DVI", "Parallel", "PS/2", "RJ45", "Serial", "StereoRCA" },
        definablePhrases = new string[] { "ONE NUMBER", "TWO NUMBERS", "THREE NUMBERS", "FOUR NUMBERS", "1 NUMBER", "2 NUMBERS", "3 NUMBERS", "4 NUMBERS", "A NUMBER" };
    int stageRuleApplied = 0, positiveDifference = 0, sumFirst3 = 0, vanillaIndOnCnt = 0, vanillaIndOffCnt = 0, rtControlCount = 0;
    void CalculateStage1()
    {
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 1 CALCULATIONS:", curModId);
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The phrases for stage 1 are {1}", curModId, FormatDebugList(stage1Phrases));
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The color of the phrases for stage 1 are {1}", curModId, FormatDebugList(stage1PhrClr));
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The button colors for stage 1 are {1}", curModId, FormatDebugList(stage1ButtonColors));
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The digits on the buttons for stage 1 are {1}", curModId, FormatDebugList(stage1ButtonDigits.ToList()));
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The starting value for stage 1 is {1}", curModId, startValueA);
        finalValueA = startValueA * 1;
        if (canOverride())
        {
            finalValueA = 15;
            scaleFactorA = 2;
            crtBtnIdxStg1 = stage1ButtonColors.IndexOf("Cyan");
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]:" + " Override present! Final Value for this stage is actually 15 seconds, with a scale factor of 2, with the correct button being the {1} button!".ToUpper(), curModId, buttonPos[(int)crtBtnIdxStg1]);
            return;
        }
        // Step 2
        string firstSerNos = curSerNo.Substring(0, curSerNo.Length / 2);
        foreach (char srlchr in firstSerNos.ToCharArray())
        {
            int charValue = GetValueofBase36Digit(srlchr);
            finalValueA -= charValue;
            sumFirst3 += charValue;
        }
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 2 applied. The sum of the first 3 base-36 digits in the serial number are {1}", curModId, sumFirst3);
        // Step 3
        finalValueA += 4 * (info.GetPortCount() + info.GetPortPlateCount());
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 3 applied.", curModId);
        // Step 4
        int speakingCount = 0;
        int forgetCount = 0;
        // Debugging only
        //print(FormatDebugList(idModsonBomb.Where(a => SpeakingModIDs.Contains(a)).ToList()));
        //print(FormatDebugList(nameModsonBomb.Where(a => a.Contains("Forget")).ToList()));
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
        forgetCount = Math.Max(1, forgetCount);
        positiveDifference = Mathf.Abs(forgetCount - speakingCount);
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
        rtControlCount = idModsonBomb.Where(a => RTControlModIDs.Contains(a)).Count();
        finalValueA += rtControlCount * batteryCount;
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 8 applied. Detected this many RT controlled modules: {1}", curModId,rtControlCount);
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
        if (finalValueA % 1176 <= 10 || finalValueA % 1176 >= 1166) // 1176, A number divisible by 24 and 49
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 10: True", curModId);
            return;
        }
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 10: False", curModId);
        // Step 11
        finalValueA += TodaysDay;
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 11 applied.", curModId);
        // Step 12
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
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 13: Detected this many 3 Stage Modules on the bomb: {1}", curModId,ThreeStgModsCnt);
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
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: At this point, the current value for this stage is {1}", curModId,finalValueA);
        // Step 17
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Begin Step 17", curModId);
        List<string> requiredOperations = new List<string>();
        for (int x = 0; x < stage1Phrases.Count; x++)
        {
            string curCell = GrabCellFromManualTable(colIndex.IndexOf(stage1PhrClr[x].ToUpper()), rowIndex.IndexOf(stage1Phrases[x].ToUpper()));
            requiredOperations.Add(curCell);
        }
        finalValueA = PerformMultipleOperations(finalValueA, requiredOperations.ToArray(), false); // Redirect to the method above. Required for compacting.
        // Step 18
        if (definablePhrases.Contains(stage1Phrases[0]))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 18: True", curModId);
            foreach (string unlitInd in info.GetOffIndicators())
                finalValueA += GetValueofBase36Digit(unlitInd.ToCharArray()[0]) - 9;
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
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 21: There are this many needies on the bomb: {1}", curModId, needyCount);
        // Step 22
        if (stage1Phrases[0].Contains("(") || stage1Phrases[0].RegexMatch(@"^\d\sNUMBERS?"))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 22: True, processing like Step 17", curModId);
            List<string> buttonRequiredOperations = new List<string>();
            for (int x = 0; x < stage1ButtonColors.Count; x++)
            {
                string curCell = GrabCellFromManualTable(colIndex.IndexOf(stage1ButtonColors[x].ToUpper()), rowIndex.IndexOf(stage1ButtonDigits[x].ToString()));
                buttonRequiredOperations.Add(curCell);
            }
            finalValueA = PerformMultipleOperations(finalValueA, buttonRequiredOperations.ToArray(), false); // Redirect to the method above. Required for compacting.
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 22: False", curModId);
        if (finalValueA < 0)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 23: True", curModId);
            finalValueA *= -6;
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 23: False", curModId);
        int sumLast3SerDigits = 0;
        string last3SerDigits = curSerNo.Substring(curSerNo.Length / 2);
        foreach (char serchar in last3SerDigits.ToCharArray())
        {
            finalValueA += GetValueofBase36Digit(serchar);
            sumLast3SerDigits += GetValueofBase36Digit(serchar);
        }
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 24 applied. The sum of the last 3 base-36 digits in the serial number is {1}", curModId,sumLast3SerDigits);
    }
    void CalculateStage2()
    {
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STAGE 2 CALCULATIONS:", curModId);
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The phrases for stage 2 are {1}", curModId, FormatDebugList(stage2Phrases));
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The color of the phrases for stage 2 are {1}", curModId, FormatDebugList(stage2PhrClr));
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The button colors for stage 2 are {1}", curModId, FormatDebugList(stage2ButtonColors));
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The digits on the buttons for stage 2 are {1}", curModId, FormatDebugList(stage2ButtonDigits.ToList()));
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The starting value for stage 2 is {1}", curModId, startValueB);
        finalValueB = startValueB * 1;
        if (canOverride())
        {
            finalValueB = 15;
            scaleFactorB = 2;
            crtBtnIdxStg2 = stage2ButtonColors.IndexOf("Cyan");
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]:" + " Override present! Final Value for this stage is actually 15 seconds, with a scale factor of 2, with the correct button being the {1} button!".ToUpper(), curModId, buttonPos[(int)crtBtnIdxStg2]);
            return;
        }
        // Step 2
        finalValueB -= sumFirst3; // Remove repeative calculations.
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 2 applied. The sum of the first 3 base-36 digits in the serial number is {1}", curModId, sumFirst3);
        // Step 3
        finalValueB += 4 * (info.GetPortCount() + info.GetPortPlateCount());
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 3 applied.", curModId);
        // Step 4
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 4: The positive difference between the number of modules on the bomb with the word \"Forget\" in its name and the number of modules made by SpeakingEvil on the bomb is {1}", curModId, positiveDifference);
        finalValueB *= positiveDifference; // Remove repeative calculations.
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
        finalValueB += rtControlCount * batteryCount;
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 8 applied. Detected this many RT controlled modules: {1}", curModId, rtControlCount);
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
        if (finalValueB % 1176 <= 10 || finalValueB % 1176 >= 1166) // 1176, A number divisible by 24 and 49
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 10: True", curModId);
            return;
        }
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 10: False", curModId);
        // Step 11
        finalValueB += TodaysDay;
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 11 applied.", curModId);
        // Step 12
        finalValueB += 20 * (vanillaIndOnCnt - vanillaIndOffCnt);
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 12 applied.", curModId);
        // Step 13
        int ThreeStgModsCnt = Math.Max(idModsonBomb.Where(a => ThreeStageModIDs.Contains(a)).Count(), 1);
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 13: Detected this many 3 Stage Modules on the bomb: {1}", curModId, ThreeStgModsCnt);
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
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: At this point, the current value for stage 2 is {1}", curModId, finalValueB);
        // Step 18
        if (definablePhrases.Contains(stage2Phrases[0]))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 18: True", curModId);
            string curCell = GrabCellFromManualTable(colIndex.IndexOf(stage2PhrClr[1].ToUpper()), rowIndex.IndexOf(stage2Phrases[1].ToUpper()));
            finalValueB = PerformMultipleOperations(finalValueB, new string[] { curCell }, true);
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 18: False", curModId);
        // Step 19
        if (startingPhrases.IndexOf(stage2Phrases[0]) >= 12)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 19: True, performing like Step 18", curModId);
            if (!stage2Phrases[stage2Phrases.Count - 2].Equals("HUNDRED"))
            {
                string curCell = GrabCellFromManualTable(colIndex.IndexOf(stage2PhrClr[stage2PhrClr.Count - 2].ToUpper()), rowIndex.IndexOf(stage2Phrases[stage2Phrases.Count - 2].ToUpper()));
                finalValueB = PerformMultipleOperations(finalValueB, new string[] { curCell }, true);
            }
            else
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The 2nd to last phrase is actually \"HUNDRED\"", curModId);
                finalValueB *= 100;
            }
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 19: False", curModId);
        // Step 20,21,22
        if (stage2Phrases[0].Contains("(") || stage2Phrases[0].RegexMatch(@"^\d NUMBERS?$"))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 20: True", curModId);
            // Step 21
            List<string> curStg2Operators = new List<string>();
            curStg2Operators.Add(GrabCellFromManualTable(7 - colIndex.IndexOf(stage2ButtonColors[1].ToUpper()), (rowIndex.IndexOf(stage2Phrases[1].ToUpper()) + rowIndex.IndexOf(stage1Phrases[1].ToUpper())) % 56));
            // 2nd Cell from step 21
            curStg2Operators.Add(GrabCellFromManualTable((colIndex.IndexOf(stage2ButtonColors[0].ToUpper()) + colIndex.IndexOf(stage2ButtonColors[2].ToUpper())) % 8, Math.Abs(rowIndex.IndexOf(stage1Phrases[1].ToUpper()) - rowIndex.IndexOf(stage2Phrases[1].ToUpper()))));
            finalValueB = PerformMultipleOperations(finalValueB, curStg2Operators.ToArray(),true);

            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: End of Step 21", curModId);
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 20: False, skipping to Step 22", curModId);
        // Step 23
        if (stage2ButtonColors[0].Equals(stage2ButtonColors[2]) && stage2ButtonColors[2].Equals(stage2PhrClr[1]) && !stage2ButtonColors[1].Equals(stage2PhrClr[1]))
        {
            crtBtnIdxStg2 = 1;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 22: True", curModId);
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 22: False", curModId);
        // Step 24
        if (stageRuleApplied == 1)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 23: Step 9 was applied to Stage 1", curModId);
            finalValueB += 25;
        }
        else if (stageRuleApplied == 2)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 23: Step 9 was applied to Stage 2", curModId);
            finalValueB += 150;
        }
        else
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 23: Step 9 was not applied to either stages", curModId);
            finalValueB -= 250;
        }
        // Step 25
        int KritsyModCount = idModsonBomb.Where(a => KritsyModIDs.Contains(a)).Count();
        finalValueB -= 10 * KritsyModCount;
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 24 applied. Detected this many modules made by Kritsy: {1}", curModId,KritsyModCount);
        // Step 26
        if (finalValueB < 0)
        {
            finalValueB *= -65;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 25: True", curModId);
            return;
        }
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 25: False", curModId);
        // Step 27
        int sumLocal = 0;
        foreach (string colorName in stage2ButtonColors)
            sumLocal += colorName.Length;
        if (sumLocal > 12)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 26: True", curModId);
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
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 26: False", curModId);
        // Step 28
        int tfc = info.GetTwoFactorCounts();
        if (tfc > 0)
        {
            finalValueB += tfc * 50;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 27: True", curModId);
            return;
        }
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 27: False", curModId);
        // Step 29
        if (stage2Phrases.Count == 7)
        {
            finalValueB /= 7;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 28: True", curModId);
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 28: False", curModId);
        // Step 30
        bool containsRTSensitive = false;
        foreach (string idMod in idModsonBomb)
        {
            containsRTSensitive = containsRTSensitive || RTSensitiveModIDs.Contains(idMod);
        } 
        if (!containsRTSensitive)// Detects if an RT sensitive module is present.
        {
            if (stage2ButtonColors[0].Length > stage2ButtonColors[1].Length && stage2ButtonColors[0].Length > stage2ButtonColors[2].Length)
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 29: The left button has the most characters in its color name.", curModId);
                crtBtnIdxStg2 = 0;
            }
            else if (stage2ButtonColors[1].Length > stage2ButtonColors[2].Length && stage2ButtonColors[1].Length > stage2ButtonColors[0].Length)
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 29: The middle button has the most characters in its color name.", curModId);
                crtBtnIdxStg2 = 1;
            }
            else if (stage2ButtonColors[2].Length > stage2ButtonColors[1].Length && stage2ButtonColors[2].Length > stage2ButtonColors[0].Length)
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 29: The right button has the most characters in its color name.", curModId);
                crtBtnIdxStg2 = 2;
            }
            else
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 29: There is a tie for the most characters in each of the button's name of the color.", curModId);
            }
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 29: An RT sensitive module is present.", curModId);
        // Step 31
        finalValueB /= 3;
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 30 applied.", curModId);
        // Step 32
        int indcCount = 0;
        foreach (string idc in info.GetIndicators())
        {
            bool toAdd = false;
            char[] idcletters = idc.ToCharArray();
            for (int x = 0; x < idcletters.Length; x++)
            {
                if ("SPEAKINGEVIL".Contains(idcletters[x]))
                {
                    toAdd = true;
                }
            }
            if (toAdd)
            {
                finalValueB += 10;
                indcCount++;
            }
        }
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Step 31 applied. There are this many indicators that share a letter in \"SPEAKINGEVIL\": {1}", curModId,indcCount);
    }
    void CalcScaleFactors()
    {
        if (canOverride()) return;
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: SCALE FACTOR CALCULATIONS:", curModId);
        if (startValueA < 5000)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Starting value for stage 1 is less than 5000", curModId);
            scaleFactorA += 1;
        }
        if (startValueB < 5000)
        {
            scaleFactorB += 1;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Starting value for stage 2 is less than 5000", curModId);
        }
        scaleFactorA -= batteryCount / 2;
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Detected this many batteries: {1} , subtracting {2} from stage 1's scale factor", curModId, batteryCount, batteryCount / 2);
        scaleFactorB -= batteryholdCount;
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Detected this many holders: {1}", curModId, batteryholdCount);

        if (idModsonBomb.Contains("forgetThis"))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Forget This is present", curModId);
            scaleFactorA += 1;
        }

        if (idModsonBomb.Contains("forgetEnigma"))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Forget Enigma is present", curModId);
            scaleFactorB += 1;
        }

        if (info.GetSerialNumberNumbers().Count() >= 4)// In a world where symbols can show up in the serial number.
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The serial number has 4 or more digits", curModId);
            scaleFactorA += 3;
        }
        if (info.GetSerialNumberLetters().Count() >= 4)// In a world where symbols can show up in the serial number.
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The serial number has 4 or more letters", curModId);
            scaleFactorB += 3;
        }

        if (idModsonBomb.Contains("timeKeeper"))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The Time Keeper is present", curModId);
            scaleFactorA += 2;
        }

        if (idModsonBomb.Contains("TurnTheKey"))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Turn The Key is present", curModId);
            scaleFactorB += 2;
        }

        if (!definablePhrases.Contains(stage1Phrases[0]))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Stage 1's starting phrase is not definable", curModId);
            scaleFactorA += 2;
        }
        if (definablePhrases.Contains(stage2Phrases[0]))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Stage 2's starting phrase is definable", curModId);
            scaleFactorB += 2;
        }

        if (startTime >= 1800)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The bomb's starting time is 30:00+", curModId);
            scaleFactorA += 2;
            scaleFactorB += 2;
        }

        if (info.GetSerialNumberLetters().Count() == 3)// In a world where symbols can show up in the serial number.
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The serial number has exactly 3 letters", curModId);
            scaleFactorA += 2;
        }
        if (info.GetSerialNumberNumbers().Count() == 3)// In a world where symbols can show up in the serial number.
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The serial number has exactly 3 digits", curModId);
            scaleFactorB += 2;
        }

        if (idModsonBomb.Contains("MemoryV2"))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Forget Me Not is present", curModId);
            scaleFactorA += 1;
        }
        if (!idModsonBomb.Contains("forgetItNot"))
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Forget It Not is not present", curModId);
            scaleFactorB += 1;
        }

        if (finalValueA >= 10000)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The final value for stage 1 is at least 10000", curModId);
            scaleFactorA /= 2;
        }
        if (finalValueB >= 10000)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The final value for stage 2 is at least 10000", curModId);
            scaleFactorB /= 2;
        }

        if (info.IsIndicatorOn(Indicator.BOB) && info.IsIndicatorOn(Indicator.FRK) && batteryCount == 4 && batteryholdCount == 2)
        {
            scaleFactorA = 2;
            scaleFactorB = 2;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Laundry Unicorn and Radiator Unicorn are present; this overrode stage 1 and 2's scale factor", curModId);
        }

        scaleFactorA = Math.Max(Math.Min(scaleFactorA, 5), 2);
        scaleFactorB = Math.Max(Math.Min(scaleFactorB, 5), 2);

        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The scale factor for stage 1 is {1}", curModId, scaleFactorA);
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The scale factor for stage 2 is {1}", curModId, scaleFactorB);
    }
    void DetermineCorrectButton()
    {
        if (canOverride()) return;
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Determining The Correct Button:", curModId);
        if (crtBtnIdxStg1 == null)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Stage 1's correct button was previously not determined during the calculation procedure.", curModId);
            int presentInStage = 0;
            int idxCon6 = -1;
            for (int x = 0; x < stage1ButtonDigits.Length; x++)
            {
                if (stage1Phrases.Contains(phraseNumList[phraseNumList.IndexOf(stage1ButtonDigits[x].ToString())]) || stage1Phrases.Contains(phraseNumList[phraseNumList.IndexOf(stage1ButtonDigits[x].ToString()) + 1]))
                    presentInStage++;
                else
                    idxCon6 = x;
            }


            if (finalValueA >= 0 && finalValueA <= 9999)
            {
                crtBtnIdxStg1 = 0;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The final value for stage 1 is between 0 to 9999 inclusive", curModId);
            }
            else if (info.IsPortPresent(Port.Parallel) && stage1ButtonColors.Count(a => a.Equals("Magenta")) == 1)
            {
                crtBtnIdxStg1 = stage1ButtonColors.IndexOf("Magenta");
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: A Parallel port is present and exactly 1 magenta button is present.", curModId);
            }
            else if (info.IsPortPresent(Port.StereoRCA) && stage1ButtonColors.Count(a => a.Equals("Red")) == 1 && stage1ButtonColors.Count(a => a.Equals("White")) == 1)
            {
                for (int x = 0; x < stage1ButtonColors.Count; x++)
                    if (!stage1ButtonColors[x].Equals("Red") && !stage1ButtonColors[x].Equals("White"))
                    {
                        crtBtnIdxStg1 = x;
                    }
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: An RCA port is present and exactly 1 red and white button are present.", curModId);
            }
            else if (idModsonBomb.Contains("PercentageGreyModule") && stage1ButtonColors.Count(a => a.Equals("Black")) == 1)
            {
                crtBtnIdxStg1 = stage1ButtonColors.IndexOf("Black");
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: % Grey is present and exactly 1 black button is present.", curModId);
            }
            else if (!stage1ButtonColors.Where(a => !a.Equals("White")).ToList().Intersect(stage1PhrClr.Where(a => !a.Equals("White")).ToList()).Any())
            {
                crtBtnIdxStg1 = 2;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Excluding white, all the phrases and buttons do not share a color.", curModId);
            }
            else if (stage1PhrClr.Count(a => a.Equals("White")) == stage1PhrClr.Count() && presentInStage == 2)
            {
                crtBtnIdxStg1 = idxCon6;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: All the phrases are white and exactly 1 button's label is not present in numerical form or decimal form.", curModId);
            }
            else if (stage1ButtonColors.Count(a => a.Equals("Blue")) == 1 && stage1ButtonColors.Count(a => a.Equals("Green")) == 1 && stage1ButtonColors.Count(a => a.Equals("Red")) == 1)
            {
                crtBtnIdxStg1 = stage1ButtonColors.IndexOf("Blue");
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: There is exactly 1 green, 1 red and 1 blue button.", curModId);
            }
            else if (idModsonBomb.Contains("leftandRight"))
            {
                crtBtnIdxStg1 = 1;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Left and Right is present.", curModId);
            }
            else
            {
                crtBtnIdxStg1 = (int?)(finalValueA % 3);
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The final value for stage 1 modulo 3, plus 1 is {1}.", curModId, (int)(finalValueA % 3 + 1));
            }
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Stage 1's correct button was previously determined during the calculation procedure.", curModId);
        if (crtBtnIdxStg2 == null)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Stage 2's correct button was previously not determined during the calculation procedure.", curModId);
            int presentInStage = 0;
            int idxCon6 = -1;
            for (int x = 0; x < stage2ButtonDigits.Length; x++)
            {
                if (stage2Phrases.Contains(phraseNumList[phraseNumList.IndexOf(stage2ButtonDigits[x].ToString())]) || stage2Phrases.Contains(phraseNumList[phraseNumList.IndexOf(stage2ButtonDigits[x].ToString()) + 1]))
                    presentInStage++;
                else
                    idxCon6 = x;
            }

            if (finalValueB >= 0 && finalValueB <= 9999)
            {
                crtBtnIdxStg2 = 0;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The final value for stage 2 is between 0 to 9999 inclusive", curModId);
            }
            else if (info.IsPortPresent(Port.Parallel) && stage2ButtonColors.Count(a => a.Equals("Magenta")) == 1)
            {
                crtBtnIdxStg2 = stage2ButtonColors.IndexOf("Magenta");
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: A Parallel port is present and exactly 1 magenta button is present.", curModId);
            }
            else if (info.IsPortPresent(Port.StereoRCA) && stage2ButtonColors.Count(a => a.Equals("Red")) == 1 && stage2ButtonColors.Count(a => a.Equals("White")) == 1)
            {
                for (int x = 0; x < stage2ButtonColors.Count; x++)
                    if (!stage2ButtonColors[x].Equals("Red") && !stage2ButtonColors[x].Equals("White"))
                    {
                        crtBtnIdxStg2 = x;
                    }
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: An RCA port is present and exactly 1 red and white button are present.", curModId);
            }
            else if (idModsonBomb.Contains("PercentageGreyModule") && stage2ButtonColors.Count(a => a.Equals("Black")) == 1)
            {
                crtBtnIdxStg2 = stage2ButtonColors.IndexOf("Black");
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: % Grey is present and exactly 1 black button is present.", curModId);
            }
            else if (!stage2ButtonColors.Where(a => !a.Equals("White")).ToList().Intersect(stage2PhrClr.Where(a => !a.Equals("White")).ToList()).Any())
            {
                crtBtnIdxStg2 = 2;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Excluding white, all the phrases and buttons do not share a color.", curModId);
            }
            else if (stage2PhrClr.Count(a => a.Equals("White")) == stage2PhrClr.Count() && presentInStage == 2)
            {
                crtBtnIdxStg1 = idxCon6;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: All the phrases are white and exactly 1 button's label is not present in numerical form or decimal form.", curModId);
            }
            else if (stage2ButtonColors.Count(a => a.Equals("Blue")) == 1 && stage2ButtonColors.Count(a => a.Equals("Green")) == 1 && stage2ButtonColors.Count(a => a.Equals("Red")) == 1)
            {
                crtBtnIdxStg2 = stage2ButtonColors.IndexOf("Blue");
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: There is exactly 1 green, 1 red and 1 blue button.", curModId);
            }
            else if (idModsonBomb.Contains("leftandRight"))
            {
                crtBtnIdxStg2 = 1;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Left and Right is present.", curModId);
            }
            else
            {
                crtBtnIdxStg2 = 2 - (int)(finalValueB % 3);
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The final value for stage 2 modulo 3, plus 1 is {1}.", curModId,(int)(finalValueB % 3 + 1));
            }
        }
        else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Stage 2's correct button was previously determined during the calculation procedure.", curModId);

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
    void DetermineStateConditions()
    {
        if (canOverride()) return;
        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: STATE CALCULATIONS:", curModId);
        if (idModsonBomb.Contains("veryAnnoyingButton") || nameModsonBomb.Count - info.GetSolvableModuleNames().Count > 0)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Condition 1 takes highest priority for both stages", curModId);
            holdCorStg1 = false;
            holdCorStg2 = false;
        }
        else
        {
            bool isOtherLabelsPresent = true;
            bool isCorrectPresent = false;
            // Stage 1
            for (int x = 0; x < stage1ButtonDigits.Length; x++)
            {
                if (x == crtBtnIdxStg1)
                {
                    isCorrectPresent = stage1Phrases.Contains(phraseNumList[phraseNumList.IndexOf(stage1ButtonDigits[x].ToString())]) || stage1Phrases.Contains(phraseNumList[phraseNumList.IndexOf(stage1ButtonDigits[x].ToString()) + 1]);
                }
                else
                {
                    isOtherLabelsPresent = isOtherLabelsPresent && (stage1Phrases.Contains(phraseNumList[phraseNumList.IndexOf(stage1ButtonDigits[x].ToString())]) || stage1Phrases.Contains(phraseNumList[phraseNumList.IndexOf(stage1ButtonDigits[x].ToString()) + 1]));
                }
            }
            if (isOtherLabelsPresent)
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Condition 2 takes highest priority for stage 1", curModId);
                holdCorStg1 = true;
            }
            else if (isCorrectPresent)
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Condition 3 takes highest priority for stage 1", curModId);
                holdCorStg1 = false;
            }
            else if ((stage1ButtonDigits[0] <= stage1ButtonDigits[1] && stage1ButtonDigits[1] <= stage1ButtonDigits[2]) || (stage1ButtonDigits[0] >= stage1ButtonDigits[1] && stage1ButtonDigits[1] >= stage1ButtonDigits[2]))
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Condition 4 takes highest priority for stage 1", curModId);
                holdCorStg1 = true;
            }
            else
            {

                int productOthers = 1;
                int sumOthers = 0;
                int? otherPosDifference = 0;

                int correctButtonDigit = 0;

                for (int x = 0; x < stage1ButtonDigits.Length; x++)
                {
                    if (x == crtBtnIdxStg1)
                    {
                        correctButtonDigit = stage1ButtonDigits[x];
                    }
                    else
                    {
                        productOthers *= stage1ButtonDigits[x];
                        sumOthers += stage1ButtonDigits[x];
                        if (otherPosDifference == null)
                        {
                            otherPosDifference = stage1ButtonDigits[x];
                        }
                        else
                            otherPosDifference = Math.Abs((int)otherPosDifference - stage1ButtonDigits[x]);
                    }
                }
                if (Math.Abs((int)otherPosDifference - correctButtonDigit) <= 2)
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Condition 5 takes highest priority for stage 1", curModId);
                    holdCorStg1 = false;
                }
                else if (sumOthers % 10 == correctButtonDigit)
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Condition 6 takes highest priority for stage 1", curModId);
                    holdCorStg1 = true;
                }
                else
                {
                    holdCorStg1 = productOthers % 10 != correctButtonDigit;
                    if (productOthers % 10 == correctButtonDigit)
                    {
                        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Condition 7 takes highest priority for stage 1", curModId);
                    }
                    else
                    {
                        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: No other conditions take highest priority for stage 1", curModId);
                    }
                }
            }
            // Stage 2
            isOtherLabelsPresent = true;
            isCorrectPresent = false;
            for (int x = 0; x < stage2ButtonDigits.Length; x++)
            {
                if (x == crtBtnIdxStg2)
                {
                    isCorrectPresent = stage2Phrases.Contains(phraseNumList[phraseNumList.IndexOf(stage2ButtonDigits[x].ToString())]) || stage2Phrases.Contains(phraseNumList[phraseNumList.IndexOf(stage2ButtonDigits[x].ToString()) + 1]);
                }
                else
                {
                    isOtherLabelsPresent = isOtherLabelsPresent && (stage2Phrases.Contains(phraseNumList[phraseNumList.IndexOf(stage2ButtonDigits[x].ToString())]) || stage2Phrases.Contains(phraseNumList[phraseNumList.IndexOf(stage2ButtonDigits[x].ToString()) + 1]));
                }
            }
            if (isOtherLabelsPresent)
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Condition 2 takes highest priority for stage 2", curModId);
                holdCorStg2 = true;
            }
            else if (isCorrectPresent)
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Condition 3 takes highest priority for stage 2", curModId);
                holdCorStg2 = false;
            }
            else if ((stage2ButtonDigits[0] <= stage2ButtonDigits[1] && stage2ButtonDigits[1] <= stage2ButtonDigits[2]) || (stage2ButtonDigits[0] >= stage2ButtonDigits[1] && stage2ButtonDigits[1] >= stage2ButtonDigits[2]))
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Condition 4 takes highest priority for stage 2", curModId);
                holdCorStg2 = true;
            }
            else
            {

                int productOthers = 1;
                int sumOthers = 0;
                int? otherPosDifference = 0;

                int correctButtonDigit = 0;

                for (int x = 0; x < stage2ButtonDigits.Length; x++)
                {
                    if (x == crtBtnIdxStg2)
                    {
                        correctButtonDigit = stage2ButtonDigits[x];
                    }
                    else
                    {
                        productOthers *= stage2ButtonDigits[x];
                        sumOthers += stage2ButtonDigits[x];
                        if (otherPosDifference == null)
                        {
                            otherPosDifference = stage2ButtonDigits[x];
                        }
                        else
                            otherPosDifference = Math.Abs((int)otherPosDifference - stage2ButtonDigits[x]);
                    }
                }
                if (Math.Abs((int)otherPosDifference - correctButtonDigit) <= 2)
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Condition 5 takes highest priority for stage 2", curModId);
                    holdCorStg2 = false;
                }
                else if (sumOthers % 10 == correctButtonDigit)
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Condition 6 takes highest priority for stage 2", curModId);
                    holdCorStg2 = true;
                }
                else
                {
                    holdCorStg2 = productOthers % 10 != correctButtonDigit;
                    if (productOthers % 10 == correctButtonDigit)
                    {
                        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Condition 7 takes highest priority for stage 2", curModId);
                    }
                    else
                    {
                        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: No other conditions take highest priority for stage 2", curModId);
                    }
                }
            }
        }
    }
    string getMorseStringofLetter(char letter)
    {
        switch (letter.ToString().ToLower().ToCharArray()[0])
        {
            case 'a':
                return ".-";
            case 'b':
                return "-...";
            case 'c':
                return "-.-.";
            case 'd':
                return "-..";
            case 'e':
                return ".";
            case 'f':
                return "..-.";
            case 'g':
                return "--.";
            case 'h':
                return "....";
            case 'i':
                return "..";
            case 'j':
                return ".---";
            case 'k':
                return "-.-";
            case 'l':
                return ".-..";
            case 'm':
                return "--";
            case 'n':
                return "-.";
            case 'o':
                return "---";
            case 'p':
                return ".--.";
            case 'q':
                return "--.-";
            case 'r':
                return ".-.";
            case 's':
                return "...";
            case 't':
                return "-";
            case 'u':
                return "..-";
            case 'v':
                return "...-";
            case 'w':
                return ".--";
            case 'x':
                return "-..-";
            case 'y':
                return "-.--";
            case 'z':
                return "--..";
            default:
                return "";
        }
    }
    private readonly List<string> primaryColorList = new List<string>() {"Red","Green","Blue"};
    private readonly List<string> primaryColorString = new List<string>() { "R","G","B" };
    int resetCount = 0;
    private readonly string[] combinedcharacterList = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};
    IEnumerator FlashLeftMostUnHeldButton(string MorseCharacter, bool isConsistent, string[] colorListIn)
    {
        GameObject[] GameOBJArray = new GameObject[] { buttonL, buttonM };
        leftmostIdxFlashable = 0;
        if (curbtnHeld == 0)
        {
            leftmostIdxFlashable = 1;
        }
        int curFlashPart = 0;
        TextMesh curText = GameOBJArray[leftmostIdxFlashable].GetComponentInChildren<TextMesh>();
        MeshRenderer curMeshRdr = GameOBJArray[leftmostIdxFlashable].GetComponent<MeshRenderer>();
        curText.text = "";
        curMeshRdr.material = materialList[0];
        if (isConsistent)
        {
            while (curbtnHeld != -1)
            {

                if (curFlashPart < colorListIn.Length)
                {
                    curMeshRdr.material.color = colorList[colorString.IndexOf(colorListIn[curFlashPart])];
                    if (colorBlindActive)
                    {
                        curText.text = primaryColorString[primaryColorList.IndexOf(colorListIn[curFlashPart])];
                        curText.color = Color.white;
                    }
                    curFlashPart++;
                    yield return new WaitForSeconds(0.2f);
                    if (curbtnHeld == -1) yield break;
                    curMeshRdr.material.color = Color.black;
                    curText.text = "";
                }
                else
                {
                    curFlashPart = 0;
                    yield return new WaitForSeconds(0.8f);
                    if (curbtnHeld == -1) yield break;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            while (curbtnHeld != -1)
            {
                if (curFlashPart < colorListIn.Length)
                {
                    curMeshRdr.material.color = colorList[colorString.IndexOf(colorListIn[curFlashPart])];
                    if (colorBlindActive)
                    {
                        curText.text = primaryColorString[primaryColorList.IndexOf(colorListIn[curFlashPart])];
                        curText.color = Color.white;
                    }
                    if (MorseCharacter.Substring(curFlashPart,1).Equals("-"))
                        yield return new WaitForSeconds(0.75f);
                    else
                        yield return new WaitForSeconds(0.25f);
                    if (curbtnHeld == -1) yield break;
                    curFlashPart++;
                    curMeshRdr.material.color = Color.black;
                    curText.text = "";
                }
                else
                {
                    curFlashPart = 0;
                    yield return new WaitForSeconds(2.25f);
                    if (curbtnHeld == -1) yield break;
                }
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f,0.5f));
            }
        }
        yield return null;
    }
    IEnumerator FlashRightMostUnHeldButton(string MorseCharacter, bool isConsistent, string[] colorListIn)
    {
        GameObject[] GameOBJArray = new GameObject[] { buttonL, buttonM, buttonR };
        rightmostIdxFlashable = 2;
        if (curbtnHeld == 2)
        {
            rightmostIdxFlashable = 1;
        }
        int curFlashPart = 0;
        TextMesh curText = GameOBJArray[rightmostIdxFlashable].GetComponentInChildren<TextMesh>();
        MeshRenderer curMeshRdr = GameOBJArray[rightmostIdxFlashable].GetComponent<MeshRenderer>();
        curText.text = "";
        curMeshRdr.material = materialList[0];
        if (isConsistent)
        {
            while (curbtnHeld != -1)
            {

                if (curFlashPart < colorListIn.Length)
                {
                    curMeshRdr.material.color = colorList[colorString.IndexOf(colorListIn[curFlashPart])];
                    if (colorBlindActive)
                    {
                        curText.text = primaryColorString[primaryColorList.IndexOf(colorListIn[curFlashPart])];
                        curText.color = Color.white;
                    }
                    curFlashPart++;
                    yield return new WaitForSeconds(0.2f);
                    if (curbtnHeld == -1) yield break;
                    curMeshRdr.material.color = Color.black;
                    curText.text = "";
                }
                else
                {
                    curFlashPart = 0;
                    yield return new WaitForSeconds(0.8f);
                    if (curbtnHeld == -1) yield break;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            while (curbtnHeld != -1)
            {
                if (curFlashPart < colorListIn.Length)
                {
                    curMeshRdr.material.color = colorList[colorString.IndexOf(colorListIn[curFlashPart])];
                    if (colorBlindActive)
                    {
                        curText.text = primaryColorString[primaryColorList.IndexOf(colorListIn[curFlashPart])];
                        curText.color = Color.white;
                    }
                    if (MorseCharacter.Substring(curFlashPart, 1).Equals("-"))
                        yield return new WaitForSeconds(0.75f);
                    else
                        yield return new WaitForSeconds(0.25f);
                    if (curbtnHeld == -1) yield break;
                    curFlashPart++;
                    curMeshRdr.material.color = Color.black;
                    curText.text = "";
                }
                else
                {
                    curFlashPart = 0;
                    yield return new WaitForSeconds(2.25f);
                    if (curbtnHeld == -1) yield break;
                }
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));
            }
        }
        yield return null;
    }
    IEnumerator PlayStartHoldAnim(int btnHoldidx)
    {
        GameObject[] GameOBJArray = new GameObject[] { buttonL, buttonM, buttonR };
        backing.GetComponent<MeshRenderer>().material.color = Color.black;
        string originalText = display.text;
        while (!(isHeld || curbtnHeld == -1))
        {
            for (int x = 0; x < GameOBJArray.Length; x++)
            {
                TextMesh curText = GameOBJArray[x].GetComponentInChildren<TextMesh>();
                MeshRenderer curMeshRdr = GameOBJArray[x].GetComponent<MeshRenderer>();
                if (x != curbtnHeld)
                {
                    curMeshRdr.material.color = new Color(curMeshRdr.material.color.r + .01f, curMeshRdr.material.color.g + .01f, curMeshRdr.material.color.b + .01f);
                    curText.color = new Color(curText.color.r, curText.color.g, curText.color.b, curText.color.a - .01f);
                }
            }
            string scrambledText = "";
            for (int x = 0; x < originalText.Length; x++)
            {
                if (originalText.Substring(x, 1).RegexMatch(@"\s"))
                    scrambledText += " ";
                else
                    scrambledText += combinedcharacterList[UnityEngine.Random.Range(0, combinedcharacterList.Length)];
            }
            display.text = scrambledText;
            display.color = new Color(1, 1, 1, display.color.a - 0.01f);
            yield return new WaitForSeconds(0);
        }
        yield return null;
    }
    IEnumerator HandleExtendedHold()
    {
        isHeld = false;
        soundwithRef = sound.PlaySoundAtTransformWithRef("HumProgressing", transform);
        StartCoroutine(PlayStartHoldAnim(curbtnHeld));
        for (int x = 0; x < 30; x++)
        {
            yield return new WaitForSeconds(.1f);
            if (curbtnHeld == -1) yield break;
        }
        soundwithRef.StopSound();
        sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CapacitorPop, transform);
        if (curbtnHeld != -1)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: For holding at {1} reset(s):", curModId, resetCount);
            resetCount++;
            

            // Handle Left flashing
            leftHoldColors.Clear();

            isLeftFlashingConsistent = UnityEngine.Random.Range(0, 5) != 0;
            int flashCountLeft;
            string leftFlashChar = "";

            if (!isLeftFlashingConsistent)
            {
                inconsistMorseLetterL = letters[UnityEngine.Random.Range(0, letters.Length)];
                leftFlashChar = getMorseStringofLetter(inconsistMorseLetterL.ToCharArray()[0]);
                flashCountLeft = leftFlashChar.Length;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The left-most unheld button is flashing inconsistently the Morse letter {1}", curModId, inconsistMorseLetterL);
            }
            else
            {
                flashCountLeft = UnityEngine.Random.Range(1, 5);
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The left-most unheld button is flashing consistently", curModId);
            }
            for (int x = 0; x < flashCountLeft; x++)
            {
                leftHoldColors.Add(primaryColorList[UnityEngine.Random.Range(0,primaryColorList.Count)]);
            }
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The left-most unheld button is flashing the following colors in order: {1}", curModId,FormatDebugList(leftHoldColors));
            // Handle Right flashing
            rightHoldColors.Clear();

            isRightFlashingConsistent = UnityEngine.Random.Range(0, 5) != 0;
            int flashCountRight;
            string rightFlashChar = "";

            if (!isRightFlashingConsistent)
            {
                inconsistMorseLetterR = letters[UnityEngine.Random.Range(0, letters.Length)];
                rightFlashChar = getMorseStringofLetter(inconsistMorseLetterR.ToCharArray()[0]);
                flashCountRight = rightFlashChar.Length;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The right-most unheld button is flashing inconsistently the Morse letter {1}", curModId, inconsistMorseLetterR);
            }
            else
            {
                flashCountRight = UnityEngine.Random.Range(1, 5);
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The right-most unheld button is flashing consistently", curModId);
            }
            for (int x = 0; x < flashCountRight; x++)
            {
                rightHoldColors.Add(primaryColorList[UnityEngine.Random.Range(0, primaryColorList.Count)]);
            }
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The right-most unheld button is flashing the following colors in order: {1}", curModId, FormatDebugList(rightHoldColors));
            timeHeldSec = 0;
            isHeld = true;
            backing.GetComponent<MeshRenderer>().material.color = Color.black;

            StartCoroutine(FlashLeftMostUnHeldButton(leftFlashChar, isLeftFlashingConsistent, leftHoldColors.ToArray()));
            StartCoroutine(FlashRightMostUnHeldButton(rightFlashChar, isRightFlashingConsistent, rightHoldColors.ToArray()));

            while (curbtnHeld != -1)
            {
                display.text = (timeHeldSec / 60).ToString("00") + ":" + (timeHeldSec % 60).ToString("00");
                display.color = Color.white;
                yield return new WaitForSeconds(1);
                if (curbtnHeld == -1) yield break;
                timeHeldSec++;
            }
        }
        yield return null;
    }
    int GetDigitalRoot(int value)
    {
        if (value <= 9) return value;
        int sum = 0;
        int processedvalue = Math.Abs(value);
        foreach (char letter in processedvalue.ToString())
        {
            sum += int.Parse(letter.ToString());
        }
        return GetDigitalRoot(sum);
    }
    bool IsCorrectOnHold()// Check if the button held is 100% correct.
    {
        
        int localTimeHr = DateTime.Today.Hour;
        int localTimeMin = DateTime.Today.Minute;
        int[] valuesUnder60 = { 0, 2, 6, 12, 20, 30, 42, 56 };
        int[] primePossibleValues = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };
        int[] swanCodes = { 4, 8, 15, 16, 23, 42 };
        int[] tribbonachiCodes = { 1, 1, 1, 3, 5, 9, 17, 31, 57 };
        int timeRemainingBomb = Mathf.FloorToInt(info.GetTime());

        // Formatted Time for the bomb and display
        string formattedTime = info.GetFormattedTime();
        string displayFormattedTime = (timeHeldSec / 60).ToString("00") + (timeHeldSec % 60).ToString("00");
        // Formatted Seconds Timer for bomb and display
        string secondsDisplayTimer = (timeHeldSec % 60).ToString("00");
        string secondsbombTimer = ((int)info.GetTime() % 60).ToString("00");


        // BEGIN RELEASE CONDITION HANDLER
        if (isLeftFlashingConsistent && leftHoldColors.Distinct().Count() == 1 && leftHoldColors.Count() == 4 && isRightFlashingConsistent && rightHoldColors.Distinct().Count() == 1 && rightHoldColors.Count() == 4)
        {// If both buttons that are not held are flashing the same color as its previous flashes four times in a row with consistent rest inbetween
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Both buttons not held are flashing the same color 4 times in a row with consistent rest inbetween", curModId);
            if (leftHoldColors.Contains("Red"))
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The leftmost button is flashing all red", curModId);
                if (rightHoldColors.Contains("Red"))
                {
                    return timeRemainingBomb / 60 % 2 == 0;
                }
                else if (rightHoldColors.Contains("Green"))
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The rightmost button is flashing all green", curModId);
                    return valuesUnder60.Contains(timeHeldSec % 60);
                }
                else if (rightHoldColors.Contains("Blue"))
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The rightmost button is flashing all blue", curModId);
                    return valuesUnder60.Contains((int)info.GetTime() % 60);
                }
            }
            else if (leftHoldColors.Contains("Green"))
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The leftmost button is flashing all green", curModId);
                if (rightHoldColors.Contains("Red"))
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The rightmost button is flashing all red", curModId);
                    List<char> nonZeroDigits = (localTimeHr.ToString() + localTimeMin.ToString()).ToCharArray().Where(x => !x.Equals('0')).ToList();
                    if (nonZeroDigits.Count > 0)
                    {
                        bool isCorrect = false;
                        foreach (char chr in formattedTime)
                        {
                            isCorrect = isCorrect || nonZeroDigits.Contains(chr);
                        }
                        return isCorrect;
                    }
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: There are no non-zero digits from the alarm clock, skipping to next release condition", curModId);
                }
                else if (rightHoldColors.Contains("Green"))
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The rightmost button is flashing all green", curModId);
                    return tribbonachiCodes.Contains(int.Parse(secondsbombTimer));
                }
                else if (rightHoldColors.Contains("Blue"))
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The rightmost button is flashing all blue", curModId);
                    List<char> nonZeroDigits = (localTimeHr.ToString() + localTimeMin.ToString()).ToCharArray().Where(x => !x.Equals('0')).ToList();
                    if (nonZeroDigits.Count > 0)
                    {
                        bool isCorrect = false;
                        foreach (char chr in displayFormattedTime)
                        {
                            isCorrect = isCorrect || nonZeroDigits.Contains(chr);
                        }
                        return isCorrect;
                    }
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: There are no non-zero digits from the alarm clock, skipping to next release condition", curModId);
                }
            }
            else if (leftHoldColors.Contains("Blue"))
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The leftmost button is flashing all blue", curModId);
                if (rightHoldColors.Contains("Red"))
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The rightmost button is flashing all red", curModId);
                    return secondsDisplayTimer == "40" || secondsDisplayTimer == "4";
                }
                else if (rightHoldColors.Contains("Green"))
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The rightmost button is flashing all green", curModId);
                    return secondsbombTimer =="40" || secondsbombTimer == "4";
                }
                else if (rightHoldColors.Contains("Blue"))
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The rightmost button is flashing all blue", curModId);

                    return tribbonachiCodes.Contains(int.Parse(secondsDisplayTimer));
                }
            }
        }
        if ((isLeftFlashingConsistent && leftHoldColors.Distinct().Count() == 1 && leftHoldColors.Count() == 4) || (isRightFlashingConsistent && rightHoldColors.Distinct().Count() == 1 && rightHoldColors.Count() == 4 && (!(isLeftFlashingConsistent && leftHoldColors.Distinct().Count() == 1 && leftHoldColors.Count() == 4 && isRightFlashingConsistent && rightHoldColors.Distinct().Count() == 1 && rightHoldColors.Count() == 4))))
        { // Otherwise, if exactly 1 button is flashing the same color four times in a row with consistent rest in between
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: There is exactly 1 button flashing the same color 4 times in a row consistently", curModId);
            int idxConsistent = -1;
            string colorReference = "";
            if (isLeftFlashingConsistent && leftHoldColors.Distinct().Count() == 1 && leftHoldColors.Count() == 4)
            {
                idxConsistent = leftmostIdxFlashable;
                colorReference = leftHoldColors[0];
            }
            else if (isRightFlashingConsistent && rightHoldColors.Distinct().Count() == 1 && rightHoldColors.Count() == 4)
            {
                idxConsistent = rightmostIdxFlashable;
                colorReference = rightHoldColors[0];
            }
            if (idxConsistent == 0)
            {
                if (colorReference.Equals("Red"))
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The left button is flashing red consistently 4 times", curModId);
                    List<int> uniqueDigits = info.GetSerialNumberNumbers().Distinct().ToList();
                    string bSecondTimer = ((int)info.GetTime() % 60).ToString("00");
                    bool containsDigit = false;
                    foreach (char secondDigit in bSecondTimer.ToCharArray())
                    {
                        containsDigit = containsDigit || uniqueDigits.Contains(int.Parse(secondDigit.ToString()));
                    }
                    return !containsDigit;
                }
                else if (colorReference.Equals("Green"))
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The left button is flashing green consistently 4 times", curModId);
                    int sum = 0;
                    foreach (char digit in secondsbombTimer)
                    {
                        sum += int.Parse(digit.ToString());
                    }
                    return primePossibleValues.Contains(sum);
                }
                else if (colorReference.Equals("Blue"))
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The left button is flashing blue consistently 4 times", curModId);
                    int sum = 0;
                    foreach (char digit in displayFormattedTime)
                    {
                        sum += int.Parse(digit.ToString());
                    }
                    return primePossibleValues.Contains(sum);
                }
            }
            else if (idxConsistent == 1)
            {
                if (colorReference.Equals("Red"))
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The middle button is flashing red consistently 4 times", curModId);
                    return swanCodes.Contains(int.Parse(secondsDisplayTimer));
                }
                else if (colorReference.Equals("Green"))
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The middle button is flashing green consistently 4 times", curModId);
                    return secondsbombTimer.ToCharArray().Distinct().Count() == 1;
                }
                else if (colorReference.Equals("Blue"))
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The middle button is flashing blue consistently 4 times", curModId);
                    return swanCodes.Contains(int.Parse(secondsbombTimer));
                }
            }
            else if (idxConsistent == 2)
            {
                if (colorReference.Equals("Red"))
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The right button is flashing red consistently 4 times", curModId);
                    List<int> uniqueDigits = info.GetSerialNumberNumbers().Distinct().ToList();
                    if (uniqueDigits.Any())
                    {
                        bool containsDigit = false;
                        foreach (char secondDigit in secondsbombTimer.ToCharArray())
                        {
                            containsDigit = containsDigit || uniqueDigits.Contains(int.Parse(secondDigit.ToString()));
                        }
                        return containsDigit;
                    }
                    else Debug.LogFormat("[Bamboozling Time Keeper #{0}]: There are no digits in the serial number, making this condition impossible. Skipping to the next \"Otherwise\"", curModId);
                }
                else if (colorReference.Equals("Green"))
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The right button is flashing green consistently 4 times", curModId);
                    int sum = 0;
                    foreach (char digit in secondsbombTimer)
                    {
                        sum += int.Parse(digit.ToString());
                    }
                    return !primePossibleValues.Contains(sum);
                }
                else if (colorReference.Equals("Blue"))
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The right button is flashing blue consistently 4 times", curModId);
                    List<int> uniqueDigits = info.GetSerialNumberNumbers().Distinct().ToList();
                    bool containsDigit = false;
                    foreach (char secondDigit in secondsDisplayTimer.ToCharArray())
                    {
                        containsDigit = containsDigit || uniqueDigits.Contains(int.Parse(secondDigit.ToString()));
                    }
                    return !containsDigit;
                }
            }
        }
        int TFC = info.GetTwoFactorCounts();
        if (TFC > 0)
        {// Otherwise if there are Two Factors on this bomb
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: There are Two Factors on this bomb", curModId);
            List<int> TwoCodes = new List<int>();
            foreach (int oneCode in info.GetTwoFactorCodes())
            {
                TwoCodes.Add(oneCode);
            }
            if (TwoCodes.Count == 1)
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Exactly 1 is present whose least significant digit displayed {1} at the time of the release.", curModId, TwoCodes[0] % 10);
                return secondsDisplayTimer.Contains((TwoCodes[0]%10).ToString());
            }
            if (new List<int>() { 2,3,4 }.Contains( TwoCodes.Count))
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Exactly 2, 3 or 4 Two Factors are present.", curModId);
                int average = 0;
                foreach (int code in TwoCodes)
                {
                    average += code;
                }
                average /= TwoCodes.Count();
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: After averaging the Two Factors, the least significant digit in the average is {1}", curModId, average % 10);
                return ((int)info.GetTime() % 60).ToString("00").Contains((average % 10).ToString());
            }
            int highestTwoFactor = TwoCodes[0];
            foreach (int oneCode in TwoCodes)
            {
                highestTwoFactor = Math.Max(oneCode, highestTwoFactor);
            }
            List<char> highestCodeDigits = highestTwoFactor.ToString().ToCharArray().Distinct().ToList();
            if (highestCodeDigits.Count >= 4)
            {
                int digitsInCommon = 0;
                foreach (char digit in formattedTime)
                {
                    if (highestCodeDigits.Contains(digit))
                        digitsInCommon++;
                }
                foreach (char digit in displayFormattedTime)
                {
                    if (highestCodeDigits.Contains(digit))
                        digitsInCommon++;
                }
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The highest Two Factor on the bomb when released is {1}.", curModId,highestTwoFactor);
                return digitsInCommon >= 4;
            }
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The highest Two Factor on the bomb has 3 or fewer distant digits, skipping to next Otherwise", curModId);
        }
        if (!(isLeftFlashingConsistent && isRightFlashingConsistent))
        {// Otherwise if at least 1 button not held is flashing irregularrly.
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: At least 1 button not held is flashing irregularrly.", curModId);
            List<int> irregularFlashesValue = new List<int>();
            if (!isLeftFlashingConsistent)
            {
                int initialValue = letters.ToList().IndexOf(inconsistMorseLetterL) + 1;
                int valuetoAdd = initialValue;
                for (int x = 0; x < leftHoldColors.Count; x++)
                {
                    switch (x)
                    {
                        case 0:
                            if (leftHoldColors[x].Equals("Red"))
                                valuetoAdd += nameModsonBomb.Count();
                            else if (leftHoldColors[x].Equals("Green"))
                                valuetoAdd += TodaysMonth;
                            else if (leftHoldColors[x].Equals("Blue"))
                                valuetoAdd += info.GetPortCount();
                            break;
                        case 1:
                            if (leftHoldColors[x].Equals("Red"))
                                valuetoAdd += GetDigitalRoot(nameModsonBomb.Count());
                            else if (leftHoldColors[x].Equals("Green"))
                                valuetoAdd -= info.GetPortPlateCount();
                            else if (leftHoldColors[x].Equals("Blue"))
                                valuetoAdd += info.GetPortPlateCount();
                            break;
                        case 2:
                            if (leftHoldColors[x].Equals("Red"))
                                valuetoAdd -= nameModsonBomb.Count();
                            else if (leftHoldColors[x].Equals("Green"))
                                valuetoAdd -= TodaysMonth;
                            else if (leftHoldColors[x].Equals("Blue"))
                                valuetoAdd -= info.GetPortCount();
                            break;
                        case 3:
                            if (leftHoldColors[x].Equals("Red"))
                                valuetoAdd -= GetDigitalRoot(nameModsonBomb.Count());
                            else if (leftHoldColors[x].Equals("Green"))
                                valuetoAdd += initialValue % 10;
                            else if (leftHoldColors[x].Equals("Blue"))
                                valuetoAdd -= initialValue % 10;
                            break;
                    }
                }

                valuetoAdd = Math.Abs(valuetoAdd);
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The left button not held gave a value of {1}", curModId, valuetoAdd);
                irregularFlashesValue.Add(valuetoAdd % 10);
            }
            if (!isRightFlashingConsistent)
            {
                int initialValue = letters.ToList().IndexOf(inconsistMorseLetterR) + 1;
                int valuetoAdd = initialValue;
                for (int x = 0; x < rightHoldColors.Count; x++)
                {
                    switch (x)
                    {
                        case 0:
                            if (rightHoldColors[x].Equals("Red"))
                                valuetoAdd += nameModsonBomb.Count();
                            else if (rightHoldColors[x].Equals("Green"))
                                valuetoAdd += TodaysMonth;
                            else if (rightHoldColors[x].Equals("Blue"))
                                valuetoAdd += info.GetPortCount();
                            break;
                        case 1:
                            if (rightHoldColors[x].Equals("Red"))
                                valuetoAdd += GetDigitalRoot(nameModsonBomb.Count());
                            else if (rightHoldColors[x].Equals("Green"))
                                valuetoAdd -= info.GetPortPlateCount();
                            else if (rightHoldColors[x].Equals("Blue"))
                                valuetoAdd += info.GetPortPlateCount();
                            break;
                        case 2:
                            if (rightHoldColors[x].Equals("Red"))
                                valuetoAdd -= nameModsonBomb.Count();
                            else if (rightHoldColors[x].Equals("Green"))
                                valuetoAdd -= TodaysMonth;
                            else if (rightHoldColors[x].Equals("Blue"))
                                valuetoAdd -= info.GetPortCount();
                            break;
                        case 3:
                            if (rightHoldColors[x].Equals("Red"))
                                valuetoAdd -= GetDigitalRoot(nameModsonBomb.Count());
                            else if (rightHoldColors[x].Equals("Green"))
                                valuetoAdd += initialValue % 10;
                            else if (rightHoldColors[x].Equals("Blue"))
                                valuetoAdd -= initialValue % 10;
                            break;
                    }
                }
                valuetoAdd = Math.Abs(valuetoAdd);
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The right button not held gave a value of {1}", curModId, valuetoAdd);
                irregularFlashesValue.Add(valuetoAdd % 10);
            }

            if (irregularFlashesValue.Count() == 1)
            {// One value used
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Exactly 1 value was used which is {1}", curModId, irregularFlashesValue[0]);
                return secondsbombTimer.Contains(irregularFlashesValue[0].ToString());
            }
            else if (irregularFlashesValue.Count() == 2)
            {// Two values used
                // One of the value is a 0
                if (irregularFlashesValue[0] == 0 && irregularFlashesValue[1] != 0)
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: 2 values were used; one of them was a 0, the other is {1}", curModId, irregularFlashesValue[1]);
                    return !secondsDisplayTimer.Contains(irregularFlashesValue[1].ToString());
                }
                if (irregularFlashesValue[0] != 0 && irregularFlashesValue[1] == 0)
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: 2 values were used; one of them was a 0, the other is {1}", curModId, irregularFlashesValue[0]);
                    return !secondsDisplayTimer.Contains(irregularFlashesValue[1].ToString());
                }
                // Both are identical and NOT 0
                if (irregularFlashesValue[0] == irregularFlashesValue[1] && irregularFlashesValue[0] != 0)
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: 2 values were used; both are identical and not 0, value is {1}", curModId, irregularFlashesValue[0]);
                    return !secondsbombTimer.Contains(irregularFlashesValue[0].ToString());
                }
                // Both are 0
                if (0 == irregularFlashesValue[1] && irregularFlashesValue[0] == 0)
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: 2 values were used; both are 0", curModId);
                    return secondsbombTimer.Equals(((GetValueofBase36Digit(inconsistMorseLetterL.ToCharArray()[0]) + GetValueofBase36Digit(inconsistMorseLetterR.ToCharArray()[0]))%60).ToString("00"));
                }
                int productValues = irregularFlashesValue[0] * irregularFlashesValue[1];
                // 0 or 2 buttons flashed a vowel (A,E,I,O,U)
                if (("AEIOU".Contains(inconsistMorseLetterL) && "AEIOU".Contains(inconsistMorseLetterR)) || !("AEIOU".Contains(inconsistMorseLetterL) || "AEIOU".Contains(inconsistMorseLetterR)))
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: 2 values were used; 0 or 2 buttons flashed the following characters: A,E,I,O,U", curModId);
                    if (productValues < 60)
                    {
                        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The given product is {1} which is possible", curModId, productValues);
                        return secondsbombTimer.Equals(productValues.ToString("00"));
                    }
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The given product is not possible to reach on the seconds bomb timer", curModId);
                }
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: 2 values were used; Otherwise condition", curModId);
                if (productValues < 60)
                {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The given product is {1} which is possible", curModId, productValues);
                    return secondsDisplayTimer.Equals(productValues.ToString("00"));
                }
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The given product is not possible to reach on the seconds display timer", curModId);
            }
        }
        // Last Otherwise Condition
        int[,] valuesList = new int[,] { { 1, 2, 3 }, { 2, 3, 1 }, { 3, 1, 2 } };
        int valueA = 0;
        int valueB = 0;
        foreach (string leftColor in leftHoldColors)
        {
            valueA += valuesList[leftmostIdxFlashable, primaryColorList.ToList().IndexOf(leftColor)];
        }
        foreach (string rightColor in rightHoldColors)
        {
            valueB += valuesList[rightmostIdxFlashable, primaryColorList.ToList().IndexOf(rightColor)];
        }
        valueA %= 5;
        valueB %= 5;

        Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Last Otherwise Condition, Values are {1},{2}", curModId, valueA, valueB);
        if (valueA == valueB)
        {
            if (timeRemainingBomb < 5) { Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The time remaining in seconds is less than 5", curModId, timeRemainingBomb); return true; }
            if (timeRemainingBomb % 2 == 0) {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The time remaining in seconds: {1} is not considered a prime number", curModId, timeRemainingBomb);
                return false;
            }
            for (int x = 3; x <= Math.Floor(Math.Sqrt(timeRemainingBomb));x+=2)
            {
                if (timeRemainingBomb % x == 0) {
                    Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The time remaining in seconds: {1} is not considered a prime number", curModId, timeRemainingBomb);
                    return false;
                }
            }
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The time remaining in seconds: {1} is considered a prime number", curModId,timeRemainingBomb);
            return true;
        }
        bool isDivisiblebyValues = true;
        if (valueA == 0 || valueB == 0) {

            isDivisiblebyValues = isDivisiblebyValues && timeRemainingBomb % 2 == 0;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The time remaining in seconds is{2} divisible by 2", curModId, timeRemainingBomb, timeRemainingBomb % 2 != 0 ? " not" : "");
        }
        if (valueA == 1 || valueB == 1)
        {
            isDivisiblebyValues = isDivisiblebyValues && timeRemainingBomb % 3 == 0;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The time remaining in seconds is{2} divisible by 3", curModId, timeRemainingBomb, timeRemainingBomb % 3 != 0 ? " not" : "");
        }
        if (valueA == 2 || valueB == 2)
        {
            isDivisiblebyValues = isDivisiblebyValues && timeRemainingBomb % 5 == 0;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The time remaining in seconds is{2} divisible by 5", curModId, timeRemainingBomb, timeRemainingBomb % 5 != 0 ? " not" : "");
        }
        if (valueA == 3 || valueB == 3)
        {
            isDivisiblebyValues = isDivisiblebyValues && timeRemainingBomb % 7 == 0;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The time remaining in seconds is{2} divisible by 7", curModId, timeRemainingBomb, timeRemainingBomb % 7 != 0 ? " not" : "");
        }
        if (valueA == 4 || valueB == 4)
        {
            isDivisiblebyValues = isDivisiblebyValues && timeRemainingBomb % 11 == 0;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The time remaining in seconds is{2} divisible by 11", curModId, timeRemainingBomb, timeRemainingBomb % 11 != 0 ? " not" : "");
        }
        return isDivisiblebyValues;
    }
    bool hasStruck = false;
    bool?[] conditionsMet = new bool?[3];
    void HandleRelease(int buttonIdx)
    {
        interactable = false;
        bool isAllCorrect = true;
        bool withinRange = false;
        bool correctButtonInt = false;
        bool isOnReleaseCorrect = true;
        bool isInteractedCorrectly = false;
        hasStruck = false;
        StopCoroutine(UpdateDisplay(currentStage));
        if (currentStage == 1 && !stagesCompleted[0])
        {
            if (info.GetTime() < 121 && holdCorStg1 == true && isHeld == false)
            {
                hasStruck = true;
                modSelf.HandleStrike();
                isInteractedCorrectly = true;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Exchanged a strike for tapping the button versus holding the button at 120 or fewer seconds remaining", curModId);
            }
            else
            {
                isInteractedCorrectly = holdCorStg1 == isHeld;
                isAllCorrect = isAllCorrect && isInteractedCorrectly;
            }
            correctButtonInt = buttonIdx == crtBtnIdxStg1;
            isAllCorrect = isAllCorrect && correctButtonInt;
            for (int x = 0; x < possibleTimesA.Count; x++)
            {
                withinRange = withinRange || Math.Abs(bTimeOnHold - possibleTimesA[x]) <= 5;
            }
            isAllCorrect = isAllCorrect && withinRange;
            
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The {1} button was {2} at {3}:{4} for stage 1", curModId, buttonPos[buttonIdx], !isHeld ? "TAPPED" : "HELD", (bTimeOnHold / 60).ToString("00"), (bTimeOnHold % 60).ToString("00"));
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: ({1} seconds left on the bomb)", curModId, bTimeOnHold);
        }
        else if (currentStage == 2 && !stagesCompleted[1])
        {
            if (info.GetTime() < 121 && holdCorStg2 == true && isHeld == false)
            {
                hasStruck = true;
                modSelf.HandleStrike();
                isInteractedCorrectly = true;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Exchanged a strike for tapping the button versus holding the button at 120 or fewer seconds remaining", curModId);
            }
            else
            {
                isInteractedCorrectly = holdCorStg2 == isHeld;
                isAllCorrect = isAllCorrect && isInteractedCorrectly;
            }
            correctButtonInt = buttonIdx == crtBtnIdxStg2;
            isAllCorrect = isAllCorrect && correctButtonInt;
            for (int x = 0; x < possibleTimesB.Count; x++)
            {
                withinRange = withinRange || Math.Abs(bTimeOnHold - possibleTimesB[x]) <= 5;
            }
            isAllCorrect = isAllCorrect && withinRange;
            
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The {1} button was {2} at {3}:{4} for stage 2", curModId, buttonPos[buttonIdx], !isHeld ? "TAPPED" : "HELD", (bTimeOnHold / 60).ToString("00"), (bTimeOnHold % 60).ToString("00"));
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: ({1} seconds left on the bomb)", curModId, bTimeOnHold);
            
        }
        if (isHeld)
        {
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: And was released at {3}:{4} ({1}:{2} on display)", curModId, (timeHeldSec / 60).ToString("00"), (timeHeldSec % 60).ToString("00"), (bTimeEndHold / 60).ToString("00"), (bTimeEndHold % 60).ToString("00"));
            isOnReleaseCorrect = IsCorrectOnHold();
            isAllCorrect = isAllCorrect && isOnReleaseCorrect;
            Debug.LogFormat("[Bamboozling Time Keeper #{0}]: The release condition was handled {1}", curModId, isOnReleaseCorrect ? "CORRECTLY" : "INCORRECTLY");
        }
        conditionsMet[0] = correctButtonInt;
        conditionsMet[1] = withinRange;
        conditionsMet[2] = isInteractedCorrectly && isOnReleaseCorrect;
        // Check if the correct button is interated within the given time ranges, held/tapped correctly, and the following:
        // Stage 1 is not finished yet and the module is on stage 1.
        // Stage 2 is not finished yet and the module is on stage 2.
        if (!isAllCorrect && ((currentStage == 1 && !stagesCompleted[0]) || (currentStage == 2 && !stagesCompleted[1]) ))
        {
            if (!hasStruck)
                modSelf.HandleStrike();

            StartCoroutine(PlayIncorrectAnim());
        }
        else
        {
            if (currentStage == 1 && !stagesCompleted[0])
            {
                if (specialDay)
                {
                    sound.PlaySoundAtTransform(specialSoundCorrect[UnityEngine.Random.Range(0, specialSoundCorrect.Length)], transform);
                }
                sound.PlaySoundAtTransform("InputCorrect", transform);
                stagesCompleted[0] = true;
                highlightStage1.GetComponent<MeshRenderer>().material.color = stagesCompleted[0] ? indcColors[2] : indcColors[0];
            }
            else if (currentStage == 1) {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Stage 1 is already completed. I'm not going to strike you for interacting with it again correctly/incorrectly. - VFlyer", curModId);
            }
            else if (currentStage == 2 && !stagesCompleted[1])
            {
                if (specialDay)
                {
                    sound.PlaySoundAtTransform(specialSoundCorrect[UnityEngine.Random.Range(0, specialSoundCorrect.Length)], transform);
                }
                sound.PlaySoundAtTransform("InputCorrect", transform);
                stagesCompleted[1] = true;
                highlightStage2.GetComponent<MeshRenderer>().material.color = stagesCompleted[1] ? indcColors[2] : indcColors[0];
            }
            else if (currentStage == 2)
            {
                Debug.LogFormat("[Bamboozling Time Keeper #{0}]: Stage 2 is already completed. I'm not going to strike you for interacting with it again correctly/incorrectly. - VFlyer", curModId);
            }
            if (stagesCompleted.ToList().TrueForAll(a => a))
            {
                StartCoroutine(PlaySolveAnim());
                return;
            }
            interactable = true;
            UpdateButtons(currentStage);
            curbtnHeld = -1;
            StartCoroutine(UpdateDisplay(currentStage));

        }
    }
    readonly string[] forceSolveMessages = new string[] { "FORCE SOLVED", "SOLVE FORCED", "NOTHING? REALLY", "REALLY NOTHING?" };
    private IEnumerator PlaySolveAnim()
    {
        interactable = false;
        GameObject[] GameOBJArray = new GameObject[] { buttonL, buttonM, buttonR };
        string buttonChars = specialDay ? "YEA" : "WOW";
        for (int x = 0; x < GameOBJArray.Length; x++)
        {
            TextMesh curText = GameOBJArray[x].GetComponentInChildren<TextMesh>();
            MeshRenderer curMeshRdr = GameOBJArray[x].GetComponent<MeshRenderer>();
            curText.text = buttonChars.Substring(x,1);
            curText.color = Color.black;
            curMeshRdr.material = materialList[1];
            curMeshRdr.material.color = Color.green;
        }

        display.text = forcedSolve ? forceSolveMessages[UnityEngine.Random.Range(0,forceSolveMessages.Length)] : specialDay ? disarmSpecialPhrases[UnityEngine.Random.Range(0, disarmSpecialPhrases.Length)] : disarmPhrases[UnityEngine.Random.Range(0, disarmPhrases.Length)].ToUpper();
        display.color = Color.white;
        backing.GetComponent<MeshRenderer>().material.color = Color.black;
        modSelf.HandlePass();
        yield return new WaitForSeconds(1);
        StartCoroutine(HideButtons());
        StartCoroutine(TypeText(display.text, true));
        yield return null;
    }

    private IEnumerator PlayIncorrectAnim()
    {
        display.text = specialDay ? specialDayStrikePhrases[UnityEngine.Random.Range(0,specialDayStrikePhrases.Length)] : strikePhrases[UnityEngine.Random.Range(0,strikePhrases.Length)].ToUpper();
        display.color = Color.white;
        if (specialDay)
        {
            sound.PlaySoundAtTransform(specialSoundStrikes[UnityEngine.Random.Range(0,specialSoundStrikes.Length)], transform);
        }
        GameObject[] GameOBJArray = new GameObject[] { buttonL, buttonM, buttonR };
        for (int x = 0; x < GameOBJArray.Length; x++)
        {
            TextMesh curText = GameOBJArray[x].GetComponentInChildren<TextMesh>();
            MeshRenderer curMeshRdr = GameOBJArray[x].GetComponent<MeshRenderer>();
            curMeshRdr.material = materialList[1];
            curText.text = "";
            curMeshRdr.material.color = (bool)conditionsMet[x] ? Color.green : Color.red;
        }
        yield return new WaitForSeconds(3);
        StartCoroutine(ChangeToStage(currentStage));
        yield return null;
    }

    private IEnumerator TypeText(string message,bool inReverse)
    {
        display.color = Color.white;
        backing.GetComponent<MeshRenderer>().material.color = Color.black;
        if (inReverse)
        {
            for (int x = message.Length; x > 0; x--)
            {
                display.text = message.Substring(0, x);
                sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.TypewriterKey, transform);
                yield return new WaitForSeconds(0.05f);
            }
            display.text = "";
        }
        else
        {
            for (int x = 0; x < message.Length; x++)
            {
                display.text = message.Substring(0, x);
                sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.TypewriterKey, transform);
                yield return new WaitForSeconds(0.05f);
            }
            display.text = message;
        }
        yield return null;
    }
    private readonly int AnimTime = 20;
    IEnumerator HideButtons()
    {
        Vector3 localDoorPt1 = animationPointDoorA.transform.localPosition;
        Vector3 localDoorPt2 = animationPointDoorB.transform.localPosition;

        Vector3 buttonGroupPt1 = animationPointButtonA.transform.localPosition;
        Vector3 buttonGroupPt2 = animationPointButtonB.transform.localPosition;

        sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.WireSequenceMechanism, transform);
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
    bool isDisplayRunning = false;
    IEnumerator UpdateDisplay(int stage)
    {
        if (isDisplayRunning) yield break;
        isDisplayRunning = true;
        currentPart = -1;
        while (!interactable)
        {
            yield return new WaitForSeconds(0);
        }
        MeshRenderer backingRenderer = backing.GetComponent<MeshRenderer>();
        while (currentStage == stage && curbtnHeld == -1 && interactable)
        {
            currentPart++;
            if (stage == 1)
            {
                if (currentPart < stage1Phrases.Count() && currentPart >= 0)
                {
                    display.text = stage1Phrases[currentPart];
                    display.color = colorList[colorString.IndexOf(stage1PhrClr[currentPart])];
                    if (display.color.Equals(Color.black))
                    {
                        backingRenderer.material.color = Color.white;
                    }
                    else
                    {
                        backingRenderer.material.color = Color.black;
                    }
                    if (colorBlindActive && !startingPhrases.Contains(display.text) && !display.text.Equals("HUNDRED") && !display.text.Equals("POINT ZERO"))
                    {
                        for (int x = 0; x < 3; x++)
                        {
                            yield return new WaitForSeconds(0.25f);
                            if (currentStage != stage || curbtnHeld != -1 || !interactable) break;
                        }
                        display.text = "IN " + stage1PhrClr[currentPart].ToUpper();
                        display.color = Color.white;
                        backingRenderer.material.color = Color.black;
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
                    if (display.color.Equals(Color.black))
                    {
                        backingRenderer.material.color = Color.white;
                    }
                    else
                    {
                        backingRenderer.material.color = Color.black;
                    }
                    if (colorBlindActive && !startingPhrases.Contains(display.text) && !display.text.Equals("HUNDRED") && !display.text.Equals("POINT ZERO"))
                    {
                        for (int x = 0; x < 3; x++)
                        {
                            yield return new WaitForSeconds(0.25f);
                            if (currentStage != stage || curbtnHeld != -1 || !interactable) break;
                        }
                        display.text = "IN " + stage2PhrClr[currentPart].ToUpper();
                        display.color = Color.white;
                        backingRenderer.material.color = Color.black;
                    }
                }
                else
                {
                    display.text = "";
                    display.color = Color.white;
                    currentPart = -1;
                }
            }
            for (int x = 0; x < 3; x++)
            {
                yield return new WaitForSeconds(0.25f);
                if (currentStage != stage || curbtnHeld != -1 || !interactable) break;
            }
        }
        isDisplayRunning = false;
    }
    readonly string[] colorblindInit = new string[] {"R", "Y", "G", "C", "B", "M", "W", "K" };
    void UpdateButtons(int stageNum)
    {
        // Debugging only, used for checking if something went wrong
        //print(FormatDebugList(stage1ButtonColors));
        //print(FormatDebugList(stage2ButtonColors));
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
            curMeshRdr.material = materialList[1];
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
            display.text = currentStage != stageNum ? "SWITCHING" : "REFRESHING";
            display.color = Color.white;
            backing.GetComponent<MeshRenderer>().material.color = Color.black;
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
            buttonL.transform.localPosition += Vector3.down / 200f;
        }
        else if (curbtnHeld == 1)
        {
            buttonM.transform.localPosition += Vector3.down / 200f;
        }
        else if (curbtnHeld == 2)
        {
            buttonR.transform.localPosition += Vector3.down / 200f;
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

    IEnumerator HandleSounds()
    {
        yield return new WaitForSeconds(0);
        countSoundsPlayed = 0;
        yield return null;
    }
    // BamTimeKeeper Settings
    class BamTimeKeeperSettings
    {
        public bool OneTapHolds = false;
    }

    static Dictionary<string, object>[] TweaksEditorSettings = new Dictionary<string, object>[]
    {
        new Dictionary<string, object>
        {
            { "Filename", "BamTimeKeeperSettings.json" },
            { "Name", "Bamboozling Time Keeper Settings" },
            { "Listing", new List<Dictionary<string, object>>{
                new Dictionary<string, object>
                {
                    { "Key", "OneTapHolds" },
                    { "Text", "Allows holding the button by just clicking on the button instead." }
                }
            } }
        }
    };

    // Begin TP Handler
#pragma warning disable IDE0044 // Add readonly modifier
    bool TwitchPlaysActive;
    bool ZenModeActive;
    bool TwitchPlaysSkipTimeAllowed = true; // Enforce skipping time on the module when needed
    readonly string TwitchHelpMessage = "To interact a given button at a specific time: \"!{0} hold/tap l[eft]/m[iddle]/r[ight] at ##:##\" Time format is MM:SS with MM being able to exceed 99 min, multiple time stamps are acceptable when releasing.\n" +
        "To release a button at a specific time based on the display or bomb timer: \"!{0} release display/bombtime at ##:##\" To release a button based on the seconds timer: \"!{0} release display/bombtime at ## ##\"\n" +
        "To get the current time on the display: \"!{0} display time\" To switch between stages: \"!{0} toggle/switch\"\n" +
        "To activate colorblind mode: \"!{0} colorblind\" You can only activate colorblind mode or switch stages if you are NOT holding a button!";
#pragma warning restore IDE0044 // End Adding readonly modifier
    void TwitchHandleForcedSolve()
    {// Handle force solving on the module.
        forcedSolve = true;
        if (curbtnHeld != -1)
        {
            curbtnHeld = -1;
            sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonRelease,transform);
        }
        StopAllCoroutines();
        StartCoroutine(PlaySolveAnim());
        stagesCompleted[0] = true;
        stagesCompleted[1] = true;
        highlightStage1.GetComponent<MeshRenderer>().material.color = indcColors[2];
        highlightStage2.GetComponent<MeshRenderer>().material.color = indcColors[2];
        modSelf.HandlePass();
    }
    readonly List<string> buttonPosInit = new List<string>() { "l", "m", "r" };
    private IEnumerator ProcessTwitchCommand(string input)
    {
        string curinput = input.ToLowerInvariant();
        string rgxStartHold = @"^(tap|hold) (l(eft)?|m(iddle)?|r(ight)?) (at|on) [0-9]+:[0-5][0-9]$";
        string rgxEndHoldSpecific = @"^release (display|bombtime)( at| on)?( [0-9]+:[0-5][0-9])+$";// End the Hold at a specific time
        string rgxEndHoldGeneral = @"^release (display|bombtime)( at| on)?( [0-5]?[0-9])+$";// End the Hold when the seconds digits show those digits
        string rgxGetDisplayTime = @"^display time$";
        List<long> possibleReleaseTimes = new List<long>();
        var music = false;
        long timeToSkipTo;
        if (!started) yield break;
        if (curinput.Equals("colorblind"))
        {
            if (curbtnHeld != -1)
            {
                yield return "sendtochaterror The module is holding a button! Release it by using the \"release\" command on this module. Colorblind cannot be enabled until the button is released.";
                yield break;
            }
            if (interactable)
            {
                if (colorBlindActive)
                {
                    yield return "sendtochaterror Colorblind mode is already enabled! You can't disable it!";
                    yield break;
                }
                colorBlindActive = true;
                Debug.LogFormat("[Bamboozling Time Keeper #{0}] Colorblind mode enabled via TP command.", curModId);
                StartCoroutine(ChangeToStage(currentStage));
                yield return null;
                yield break;
            }
        }
        else if (curinput.EqualsIgnoreCase("switch") || curinput.EqualsIgnoreCase("toggle"))
        {
            if (curbtnHeld != -1)
            {
                yield return "sendtochaterror The module is holding a button! Release it by using the \"release\" command on this module.";
                yield break;
            }
            yield return null;
            if (currentStage == 1)
                stageSelectables[1].OnInteract();
            else
                stageSelectables[0].OnInteract();
            yield break;
        }
        else if (curinput.RegexMatch(rgxGetDisplayTime))
        {
            if (curbtnHeld == -1)
            {
                yield return "sendtochaterror The module is not holding a button! Hold the button by using the \"hold\" command on this module first.";
                yield break;
            }
            while (!isHeld) yield return new WaitForSeconds(0);
            yield return "sendtochat The display showed at the time this command was invoked \"" + (timeHeldSec / 60).ToString("00") + ":" + (timeHeldSec % 60).ToString("00") + "\"";
            yield break;
        }
        else if (curinput.RegexMatch(rgxEndHoldSpecific))// End the hold at a specific time stamps
        {
            if (curbtnHeld == -1)
            {
                yield return "sendtochaterror The module is not holding a button! Hold the button by using the \"hold\" command on this module first.";
                yield break;
            }
            while (!isHeld) yield return new WaitForSeconds(0);
            var split = input.ToLowerInvariant().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            int posCurrent = split.Length - 1;
            while (split[posCurrent].RegexMatch(@"^[0-9]+:[0-5][0-9]$"))
            {
                var timeLocal = split[posCurrent].ToLowerInvariant().Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                long secondsLocal = 60 * int.Parse(timeLocal[0]) + int.Parse(timeLocal[1]);
                possibleReleaseTimes.Add(secondsLocal);
                posCurrent--;
            }

            if (split[1].Equals("bombtime"))
            {
                if (!zenModeDetected)
                {
                    for (int x = 0; x < possibleReleaseTimes.Count; x++)
                        if (Mathf.FloorToInt(info.GetTime()) < possibleReleaseTimes[x])
                        {
                            possibleReleaseTimes.RemoveAt(x);
                            x--;
                        }
                    possibleReleaseTimes.Sort();
                }
                else
                {
                    for (int x = 0; x < possibleReleaseTimes.Count; x++)
                        if (Mathf.FloorToInt(info.GetTime()) > possibleReleaseTimes[x])
                        {
                            possibleReleaseTimes.RemoveAt(x);
                            x--;
                        }
                    possibleReleaseTimes.Sort();
                    possibleReleaseTimes.Reverse();
                }
                if (possibleReleaseTimes.Count == 0)
                {
                    yield return "sendtochaterror Sorry but the specified time(s) cannot be reached.";
                    yield break;
                }
                yield return "strike";
                yield return "sendtochat Next bomb time to release: " + (possibleReleaseTimes[0] / 60).ToString("00") + ":" + (possibleReleaseTimes[0] % 60).ToString("00");
                if (zenModeDetected)
                {
                    timeToSkipTo = possibleReleaseTimes[0] - 5;
                    if (possibleReleaseTimes[0] - info.GetTime() > 15) yield return "skiptime " + timeToSkipTo;
                }
                else
                {
                    timeToSkipTo = possibleReleaseTimes[0] + 5;
                    if (info.GetTime() - possibleReleaseTimes[0] > 15) yield return "skiptime " + timeToSkipTo;
                }
                music = Math.Abs(info.GetTime() - possibleReleaseTimes[0]) > 30;
                if (music) yield return "waiting music";
                do
                {
                    if ((Mathf.FloorToInt(info.GetTime()) < possibleReleaseTimes[0] && !zenModeDetected) || (Mathf.FloorToInt(info.GetTime()) > possibleReleaseTimes[0] && zenModeDetected))
                    {
                        yield return "sendtochaterror Sorry, but a sudden change to the timer caused the interaction with the button to cancel automatically.";
                        yield return "cancelled";
                        yield break;
                    }
                    yield return "trycancel The button that was going to be interacted got canceled.";
                } while (Mathf.FloorToInt(info.GetTime()) != possibleReleaseTimes[0]);
                if (music) yield return "end waiting music";
            }
            else if (split[1].Equals("display"))
            {

                for (int x = 0; x < possibleReleaseTimes.Count; x++)
                    if (timeHeldSec > possibleReleaseTimes[x])
                    {
                        possibleReleaseTimes.RemoveAt(x);
                        x--;
                    }


                if (possibleReleaseTimes.Count == 0)
                {
                    yield return "sendtochaterror Sorry but the specified time(s) cannot be reached.";
                    yield break;
                }
                possibleReleaseTimes.Sort();
                yield return "strike";
                yield return "sendtochat Next display time to release: " + (possibleReleaseTimes[0] / 60).ToString("00") + ":" + (possibleReleaseTimes[0] % 60).ToString("00");
                music = Math.Abs(timeHeldSec - possibleReleaseTimes[0]) > 30;
                if (music) yield return "waiting music";
                do
                {
                    yield return "trycancel The button that was going to be interacted got canceled.";
                }
                while (timeHeldSec != possibleReleaseTimes[0]);
                if (music) yield return "end waiting music";
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield return "sendtochaterror Sorry but which timer does the command need to refer to?";
                yield break;
            }
            yield return "solve";
            buttonsSelectable[curbtnHeld].OnInteract();
        }
        else if (curinput.RegexMatch(rgxEndHoldGeneral))
        {
            if (curbtnHeld == -1)
            {
                yield return "sendtochaterror The module is not holding a button! Hold the button by using the \"hold\" command on this module first.";
                yield break;
            }
            while (!isHeld) yield return new WaitForSeconds(0);
            var split = input.ToLowerInvariant().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            int posCurrent = split.Length - 1;

            long curDisplayTimeMin = timeHeldSec / 60;
            long curBombTimeMin = (long)info.GetTime() / 60;

            if (split[1].Equals("bombtime"))
            {
                while (split[posCurrent].RegexMatch(@"^[0-5]?[0-9]$"))
                {
                    var timeLocal = split[posCurrent];
                    for (long x = -1; x <= 1; x++)
                    {
                        long secondsLocal = (60 * (curBombTimeMin+x)) + int.Parse(timeLocal);
                        if (secondsLocal >= 0)
                            possibleReleaseTimes.Add(secondsLocal);
                    }
                    posCurrent--;
                }

                if (!zenModeDetected)
                {
                    for (int x = 0; x < possibleReleaseTimes.Count; x++)
                        if (Mathf.FloorToInt(info.GetTime()) < possibleReleaseTimes[x])
                        {
                            possibleReleaseTimes.RemoveAt(x);
                            x--;
                        }
                    possibleReleaseTimes.Sort();
                    possibleReleaseTimes.Reverse();
                }
                else
                {
                    for (int x=0;x<possibleReleaseTimes.Count;x++)
                        if (Mathf.FloorToInt(info.GetTime()) > possibleReleaseTimes[x])
                        {
                            possibleReleaseTimes.RemoveAt(x);
                            x--;
                        }
                    possibleReleaseTimes.Sort();
                }
                if (possibleReleaseTimes.Count == 0)
                {
                    yield return "sendtochaterror Sorry but the specified time(s) cannot be reached.";
                    yield break;
                }
                yield return "strike";
                yield return "sendtochat Next bomb time to release: " + (possibleReleaseTimes[0] / 60).ToString("00") + ":" + (possibleReleaseTimes[0] % 60).ToString("00");
                if (zenModeDetected)
                {
                    timeToSkipTo = possibleReleaseTimes[0] - 5;
                    if (possibleReleaseTimes[0] - info.GetTime() > 15) yield return "skiptime " + timeToSkipTo;
                }
                else
                {
                    timeToSkipTo = possibleReleaseTimes[0] + 5;
                    if (info.GetTime() - possibleReleaseTimes[0] > 15) yield return "skiptime " + timeToSkipTo;
                }
                music = Math.Abs(info.GetTime() - possibleReleaseTimes[0]) > 30;
                if (music) yield return "waiting music";
                do
                {
                    if ((Mathf.FloorToInt(info.GetTime()) < possibleReleaseTimes[0] && !zenModeDetected) || (Mathf.FloorToInt(info.GetTime()) > possibleReleaseTimes[0] && zenModeDetected))
                    {
                        yield return "sendtochaterror Sorry, but a sudden change to the bomb time caused the interaction with the button to cancel automatically.";
                        yield return "cancelled";
                        yield break;
                    }
                    yield return "trycancel The button that was going to be interacted got canceled.";
                } while (Mathf.FloorToInt(info.GetTime()) != possibleReleaseTimes[0]);
                if (music) yield return "end waiting music";
            }
            else if (split[1].Equals("display"))
            {
                while (split[posCurrent].RegexMatch(@"^[0-5]?[0-9]$"))
                {
                    var timeLocal = split[posCurrent];
                    for (long x = 0; x <= 1; x++)
                    {
                        long secondsLocal = (60 * (curDisplayTimeMin + x)) + int.Parse(timeLocal);
                        possibleReleaseTimes.Add(secondsLocal);
                    }
                    posCurrent--;
                }
                for (int x = 0; x < possibleReleaseTimes.Count; x++)
                    if (timeHeldSec > possibleReleaseTimes[x])
                    {
                        possibleReleaseTimes.RemoveAt(x);
                        x--;
                    }

                if (possibleReleaseTimes.Count == 0)
                {
                    yield return "sendtochaterror Sorry but the specified time(s) cannot be reached.";
                    yield break;
                }
                possibleReleaseTimes.Sort();
                yield return "strike";
                yield return "sendtochat Next display time to release: " + (possibleReleaseTimes[0] / 60).ToString("00") + ":" + (possibleReleaseTimes[0] % 60).ToString("00");
                music = Math.Abs(timeHeldSec - possibleReleaseTimes[0]) > 30;
                if (music) yield return "waiting music";
                do
                {
                    yield return "trycancel The button that was going to be interacted got canceled.";
                }
                while (timeHeldSec != possibleReleaseTimes[0]);
                if (music) yield return "end waiting music";
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield return "sendtochaterror Sorry but which timer does the command need to refer to?";
                yield break;
            }
            yield return "solve";
            buttonsSelectable[curbtnHeld].OnInteract();
        }
        else if (curinput.RegexMatch(rgxStartHold))
        {
            if (curbtnHeld != -1)
            {
                yield return "sendtochaterror The module is holding a button! Release it by using the \"release\" command on this module.";
                yield break;
            }
            var split = input.ToLowerInvariant().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var time = split[3].ToLowerInvariant().Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

            int buttonSpecified = buttonPosInit.IndexOf(split[1]) != -1 ? buttonPosInit.IndexOf(split[1]) : buttonPos.ToList().IndexOf(split[1]);
            long seconds = 60 * int.Parse(time[0]) + int.Parse(time[1]);
            if (buttonSpecified < 0 || buttonSpecified >= 3)
            {
                yield return "sendtochaterror Sorry but what button is \"" + split[1] + "?\"";
                yield break;
            }
            if (!zenModeDetected)
            {
                if (Mathf.FloorToInt(info.GetTime()) < seconds)
                {
                    yield return "sendtochaterror Sorry but the specified time is not possible.";
                    yield break;
                }
            }
            else
            {
                if (Mathf.FloorToInt(info.GetTime()) > seconds)
                {
                    yield return "sendtochaterror Sorry but the specified time is not possible in Zen Mode.";
                    yield break;
                }
            }
            if (zenModeDetected)
            {
                timeToSkipTo = seconds - 5;
                if (seconds - info.GetTime() > 15) yield return "skiptime " + timeToSkipTo;
                if (seconds - info.GetTime() > 30) music = true;
            }
            else
            {
                timeToSkipTo = seconds + 5;
                if (info.GetTime() - seconds > 15) yield return "skiptime " + timeToSkipTo;
                if (info.GetTime() - seconds > 30) music = true;
            }
            yield return null;
            if (music) yield return "waiting music";
            do
            {
                if ((Mathf.FloorToInt(info.GetTime()) < seconds && !zenModeDetected) || (Mathf.FloorToInt(info.GetTime()) > seconds && zenModeDetected))
                {
                    yield return "sendtochaterror A sudden change to the timer caused the interaction with the button to cancel automatically.";
                    yield return "cancelled";
                    yield break;
                }
                yield return "trycancel The button that was going to be interacted got canceled.";
            } while (Mathf.FloorToInt(info.GetTime()) != seconds);
            if (music) yield return "end waiting music";
            buttonsSelectable[buttonSpecified].OnInteract();
            if (split[0].EqualsIgnoreCase("tap"))
                buttonsSelectable[buttonSpecified].OnInteract();
        }
    }

}
