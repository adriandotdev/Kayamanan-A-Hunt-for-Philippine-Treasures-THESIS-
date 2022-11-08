using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class SchoolAssetManager : MonoBehaviour
{
    public static SchoolAssetManager instance;

    public Story story;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void StartShowingInfo(TextAsset textAsset)
    {
        this.story =
            new Story(textAsset.text);

        if (story.canContinue)
        {
            string storyText = story.Continue();

            if (storyText == "")
            {
               
            }

            print(storyText);
        }
        else
        {
            
        }
    }

    public void ContinueInfo()
    {
        if (story.canContinue)
        {
            string storyText = story.Continue();

            if (storyText == "")
            {

            }

            print(storyText);
        }
        else
        {

        }
    }

    public void ExitInfo()
    {

    }
}
