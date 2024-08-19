namespace Kritor.Core;

public partial class GetCurrentAccountResponse {
    private GetCurrentAccountResponse(string account_uid, ulong account_uin, string account_name) {
        AccountUid = account_uid;
        AccountUin = account_uin;
        AccountName = account_name;
    }

    public static GetCurrentAccountResponse Create(string account_uid, ulong account_uin, string account_name) {
        return new(account_uid, account_uin, account_name);
    }
}