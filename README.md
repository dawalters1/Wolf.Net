# Wolf.Net
Unofficial C# API For connecting to WOLF (AKA Palringo) 


# Version 5

### What's New
 
  - [Chained Helpers](#helpers)
    - [Authorization](#authorization-)
    - [Banned](#banned-)
    - [Achievement](#achievement-) - WIP
    - [Blocked](#blocked-)
    - [Charm](#charm-)
    - [Contact](#contact-)
    - [Group](#group-)
    - [Messaging](#messaging-)
    - [Notification](#notification-)
    - [Phrase](#phrase-)
    - [Subscriber](#subscriber-)
    - [Tip](#tip-)



## Helpers

#### Authorization ```Bot.Authorization()``` - Allows user ids to be added to bypass permission checks

 - ```Bot.Authorization().List()``` - Returns the list of IDs that are authorized 
 - ```Bot.Authorization().IsAuthorized(int id)``` - Boolean of whether or not a user is authorized 
 - ```Bot.Authorization().Authorize(int id)```/```await Bot.Authorization().Authorize(int[] ids)``` - Authorize a single user or multiple users 
 - ```Bot.Authorization().Deauthorize(int id)```/```await Bot.Authorization().Deauthorize(int[] ids)``` - Unauthorize a single user or multiple users 

#### Banned ```Bot.Banned()``` - Allows user ids to be added to prevent them from using the bot (Messages wont be processed)

 - ```Bot.Banned().List()``` - Returns the list of IDs that are banned 
 - ```Bot.Banned().IsBanned(1)``` - Boolean of whether or not a user is banned 
 - ```Bot.Banned().Ban(int id)```/```await Bot.Authorization().Ban(1,2,3)``` - Ban a single user or multiple users 
 - ```Bot.Banned().Unban(int id)```/```await Bot.Authorization().Unban(int[] ids)``` - Unban a single user or multiple users 

#### Blocked ```Bot.Blocked()``` - Contacts Blocked List Manager

 - ```Bot.Blocked().ListAsync()``` - Returns the list of Contacts that are blocked 
 - ```Bot.Blocked().IsBlockedAsync(int id)``` - See if a user is blocked
 - ```Bot.Blocked().BlockedAsync(int id)``` - Block a user
 - ```Bot.Blocked().UnblockedAsync(int id)``` - Unblock a user 

#### Charm ```Bot.Charm()``` - Charm Manager

 - ```Bot.Charm().ListAsync(Language language)``` - Returns the list of charms 
 - ```Bot.Charm().GetByIdAsync(int id)``` - Returns a charms if it exists
 - ```Bot.Charm().GetByIdsAsync(int[] ids)``` - Returns a list of charms if they exist
 - ```Bot.Charm().GetSubscriberStatisticsAsync(int id)``` - Return subscribers charm statistics (Total Gifted, Total Sent, Overall, ETC)
 - ```Bot.Charm().GetSubscriberExpiredList(int id, int offset = 0, int limit = 25)``` - Returns a list of expired charms for a subscriber
 - ```Bot.Charm().GetSubscriberActiveList(int id, int offset = 0, int limit = 25)``` - Returns a list of active charms for a subscriber


#### Contact ```Bot.Contact()``` - Contacts List Manager

 - ```Bot.Contact().ListAsync()``` - Returns the list of Contacts
 - ```Bot.Contact().IsContactAsync(int id)``` - See if a user is contact
 - ```Bot.Contact().AddAsync(int id)``` - Add a user
 - ```Bot.Contact().DeleteAsync(int id)``` - Delete a user 

#### Group ```Bot.Group()``` - Group Manager

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
 
 #### Messaging ```Bot.Messaging()``` - Messaging Manager

 - ```Bot.Messaging().SendGroupMessageAsync(int groupId, object content, bool encludeEmbeds = false)``` - Send a group message (Text/Image)
 - ```Bot.Messaging().SendPrivateMessageAsync(int subscriberId, object content, bool encludeEmbeds = false)``` - Send a private message (Text/Image)
 - ```Bot.Messaging().DeleteAsync(int targetId, long targetTimestamp, bool isGroup = true)``` - Delete a message (Group Only)
 - ```Bot.Messaging().DeleteAsync(Message message)``` - Delete a message (Group only)
 - ```Bot.Messaging().RestoreAsync(int targetId, long targetTimestamp, bool isGroup = true)``` - Restore a message (Group Only)
 - ```Bot.Messaging().RestoreAsync(Message message)``` - Restore a message (Group only)


---WIP
