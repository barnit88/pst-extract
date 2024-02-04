using System;
using System.ComponentModel;

namespace core.Messaging
{
    public enum SpecialInternalNId : UInt32
    {
        [Description("Message store node")]
        NID_MESSAGE_STORE = 0x21,
        [Description("Named Properties Map")]
        NID_NAME_TO_ID_MAP = 0x61,
        [Description("Special template node for an empty Folder object")]
        NID_NORMAL_FOLDER_TEMPLATE = 0xA1,
        [Description("Special template node for an empty search Folder object")]
        NID_SEARCH_FOLDER_TEMPLATE = 0xC1,
        [Description("Root Mailbox Folder object of PST")]
        NID_ROOT_FOLDER = 0x122,
        [Description("Queue of Pending Search-related updates")]
        NID_SEARCH_MANAGEMENT_QUEUE = 0x1E1,
        [Description("Folder object NIDs with active Search activity")]
        NID_SEARCH_ACTIVITY_LIST = 0x201,
        [Description("Reserved")]
        NID_RESERVED1 = 0x241,
        [Description("Global list of all Folder objects that are referenced by any Folder object's Search Criteria")]
        NID_SEARCH_DOMAIN_OBJECT = 0x261,
        [Description("Search Gatherer Queue")]
        NID_SEARCH_GATHERER_QUEUE = 0x281,
        [Description("Search Gatherer Descriptor")]
        NID_SEARCH_GATHERER_DESCRIPTOR = 0x2A1,
        [Description("Reserved")]
        NID_RESERVED2 = 0x2E1,
        [Description("Reserved")]
        NID_RESERVED3 = 0x301,
        [Description("Search Gatherer Folder Queue")]
        NID_SEARCH_GATHERER_FOLDER_QUEUE = 0x321
    }
    public enum FolderProperty : ushort
    {
        [Description("Display name of the Folder object. PtypString")]
        PidTagDisplayName = 0x3001,
        [Description("Total number of items in the Folder object. PtypInteger32")]
        PidTagContentCount = 0x3602,
        [Description("Number of unread items in the Folder object. PtypInteger32")]
        PidTagContentUnreadCount = 0x3603,
        [Description("Whether the Folder object has any sub-Folder objects. PtypBoolean")]
        PidTagSubfolders = 0x360A,
    }
    public enum NameIDwGuid : byte
    {
        [Description("No GUID(N= 1).")]
        NAMEID_GUID_NONE = 0x0000,
        [Description("The GUID is PS_MAPI([MS-OXPROPS] section 1.3.2).")]
        NAMEID_GUID_MAPI = 0x0001,
        [Description("The GUID is PS_PUBLIC_STRINGS([MS\u0002OXPROPS] section 1.3.2).")]
        NAMEID_GUID_PUBLIC_STRINGS = 0x0002,
        [Description("GUID is found at the(N-3) * 16 byte offset in the GUID Stream.")]
        None = 0x0003,
    }
    public enum ObjectType
    {
        STORE = 0x01,
        ADDRESS_BOOK = 0x02,
        ADDRESS_BOOK_CONTAINER = 0x04,
        MESSAGE_OBJECT = 0x05,
        MAIL_USER = 0x06,
        ATTACHMENT = 0x07,
        DISTRIBUTION_LIST = 0x08
    }
    public enum RecipientType
    {
        FROM = 0x00,
        TO = 0x01,
        CC = 0x02,
        BCC = 0x03
    }
    public enum Importance
    {
        LOW = 0x00,
        NORMAL = 0x01,
        HIGH = 0x02
    }
    public enum Sensitivity
    {
        Normal = 0x00,
        Personal = 0x01,
        Private = 0x02,
        Confidential = 0x03
    }
}
