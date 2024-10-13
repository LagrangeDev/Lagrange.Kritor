# Lagrange.Kritor

## Get Started

1. Download .NET 8.0 Runtime from [dotnet.microsoft.com](https://dotnet.microsoft.com/en-us/download/dotnet/8.0#:~:text=The%20runtime%20includes%20everything%20you%20need)

2. Download the latest Artifacts from [Actions](https://github.com/LagrangeDev/Lagrange.Kritor/actions/workflows/build.yml)

3. Place `appsettings.json` in your working directory.

4. Modify and write the following to `appsettings.json`

5. Launch

```jsonc
{
    "Logging": {
        "LogLevel": {
            // Log level, please modify to `Trace` when providing feedback on issues
            "Default": "Information"
        }
    },
    "Core": {
        "Protocol": {
            // Protocol platform, please modify according to the Signer version
            // Type: String ("Windows", "MacOs", "Linux")
            // Default: "Linux"
            "Platform": "Linux",
            "Signer": {
                // Signer server url
                // Type: String (HTTP URL, HTTPS URL)
                "Url": "",
                // Signer server proxy
                // Type: String (HTTP URL)
                "Proxy": ""
            }
        },
        "Server": {
            // Whether to automatically reconnect to the TX server
            // Type: bool
            // Default: false
            "AutoReconnect": true,
            // Whether to get optimum server
            // Type: bool
            // Default: false
            "GetOptimumServer": true
        }
    },
    "Kritor": {
        "Network": {
            // Address of the Kritor service binding
            // Type: String (ip)
            "Address": "0.0.0.0",
            // Port of the Kritor service binding
            // Type: Number ([1-65535])
            "Port": 9000
        },
        "Authentication": {
            // Whether to enable authentication
            // Type: bool
            "Enabled": false,
            // Ticket with maximum privileges
            // Type: String
            "SuperTicket": "",
            // Ticket list
            // Type: String[]
            "Tickets": []
        },
        "Message": {
            // Whether to ignore your own messages
            // Type: bool
            "IgnoreSelf": false
        }
    }
}
```

## Grpc Features

### Authentication

| Method                                | Method                    |
| ------------------------------------- | ------------------------- |
| :green_circle: GetAuthenticationState | :red_circle: AddTicket    |
| :green_circle: GetTicket              | :red_circle: DeleteTicket |

### Core

| Method                           | Method                     |
| -------------------------------- | -------------------------- |
| :green_circle: GetVersion        | :red_circle: DownloadFile  |
| :green_circle: GetCurrentAccount | :red_circle: SwitchAccount |

### Event

| Method                                | Method                               |
| ------------------------------------- | ------------------------------------ |
| :green_circle: RegisterActiveListener | :red_circle: RegisterPassiveListener |

### File

| Method                           | Method                    |
| -------------------------------- | ------------------------- |
| :green_circle: DeleteFolder      | :red_circle: CreateFolder |
| :green_circle: UploadFile        | :red_circle: RenameFolder |
| :green_circle: DeleteFile        |                           |
| :green_circle: GetFileSystemInfo |                           |
| :green_circle: GetFileList       |                           |

### Friend

| Method                                | Method                         |
| ------------------------------------- | ------------------------------ |
| :green_circle: GetFriendList          | :red_circle: SetProfileCard    |
| :green_circle: GetFriendProfileCard   | :red_circle: IsBlackListUser   |
| :green_circle: GetStrangerProfileCard | :red_circle: GetUidByUin       |
| :green_circle: VoteUser               | :red_circle: GetUinByUid       |
|                                       | :red_circle: UploadPrivateFile |

### Group

| Method                             | Method                             |
| ---------------------------------- | ---------------------------------- |
| :green_circle: BanMember           | :red_circle: GetProhibitedUserList |
| :green_circle: PokeMember          | :red_circle: GetRemainCountAtAll   |
| :green_circle: KickMember          | :red_circle: GetNotJoinedGroupInfo |
| :green_circle: LeaveGroup          | :red_circle: GetGroupHonor         |
| :green_circle: ModifyMemberCard    | :red_circle: UploadGroupFile       |
| :green_circle: ModifyGroupName     |                                    |
| :green_circle: ModifyGroupRemark   |                                    |
| :green_circle: SetGroupAdmin       |                                    |
| :green_circle: SetGroupUniqueTitle |                                    |
| :green_circle: SetGroupWholeBan    |                                    |
| :green_circle: GetGroupInfo        |                                    |
| :green_circle: GetGroupList        |                                    |
| :green_circle: GetGroupMemberInfo  |                                    |
| :green_circle: GetGroupMemberList  |                                    |

### Guild

| Method | Method                           |
| ------ | -------------------------------- |
|        | :red_circle: GetBotInfo          |
|        | :red_circle: GetChannelList      |
|        | :red_circle: GetGuildMetaByGuest |
|        | :red_circle: GetGuildChannelList |
|        | :red_circle: GetGuildMemberList  |
|        | :red_circle: GetGuildMember      |
|        | :red_circle: SendChannelMessage  |
|        | :red_circle: GetGuildFeedList    |
|        | :red_circle: GetGuildRoleList    |
|        | :red_circle: DeleteGuildRole     |
|        | :red_circle: SetGuildMemberRole  |
|        | :red_circle: UpdateGuildRole     |
|        | :red_circle: CreateGuildRole     |

### Message

| Method                                | Method                             |
| ------------------------------------- | ---------------------------------- |
| :green_circle: SendMessage            | :red_circle: SetMessageReaded      |
| :green_circle: SendMessageByResId     | :red_circle: UploadForwardMessage  |
| :green_circle: RecallMessage          | :red_circle: GetEssenceMessageList |
| :green_circle: ReactMessageWithEmoji  | :red_circle: SetEssenceMessage     |
| :green_circle: GetMessage             | :red_circle: DeleteEssenceMessage  |
| :green_circle: GetMessageBySeq        |                                    |
| :green_circle: GetHistoryMessage      |                                    |
| :green_circle: GetHistoryMessageBySeq |                                    |
| :green_circle: DownloadForwardMessage |                                    |

### Process

| Method | Method                                 |
| ------ | -------------------------------------- |
|        | :red_circle: SetFriendApplyResult      |
|        | :red_circle: SetGroupApplyResult       |
|        | :red_circle: SetInvitedJoinGroupResult |

### Reverse

| Method | Method                     |
| ------ | -------------------------- |
|        | :red_circle: ReverseStream |

### Web

| Method                    | Method                      |
| ------------------------- | --------------------------- |
| :green_circle: GetCookies | :red_circle: GetCredentials |
|                           | :red_circle: GetCSRFToken   |
|                           | :red_circle: GetHttpCookies |

## Event Features

### Core

| Event | Event |
| ----- | ----- |

### Message

| Event                  | Event |
| ---------------------- | ----- |
| :green_circle: Message |       |

### Notice

| Event                                         | Event                                          |
| --------------------------------------------- | ---------------------------------------------- |
| :green_circle: PRIVATE_POKE                   | :red_circle: PRIVATE_FILE_UPLOADED             |
| :green_circle: PRIVATE_RECALL                 | :red_circle: GROUP_FILE_UPLOADED               |
| :green_circle: GROUP_POKE                     | :red_circle: GROUP_CARD_CHANGED                |
| :green_circle: GROUP_RECALL                   | :red_circle: GROUP_MEMBER_UNIQUE_TITLE_CHANGED |
| :green_circle: GROUP_ESSENCE_CHANGED          | :red_circle: GROUP_SIGN_IN                     |
| :green_circle: GROUP_MEMBER_INCREASE          | :red_circle: GROUP_TRANSFER                    |
| :green_circle: GROUP_MEMBER_DECREASE          | :red_circle: FRIEND_INCREASE                   |
| :green_circle: GROUP_ADMIN_CHANGED            | :red_circle: FRIEND_DECREASE                   |
| :green_circle: GROUP_MEMBER_BAN               |                                                |
| :green_circle: GROUP_WHOLE_BAN                |                                                |
| :green_circle: GROUP_REACT_MESSAGE_WITH_EMOJI |                                                |

### Request

| Event                        | Event |
| ---------------------------- | ----- |
| :green_circle: FRIEND_APPLY  |       |
| :green_circle: GROUP_APPLY   |       |
| :green_circle: INVITED_GROUP |       |

## Element Features

### Received

| Element                | Element                  |
| ---------------------- | ------------------------ |
| :green_circle: TEXT    | :red_circle: BUBBLE_FACE |
| :green_circle: AT      | :red_circle: BASKETBALL  |
| :green_circle: FACE    | :red_circle: DICE        |
| :green_circle: REPLY   | :red_circle: RPS         |
| :green_circle: IMAGE   | :red_circle: MUSIC       |
| :green_circle: VOICE   | :red_circle: WEATHER     |
| :green_circle: VIDEO   | :red_circle: LOCATION    |
| :green_circle: POKE    | :red_circle: SHARE       |
| :green_circle: FORWARD | :red_circle: GIFT        |
| :green_circle: JSON    | :red_circle: MARKET_FACE |
| :green_circle: XML     | :red_circle: CONTACT     |
|                        | :red_circle: FILE        |
|                        | :red_circle: MARKDOWN    |
|                        | :red_circle: KEYBOARD    |

### Send

| Element                 | Element                  |
| ----------------------- | ------------------------ |
| :green_circle: TEXT     | :red_circle: BUBBLE_FACE |
| :green_circle: AT       | :red_circle: BASKETBALL  |
| :green_circle: FACE     | :red_circle: DICE        |
| :green_circle: REPLY    | :red_circle: RPS         |
| :green_circle: IMAGE    | :red_circle: MUSIC       |
| :green_circle: VOICE    | :red_circle: WEATHER     |
| :green_circle: VIDEO    | :red_circle: LOCATION    |
| :green_circle: POKE     | :red_circle: SHARE       |
| :green_circle: FORWARD  | :red_circle: GIFT        |
| :green_circle: JSON     | :red_circle: MARKET_FACE |
| :green_circle: XML      | :red_circle: CONTACT     |
| :green_circle: MARKDOWN | :red_circle: FILE        |
| :green_circle: KEYBOARD |                          |

## File Data Features

| Type                     | Type                   |
| ------------------------ | ---------------------- |
| :green_circle: file      | :red_circle: file_name |
| :green_circle: file_path |                        |
| :green_circle: file_url  |                        |
