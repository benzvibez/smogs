using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class DiscordController : MonoBehaviour
{
    public static bool isDiscordAvailable;
    public bool setStatus;
    private Discord.Discord discord;
    void Start()
    {
        if (false)//set this to true when the game is shipped
        {
            var unixTimeStamp = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            discord = new Discord.Discord(1042246460542033931, (UInt64)Discord.CreateFlags.Default);
            var activityManager = discord.GetActivityManager();
            var acitivity = new Discord.Activity
            {

                State = "building that shit",
                Details = "building ong",
                Assets =
            {
                LargeImage = "1",
                LargeText = "fr fr"
            },
                Timestamps =
            {
                Start = (int)unixTimeStamp
            }
            };

            activityManager.UpdateActivity(acitivity, (res) =>
            {
                if (res != Discord.Result.Ok)
                {
                    isDiscordAvailable = false;
                    print("Discord is fucking dying");
                    discord.Dispose();
                }
                else if (res == Discord.Result.Ok)
                {
                    isDiscordAvailable = true;
                    disableDiscord = true;
                    print("Discord Available!");
                }
            });
        }
    }


public string GetDiscordUsername()
{
    var userManager = discord.GetUserManager();

    return userManager.GetCurrentUser().Username;
}

    private bool disableDiscord;
void Update()
    {
        if (setStatus)
        {
            setStatus = false;
            var unixTimeStamp = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            discord = new Discord.Discord(1042246460542033931, (UInt64)Discord.CreateFlags.Default);
            var activityManager = discord.GetActivityManager();
            var acitivity = new Discord.Activity
            {

                State = "building that shit",
                Details = "building ong",
                Assets =
            {
                LargeImage = "smongs_logo",
                LargeText = "fr fr"
            },
                Timestamps =
            {
                Start = (int)unixTimeStamp
            }
            };

            activityManager.UpdateActivity(acitivity, (res) =>
            {
                if (res != Discord.Result.Ok)
                {
                    isDiscordAvailable = false;
                    print("Discord is fucking dying");
                    //discord.Dispose();
                }
                else if (res == Discord.Result.Ok)
                {
                    isDiscordAvailable = true;
                    disableDiscord = true;
                    print("Discord Available!");
                }
            });
        }

        if (!disableDiscord)
        {
            discord.RunCallbacks();
            Thread.Sleep(1000 / 60);
        }
}

private void OnApplicationQuit()
{
    //discord.Dispose();
}

}
