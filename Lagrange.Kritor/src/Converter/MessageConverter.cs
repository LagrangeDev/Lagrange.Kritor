using System;
using System.Linq;
using Kritor.Common;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

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
            PokeEntity poke => Element.CreatePoke(0, poke.Type, 0), // TODO: strength
            _ => null,
        };
    }
}