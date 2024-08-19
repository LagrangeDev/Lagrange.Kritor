namespace Kritor.Common;

public partial class Contact {
    public static Contact CreateGroup(string peer) {
        return new() { Scene = Scene.Group, Peer = peer };
    }
}