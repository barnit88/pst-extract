using System.ComponentModel;

namespace core.LTP.PropertyContext
{
    public enum RequireProperties : ushort
    {
        [Description("Record key.This is the Provider UID of this PST.")]
        PidTagRecordKey = 0x0FF9,
        [Description("Display name of PST")]
        PidTagDisplayName = 0x3001,
        [Description("EntryID of the Root Mailbox Folder object")]
        PidTagIpmSubTreeEntryId = 0x35E0,
        [Description("EntryID of the Deleted Items Folder object")]
        PidTagIpmWastebasketEntryId = 0x35E3,
        [Description("EntryID of the search Folder object")]
        PidTagFinderEntryId = 0x35E7
    }
}
