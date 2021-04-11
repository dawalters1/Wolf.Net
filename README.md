# WOLF.Net Version 5 -- WIP

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

# Approval

Bots _**MUST**_ be approved by WOLF staff in [bot approval](http://wolflive.com/bot+approval?r=80280172) or [bot approval.ar](http://wolflive.com/bot+approval.ar?r=80280172)
 


# What's New in Version 5
 
  - [Chained Helpers](#helpers)
    - [Authorization](#authorization)
    - [Banned](#banned)
    - [Achievement](#achievement) - WIP
    - [Blocked](#blocked)
    - [Charm](#charm)
    - [Contact](#contact)
    - [Group](#group)
    - [Messaging](#messaging)
    - [Notification](#notification)
    - [Phrase](#phrase)
    - [Subscriber](#subscriber)
    - [Tip](#tip)

 - [New Features](#features)
   - [Embeds](#embeds)
   - [Formatting](#formatting)
   - [Group Achievements](#group-achievements) - WIP
   - [Login Types](#login-types)

 - [Bug Fixes](#bug-fixes)
   - [Command Case Sensitivity](#command-case-sensitivity)
   - [[INVALID EVENT\]: private message accept response](#invalid-event)

## Helpers

#### Authorization 
```Bot.Authorization()``` - Allows user ids to be added to bypass permission checks

 - ```Bot.Authorization().List()``` - Returns the list of IDs that are authorized 
 - ```Bot.Authorization().IsAuthorized(int id)``` - Boolean of whether or not a user is authorized 
 - ```Bot.Authorization().Authorize(int id)```/```await Bot.Authorization().Authorize(int[] ids)``` - Authorize a single user or multiple users 
 - ```Bot.Authorization().Deauthorize(int id)```/```await Bot.Authorization().Deauthorize(int[] ids)``` - Unauthorize a single user or multiple users 
---
#### Banned 
```Bot.Banned()``` - Allows user ids to be added to prevent them from using the bot (Messages wont be processed)

 - ```Bot.Banned().List()``` - Returns the list of IDs that are banned 
 - ```Bot.Banned().IsBanned(1)``` - Boolean of whether or not a user is banned 
 - ```Bot.Banned().Ban(int id)```/```await Bot.Authorization().Ban(1,2,3)``` - Ban a single user or multiple users 
 - ```Bot.Banned().Unban(int id)```/```await Bot.Authorization().Unban(int[] ids)``` - Unban a single user or multiple users 
---
#### Blocked 
```Bot.Blocked()``` - Contacts Blocked List Manager

 - ```Bot.Blocked().ListAsync()``` - Returns the list of Contacts that are blocked 
 - ```Bot.Blocked().IsBlockedAsync(int id)``` - See if a user is blocked
 - ```Bot.Blocked().BlockedAsync(int id)``` - Block a user
 - ```Bot.Blocked().UnblockedAsync(int id)``` - Unblock a user 
---
#### Charm 
```Bot.Charm()``` - Charm Manager

 - ```Bot.Charm().ListAsync(Language language)``` - Returns the list of charms 
 - ```Bot.Charm().GetByIdAsync(int id)``` - Returns a charms if it exists
 - ```Bot.Charm().GetByIdsAsync(int[] ids)``` - Returns a list of charms if they exist
 - ```Bot.Charm().GetSubscriberStatisticsAsync(int id)``` - Return subscribers charm statistics (Total Gifted, Total Sent, Overall, ETC)
 - ```Bot.Charm().GetSubscriberExpiredList(int id, int offset = 0, int limit = 25)``` - Returns a list of expired charms for a subscriber
 - ```Bot.Charm().GetSubscriberActiveList(int id, int offset = 0, int limit = 25)``` - Returns a list of active charms for a subscriber
---
#### Contact
```Bot.Contact()``` - Contacts List Manager

 - ```Bot.Contact().ListAsync()``` - Returns the list of Contacts
 - ```Bot.Contact().IsContactAsync(int id)``` - See if a user is contact
 - ```Bot.Contact().AddAsync(int id)``` - Add a user
 - ```Bot.Contact().DeleteAsync(int id)``` - Delete a user 
---
#### Group 
```Bot.Group()``` - Group Manager

 - ```Bot.Group().CreateAsync()``` - Returns the profile builder class
 - ```Bot.Group().GetByIdAsync(int id)``` - Get a group by ID if it exists
 - ```Bot.Group().GetByIdsAsync(int[] id)``` - Get a groups by list of IDs if they exist
 - ```Bot.Group().GetByNameAsync(string name)``` - Get a group by name if it exists
 - ```Bot.Group().GetSubscribersListAsync(int id)``` - Gets a groups member list
 - ```Bot.Group().JoinAsync(int id, string password = null)``` - Join a group by ID
 - ```Bot.Group().JoinAsync(string name, string password = null)``` - Join a group by name
 - ```Bot.Group().LeaveAsync(int id)``` - Leave a group by ID
 - ```Bot.Group().LeaveAsync(string name)``` - Leave a group by name
 - ```Bot.Group().GetStatsAsync(int id)``` - Gets a list of group stats
 - ```Bot.Group().UpdateGroupSubscriberAsync(int groupId, int subscriberId, ActionType actionType)``` - Update a group members capability/role
 - ```Bot.Group().UpdateAsync(Group group)``` - Returns the profile builder class
 ---
 #### Messaging 
 ```Bot.Messaging()``` - Messaging Manager

 - ```Bot.Messaging().SendGroupMessageAsync(int groupId, object content, bool encludeEmbeds = false)``` - Send a group message (Text/Image)
 - ```Bot.Messaging().SendPrivateMessageAsync(int subscriberId, object content, bool encludeEmbeds = false)``` - Send a private message (Text/Image)
 - ```Bot.Messaging().DeleteAsync(int targetId, long targetTimestamp, bool isGroup = true)``` - Delete a message (Group Only)
 - ```Bot.Messaging().DeleteAsync(Message message)``` - Delete a message (Group only)
 - ```Bot.Messaging().RestoreAsync(int targetId, long targetTimestamp, bool isGroup = true)``` - Restore a message (Group Only)
 - ```Bot.Messaging().RestoreAsync(Message message)``` - Restore a message (Group only)
 - ```Bot.Messaging().RestoreAsync(Message message)``` - Restore a message (Group only)
 - ```Bot.Messaging().SubscribeToNextMessageAsync(Func<Message, bool> fun)``` - Subscribe to a specific message predic
 - ```Bot.Messaging().SubscribeToNextGroupMessageAsync(int groupId)``` - Subscribe to the next group message
 - ```Bot.Messaging().SubscribeToNextPrivateMessageAsync(int subscriberId)``` - Subscribe to the next subscriber message
 - ```Bot.Messaging().SubscribeToNextGroupSubscriberMessageAsync(int subscriberId, int groupId)``` - Subscribe to the next group subscriber message
 - ```Bot.Messaging().GetGroupHistoryAsync(int groupId, long timestamp)``` - Get the last 5 group messages from the given timestamp by group ID
 - ```Bot.Messaging().GetGroupHistoryAsync(Message message)``` - Get the last 5 group messages from the given message timestamp
 - ```Bot.Messaging().GetPrivateHistoryAsync(int subscriberId, long timestamp)``` - Get the last 5 private messages from the given timestamp by subscriber ID
 - ```Bot.Messaging().GetPrivateHistoryAsync(Message message)``` - Get the last 5 private messages from the given message timestamp
 - ```Bot.Messaging().LinkMetadataAsync(Uri uri)``` - Retrieve a links metadata
---
#### Notification 
```Bot.Notification()``` - Notification Manager

 - ```Bot.Notification().ListAsync()``` - Returns the list of notifications (404 if none)
 - ```Bot.Notification().ClearAsync()``` - Clears the the notification list
---
#### Phrase 
```Bot.Phrase()``` - Phrase Manager

 - ```Bot.Phrase().Load(List<Phrase> phrases)``` - Load a list of phrases to be used by the bot
 - ```Bot.Phrase().Load(Phrase[] phrases)``` - Load an array of phrases to be used by the bot
 - ```Bot.Phrase().GetByName(string language, string name)``` - Get a phrase by language and name (Default language is 'en')
 - ```Bot.Phrase().IsRequestedPhrase(string name, string value)``` - See if input is the requested phrase
 - ```Bot.Phrase().GetNameByValue(string value)``` - Gets a phrase name by the given value (Null if not a valid phrase)
---
#### Subscriber 
```Bot.Subscriber()``` - Subscriber Manager

 - ```Bot.Subscriber().GetByIdAsync(int id)``` - Get a subscriber by ID if it exists
 - ```Bot.Subscriber().GetByIdsAsync(List<int> id)``` - Get a list of subscribers by list of IDs if they exist
 - ```Bot.Subscriber().GetByIdsAsync(int[] id)``` - Get a list of subscribers by array of IDs if they exist
---
#### Tip 
```Bot.Tip()``` - Tip Manager

 - ```Bot.Tip().AddTip(int subscriberId, int groupId, long timestamp, ContextType contextType, params TipCharm[] charms)``` - Gift a tip to a subscriber by Subscriber and Group Id
 - ```Bot.Tip().AddTip(Message, params TipCharm[] charms)``` - Gift a tip to a subscriber by message
 - ```Bot.Tip().GetTipDetails(int groupId, long timestamp, ContextType contextType, int limit = 20, int offset = 0)``` - Get a message tip details
 - ```Bot.Tip().GetTipDetails(Message message, int limit = 20, int offset = 0)``` - Get a message tip details
 - ```Bot.Tip().GetTipSummary(int groupId, long timestamp, ContextType contextType, int limit = 20, int offset = 0)``` - Get a message tip summary
 - ```Bot.Tip().GetTipSummary(Message message, int limit = 20, int offset = 0)``` - Get a message tip summary
 - ```Bot.Tip().GetByIdsAsync(int[] id)``` - Get a list of subscribers by array of IDs if they exist
 - ```Bot.Tip().GetGroupLeaderboard(int groupId, TipPeriod tipPeriod = TipPeriod.DAY, TipType tipType = TipType.SUBSCRIBER, TipDirection tipDirection = TipDirection.SENT)``` - Get a specific group tip leaderboard
 - ```Bot.Tip().GetGlobalLeaderboard(TipPeriod tipPeriod = TipPeriod.DAY, TipType tipType = TipType.SUBSCRIBER, TipDirection tipDirection = TipDirection.SENT)``` - Get a specific global tip leaderboard
 - ```Bot.Tip().GetGroupLeaderboardSummary(int groupId, TipPeriod tipPeriod = TipPeriod.DAY, TipType tipType = TipType.SUBSCRIBER, TipDirection tipDirection = TipDirection.SENT)``` - Get a specific group tip leaderboard summary
 - ```Bot.Tip().GetGlobalLeaderboardSummary(TipPeriod tipPeriod = TipPeriod.DAY, TipType tipType = TipType.SUBSCRIBER, TipDirection tipDirection = TipDirection.SENT)``` - Get a specific global tip leaderboard summary


## New Features

### Embeds
WOLF V10.9 Introduces 'embeds' to messages, which allow users to pick or display embeds with group ads or urls

### Formatting
WOLF V10.9 Introduces 'formatting' which allows clients to easily determine where group ads or urls are located at in a message

### Group Achievements 
WORK IN PROGRESS
WOLF V10.8 Introduced 'group achievements'

### Login Types
As requested, I have added support for logging into the various login types that WOLF has supported over the past year
 - How do I get the login information?
   - Chrome or Firefox Developer console -> Network -> WS -> Login -> Retreive the email and password from the login packet
 - How do I login with this information?
   - Simple: ```await Bot.LoginAsync("login email", "login password", loginType: LoginType.YOUR_TYPE)```


## Bug Fixes

### Command Case Sensitivity
```[Command("your command")]``` case sensitivity has been addressed

### Invalid Event
Event 'private message accept response' now has an event ```Bot.On.PrivateMessageRequestAccepted +=(subscriber)=>{}```


# Known Issues
- Cannot join Stages
- Cannot update avatars
- Cannot receive new notifications
- Cannot send Voice Messages or GIFS
