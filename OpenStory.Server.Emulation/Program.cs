﻿using System;
using System.Threading;

namespace OpenStory.Server.Emulation
{
    class Program
    {
        static void Main(string[] args)
        {
            Emulator emulator = new Emulator();
            emulator.Start();
        }
    }
}
