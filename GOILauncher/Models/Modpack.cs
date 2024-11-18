﻿using Avalonia.Interactivity;
using Downloader;
using FluentAvalonia.UI.Controls;
using GOILauncher.Helpers;
using GOILauncher.Interfaces;
using Ionic.Zip;
using LeanCloud.Storage;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GOILauncher.Models
{
    public class Modpack() : Mod(nameof(Modpack))
    {

    }
}
