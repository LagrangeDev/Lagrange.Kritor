namespace Kritor.Event;

public partial class PrivatePokeNotice {
    public PrivatePokeNotice SetOperatorUid(string operatorUid) {
        OperatorUid = operatorUid;
        return this;
    }

    public PrivatePokeNotice SetOperatorUin(ulong operatorUin) {
        OperatorUin = operatorUin;
        return this;
    }

    public PrivatePokeNotice SetAction(string action) {
        Action = action;
        return this;
    }

    public PrivatePokeNotice SetSuffix(string suffix) {
        Suffix = suffix;
        return this;
    }
    
    public PrivatePokeNotice SetActionImage(string actionImage) {
        ActionImage = actionImage;
        return this;
    }
}