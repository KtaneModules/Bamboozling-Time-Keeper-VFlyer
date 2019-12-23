using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using Random = UnityEngine.Random;

public class BamTimeKeeperHandlerFallBack : MonoBehaviour {

    public KMBombInfo info;
    public KMColorblindMode colorblindMode;
    public KMAudio sound;
    public List<KMSelectable> buttons;
    public GameObject door;
    public TextMesh[] displays;
    public Renderer[] brends;
    public Material[] buttonColors = new Material[8];
    public Material[] indicatorStatus = new Material[3];

    private string[][] vanilla = new string[2][] { new string[6] { "Serial", "Parallel", "DVI", "PS2", "RJ45", "StereoRCA" }, new string[11] { "SND", "CLR", "CAR", "IND", "FRQ", "SIG", "NSA", "MSA", "TRN", "BOB", "FRK" } };
    private string[][] namecheck = new string[][] { new string[] { "Affine Cycle", "Bamboozled Again", "Bamboozling Button", "Bamboozling Button Grid", "Bamboozling Time Keeper", "Bordered Keys", "Caesar Cycle", "Cryptic Cycle", "Disordered Keys", "Double Arrows", "Faulty RGB Maze", "Forget Me Later", "Hill Cycle", "Jumble Cycle", "Misordered Keys", "Ordered Keys", "Pigpen Cycle", "Playfair Cycle", "Recorded Keys", "Reordered Keys", "RGB Maze", "Simon Stores", "Tallordered Keys", "Ultimate Cycle", "UltraStores", "Unordered Keys", "The Very Annoying Button", "The Task Master", "Electric Bamboozaloo", "Hyper Bamboozler", "Telegraphic Cycle", "14", "The Very Annoying Button Grid", "Simon Supervises", "Simon Superintends", "Bad Mouth", "Bad TV"}, //Speakingevil modules
                                                    new string[] { "3D Tunnels", "Algebra", "Challenge & Contact", "Color Decoding", "English Test", "Find The Date", "Flavor Text EX", "Guitar Chords", "Ice Cream", "Kanji", "Krazy Talk", "Logical Buttons", "The London Underground", "Modern Cipher", "Module Movements", "Monsplode Trading Cards", "Number Nimbleness", "Orange Arrows", "Ordered Keys", "Partial Derivatives", "Passport Control", "Poetry", "Prime Checker", "The Screw", "Sea Shells", "Simon Samples", "Simon Screams", "Simon Selects", "Simon Sings", "Simon's On First", "Simon Stops", "Simon Stores", "Sonic The Hedgehog", "Symbolic Coordinates", "Third Base", "Timing Is Everything", "UltraStores", "Wavetapping", "Who's On First", "Who's On First Translated", "Zoni", "Electric Bamboozaloo"}, //3 stage modules
                                                    new string[] { "Blackjack", "Command Prompt", "Connection Device", "Flip The Coin", "Four-Card Monte", "Hold Ups", "Micro-Modules", "Module Homework", "The Radio", "Scripting"}, //Kritzy modules
                                                    new string[] { }, //RT Controlled modules
                                                    new string[] { } }; //RT Sensitive modules
    private string[] textList = new string[75] {"SOME NUMBERS", "THE NUMBERS", "NUMBERS", "SOME NUMBER(S)", "THE NUMBER(S)", "NUMBER(S)", "SOME NUMBER", "THE NUMBER", "NUMBER",
                                              "TWO NUMBERS", "THREE NUMBERS", "FOUR NUMBERS", "2 NUMBERS", "3 NUMBERS", "4 NUMBERS", "ONE NUMBER", "A NUMBER", "1 NUMBER",
                                              "0",  "1",  "2",  "3",  "4",  "5",  "6",  "7",  "8",  "9",
                                              "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
                                              "20", "30", "40", "50", "60", "70", "80", "90",
                                              "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE",
                                              "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN",
                                              "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY", "HUNDRED"};
    private string[][] operationlist = new string[8][] {new string[56] { "STOP", "+1", "-10", "B1", "B3", "+559", "*3", "B1", "-65", "B1", "*10", "+84", "/4", "+485", "+459", "+45", "*2", "+456", "+45", "B3", "STOP", "+45", "-512", "*5", "*5", "*2", "*2", "B1", "/5", "-1", "+10", "B3", "*4", "-559", "+3", "STOP", "+65", "B2", "/10", "-84", "+4", "-485", "-459", "-45", "/2", "-456", "-45", "+0", "*2", "+330", "+512", "/5", "/5", "/2", "/2", "STOP" },
                                                        new string[56] { "*5", "STOP", "+25", "/2", "B2", "-58", "/3", "+158", "+46", "B1", "/10", "-46", "*3", "+54", "-485", "-90", "+256", "-98", "*5", "B3", "-59", "STOP", "+128", "/5", "*12", "*5", "/2", "B2", "-50", "+1", "-25", "*2", "/4", "+58", "STOP", "B3", "-46", "B2", "*10", "+46", "-3", "-84", "+485", "+90", "-256", "+98", "/5", "-25", "/2", "-330", "-126", "*5", "/12", "/5", "STOP", "B1"},
                                                        new string[56] { "+50", "-1", "STOP", "*2", "B1", "+25", "*6", "B2", "-415", "B1", "*10", "+94", "/3", "+95", "-458", "+30", "-32", "-485", "/9", "B3", "/3", "-89", "STOP", "*10", "-120", "*7", "*2", "B3", "*2", "B3", "+25", "/2", "+4", "STOP", "/3", "-158", "+415", "B2", "/10", "-94", "+3", "-95", "+458", "-30", "+32", "+485", "*9", "+25", "*5", "+90", "/2", "/10", "+120", "STOP", "*2", "B3"},
                                                        new string[56] { "/2", "B2", "-25", "STOP", "B2", "-48", "/6", "-264", "+48", "B1", "/10", "+54", "*4", "+24", "+478", "+120", "+64", "+741", "+45", "B3", "+55", "*7", "*2", "STOP", "-60", "+21", "/2", "B3", "+25", "B2", "-66", "B1", "STOP", "-25", "*3", "B1", "-48", "B2", "*10", "-54", "-4", "-24", "-478", "-120", "-64", "-741", "-45", "-25", "/5", "-90", "*5", "*10", "STOP", "/7", "/2", "B2"},
                                                        new string[56] { "-25", "B1", "+66", "B2", "STOP", "+99", "+9", "*56", "-56", "B1", "*10", "+49", "/2", "-48", "+0", "-150", "-64", "-46", "-5", "B3", "-20", "/5", "/5", "/10", "STOP", "/2", "*2", "B2", "B1", "/3", "+512", "STOP", "-4", "+48", "/6", "+264", "+56", "B2", "/10", "-49", "+2", "+48", "+0", "+150", "+64", "+46", "+5", "+25", "*10", "/3", "/5", "STOP", "+60", "-21", "*2", "B1"},
                                                        new string[56] { "B3", "*3", "-512", "/9", "B3", "STOP", "-6", "/7", "-99", "B1", "/10", "+21", "*2", "-78", "-45", "+15", "+32", "+155", "+46", "B3", "+23", "+48", "*5", "+1", "+180", "STOP", "/2", "B3", "B2", "*3", "STOP", "*9", "*4", "-99", "*6", "/56", "+99", "B2", "*10", "-21", "-2", "+78", "+45", "-15", "-32", "-155", "-46", "-25", "/10", "*3", "STOP", "-1", "-180", "*2", "/2", "B3"},
                                                        new string[56] { "B1", "/3", "+12", "*6", "B1", "-454", "STOP", "B3", "+284", "B1", "-100", "+86", "/6", "-459", "*5", "+15", "-256", "-45", "+75", "B3", "+59", "+89", "/2", "-151", "/5", "/5", "STOP", "B1", "B3", "STOP", "-12", "/6", "/4", "+454", "-9", "*7", "-284", "B2", "+100", "-86", "-6", "+459", "/5", "-15", "+256", "+45", "-75", "+25", "+20", "STOP", "*2", "+151", "*5", "*5", "*2", "B2"},
                                                        new string[56] { "B2", "B3", "/5", "B3", "B2", "*5", "-3", "STOP", "-54", "B1", "-100", "*11", "*6", "-101", "/5", "*15", "/2", "+485", "-87", "B3", "-41", "-78", "+155", "+150", "/12", "/7", "+40", "STOP", "STOP", "B1", "*5", "B2", "+0", "/5", "+6", "B2", "+54", "B2", "+100", "/11", "+6", "+101", "*5", "-15", "*2", "-485", "+87", "+0", "STOP", "/10", "-155", "-150", "*12", "*7", "-40", "B1"} };
    private int[] textlength;
    private List<int>[][] textrandomiser = new List<int>[2][] { new List<int>[2] { new List<int> { }, new List<int> { 6 } }, new List<int>[2] { new List<int> { }, new List<int> { 6 } } };
    private string[] morselist = new string[36]{"###-###-###-###-###---", "#-####-###-###-###---", "#-#-###-###-###---", "#-#-#-###-###---", "#-#-#-#-###---", "#-#-#-#-#---", "###-#-#-#-#---", "###-###-#-#-#---", "###-###-###-#-#---", "###-###-###-###-#---", "#-###---", "###-#-#-#---", "###-#-###-#---", "###-#-#---", "#---", "#-#-###-#---", "###-###-#", "#-#-#-#---", "#-#---", "#-###-###-###---", "###-#-###---", "#-###-#-#---", "###-###---", "###-#---", "###-###-###---", "#-###-###-#---", "###-###-#-###---", "#-###-#---", "#-#-#---", "###---", "#-#-###---", "#-#-#-###---", "#-###-###---", "###-#-#-###---", "###-#-###-###---", "###-###-#-#---"};
    private int[][][] buttonrandomiser = new int[2][][] { new int[2][] { new int[3], new int[3] }, new int[2][] { new int[3], new int[3] } };
    private List<string>[] displaytext = new List<string>[2] { new List<string> { }, new List<string> { } };
    private List<int>[] interVals = new List<int>[2] { new List<int> { 0 }, new List<int> { 0 } };
    private int[] finalVals = new int[2];
    private int[] scalefactor = new int[2];
    private List<int>[] possibleTimes = new List<int>[2] { new List<int> { }, new List<int> { } };
    private int[] correctbutton = new int[2] { -1, -1};
    private bool[] checkanswer = new bool[3];
    private bool[] dtap = new bool[2];
    private bool[] stagecomplete = new bool[2];
    private bool stageswitch;
    private bool pressed;
    private bool pop;
    private int[] taprule = new int[2];
    private int disptime;
    private string formatteddisptime;
    private int[][] buttonsequencer = new int[2][] { new int[5], new int[5] };
    private List<int>[] morseseq = new List<int>[2] { new List<int> { }, new List<int> { } };
    private int stage;

    private static int moduleIDCounter = 1;
    private int moduleID;
    private bool moduleSolved;

    private void Awake()
    {
        moduleID = moduleIDCounter++;
        foreach(KMSelectable button in buttons)
        {
            int b = buttons.IndexOf(button);
            button.OnInteract += delegate () { ButtonOn(b); return false; };
        }
    }

    void Start () {
        sound.PlaySoundAtTransform("KefkaLaugh", transform);
        int time = (int)info.GetTime();
        int[] nameindex = new int[10];
        foreach(string name in info.GetModuleNames())
        {
            if (name.Contains("Forget"))
            {
                if (name == "Forget Me Now")
                    nameindex[0] += 2;
                else if (name == "Forget Them All")
                    nameindex[0] += 5;
                else
                    nameindex[0]++;
            }
            foreach (string thing in namecheck[0])
            {
                if (name == thing)
                {
                    if (name == "Simon Stores")
                        nameindex[1] += 2;
                    else if (name == "UltraStores")
                        nameindex[1] += 5;
                    else if (name == "Bamboozled Again")
                    {
                        nameindex[1]++;
                        nameindex[2]++;
                    }
                    else
                        nameindex[1]++;
                }
            }
            foreach (string thing in namecheck[1])
            {
                if (name == thing)
                {
                    nameindex[3]++;
                }
            }
            foreach (string thing in namecheck[2])
            {
                if (name == thing)
                {
                    nameindex[4]++;
                }
            }
        }
        if (nameindex[0] == 0)
            nameindex[0]++;
        if (nameindex[1] == 0)
            nameindex[1]++;
        if (nameindex[3] == 0)
            nameindex[3]++;
        if (nameindex[4] == 0)
            nameindex[4]++;
        int[] D = new int[6];
        for(int i = 0; i < 6; i++)
        {
           D[i] = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(info.GetSerialNumber()[i]);
        }
        foreach(string port in info.GetPorts())
        {
            bool v = false;
            foreach (string thing in vanilla[0])
            {
                if(port == thing)
                {
                    v = true;
                    break;
                }
            }
            if (!v)
                nameindex[5]++;
        }
        foreach (string ind in info.GetIndicators())
        {
            bool v = false;
            foreach (string thing in vanilla[1])
            {
                if (ind == thing)
                {
                    v = true;
                    break;
                }
            }
            if (!v)
                nameindex[6]++;
            foreach (char thing in ind)
            {
                if ("SPEAKINGVL".Contains(thing.ToString()))
                {
                    nameindex[7]++;
                    break;
                }
            }
        }
        foreach (string thing in namecheck[3])
        {
            if (name == thing)
            {
                nameindex[8]++;
            }
        }
        foreach (string thing in namecheck[4])
        {
            if (name == thing)
            {
                nameindex[9]++;
            }
        }
        textlength = new int[2] { Random.Range(2, 7), Random.Range(2, 7) };
        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 2; i++) //Randomising buttons
            {
                for (int k = 0; k < 3; k++)
                {
                    buttonrandomiser[j][i][k] = Random.Range(0, 8 + 2*i);
                    brends[k].material = buttonColors[buttonrandomiser[0][0][k]];
                    displays[k + 1].text = buttonrandomiser[0][1][k].ToString();
                    displays[k + 1].color = buttonrandomiser[0][0][k] == 7 ? new Color32(0, 255, 0, 255) : new Color32(0, 0, 0, 255);
                }
            }
            for (int i = 0; i < textlength[j]; i++) //Starting value/display text generation
            {
                if (i != 0)
                {
                    int r = Random.Range(18, 75);
                    textrandomiser[j][0].Add(r);
                    textrandomiser[j][1].Add(r == 74 ? 6 : Random.Range(0, 8));
                    displaytext[j].Add(textList[r]);
                    if (r == 74)
                    {
                        if (i != 1)
                        {

                            interVals[j][0] *= 100;
                        }
                        else
                        {
                            interVals[j][0] = 100;
                        }
                    }
                    else
                    {
                        if (textrandomiser[j][0][i - 1] != 74 && !((textrandomiser[j][0][i - 1] - 18) % 28 > 19 && (r - 18) % 28 < 10))
                        {
                            if ((r - 18) % 28 > 9)
                            {
                                interVals[j][0] *= 10;
                            }
                            interVals[j][0] *= 10;
                        }
                        interVals[j][0] += new int[28] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 30, 40, 50, 60, 70, 80, 90 }[(r - 18) % 28];
                    }
                }
                else
                {
                    textrandomiser[j][0].Add(Random.Range(0, 18));
                    displaytext[j].Add(textList[textrandomiser[j][0][0]]);
                }
                Debug.Log(interVals[j][0] + " " + "1");
                if (interVals[j][0] > 999) //Stops generating starting value once it exceeds 3 digits
                {
                    textlength[j] = i + 1;
                    break;
                }
            }
            displaytext[j].Add("POINT ZERO");
            textrandomiser[j][1].Add(6);
            Debug.Log(string.Join(" - ", displaytext[j].ToArray()) + " " + interVals[j][0]);
        }
        if (((interVals[0][0] == 2424 && interVals[1][0] == 4949) || (interVals[0][0] == 4949 && interVals[1][0] == 2424)) && buttonrandomiser[0][0].Contains(3) && buttonrandomiser[1][0].Contains(3) && info.IsIndicatorPresent("SIG") && info.GetModuleNames().Contains("Needy Tetris")) //Override
        {
            for (int i = 0; i < 2; i++)
            {
                correctbutton[i] = buttonrandomiser[i][0].ToList().IndexOf(3);
                finalVals[i] = 15;
                dtap[i] = true;
                possibleTimes[i].Add(15);
                scalefactor[i] = 2;
                while (possibleTimes[i].Last() * 2 < 86400)
                    possibleTimes[i].Add(possibleTimes[i].Last() * 2);
            }
        }
        else
        {
            bool[] stagenine = new bool[2] { false, false };
            for (int j = 0; j < 2; j++)
            {
                interVals[j].Add(interVals[j][0] - D[0] - D[1] - D[2]); //Step 2
                Debug.Log(interVals[j].Last() + " " + "2");
                interVals[j].Add(interVals[j][1] + 4 * (info.GetPortCount() + info.GetPortPlateCount())); //Step 3
                Debug.Log(interVals[j].Last() + " " + "3");
                interVals[j].Add(interVals[j][2] * Mathf.Max(1, Mathf.Abs(nameindex[0] - nameindex[1]))); //Step 4
                Debug.Log(interVals[j].Last() + " " + "4");
                if (nameindex[2] == 0) //Step 5
                    interVals[j].Add(Mathf.FloorToInt(interVals[j][3] / 2));
                else
                    interVals[j].Add(Mathf.FloorToInt(interVals[j][3] * 1.5f));
                Debug.Log(interVals[j].Last() + " " + "5");
                if ((buttonrandomiser[j][1][0] == buttonrandomiser[j][1][1]) && (buttonrandomiser[j][1][0] == buttonrandomiser[j][1][2])) //Step 6
                    correctbutton[j] = j;
                if (buttonrandomiser[j][0].ToList().IndexOf(0) != -1 && textrandomiser[j][1].ToList().IndexOf(0) == -1) //Step 7
                    interVals[j].Add(interVals[j][4] + interVals[1 - j][0]);
                Debug.Log(interVals[j].Last() + " " + "7");
                interVals[j].Add(interVals[j].Last() + (info.GetBatteryCount() * nameindex[8])); //Step 8
                Debug.Log(interVals[j].Last() + " " + "8");
                if (buttonrandomiser[j][0][2] == 2 || buttonrandomiser[j][0][2] == 5)//Step 9
                    stagenine[j] = true;
                correctbutton[j] = 2;
                interVals[j].Add(interVals[j].Last() + DateTime.Now.Day);//Step 10
                Debug.Log(interVals[j].Last() + " " + "10");
                interVals[j].Add(interVals[j].Last() + 20 * (info.GetOnIndicators().Count() - info.GetOffIndicators().Count()));//Step 11
                Debug.Log(interVals[j].Last() + " " + "11");
                if (interVals[j].Last() < 5000)//Step 12
                    interVals[j].Add(interVals[j].Last() * nameindex[3]);
                else
                    interVals[j].Add(interVals[j].Last() + nameindex[3]);
                Debug.Log(interVals[j].Last() + " " + "12");
                interVals[j].Add(interVals[j].Last() + 6 * (nameindex[6] - nameindex[5]));//Step 13
                interVals[j].Add(interVals[j].Last());
                Debug.Log(interVals[j].Last() + " " + "13");
            }
            for (int i = 1; i < textlength[0]; i++)//Step A1
            {
                if (textrandomiser[0][0][i] != 74)
                {
                    if (operationlist[textrandomiser[0][1][i]][textrandomiser[0][0][i] - 18] == "STOP")
                        break;
                    else
                        interVals[0][interVals[0].Count - 1] = GridProcess(interVals[0].Last(), operationlist[textrandomiser[0][1][i]][textrandomiser[0][0][i] - 18]);
                }
            }
            Debug.Log(interVals[0].Last() + " " + "A1");
            if (textrandomiser[0][0][0] > 8)//Step A2
            {
                interVals[0].Add(interVals[0].Last());
                foreach (string ind in info.GetOffIndicators())
                {
                    interVals[0][interVals[0].Count - 1] += "#ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(ind[0].ToString());
                }
                Debug.Log(interVals[0].Last() + " " + "A2");
            }
            if (textrandomiser[0][0].IndexOf(74) != -1)//Step A3
            {
                interVals[0].Add(Mathf.FloorToInt(interVals[0].Last() / 100));
                Debug.Log(interVals[0].Last() + " " + "A3");
            }
            if (!info.GetSolvedModuleNames().Contains("Simon's Stages") && info.GetSolvedModuleNames().Contains("Übermodule"))//Step A4
            {
                interVals[0].Add(interVals[0].Last() + 99 * (info.GetModuleNames().Count - info.GetSolvableModuleNames().Count));//Step A5
                Debug.Log(interVals[0].Last() + " " + "A5");
                if (((textrandomiser[0][0][0] % 9 < 6) && (textrandomiser[0][0][0] % 9 > 2)) || textrandomiser[0][0][0] == 17)//Step A6
                {
                    interVals[0].Add(interVals[0].Last());
                    for (int i = 0; i < 3; i++)
                    {
                        interVals[0][interVals[0].Count - 1] = GridProcess(interVals[0].Last(), operationlist[buttonrandomiser[0][1][i]][buttonrandomiser[0][0][i]]);
                    }
                    Debug.Log(interVals[0].Last() + " " + "A6");
                }
                if (interVals[0].Last() < 0)//Step A7
                {
                    interVals[0].Add(interVals[0].Last() * -6);
                    Debug.Log(interVals[0].Last() + " " + "A7");
                }
            }
            if (textrandomiser[1][0][0] > 8 && textrandomiser[1][0][1] != 74)//Step B1
            {
                if (operationlist[textrandomiser[1][1][1]][textrandomiser[1][0][1] - 18] == "STOP")
                    interVals[1].Add(Mathf.FloorToInt(interVals[1].Last() / 17));
                else if (operationlist[textrandomiser[1][1][1]][textrandomiser[1][0][1] - 18][0] == 'B')
                    interVals[1].Add(interVals[1].Last() * "##123".IndexOf(operationlist[textrandomiser[1][1][1]][textrandomiser[1][0][1] - 18][1].ToString()));
                else
                    interVals[1].Add(GridProcess(interVals[1].Last(), operationlist[textrandomiser[1][1][1]][textrandomiser[1][0][1] - 18]));
                Debug.Log(interVals[1].Last() + " " + "B1");
            }
            if ((textrandomiser[1][0][0] % 9) > 6)//Step B2 
            {
                if (textrandomiser[1][0][textlength[1] - 1] == 74)
                    interVals[1].Add(Mathf.FloorToInt(interVals[1].Last() * 100));
                else if (operationlist[textrandomiser[1][1][textlength[1] - 1]][textrandomiser[1][0][textlength[1] - 1] - 18] == "STOP")
                    interVals[1].Add(Mathf.FloorToInt(interVals[1].Last() / 17));
                else if (operationlist[textrandomiser[1][1][textlength[1] - 1]][textrandomiser[1][0][textlength[1] - 1] - 18][0] == 'B')
                    interVals[1].Add(interVals[1].Last() * "##123".IndexOf(operationlist[textrandomiser[1][1][1]][textrandomiser[1][0][1] - 18][1].ToString()));
                else
                    interVals[1].Add(GridProcess(interVals[1].Last(), operationlist[textrandomiser[1][1][1]][textrandomiser[1][0][1] - 18]));
                Debug.Log(interVals[1].Last() + " " + "B2");
            }
            if (buttonrandomiser[1][0][0] == textrandomiser[1][1][1] && buttonrandomiser[1][0][1] != textrandomiser[1][1][1] && buttonrandomiser[1][0][2] == textrandomiser[1][1][1])//Step B3
                correctbutton[1] = 1;
            if (stagenine[0]) //Step B4
                interVals[1].Add(interVals[1].Last() + 25);
            else if (stagenine[1])
                interVals[1].Add(interVals[1].Last() + 150);
            else
                interVals[1].Add(interVals[1].Last() - 250);
            Debug.Log(interVals[1].Last() + " " + "B4");
            interVals[1].Add(interVals[1].Last() - 10 * nameindex[4]);//Step B5
            Debug.Log(interVals[1].Last() + " " + "B5");
            if (interVals[1].Last() < 0)//Step B6
            {
                interVals[1].Add(interVals[1].Last() * -65);
                Debug.Log(interVals[1].Last() + " " + "B6");
            }
            else
            {
                int[] colournames = new int[8] { 3, 6, 5, 4, 4, 7, 5, 5 }; //Step B7
                int[] colourtotals = new int[2] { 0, 0 };
                for (int i = 0; i < 3; i++)
                {
                    colourtotals[0] += colournames[buttonrandomiser[1][0][i]];
                    if (buttonrandomiser[1][0][i] != 6)
                    {
                        colourtotals[1] += colournames[buttonrandomiser[1][0][i]];
                    }
                }
                if (colournames[0] > 12)
                {
                    interVals[1].Add(Mathf.FloorToInt(interVals[1].Last() / Mathf.Max(0.5f, colourtotals[1])));
                    Debug.Log(interVals[1].Last() + " " + "B6");
                }
                if (info.GetTwoFactorCodes().Count() > 0) //Step B8
                {
                    interVals[1].Add(interVals[1].Last() + 50 * info.GetTwoFactorCodes().Count());
                    Debug.Log(interVals[1].Last() + " " + "B8");
                }
                else
                {
                    if (textlength[1] == 6)//Step B9
                    {
                        interVals[1].Add(Mathf.FloorToInt(interVals[1].Last() / 7));
                        Debug.Log(interVals[1].Last() + " " + "B9");
                    }
                    if (colournames[buttonrandomiser[1][0][0]] != colournames[buttonrandomiser[1][0][1]] && colournames[buttonrandomiser[1][0][0]] != colournames[buttonrandomiser[1][0][2]] && colournames[buttonrandomiser[1][0][1]] != colournames[buttonrandomiser[1][0][2]] && info.GetBatteryCount() < 2 * info.GetBatteryHolderCount())//Step B9
                    {
                        correctbutton[1] = buttonrandomiser[1][0].ToList().IndexOf(Mathf.Max(buttonrandomiser[1][0]));
                    }
                    interVals[1].Add(Mathf.FloorToInt(interVals[1].Last() / 3));//Step B10
                    Debug.Log(interVals[1].Last() + " " + "B10");
                    interVals[1].Add(interVals[1].Last() + 10 * nameindex[7]);//Step B11
                    Debug.Log(interVals[1].Last() + " " + "B11");
                }
            }
            for (int i = 0; i < 2; i++)
            {
                finalVals[i] = interVals[i].Last();
                while (finalVals[i] > time / 6)
                    finalVals[i] = Mathf.FloorToInt(finalVals[i] / 10);//Removing digits from final value until it falls below one tenth of the bomb's starting time 
                Debug.Log("Final = " + finalVals[i]);
            }
            //Scale Factor
            for (int j = 0; j < 2; j++)
            {
                if (info.IsIndicatorOn("BOB") && info.IsIndicatorOn("FRK") && info.GetBatteryCount() == 4 && info.GetBatteryHolderCount() == 2)
                {
                    scalefactor[j] = 2;
                }
                else
                {
                    if(interVals[j][0] < 5000)                    
                        scalefactor[j]++;
                    if (time > 1799)
                        scalefactor[j] += 2;
                    if (nameindex[5] == 0 && nameindex[6] == 0 && info.GetColoredIndicators().Count() == 0)
                        scalefactor[j]++;
                    if(j == 0)
                    {
                        scalefactor[0] -= Mathf.FloorToInt(info.GetBatteryCount() / 2);
                        if (info.GetSolvableModuleNames().Contains("Ultimate Cycle"))
                            scalefactor[0]++;
                        if (info.GetSerialNumberNumbers().Count() > 4)
                            scalefactor[0] += 3;
                        if (textrandomiser[0][0][0] < 9)
                            scalefactor[0] -= 2;
                        if (info.GetSolvableModuleNames().Contains("Hogwarts"))
                            scalefactor[0] += 2;
                        if (info.GetSerialNumberLetters().Count() == 3)
                            scalefactor[0] -= 2;
                    }
                    else
                    {
                        scalefactor[0] -= info.GetBatteryHolderCount();
                        if (info.GetSolvableModuleNames().Contains("Forget Enigma"))
                            scalefactor[1]++;
                        if (info.GetSerialNumberLetters().Count() > 4)
                            scalefactor[1] += 3;
                        if (textrandomiser[1][0][0] > 9)
                            scalefactor[1] += 2;
                        if (info.GetSolvableModuleNames().Contains("Encryption Bingo"))
                            scalefactor[1] -= 2;
                        if (info.GetSerialNumberNumbers().Count() == 3)
                            scalefactor[1] -= 2;
                    }
                    if (interVals[j].Last() > 10000)
                        scalefactor[j] = Mathf.FloorToInt(scalefactor[j] / 2);
                }
                if (scalefactor[j] < 2)
                    scalefactor[j] = 2;
                else if (scalefactor[j] > 5)
                    scalefactor[j] = 5;
                List<string> plog = new List<string> { };
                possibleTimes[j].Add(finalVals[j]);
                plog.Add(finalVals[j].ToString());
                Debug.Log("Scale Factor = " + scalefactor[j]);
                while (possibleTimes[j].Last() * scalefactor[j] < 86400)
                {
                    possibleTimes[j].Add(possibleTimes[j].Last() * scalefactor[j]);
                    plog.Add((possibleTimes[j].Last()).ToString());
                }
                Debug.Log(string.Join(", ", plog.ToArray()));
            }
            //Determining Correct Button
            for(int j = 0; j < 2; j++)
            {
                if (correctbutton[j] == -1)
                {
                    if (interVals[j].Last() > -1 && interVals[j].Last() < 10000)
                    {
                        correctbutton[j] = 0;
                    }
                    else
                    {
                        int[] btotals = new int[8];
                        for (int i = 0; i < 3; i++)
                        {
                            btotals[buttonrandomiser[j][0][i]]++;
                        }
                        if (info.GetPorts().Contains("Parallel") && btotals[5] == 1)
                            correctbutton[j] = buttonrandomiser[j][0].ToList().IndexOf(5);
                        else if (info.GetPorts().Contains("StereoRCA") && btotals[6] == 1 && btotals[0] == 1)
                            correctbutton[j] = 3 - buttonrandomiser[j][0].ToList().IndexOf(6) - buttonrandomiser[j][0].ToList().IndexOf(0);
                        else if (!textrandomiser[j][1].Contains(7) && btotals[7] == 1) 
                            correctbutton[j] = buttonrandomiser[j][0].ToList().IndexOf(7);
                        else
                        {
                            int[] ttotals = new int[8];
                            int m = 0;
                            for (int i = 0; i < textlength[j]; i++)
                            {
                                ttotals[textrandomiser[j][1][i]]++;
                            }
                            for(int i = 0; i < 8; i++)
                            {
                                if (btotals[i] != 0 && ttotals[i] != 0 && i != 6)
                                    m++;
                            }
                            if (m == 0)
                                correctbutton[j] = 2;                           
                            else
                            {
                                int mtotal = 0;
                                for (int i = 0; i < 3; i++)
                                {
                                    if (buttonrandomiser[j][1].Contains((textrandomiser[j][0][i] - 18) % 28))
                                    {
                                        mtotal += i + 10;
                                        continue;
                                    }
                                }
                                if (ttotals[6] == textlength[j] && mtotal > 19 && mtotal < 30)
                                    correctbutton[j] = 23 - mtotal;                                
                                else if (btotals[0] == 1 && btotals[2] == 1 && btotals[4] == 1)
                                    correctbutton[j] = buttonrandomiser[j][0].ToList().IndexOf(4);
                                else                               
                                    correctbutton[j] = 2 * j + (1 - 2 * j) * (interVals[j].Last() % 3);  
                            }
                        }
                    }
                }
            }
            //Dtap?
            for (int j = 0; j < 2; j++)
            {
                if (info.GetSolvableModuleNames().Contains("The Very Annoying Button"))
                    dtap[j] = true;
                else
                {
                    bool[] match = new bool[3] { false, false, false };
                    for (int k = 0; k < 3; k++) 
                    {
                        for (int i = 1; i < textlength[j]; i++)
                        {
                            if ((textrandomiser[j][0][i] - 18) % 28 == buttonrandomiser[j][1][k])
                            {
                                match[k] = true;
                                break;
                            }
                        }
                    }
                    if (match[(correctbutton[j] + 1) % 3] && match[(correctbutton[j] + 2) % 3])
                        dtap[j] = false;
                    else if (match[correctbutton[j]])
                        dtap[j] = true;
                    else if (((buttonrandomiser[j][1][0] > buttonrandomiser[j][1][1]) && buttonrandomiser[j][1][1] > buttonrandomiser[j][1][2]) || ((buttonrandomiser[j][1][2] > buttonrandomiser[j][1][1]) && buttonrandomiser[j][1][1] > buttonrandomiser[j][1][0]))
                        dtap[j] = false;
                    else if (Mathf.Abs(buttonrandomiser[j][1][(correctbutton[j] + 1) % 3] - buttonrandomiser[j][1][(correctbutton[j] + 2) % 3]) > buttonrandomiser[j][1][correctbutton[j]] - 3 && Mathf.Abs(buttonrandomiser[j][1][(correctbutton[j] + 1) % 3] - buttonrandomiser[j][1][(correctbutton[j] + 2) % 3]) < buttonrandomiser[j][1][correctbutton[j]] + 3)
                        dtap[j] = true;
                    else if ((buttonrandomiser[j][1][(correctbutton[j] + 1) % 3] + buttonrandomiser[j][1][(correctbutton[j] + 2) % 3]) % 10 == buttonrandomiser[j][1][correctbutton[j]])
                        dtap[j] = false;
                    else if ((buttonrandomiser[j][1][(correctbutton[j] + 1) % 3] * buttonrandomiser[j][1][(correctbutton[j] + 2) % 3]) % 10 == buttonrandomiser[j][1][correctbutton[j]])
                        dtap[j] = true;
                    else
                        dtap[j] = false;
                }
            }
        }
        for(int i = 0; i < 2; i++)
        {
            if (scalefactor[i] < 2)
                scalefactor[i] = 2;
            else if (scalefactor[i] > 5)
                scalefactor[i] = 5;
            Debug.LogFormat("{0} the {1} button when the number of remaining seconds is of the form {2} * {3} ^ n", dtap[i] ? "Double-tap" : "Press", new string[3] { "left", "middle", "right" }[correctbutton[i]], finalVals[i], scalefactor[i]);
        }
        StartCoroutine(CycleText(0));
    }

    private int GridProcess(int x, string y)
    {
        int gridnum = 0;
        for(int i = 1; i < y.Length; i++)
        {
            gridnum *= 10;
            switch (y[i])
            {
                case '1':
                    gridnum++;
                    break;
                case '2':
                    gridnum += 2;
                    break;
                case '3':
                    gridnum += 3;
                    break;
                case '4':
                    gridnum += 4;
                    break;
                case '5':
                    gridnum += 5;
                    break;
                case '6':
                    gridnum += 6;
                    break;
                case '7':
                    gridnum += 7;
                    break;
                case '8':
                    gridnum += 8;
                    break;
                case '9':
                    gridnum += 9;
                    break;
            }
        }
        int v = x;
        switch (y[0])
        {
            case 'B':
                correctbutton[0] = gridnum - 1;
                Debug.Log(y);
                break;
            case '+':
                x += gridnum;
                Debug.Log(v + " + " + gridnum + " = " + x);
                break;
            case '-':
                x -= gridnum;
                Debug.Log(v + " - " + gridnum + " = " + x);
                break;
            case '*':
                x *= gridnum;
                Debug.Log(v + " * " + gridnum + " = " + x);
                break;
            case '/':
                x /= gridnum;
                Debug.Log(v + " / " + gridnum + " = " + x);
                break;
        }
        return x;
    }
    
    private void ButtonOn(int b)
    {
        if (!moduleSolved && !stageswitch)
        {
            if (b < 3)
            {
                if (pressed)
                {
                    StopAllCoroutines();
                    displays[0].text = string.Empty;
                    morseseq[0].Clear();
                    morseseq[1].Clear();
                    buttonsequencer = new int[2][] { new int[5], new int[5] };
                    pressed = false;
                    if (!pop && dtap[stage])
                    {
                        checkanswer[2] = true;
                    }
                    else if(!pop && !dtap[stage])
                    {
                        GetComponent<KMBombModule>().HandleStrike();
                        for (int k = 0; k < 3; k++)
                        {
                            brends[k].material = buttonColors[buttonrandomiser[0][0][k]];
                            displays[k + 1].text = buttonrandomiser[0][1][k].ToString();
                            displays[k + 1].color = buttonrandomiser[0][0][k] == 7 ? new Color32(0, 255, 0, 255) : new Color32(0, 0, 0, 255);
                        }
                        StartCoroutine(CycleText(stage));
                    }
                    else if (pop && !dtap[stage])
                    {
                        switch (taprule[0])
                        {
                            case 1:
                                switch (taprule[1])
                                {
                                    case 0:
                                        int n0 = 0;
                                        foreach (char d in formatteddisptime)
                                            if (d != ':' && info.GetFormattedTime().Contains(d.ToString()))
                                                n0++;
                                        if (n0 > 1)
                                            checkanswer[2] = true;
                                        break;
                                    case 1:
                                        if (new int[] { 0, 2, 6, 12, 20, 30, 42, 56 }.Contains(disptime % 60))
                                            checkanswer[2] = true;
                                        break;
                                    case 2:
                                        if (new int[] { 0, 2, 6, 12, 20, 30, 42, 56 }.Contains((int)info.GetTime() % 60))
                                            checkanswer[2] = true;
                                        break;
                                    case 3:
                                        int n3 = 0;
                                        foreach (char d in DateTime.Now.ToString("HH:mm:ss"))
                                            if (d != ':' && d != '0' && info.GetFormattedTime().Contains(d.ToString()))
                                                n3++;
                                        if (n3 > 0)
                                            checkanswer[2] = true;
                                        break;
                                    case 4:
                                        if (Mathf.Abs((int)info.GetTime() % 10 - disptime % 10) < 2)
                                            checkanswer[2] = true;
                                        break;
                                    case 5:
                                        int n5 = 0;
                                        foreach (char d in DateTime.Now.ToString("HH:mm:ss"))
                                            if (d != ':' && d != '0' && formatteddisptime.Contains(d.ToString()))
                                                n5++;
                                        if (n5 > 0)
                                            checkanswer[2] = true;
                                        break;
                                    case 6:
                                        if (disptime % 60 == 4 || disptime % 60 == 40)
                                            checkanswer[2] = true;
                                        break;
                                    case 7:
                                        if ((int)info.GetTime() % 60 == 4 || (int)info.GetTime() % 60 == 40)
                                            checkanswer[2] = true;
                                        break;
                                    case 8:
                                        if (!new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 14, 20, 24, 30, 34, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 54 }.Contains(disptime % 60) && !new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 14, 20, 24, 30, 34, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 54 }.Contains((int)info.GetTime() % 60))
                                            checkanswer[2] = true;
                                        break;
                                }
                                break;
                            case 2:
                                switch (taprule[1])
                                {
                                    case 0:
                                        if (!info.GetSerialNumberNumbers().Contains((int)info.GetTime() % 10) && !info.GetSerialNumberNumbers().Contains(Mathf.FloorToInt(((int)info.GetTime() % 60) / 10)))
                                            checkanswer[2] = true;
                                        break;
                                    case 1:
                                        if (new int[] { 2, 3, 5, 7, 11, 13 }.Contains(((int)info.GetTime() % 10) + (Mathf.FloorToInt(((int)info.GetTime() % 60) / 10))))
                                            checkanswer[2] = true;
                                        break;
                                    case 2:
                                        if (new int[] { 2, 3, 5, 7, 11, 13 }.Contains((disptime % 10) + (Mathf.FloorToInt((disptime % 60) / 10))))
                                            checkanswer[2] = true;
                                        break;
                                    case 3:
                                        if (new int[] { 4, 8, 15, 16, 23, 42 }.Contains(disptime % 60))
                                            checkanswer[2] = true;
                                        break;
                                    case 4:
                                        if (((int)info.GetTime() % 60) % 10 == Mathf.FloorToInt(((int)info.GetTime() % 60) / 10))
                                            checkanswer[2] = true;
                                        break;
                                    case 5:
                                        if (new int[] { 4, 8, 15, 16, 23, 42 }.Contains((int)info.GetTime() % 60))
                                            checkanswer[2] = true;
                                        break;
                                    case 6:
                                        if (info.GetSerialNumberNumbers().Contains(((int)info.GetTime() % 60) % 10) || info.GetSerialNumberNumbers().Contains(Mathf.FloorToInt(((int)info.GetTime() % 60) / 10)))
                                            checkanswer[2] = true;
                                        break;
                                    case 7:
                                        if (!new int[] { 2, 3, 5, 7, 11, 13 }.Contains((((int)info.GetTime() % 60) % 10) + (Mathf.FloorToInt(((int)info.GetTime() % 60) / 10))))
                                            checkanswer[2] = true;
                                        break;
                                    case 8:
                                        if (!info.GetSerialNumberNumbers().Contains((disptime % 60) % 10) && !info.GetSerialNumberNumbers().Contains(Mathf.FloorToInt((disptime % 60) / 10)))
                                            checkanswer[2] = true;
                                        break;
                                }
                                break;
                            case 3:
                                switch (taprule[1])
                                {
                                    case 0:
                                        if ((int)info.GetTime() % 60 == info.GetTwoFactorCodes().ToArray()[0] % 60)
                                            checkanswer[2] = true;
                                        break;
                                    case 1:
                                        int n1 = 0;
                                        List<int> m1 = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                                        foreach (int t0 in info.GetTwoFactorCodes())
                                            for (int i = 0; i < Mathf.Log10(t0); i++)
                                                if (m1.Contains(Mathf.FloorToInt(t0 / Mathf.Pow(10, i)) % 10))
                                                    m1.Remove(Mathf.FloorToInt(t0 / Mathf.Pow(10, i)) % 10);
                                        foreach (char d in formatteddisptime)
                                        {                                         
                                            foreach (int t1 in info.GetTwoFactorCodes())
                                                if (t1.ToString().Contains(d))
                                                {
                                                    n1++;
                                                    break;
                                                }
                                        }
                                        if (n1 < 3 || m1.Count() < 2)
                                            checkanswer[2] = true;
                                        break;
                                    case 2:
                                        int n2 = 0;
                                        int m = Mathf.Max(info.GetTwoFactorCodes().ToArray());
                                        List<char> m2 = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                                        foreach (char t1 in info.GetFormattedTime())
                                            if (m.ToString().Contains(t1.ToString()))
                                                n2++;
                                        foreach (char t2 in formatteddisptime)
                                            if (m.ToString().Contains(t2.ToString()))
                                                n2++;
                                        foreach (char tfd in m.ToString())
                                            if (m2.Contains(tfd))
                                                m2.Remove(tfd);
                                        if (n2 > 3 || m2.Count() > 7)
                                            checkanswer[2] = true;
                                        break;
                                }
                                break;
                            case 4:
                                if (taprule[1] > 299 && disptime % 60 == (taprule[1] - 300) % 60)
                                    checkanswer[2] = true;
                                else if (taprule[1] > 199 && (int)info.GetTime() % 60 == (taprule[1] - 200) % 60)
                                    checkanswer[2] = true;
                                else if (taprule[1] > 99 && (int)info.GetTime() % 60 == taprule[1] - 100)
                                    checkanswer[2] = true;
                                else if (taprule[1] > 19 && ((int)info.GetTime() % 60) % 10 != taprule[1] - 20 && Mathf.FloorToInt(((int)info.GetTime() % 60) / 10) != taprule[1] - 20)
                                    checkanswer[2] = true;
                                else if (taprule[1] > 9 && (disptime % 60) % 10 != taprule[1] - 10 && Mathf.FloorToInt((disptime % 60) / 10) != taprule[1] - 10)
                                    checkanswer[2] = true;
                                else if (((int)info.GetTime() % 60) % 10 == taprule[1] || Mathf.FloorToInt(((int)info.GetTime() % 60) / 10) == taprule[1])
                                    checkanswer[2] = true;
                                break;
                            default:
                                if (taprule[1] != 0 && (int)info.GetTime() % taprule[1] == 0)
                                    checkanswer[2] = true;
                                else if (IsPrime((int)info.GetTime()))
                                    checkanswer[2] = true;
                                break;
                        }
                    }
                    pop = false;
                    if (checkanswer[0] && checkanswer[1] && checkanswer[2])
                    {
                        stagecomplete[stage] = true;
                        if(!stagecomplete[0] || !stagecomplete[1])                                                       
                            brends[4 - stage].material = indicatorStatus[0];
                        brends[stage + 3].material = indicatorStatus[2];
                        if (stagecomplete[0] && stagecomplete[1])
                        {
                            moduleSolved = true;
                            StartCoroutine(SolveAnim());
                        }
                        else
                        {
                            stage = 1 - stage;
                            StartCoroutine(SwitchStages());
                        }
                    }
                    else
                    {
                        GetComponent<KMBombModule>().HandleStrike();
                        for (int k = 0; k < 3; k++)
                        {
                            brends[k].material = buttonColors[buttonrandomiser[0][0][k]];
                            displays[k + 1].text = buttonrandomiser[0][1][k].ToString();
                            displays[k + 1].color = buttonrandomiser[0][0][k] == 7 ? new Color32(0, 255, 0, 255) : new Color32(0, 0, 0, 255);
                        }
                        StartCoroutine(CycleText(stage));
                    }
                    checkanswer = new bool[3] { false, false, false };
                }
                else
                {
                    checkanswer = new bool[3] { false, false, false };
                    if (b == correctbutton[stage])
                        checkanswer[0] = true;
                    foreach(int t in possibleTimes[stage])
                        if(Mathf.Abs((int)info.GetTime() - t) < 6)
                        {
                            checkanswer[1] = true;
                            break;
                        }
                    if (checkanswer[0])
                    {
                        pressed = true;
                        StopAllCoroutines();
                        displays[0].text = string.Empty;
                        for (int i = 0; i < 3; i++)
                        {
                            brends[i].material = buttonColors[7];
                            displays[i + 1].text = string.Empty;
                        }
                        StartCoroutine(Hold(b));
                    }
                    else
                        GetComponent<KMBombModule>().HandleStrike();
                }
            }
            else if(!stagecomplete[0] && !stagecomplete[1] && !pressed && b != stage + 3)
            {
                stageswitch = true;
                StopAllCoroutines();
                displays[0].text = string.Empty;
                brends[b].material = indicatorStatus[0];
                stage = 1 - stage;
                StartCoroutine(SwitchStages());
            }
        }
    }

    private bool IsPrime(int x)
    {
        if (x < 5)
            return true;
        else
        {
            for (int i = 2; i < x/2; i++)
                if (x % i == 0)
                    return false;
        }
        return true;
    }

    private IEnumerator CycleText(int b)
    {
        for(int i = 0; i < textlength[b] + 2; i++)
        {
            if (i > textlength[b])
            {
                displays[0].text = string.Empty;
                i = -1;
            }
            else
            {
                displays[0].text = displaytext[b][i];
                switch (textrandomiser[b][1][i])
                {
                    case 0:
                        displays[0].color = new Color32(255, 0, 0, 255);
                        break;
                    case 1:
                        displays[0].color = new Color32(255, 255, 0, 255);
                        break;
                    case 2:
                        displays[0].color = new Color32(0, 255, 0, 255);
                        break;
                    case 3:
                        displays[0].color = new Color32(0, 255, 255, 255);
                        break;
                    case 4:
                        displays[0].color = new Color32(0, 0, 255, 255);
                        break;
                    case 5:
                        displays[0].color = new Color32(255, 0, 255, 255);
                        break;
                    case 6:
                        displays[0].color = new Color32(255, 255, 255, 255);
                        break;
                    default:
                        displays[0].color = new Color32(50, 50, 50, 255);
                        break;
                }
            }
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator SwitchStages()
    {
        for (int i = 0; i < 25; i++)
        {
            if (i < 10)
                door.transform.localPosition += new Vector3(0,0,0.005f);
            else if(i == 10)
            {
                for (int k = 0; k < 3; k++)
                {
                    brends[k].material = buttonColors[buttonrandomiser[stage][0][k]];
                    displays[k + 1].text = buttonrandomiser[stage][1][k].ToString();
                    displays[k + 1].color = buttonrandomiser[stage][0][k] == 7 ? new Color32(0, 255, 0, 255) : new Color32(0, 0, 0, 255);
                }
            }
            else if (i > 14)
                door.transform.localPosition -= new Vector3(0,0,0.005f);
            yield return new WaitForSeconds(0.1f);
        }
        if(!stagecomplete[0] && !stagecomplete[1])
            brends[4 - stage].material = indicatorStatus[1];
        stageswitch = false;
        StartCoroutine(CycleText(stage));
    }

    private IEnumerator Hold(int b)
    {
        yield return new WaitForSeconds(5);
        sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CapacitorPop, transform);
        if (dtap[stage])
        {
            GetComponent<KMBombModule>().HandleStrike();
            for(int k = 0; k < 3; k++)
            {
                brends[k].material = buttonColors[buttonrandomiser[0][0][k]];
                displays[k + 1].text = buttonrandomiser[0][1][k].ToString();
                displays[k + 1].color = buttonrandomiser[0][0][k] == 7 ? new Color32(0, 255, 0, 255) : new Color32(0, 0, 0, 255);
            }
            StartCoroutine(CycleText(stage));
            yield break;
        }
        else
        {
            pop = true;
            int flashindex = 0;
            for (int i = 0; i < 3; i++)
            {
                if (i != b)
                {
                    buttonsequencer[flashindex][0] = Random.Range(0, 3);
                    switch (buttonsequencer[flashindex][0])
                    {
                        case 0:
                            int r = Random.Range(0, 3);
                            buttonsequencer[flashindex] = new int[5] { 0, r, r, r, r };
                            break;
                        case 1:
                            while (buttonsequencer[flashindex][1] == buttonsequencer[flashindex][2] && buttonsequencer[flashindex][2] == buttonsequencer[flashindex][3] && buttonsequencer[flashindex][3] == buttonsequencer[flashindex][4])
                            {
                                buttonsequencer[flashindex] = new int[5] { 1, Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3) };
                            }
                            break;
                        default:
                            buttonsequencer[flashindex][1] = Random.Range(0, 36);
                            morseseq[flashindex].Add(Random.Range(0, 3));
                            for (int dot = 1; dot < morselist[buttonsequencer[flashindex][1]].Length; dot++)
                                if (morselist[buttonsequencer[flashindex][1]][dot - 1] == '-' && morselist[buttonsequencer[flashindex][1]][dot] == '#')
                                    morseseq[flashindex].Add(Random.Range(0, 3));
                            break;
                    }
                    Debug.Log(string.Join(", ", buttonsequencer[flashindex].Select(x => x.ToString()).ToArray()));
                    StartCoroutine(Flash(i, flashindex, buttonsequencer[flashindex]));
                    flashindex++;
                }
            }
            if (buttonsequencer[0][0] == 0 && buttonsequencer[1][0] == 0)
            {
                taprule[0] = 1;
                taprule[1] = 3 * buttonsequencer[0][1] + buttonsequencer[1][1];
            }
            else if (buttonsequencer[0][0] == 0 || buttonsequencer[1][0] == 0)
            {
                taprule[0] = 2;
                taprule[1] = buttonsequencer[0][0] == 0 ? (3 * Mathf.Max(0, 1 - b) + buttonsequencer[0][1]) : (3 * Mathf.Min(2, 3 - b) + buttonsequencer[1][1]);
            }
            else if (info.GetTwoFactorCodes().Count() > 0)
            {
                taprule[0] = 3;
                taprule[1] = Mathf.Min(2, info.GetTwoFactorCodes().Count() - 1);
            }
            else if (buttonsequencer[0][0] == 2 || buttonsequencer[1][0] == 2)
            {
                taprule[0] = 4;
                int[] code = new int[2] { 0, 0 };
                bool[] ismorse = new bool[2] { false, false };
                for (int i = 0; i < 2; i++)
                    if (buttonsequencer[i][0] == 2)
                    {
                        ismorse[i] = true;
                        code[i] = buttonsequencer[i][1];
                        for (int j = 0; j < morseseq[i].Count(); j++)
                            switch (j)
                            {
                                case 0:
                                    switch (morseseq[i][j])
                                    {
                                        case 0:
                                            code[i] += info.GetModuleNames().Count();
                                            break;
                                        case 1:
                                            code[i] += DateTime.Today.Month;
                                            break;
                                        default:
                                            code[i] += info.GetPortCount();
                                            break;
                                    }
                                    break;
                                case 1:
                                    switch (morseseq[i][j])
                                    {
                                        case 0:
                                            code[i] += ((info.GetModuleNames().Count() - 1) % 9) + 1;
                                            break;
                                        case 1:
                                            code[i] += ((code[i] - 1) % 9) + 1;
                                            break;
                                        default:
                                            code[i] -= ((code[i] - 1) % 9) + 1;
                                            break;
                                    }
                                    break;
                                case 2:
                                    switch (morseseq[i][j])
                                    {
                                        case 0:
                                            code[i] -= info.GetModuleNames().Count();
                                            break;
                                        case 1:
                                            code[i] -= DateTime.Today.Month;
                                            break;
                                        default:
                                            code[i] -= info.GetPortCount();
                                            break;
                                    }
                                    break;
                                default:
                                    switch (morseseq[i][j])
                                    {
                                        case 0:
                                            code[i] -= ((info.GetModuleNames().Count() - 1) % 9) + 1;
                                            break;
                                        case 1:
                                            code[i] -= Mathf.FloorToInt(Mathf.Log10(code[i])) + 1;
                                            break;
                                        default:
                                            code[i] += Mathf.FloorToInt(Mathf.Log10(code[i])) + 1;
                                            break;
                                    }
                                    break;
                            }
                        code[i] = Mathf.Abs(code[i]) % 10;
                    }
                if (!ismorse[0] || !ismorse[1])
                    taprule[1] = ismorse[0] ? code[0] : code[1];
                else if (code[0] == 0 && code[1] != 0)
                    taprule[1] = 10 + code[1];
                else if (code[0] != 0 && code[1] == 0)
                    taprule[1] = 10 + code[0];
                else if (code[0] == code[1])
                    taprule[1] = 20 + code[0];
                else if (new int[] { 10, 14, 18, 24, 30 }.Contains(buttonsequencer[0][1]) && new int[] { 10, 14, 18, 24, 30 }.Contains(buttonsequencer[1][1]))
                    taprule[1] = 100 + (code[0] + code[1]) % 60;
                else if (new int[] { 10, 14, 18, 24, 30 }.Contains(buttonsequencer[0][1]) || new int[] { 10, 14, 18, 24, 30 }.Contains(buttonsequencer[1][1]))
                    taprule[1] = 200 + (code[0] * code[1]) % 60;
                else
                    taprule[1] = 300 + (code[0] * code[1]) % 60;
            }
            else
            {
                taprule[0] = 0;
                int[] val = new int[2] { 0, 0 };
                flashindex = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (i != b)
                    {
                        for (int j = 1; j < 5; j++)
                            val[flashindex] += ((buttonsequencer[flashindex][j] + i) % 3) + 1;
                        val[flashindex] %= 5;
                        flashindex++;
                    }
                }
                if (val[0] != val[1])
                    taprule[1] = new int[] { 2, 3, 5, 7, 11 }[val[0]] * new int[] { 2, 3, 5, 7, 11 }[val[1]];
            }
            Debug.Log(taprule[0] + " " + taprule[1]);
            displays[0].color = new Color32(255, 255, 255, 255);
            disptime = 0;
            while (true)
            {
                formatteddisptime = (Mathf.FloorToInt(disptime / 60) < 10 ? "0" : string.Empty) + "" + Mathf.FloorToInt(disptime / 60).ToString() + ":" + (disptime % 60 < 10 ? "0" : string.Empty) + "" + (disptime % 60).ToString();
                displays[0].text = formatteddisptime;
                yield return new WaitForSeconds(1);
                disptime++;
            }
        }
    }

    private IEnumerator Flash(int b, int j, int[] seq)
    {
        switch (seq[0])
        {
            case 2:
                int n = 0;
                for(int i = 1; i < morselist[seq[1]].Length; i++)
                {
                    if (morselist[seq[1]][i - 1] == '-' && morselist[seq[1]][i] == '#')
                    {
                        brends[b].material = buttonColors[morseseq[j][n] * 2];
                        n++;
                    }
                    else if ((morselist[seq[1]][i - 1] == '#' && morselist[seq[1]][i] == '-'))
                        brends[b].material = buttonColors[7];
                    if(i == morselist[seq[1]].Length - 1)
                    {
                        i = 0;
                        brends[b].material = buttonColors[morseseq[j][n] * 2];
                        n = 0;
                    }
                    yield return new WaitForSeconds(0.25f);
                }
                break;
            default:
                for (int i = 1; i < 9; i++)
                {
                    if (i % 2 == 1)
                        brends[b].material = buttonColors[seq[(i + 1) / 2] * 2];
                    else
                        brends[b].material = buttonColors[7];
                    if (i == 8)
                        i = 0;
                    yield return new WaitForSeconds(0.25f);
                }
                break;
        }
    }

    private IEnumerator SolveAnim()
    {
        for (int i = 0; i < 25; i++)
        {
            if (i < 10)
                door.transform.localPosition += new Vector3(0, 0, 0.005f);
            else if (i == 10)
            {
                int r = Random.Range(0, 7);
                for (int k = 0; k < 3; k++)
                {
                    brends[k].material = buttonColors[r];
                    displays[k + 1].color = new Color32(0, 0, 0, 255); 
                    displays[k + 1].text = "W";                   
                }
                displays[2].text = "O";
            }
            else if (i > 14)
                door.transform.localPosition -= new Vector3(0, 0, 0.005f);
            yield return new WaitForSeconds(0.1f);
        }
        displays[0].color = new Color32(255, 255, 255, 255);
        displays[0].text = new string[10] { "EXCELLENT", "AWESOME", "AMAZING", "STUPENDOUS", "REMARKABLE", "MAGNIFICENT", "IMPRESSIVE", "PHENOMENAL", "ASTOUNDING", "EXTRAORDINARY" }[Random.Range(0, 10)];
        GetComponent<KMBombModule>().HandlePass();
    }
}
