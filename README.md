# Lagrange.Kritor

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
                "Url": "",
                "Proxy": "http://127.0.0.1:9090"
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
