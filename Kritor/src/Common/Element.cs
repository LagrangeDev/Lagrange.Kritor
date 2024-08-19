using Google.Protobuf;
using static Kritor.Common.Element.Types;

namespace Kritor.Common;

public partial class Element {
    public static Element CreateText(string text) {
        return new() { Type = ElementType.Text, Text = TextElement.Create(text) };
    }

    public static Element CreateAt(ulong uin) {
        return new() { Type = ElementType.At, At = AtElement.Create(uin) };
    }

    public static Element CreateFace(uint id) {
        return new() { Type = ElementType.Face, Face = FaceElement.Create(id) };
    }

    // public static Element CreateBubbleFace( ... ) {
    //     return new(ElementType.BubbleFace) { BubbleFace = ... };
    // }

    public static Element CreateReply(string messageId) {
        return new() { Type = ElementType.Reply, Reply = ReplyElement.Create(messageId) };
    }

    public static Element CreateCommonFileUrl(string fileUrl, string? fileMd5 = null, uint? subType = null) {
        return new() { Type = ElementType.Image, Image = ImageElement.CreateCommonImageUrl(fileUrl, fileMd5, subType) };
    }

    public static Element CreateVoiceUrl(string fileUrl, string? fileMd5 = null, bool? magic = null) {
        return new() { Type = ElementType.Voice, Voice = VoiceElement.CreateVoiceUrl(fileUrl, fileMd5, magic) };
    }

    public static Element CreateVideoUrl(string fileUrl, string? fileMd5 = null) {
        return new() { Type = ElementType.Video, Video = VideoElement.CreateVideoUrl(fileUrl, fileMd5) };
    }

    // public static Element CreateBasketball( ... ) {
    //     return new() { Type = ElementType.Basketball, Basketball = ... };
    // }

    // public static Element CreateDice( ... ) {
    //     return new() { Type = ElementType.Dice, Dice = ... };
    // }

    // public static Element CreateRps( ... ) {
    //     return new() { Type = ElementType.Rps, Rps = ... };
    // }

    public static Element CreatePoke(uint id, uint pokeType, uint strength) {
        return new() { Type = ElementType.Poke, Poke = PokeElement.Create(id, pokeType, strength) };
    }

    // public static Element CreateMusic( ... ) {
    //     return new() { Type = ElementType.Music, Music = ... };
    // }

    // public static Element CreateWeather( ... ) {
    //     return new() { Type = ElementType.Weather, Weather = ... };
    // }

    // public static Element CreateLocation( ... ) {
    //     return new() { Type = ElementType.Location, Location = ... };
    // }

    // public static Element CreateShare( ... ) {
    //     return new() { Type = ElementType.Share, Share = ... };
    // }

    // public static Element CreateGift( ... ) {
    //     return new() { Type = ElementType.Gift, Gift = ... };
    // }

    // public static Element CreateMarketFace( ... ) {
    //     return new() { Type = ElementType.MarketFace, MarketFace = ... };
    // }

    // public static Element CreateForward( ... ) {
    //     return new() { Type = ElementType.Forward, Forward = ... };
    // }

    // public static Element CreateContact( ... ) {
    //     return new() { Type = ElementType.Contact, Contact = ... };
    // }

    // public static Element CreateJson( ... ) {
    //     return new() { Type = ElementType.Json, Json = ... };
    // }

    // public static Element CreateXml( ... ) {
    //     return new() { Type = ElementType.Xml, Xml = ... };
    // }

    // public static Element CreateFile( ... ) {
    //     return new() { Type = ElementType.File, File = ... };
    // }

    // public static Element CreateMarkdown( ... ) {
    //     return new() { Type = ElementType.Markdown, Markdown = ... };
    // }

    // public static Element CreateKeyboard( ... ) {
    //     return new() { Type = ElementType.Keyboard, Keyboard = ... };
    // }
}