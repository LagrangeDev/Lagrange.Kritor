# Lagrange.Kritor

# WIP

## Update Submodule

```bash
git submodule update --remote
```

## Config

```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Trace"
        }
    },
    "Core": {
        "Protocol": {
            "Platform": "Linux",
            "Signer":{
                "Url": "https://sign.lagrangecore.org/api/sign/25765",
                "Proxy": ""
            }
        },
        "Server": {
            "AutoReconnect": true,
            "GetOptimumServer": true
        }
    },
    "Kritor": {
        "Network": {
            "Address": "0.0.0.0",
            "Port": 9000
        },
        "Authentication": {
            "Enabled": false,
            "SuperTicket": "",
            "Tickets": []
        },
        "Message": {
            "IgnoreSelf": false
        }
    }
}

```
