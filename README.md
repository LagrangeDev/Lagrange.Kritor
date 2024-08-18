# Lagrange.Kritor

## Update Submodule

```bash
$ git submodule update --remote
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
            "AutoReconnect": true,
            "GetOptimumServer": true,
            "SignerUrl": ""
        }
    },
    "Kritor": {
        "Network": {
            "Address": "0.0.0.0",
            "Port": 9000
        },
        "Message": {
            "IgnoreSelf": false
        }
    }
}
```