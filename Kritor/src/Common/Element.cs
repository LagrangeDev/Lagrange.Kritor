using static Kritor.Common.Element.Types;

namespace Kritor.Common;

public partial class Element {
    public Element SetType(ElementType type) {
        Type = type;
        return this;
    }

    public Element SetTextElement(TextElement text) {
        Text = text;
        return this;
    }

    public Element SetAtElement(AtElement at) {
        At = at;
        return this;
    }

    public Element SetFaceElement(FaceElement face) {
        Face = face;
        return this;
    }

    public Element SetBubbleFaceElement(BubbleFaceElement bubbleFace) {
        BubbleFace = bubbleFace;
        return this;
    }

    public Element SetReplyElement(ReplyElement reply) {
        Reply = reply;
        return this;
    }

    public Element SetImageElement(ImageElement image) {
        Image = image;
        return this;
    }

    public Element SetVoiceElement(VoiceElement voice) {
        Voice = voice;
        return this;
    }

    public Element SetVideoElement(VideoElement video) {
        Video = video;
        return this;
    }

    public Element SetBasketballElement(BasketballElement basketball) {
        Basketball = basketball;
        return this;
    }

    public Element SetDiceElement(DiceElement dice) {
        Dice = dice;
        return this;
    }

    public Element SetRpsElement(RpsElement rps) {
        Rps = rps;
        return this;
    }

    public Element SetPokeElement(PokeElement poke) {
        Poke = poke;
        return this;
    }

    public Element SetMusicElement(MusicElement music) {
        Music = music;
        return this;
    }

    public Element SetWeatherElement(WeatherElement weather) {
        Weather = weather;
        return this;
    }

    public Element SetLocationElement(LocationElement location) {
        Location = location;
        return this;
    }

    public Element SetShareElement(ShareElement share) {
        Share = share;
        return this;
    }

    public Element SetGiftElement(GiftElement gift) {
        Gift = gift;
        return this;
    }

    public Element SetMarketFaceElement(MarketFaceElement marketFace) {
        MarketFace = marketFace;
        return this;
    }

    public Element SetForwardElement(ForwardElement forward) {
        Forward = forward;
        return this;
    }

    public Element SetContactElement(ContactElement contact) {
        Contact = contact;
        return this;
    }

    public Element SetJsonElement(JsonElement json) {
        Json = json;
        return this;
    }

    public Element SetXmlElement(XmlElement xml) {
        Xml = xml;
        return this;
    }

    public Element SetFileElement(FileElement file) {
        File = file;
        return this;
    }

    public Element SetMarkdownElement(MarkdownElement markdown) {
        Markdown = markdown;
        return this;
    }

    public Element SetKeyboardElement(KeyboardElement keyboard) {
        Keyboard = keyboard;
        return this;
    }
}