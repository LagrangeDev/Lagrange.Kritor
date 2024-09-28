using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml;
using Kritor.Common;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.Kritor.Utilities;
using CoreXmlEntity = Lagrange.Core.Message.Entity.XmlEntity;
using KritorXmlElement = Kritor.Common.XmlElement;

namespace Lagrange.Kritor.Converters;

public static class MessageConverter {
    public static List<Element> ToElements(this MessageChain chain) {
        return chain.Aggregate(
            new List<Element>(),
            (list, entity) => list.AddEntity(chain, entity)
        );
    }

    private static List<Element> AddEntity(this List<Element> list, MessageChain chain, IMessageEntity entity) {
        return entity switch {
            TextEntity text => list.AddWithReturnSelf(new Element {
                Type = Element.Types.ElementType.Text,
                Text = new TextElement {
                    Text = text.Text,
                }
            }),
            MentionEntity mention => list.AddWithReturnSelf(new Element {
                Type = Element.Types.ElementType.At,
                At = new AtElement {
                    Uin = mention.Uin
                }
            }),
            FaceEntity face => list.AddWithReturnSelf(new Element {
                Type = Element.Types.ElementType.Face,
                Face = new FaceElement {
                    Id = face.FaceId,
                    IsBig = face.IsLargeFace
                }
            }),
            ForwardEntity forward => list.AddWithReturnSelf(new Element {
                Type = Element.Types.ElementType.Reply,
                Reply = new ReplyElement {
                    MessageId = chain.Type switch {
                        MessageChain.MessageType.Temp or
                        MessageChain.MessageType.Friend => MessageIdUtility.BuildPrivateMessageId(
                            chain.FriendUin,
                            forward.Sequence
                        ),
                        MessageChain.MessageType.Group => MessageIdUtility.BuildGroupMessageId(
                            chain.GroupUin ?? throw new Exception("MessageChain.GroupUin is null"),
                            forward.Sequence
                        ),
                        MessageChain.MessageType type => throw new Exception(
                            $"Unknown MessageChain.MessageType: {type}"
                        )
                    }
                }
            }),
            ImageEntity image => list.AddWithReturnSelf(new Element {
                Type = Element.Types.ElementType.Image,
                Image = new ImageElement {
                    FileUrl = image.ImageUrl,
                    FileMd5 = Convert.ToHexString(image.ImageMd5),
                    SubType = (uint)image.SubType
                }
            }),
            RecordEntity record => list.AddWithReturnSelf(new Element {
                Type = Element.Types.ElementType.Voice,
                Voice = new VoiceElement {
                    FileUrl = record.AudioUrl,
                    FileMd5 = Convert.ToHexString(record.AudioMd5)
                }
            }),
            VideoEntity video => list.AddWithReturnSelf(new Element {
                Type = Element.Types.ElementType.Video,
                Video = new VideoElement {
                    FileUrl = video.VideoUrl,
                    FileMd5 = video.VideoHash
                }
            }),
            PokeEntity poke => list.AddWithReturnSelf(new Element {
                Type = Element.Types.ElementType.Poke,
                Poke = new PokeElement {
                    Id = 1,
                    PokeType = 0,
                    Strength = poke.Strength
                }
            }),
            CoreXmlEntity xml => list.AddXmlEntity(xml),
            JsonEntity json => list.AddJsonEntity(json),
            _ => list,
        };
    }

    public static List<Element> AddXmlEntity(this List<Element> list, CoreXmlEntity xml) {
        XmlDocument document = new();
        document.LoadXml(xml.Xml);

        if (document.TryToForwardElement(out Element? forward)) {
            return list.AddWithReturnSelf(forward);
        }

        return list.AddWithReturnSelf(new Element {
            Type = Element.Types.ElementType.Xml,
            Xml = new KritorXmlElement {
                Xml = xml.Xml
            }
        });
    }

    public static List<Element> AddJsonEntity(this List<Element> list, JsonEntity json) {
        return list.AddWithReturnSelf(new Element {
            Type = Element.Types.ElementType.Json,
            Json = new JsonElement {
                Json = json.Json
            }
        });
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

        forward = new Element {
            Type = Element.Types.ElementType.Forward,
            Forward = new ForwardElement {
                ResId = resId,
                Uniseq = uniseq,
                Summary = summary,
                Description = description
            }
        };
        return true;
    }
}