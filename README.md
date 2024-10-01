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
