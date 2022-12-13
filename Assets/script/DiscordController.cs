using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class DiscordController : MonoBehaviour
{
    public bool isDiscordAvailable;
    public Discord.Discord discord;
    public static DiscordController singleton;

    private void Awake()
    {
        discord = new Discord.Discord(1042246460542033931, (UInt64)Discord.CreateFlags.Default);
        singleton = this;
    }

    void Start()
    {
        if (false == true)
        {
            var unixTimeStamp = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
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

    void Update()
    {
            discord.RunCallbacks();
            Thread.Sleep(1000 / 60);
    }

    private void OnApplicationQuit()
    {
        //discord.Dispose();
    }

}
