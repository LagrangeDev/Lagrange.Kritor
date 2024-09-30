# Lagrange.Kritor (WIP)

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
            "Default": "Information" // Log level, please modify to `Trace` when providing feedback on issues
        }
    },
    "Core": {
        "Protocol": {
            "Platform": "Linux", // Protocol platform, please modify according to the Signer version
            "Signer": {
                "Url": "", // Signer server url
                "Proxy": "" // Signer server proxy
            }
        },
        "Server": {
            "AutoReconnect": true, // Whether to automatically reconnect to the TX server
            "GetOptimumServer": true // Whether to get optimum server
        }
    },
    "Kritor": {
        "Network": {
            "Address": "0.0.0.0", // Address of the Kritor service binding
            "Port": 9000 // Port of the Kritor service binding
        },
        "Authentication": {
            "Enabled": false, // Whether to enable authentication
            "SuperTicket": "", // Ticket with maximum privileges
            "Tickets": [] // Ticket list
        },
        "Message": {
            "IgnoreSelf": false // Whether to ignore your own messages
        }
    }
}
```
