namespace Kritor.Core;

public partial class GetCurrentAccountResponse {
    public GetCurrentAccountResponse SetAccountUid(string accountUid) {
        AccountUid = accountUid;
        return this;
    }
    
    public GetCurrentAccountResponse SetAccountUin(string accountUin) {
        AccountUid = accountUin;
        return this;
    }

    public GetCurrentAccountResponse SetAccountName(string accountName) {
        AccountName = accountName;
        return this;
    }
}