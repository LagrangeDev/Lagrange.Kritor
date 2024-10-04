using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Google.Protobuf.Collections;
using Kritor.Common;
using Lagrange.Core;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using Lagrange.Kritor.Utilities;
using CoreAction = Lagrange.Core.Message.Entity.Action;
using CoreButton = Lagrange.Core.Message.Entity.Button;
using CoreXmlEntity = Lagrange.Core.Message.Entity.XmlEntity;
using KritorXmlElement = Kritor.Common.XmlElement;

namespace Lagrange.Kritor.Converters;

public static class MessageConverter {
    public static PushMessageBody ToPushMessageBody(this MessageChain chain) {
        return chain.Type switch {
            MessageChain.MessageType.Group => new PushMessageBody {
                Time = (ulong)new DateTimeOffset(chain.Time).ToUnixTimeSeconds(),
                MessageId = MessageIdUtility.BuildPrivateMessageId(chain.FriendUin, chain.Sequence),
                MessageSeq = chain.Sequence,
                Scene = Scene.Group,
                Group = new GroupSender {
                    GroupId = chain.GroupUin?.ToString() ?? throw new Exception($"`MessageChain.GroupUin` is null"),
                    Uin = chain.FriendUin,
                    Nick = chain.GroupMemberInfo?.MemberName ?? throw new Exception($"`MessageChain.GroupMemberInfo` is null")
                },
                Elements = { chain.ToElements() }
            },
            MessageChain.MessageType.Temp => throw new NotSupportedException(
                $"Not supported MessageChain.MessageType({MessageChain.MessageType.Temp})"
            ),
            MessageChain.MessageType.Friend => new PushMessageBody {
                Time = (ulong)new DateTimeOffset(chain.Time).ToUnixTimeSeconds(),
                MessageId = MessageIdUtility.BuildPrivateMessageId(chain.FriendUin, chain.Sequence),
                MessageSeq = chain.Sequence,
                Scene = Scene.Friend,
                Private = new PrivateSender {
                    Uin = chain.FriendUin,
                    Nick = chain.FriendInfo?.Nickname ?? throw new Exception($"`MessageChain.FriendInfo` is null")
                },
                Elements = { chain.ToElements() }
            },
            _ => throw new NotSupportedException(
                $"Not supported MessageChain.MessageType({chain.Type})"
            ),
        };
    }

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
                        _ => throw new NotSupportedException($"Not supported MessageChain.MessageType({chain.Type})")
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

    public static async Task<MessageChain> ToGroupChainAsync(this RepeatedField<Element> elements, BotContext bot, uint groupUin, CancellationToken token) {
        MessageBuilder builder = MessageBuilder.Group(groupUin);
        foreach (Element element in elements) {
            await builder.AddElementAsync(bot, element, token);
        }
        return builder.Build();
    }

    public static async Task<MessageChain> ToFriendChainAsync(this RepeatedField<Element> elements, BotContext bot, uint userUin, CancellationToken token) {
        MessageBuilder builder = MessageBuilder.Friend(userUin);
        foreach (Element element in elements) {
            await builder.AddElementAsync(bot, element, token);
        }
        return builder.Build();
    }

    public static async Task<MessageBuilder> AddElementAsync(this MessageBuilder builder, BotContext bot, Element element, CancellationToken token) {
        return element.Type switch {
            Element.Types.ElementType.Unspecified => throw new NotSupportedException(
                $"Not supported Element.Types.ElementType({Element.Types.ElementType.Unspecified})"
            ),
            Element.Types.ElementType.Text => builder.Text(element.Text.Text),
            Element.Types.ElementType.At => builder.Mention(
                element.At.HasUin ? (uint)element.At.Uin : throw new Exception("Not support uin is null")
            ),
            Element.Types.ElementType.Face => builder.Face(
                (ushort)element.Face.Id,
                element.Face.HasIsBig && element.Face.IsBig
            ),
            Element.Types.ElementType.BubbleFace => throw new NotSupportedException(
                $"Not supported Element.Types.ElementType({Element.Types.ElementType.BubbleFace})"
            ),
            Element.Types.ElementType.Reply => builder.Forward(await bot.GetMessageByMessageIdAsync(element.Reply.MessageId, token)),
            Element.Types.ElementType.Image => element.Image.DataCase switch {
                ImageElement.DataOneofCase.None => throw new NotSupportedException(
                    $"Not supported ImageElement.DataOneofCase({ImageElement.DataOneofCase.None})"
                ),
                ImageElement.DataOneofCase.File => builder.Image([.. element.Image.File]),
                ImageElement.DataOneofCase.FileName => throw new NotSupportedException(
                    $"Not supported ImageElement.DataOneofCase({ImageElement.DataOneofCase.FileName})"
                ),
                ImageElement.DataOneofCase.FilePath => builder.Image(element.Image.FilePath),
                ImageElement.DataOneofCase.FileUrl => builder.Image(HttpClientUtility.GetBytes(element.Image.FileUrl)),
                _ => throw new NotSupportedException(
                    $"Not supported ImageElement.DataOneofCase({element.Image.DataCase})"
                ),
            },
            Element.Types.ElementType.Voice => element.Voice.DataCase switch {
                VoiceElement.DataOneofCase.None => throw new NotSupportedException(
                    $"Not supported VoiceElement.DataOneofCase({VoiceElement.DataOneofCase.None})"
                ),
                VoiceElement.DataOneofCase.File => builder.Record([.. element.Voice.File]),
                VoiceElement.DataOneofCase.FileName => throw new NotSupportedException(
                    $"Not supported VoiceElement.DataOneofCase({VoiceElement.DataOneofCase.FileName})"
                ),
                VoiceElement.DataOneofCase.FilePath => builder.Record(element.Voice.FilePath),
                VoiceElement.DataOneofCase.FileUrl => builder.Record(HttpClientUtility.GetBytes(element.Voice.FileUrl)),
                _ => throw new NotSupportedException(
                    $"Not supported VoiceElement.DataOneofCase({element.Voice.DataCase})"
                ),
            },
            Element.Types.ElementType.Video => element.Video.DataCase switch {
                VideoElement.DataOneofCase.None => throw new NotSupportedException(
                    $"Not supported VideoElement.DataOneofCase({VideoElement.DataOneofCase.None})"
                ),
                VideoElement.DataOneofCase.File => builder.Video([.. element.Video.File]),
                VideoElement.DataOneofCase.FileName => throw new NotSupportedException(
                    $"Not supported VideoElement.DataOneofCase({VideoElement.DataOneofCase.FileName})"
                ),
                VideoElement.DataOneofCase.FilePath => builder.Video(element.Video.FilePath),
                VideoElement.DataOneofCase.FileUrl => builder.Video(HttpClientUtility.GetBytes(element.Video.FileUrl)),
                _ => throw new NotSupportedException(
                    $"Not supported VideoElement.DataOneofCase({element.Video.DataCase})"
                ),
            },
            Element.Types.ElementType.Basketball => throw new NotSupportedException(
                $"Not supported Element.Types.ElementType({Element.Types.ElementType.Basketball})"
            ),
            Element.Types.ElementType.Dice => throw new NotSupportedException(
                $"Not supported Element.Types.ElementType({Element.Types.ElementType.Dice})"
            ),
            Element.Types.ElementType.Rps => throw new NotSupportedException(
                $"Not supported Element.Types.ElementType({Element.Types.ElementType.Rps})"
            ),
            Element.Types.ElementType.Poke => builder.Poke(element.Poke.Id, element.Poke.Strength),
            Element.Types.ElementType.Music => throw new NotSupportedException(
                $"Not supported Element.Types.ElementType({Element.Types.ElementType.Music})"
            ), // TODO
            Element.Types.ElementType.Weather => throw new NotSupportedException(
                $"Not supported Element.Types.ElementType({Element.Types.ElementType.Weather})"
            ),
            Element.Types.ElementType.Location => throw new NotSupportedException(
                $"Not supported Element.Types.ElementType({Element.Types.ElementType.Location})"
            ),
            Element.Types.ElementType.Share => throw new NotSupportedException(
                $"Not supported Element.Types.ElementType({Element.Types.ElementType.Share})"
            ),
            Element.Types.ElementType.Gift => throw new NotSupportedException(
                $"Not supported Element.Types.ElementType({Element.Types.ElementType.Gift})"
            ),
            Element.Types.ElementType.MarketFace => throw new NotSupportedException(
                $"Not supported Element.Types.ElementType({Element.Types.ElementType.MarketFace})"
            ), // Kritor miss arg
            Element.Types.ElementType.Forward => await builder.AddForwardElementAsync(bot, element.Forward),
            Element.Types.ElementType.Contact => throw new NotSupportedException(
                $"Not supported Element.Types.ElementType({Element.Types.ElementType.Contact})"
            ),
            Element.Types.ElementType.Json => builder.Add(new JsonEntity(element.Json.Json)),
            Element.Types.ElementType.Xml => builder.Add(new CoreXmlEntity(element.Xml.Xml)),
            Element.Types.ElementType.File => throw new NotSupportedException(
                $"Not supported Element.Types.ElementType({Element.Types.ElementType.Contact})"
            ), // Please use file related APIs
            Element.Types.ElementType.Markdown => builder.Markdown(new MarkdownData {
                Content = element.Markdown.Markdown
            }),
            Element.Types.ElementType.Keyboard => builder.Keyboard(new KeyboardData {
                Rows = element.Keyboard.Rows
                    .Select(row => new Row {
                        Buttons = row.Buttons
                            .Select(button => new CoreButton {
                                Id = button.Id,
                                RenderData = new RenderData {
                                    Label = button.RenderData.Label,
                                    VisitedLabel = button.RenderData.VisitedLabel,
                                    Style = button.RenderData.Style
                                },
                                Action = new CoreAction {
                                    Type = button.Action.Type,
                                    Permission = new Permission {
                                        Type = button.Action.Permission.Type,
                                        SpecifyRoleIds = [.. button.Action.Permission.RoleIds],
                                        SpecifyUserIds = [.. button.Action.Permission.UserIds],
                                    },
                                    UnsupportTips = button.Action.UnsupportedTips,
                                    Data = button.Action.Data,
                                    Reply = button.Action.Reply,
                                    Enter = button.Action.Enter
                                }
                            })
                            .ToList()
                    })
                    .ToList()
            }),
            _ => throw new NotSupportedException($"Not supported Element.Types.ElementType({element.Type})")
        };
    }

    public static async Task<MessageBuilder> AddForwardElementAsync(this MessageBuilder builder, BotContext bot, ForwardElement element) {
        (int code, List<MessageChain>? chains) = await bot.GetMessagesByResId(element.ResId);
        if (code != 0) throw new Exception($"Get message by res id failed");

        return builder.MultiMsg([.. chains]);
    }
}