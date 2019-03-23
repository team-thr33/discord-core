using Discord;
using DiscordCore.Interfaces;
using System;
using System.IO;

namespace DiscordCore
{
    public static class Helpers
    {
        private static Stream mOutputStream = null;
        private static TextWriter mOutput = null;

        public enum LogLevel
        {
            None,
            Verbose,
            Debug
        }

        public static void Initialize(Stream s)
        {
            mOutputStream = s;
            mOutput = new StreamWriter(mOutputStream);
        }

        public static void LogDebug(string outputString)
        {
            Log(LogLevel.Debug, outputString);
        }

        public static void LogVerbose(string outputString)
        {
            Log(LogLevel.Verbose, outputString);
        }

        public static void Log(string outputString)
        {
            Log(LogLevel.None, outputString);
        }

        public static void Log(LogLevel level, string outputString)
        {
            // If never initialized, default to stdout
            if (mOutputStream == null)
                mOutput = Console.Out;
            mOutput.WriteLine($"{level.ToString()}: {outputString}");
        }

        public static UserState ToUserState(this UserStatus status)
        {
            switch(status)
            {
                case UserStatus.AFK:
                    return UserState.AFK;
                case UserStatus.DoNotDisturb:
                    return UserState.DoNotDisturb;
                case UserStatus.Idle:
                    return UserState.Idle;
                case UserStatus.Invisible:
                    return UserState.Invisible;
                case UserStatus.Offline:
                    return UserState.Offline;
                case UserStatus.Online:
                    return UserState.Online;
            }
            return UserState.Offline;
        }
    }
}