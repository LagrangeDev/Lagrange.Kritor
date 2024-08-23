using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml;
using Kritor.Common;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using CoreXmlEntity = Lagrange.Core.Message.Entity.XmlEntity;

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
            TextEntity text => Element.CreateText(text.Text),
            MentionEntity mention => Element.CreateAt(mention.Uin),
            FaceEntity face => Element.CreateFace(face.FaceId),
            ForwardEntity forward => Element.CreateReply($"{new DateTimeOffset(forward.Time).ToUnixTimeSeconds():D32}_{forward.MessageId:D20}_{forward.Sequence:D10}"),
            ImageEntity image => Element.CreateCommonFileUrl(image.ImageUrl),
            RecordEntity record => Element.CreateVoiceUrl(record.AudioUrl),
            VideoEntity video => Element.CreateVideoUrl(video.VideoUrl),
            PokeEntity poke => Element.CreatePoke(0, poke.Type, poke.Strength),
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

        return Element.CreateXml(xml.Xml);
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

        forward = Element.CreateForward(resId, uniseq, summary, description);
        return true;
    }

    public static Element? ToElement(this LightAppEntity lightApp) {
        return Element.CreateJson(lightApp.Payload);
    }
}