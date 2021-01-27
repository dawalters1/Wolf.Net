# WOLF.Net

**Current Release:** [![Nuget Status](https://img.shields.io/nuget/v/wolf.net?style=flat-square)](https://www.nuget.org/packages/Wolf.Net/)![Nuget Downloads](https://img.shields.io/nuget/dt/wolf.net?style=flat-square)

**Prerelease:** [![Nuget Status](https://img.shields.io/nuget/vpre/wolf.net?style=flat-square)](https://www.nuget.org/packages/Wolf.Net/)

[![WOLF](https://i.imgur.com/SwV8IYZ.png)](https://wolf.live/)

An unofficial .NET API for [WOLF](https://wolf.live.com/) (AKA Palringo).

# Installation 

You can install the WOLF.Net package directly from [Nuget](https://www.nuget.org/packages/Wolf.Net/) or by using Install-Package Wolf.Net
 
# Required

- [Net 5](https://dotnet.microsoft.com/download/dotnet/5.0) or above
- [Net Core 3.0](https://dotnet.microsoft.com/download/dotnet-core/3.0) or above
- [Visual Studio 2019](https://docs.microsoft.com/en-us/visualstudio/windows/?view=vs-2019) OR [VS Code](https://code.visualstudio.com/download)

# Optional

- [Redis](https://redis.io/download)
  - I recommend using this if you plan on making a game bot
  - Redis isnt supported on windows anymore, I recommend using this [Port](https://github.com/tporadowski/redis/releases/download/v5.0.10/Redis-x64-5.0.10.msi)
  - A generic Redis client wrapper package is available for bots Install-Package WOLF.Net.Redis

# Getting Started

 **Planning on supporting multiple languages?**
   - Check out the translation [example project](https://github.com/dawalters1/Wolf.Net/tree/main/WOLF.Net.Example.Translations) to get started 

 **Plan on hardcoding a single language?**
   - Check out the [example project](https://github.com/dawalters1/Wolf.Net/tree/main/WOLF.Net.Example.NoTranslations) to get started

# Approval

Bots _**MUST**_ be approved by WOLF staff in [bot approval](http://wolflive.com/bot+approval?r=80280172) or [bot approval.ar](http://wolflive.com/bot+approval.ar?r=80280172)
 
# Known Issues/Lacking features

- Cannot join Stages
- Cannot update avatars
- Cannot receive new notifications
- Cannot send Voice Messages
