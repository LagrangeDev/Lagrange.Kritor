using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml;
using Kritor.Common;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.Kritor.Utility;
using static Kritor.Common.Element.Types;
using static Kritor.Common.ImageElement.Types;
using CoreXmlEntity = Lagrange.Core.Message.Entity.XmlEntity;
using KritorXmlElement = Kritor.Common.XmlElement;

namespace Lagrange.Kritor.Converter;

public static class MessageConverter {
    public static Element[] ToElements(this MessageChain chain) {
        return chain
            .Select((entity) => entity.ToElement())
            .Where((element) => element != null)
            .Select((element) => element!)
            .ToArray();
    }

    public static Element? ToElement(this IMessageEntity entity) {
        return entity switch {
            TextEntity text => new Element()
                .SetType(ElementType.Text)
                .SetTextElement(new TextElement()
                    .SetText(text.Text)
                ),
            MentionEntity mention => new Element()
                .SetType(ElementType.At)
                .SetAtElement(new AtElement()
                    .SetUin(mention.Uin)
                ),
            FaceEntity face => new Element()
                .SetType(ElementType.Face)
                .SetFaceElement(new FaceElement()
                    .SetId(face.FaceId)
                    .SetIsBig(face.IsLargeFace)
                ),
            ForwardEntity forward => new Element()
                .SetType(ElementType.Reply)
                .SetReplyElement(new ReplyElement()
                    .SetMessageId(MessageIdUtility.BuildMessageId(forward.Time, forward.Sequence))
                ),
            ImageEntity image => new Element()
                .SetType(ElementType.Image)
                .SetImageElement(new ImageElement()
                    .SetFileUrl(image.ImageUrl)
                    // .SetFileMd5(image.ImageHash) // TODO: Lagrange NotSupport
                    // .SetSubType(image.SubType) // TODO: Lagrange IneternalValue
                    .SetFileType(ImageType.Common)
                ),
            RecordEntity record => new Element()
                .SetType(ElementType.Voice)
                .SetVoiceElement(new VoiceElement()
                    .SetFileUrl(record.AudioUrl)
                // .SetFileMd5(record.AudioHash) // TODO: Lagrange NotSupport
                // .SetMagic(record.IsMagic) // TODO: Lagrange NotSupport
                ),
            VideoEntity video => new Element()
                .SetType(ElementType.Video)
                .SetVideoElement(new VideoElement()
                    .SetFileUrl(video.VideoUrl)
                    .SetFileMd5(video.VideoHash)
                ),
            PokeEntity poke => new Element()
                .SetType(ElementType.Poke)
                .SetPokeElement(new PokeElement() // TODO: Lagrange NotSupport Id 23 Poke
                    .SetId(1)
                    .SetPokeType(0)
                    .SetStrength(poke.Strength)
                ),
            CoreXmlEntity xml => xml.ToElement(),
            LightAppEntity lightApp => lightApp.ToElement(),
            _ => null,
        };
    }

    public static Element? ToElement(this CoreXmlEntity xml) {
        XmlDocument document = new();
        document.LoadXml(xml.Xml);

        if (document.TryToForwardElement(out Element? forward)) {
            return forward;
        }

        return new Element().SetType(ElementType.Xml).SetXmlElement(new KritorXmlElement().SetXml(xml.Xml));
    }

    public static bool TryToForwardElement(this XmlDocument document, [NotNullWhen(true)] out Element? forward) {
        forward = null;

        XmlNode? msgNode = document.SelectSingleNode("/msg");
        if (msgNode == null) return false;

        XmlNode? summaryNode = document.SelectSingleNode("/msg/item/summary");
        if (summaryNode == null) return false;

        string? resId = msgNode.Attributes?["m_resid"]?.Value;
        if (resId == null) return false;

        string? uniseq = msgNode.Attributes?["m_fileName"]?.Value;
        if (uniseq == null) return false;

        string summary = summaryNode.InnerText;

        string? description = msgNode.Attributes?["brief"]?.Value;
        if (description == null) return false;

        forward = new Element()
            .SetType(ElementType.Forward)
            .SetForwardElement(new ForwardElement()
                .SetResId(resId)
                .SetUniseq(uniseq)
                .SetSummary(summary)
                .SetDescription(description)
            );
        return true;
    }

    public static Element? ToElement(this LightAppEntity lightApp) {
        return new Element()
            .SetType(ElementType.Json)
            .SetJsonElement(new JsonElement()
                .SetJson(lightApp.Payload)
            );
    }
}