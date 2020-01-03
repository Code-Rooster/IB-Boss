using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialConditions : MonoBehaviour
{
    public ModifiedDialogue CheckForSpecialConditions(ModifiedDialogue mD)
    {
        ModifiedDialogue newMD = mD;

        string sentence = newMD.sentence;

        string toReplace = null;

        if (sentence.Contains("~YesNo:"))
        {
            bool foundFirstTerm = false;
            bool foundFirstTermName = false;
            bool foundAllFirstTerm = false;
            bool foundSecondTermName = false;
            bool foundAllSecondTerm = false;

            string firstTerm = null;
            string secondTerm = null;

            string firstResponseName = null;
            string secondResponseName = null;

            int firstResponseIndex = 0;
            int secondResponseIndex = 0;

            string firstTermParse = null;
            string secondTermParse = null;

            for (int i = System.Array.IndexOf(sentence.ToCharArray(), ':') + 1; i < sentence.ToCharArray().Length; i++)
            {
                if (foundFirstTerm == false)
                {
                    if (sentence.ToCharArray()[i] != ',')
                    {
                        firstTerm += sentence.ToCharArray()[i];
                    }
                    else
                    {
                        for (int j = System.Array.IndexOf(firstTerm.ToCharArray(), '(') + 1; j < firstTerm.ToCharArray().Length; j++)
                        {
                            if (!foundAllFirstTerm)
                            {
                                if (firstTerm.ToCharArray()[j] != '[' && !foundFirstTermName)
                                {
                                    firstResponseName += firstTerm.ToCharArray()[j];
                                }
                                else
                                {
                                    if (firstTerm.ToCharArray()[j] == '[')
                                    {
                                        foundFirstTermName = true;

                                        j++;
                                    }

                                    if (firstTerm.ToCharArray()[j] != ']')
                                    {
                                        firstTermParse += firstTerm.ToCharArray()[j];
                                    }
                                    else
                                    {
                                        int.TryParse(firstTermParse, out firstResponseIndex);

                                        if (firstResponseIndex < 0)
                                        {
                                            firstResponseIndex = 0;
                                            Debug.LogError("First response index failed to parse properly.");
                                        }

                                        foundAllFirstTerm = true;
                                    }
                                }
                            }
                        }

                        firstTerm = firstTerm.Replace("(" + firstResponseName + "[" + firstResponseIndex.ToString() + "])", "");

                        newMD.yNFirstTerm = firstTerm;

                        foundFirstTerm = true;
                    }
                }
                else
                {
                    if (sentence.ToCharArray()[i] == ',')
                    {
                        i++;
                    }
                    if (sentence.ToCharArray()[i] != '~')
                    {
                        secondTerm += sentence.ToCharArray()[i];
                    }
                    //EDIT HERE
                    else
                    {
                        for (int l = System.Array.IndexOf(secondTerm.ToCharArray(), '(') + 1; l < secondTerm.ToCharArray().Length; l++)
                        {
                            if (!foundAllSecondTerm)
                            {
                                if (secondTerm.ToCharArray()[l] != '[' && !foundSecondTermName)
                                {
                                    secondResponseName += secondTerm.ToCharArray()[l];
                                }
                                else
                                {
                                    if (secondTerm.ToCharArray()[l] == '[')
                                    {
                                        foundSecondTermName = true;

                                        l++;
                                    }

                                    if (secondTerm.ToCharArray()[l] != ']')
                                    {
                                        secondTermParse += secondTerm.ToCharArray()[l];
                                    }
                                    else
                                    {
                                        int.TryParse(secondTermParse, out secondResponseIndex);

                                        foundAllSecondTerm = true;
                                    }
                                }
                            }
                        }

                        secondTerm = secondTerm.Replace("(" + secondResponseName + "[" + secondResponseIndex.ToString() + "])", "");

                        newMD.yNSecondTerm = secondTerm;

                        newMD.yNFirstResponseTriggerName = firstResponseName;
                        newMD.yNSecondResponseTriggerName = secondResponseName;

                        newMD.yNFirstResponseTriggerIndex = firstResponseIndex;
                        newMD.yNSecondResponseTriggerIndex = secondResponseIndex;

                        break;
                    }
                }
            }
            toReplace = "~YesNo:" + firstTerm + "(" + firstResponseName + "[" + firstResponseIndex.ToString() + "])," + secondTerm + "(" + secondResponseName + "[" + secondResponseIndex.ToString() + "])~";

            mD.sentence = sentence.Replace(toReplace, "");

            newMD.yesNoQuestion = true;

            if (sentence.Contains("{Essay}"))
            {
                sentence = sentence.Replace("{Essay}", "");

                newMD.essayQuestion = true;
            }
        }

        return newMD;
    }
}
