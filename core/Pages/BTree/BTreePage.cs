﻿using Core.PST.Pages.Base;
using System;
using System.Collections.Generic;

namespace Core.PST.Pages.BTree
{
    /// <summary>
    /// BTrees are widely used throughout the PST file format. 
    /// In the NDB Layer, BTrees are the building blocks for the NBT and BBT, 
    /// which are used to quickly navigate and search nodes and blocks.The PST
    /// file format uses a general BTree implementation that supports up to 8 
    /// intermediate levels.
    /// 
    /// A BTPAGE structure implements a generic BTree using 512-byte pages. 
    /// </summary>
    public class BTreePage : Page
    {
        public const string PageTypeName = "Block Tree Page(BTPage)";
        /// <summary>
        /// Lists all the BTPageEntries. This BTPageEntries can be 
        /// - BTEntry
        ///     BTEntry is a type of entry which specifies that the current node
        ///     is an root node or an intermediate node.
        /// - NodeBTEntry (NBTEntry)
        ///     NodeBTEntry is a type of entry which specifies that the current node
        ///     is an NodeBTree child node.
        /// - BlockBTreeEntry (BBTEntry)
        ///     BlockBTEntry is a type of entry which specifies that the current node
        ///     is an BlockBTree child node.
        /// </summary>
        public List<IBTPageEntry> BTPageEntries { get; set; }
        /// <summary>
        /// Type of BTree NodeBTree(NBT) or BlockBTree(BBT)
        /// </summary>
        public BTreeType BTreeType { get; set; }
        /// <summary>
        /// 
        ///     BTree Type | cLevel           |   rgentries structure   |  cbEnt(bytes)
        ///     NBT        | 0                |   NBTENTRY              |  ANSI: 16, Unicode: 32 
        ///                | Greater than 0   |   BTENTRY               |  ANSI: 12, Unicode: 24
        ///     BBT        | 0                |   BBTENTRY              |  ANSI: 12, Unicode: 24
        ///                | Less than 0      |   BTENTRY               |  ANSI: 12, Unicode: 24
        /// 
        /// </summary>
        public BTreeEntryType BTreeEntryType { get; set; }
        /// <summary>
        /// rgentries (Unicode: 488 bytes; ANSI: 496 bytes): Entries of the BTree array. 
        /// The entries in the array depend on the value of the cLevel field.
        /// If cLevel is greater than 0, then each entry in the array is of type BTENTRY.
        /// If cLevel is 0, then each entry is either of type BBTENTRY or NBTENTRY, 
        /// depending on the ptype of the page.
        /// </summary>
        protected byte[] rgentries;
        /// <summary>
        /// cEnt (1 byte): The number of BTree entries stored in the page data.
        /// </summary>
        protected byte cEnt;
        /// <summary>
        /// cEntMax (1 byte): The maximum number of entries that can fit inside the page data.
        /// </summary>
        protected byte cEntMax;
        /// <summary>
        /// cbEnt (1 byte): The size of each BTree entry, in bytes. 
        /// Note that in some cases, cbEnt can be greater than the corresponding size 
        /// of the corresponding rgentries structure because of
        /// alignment or other considerations.
        /// Implementations MUST use the size specified in cbEnt to
        /// advance to the next entry.
        /// 
        /// 
        /// Tree Type | cLevel          | rgentries structure | cbEnt(bytes)
        /// NBT       |  0              | NBTENTRY            | ANSI: 16, Unicode: 32 
        ///           |  Greater than 0 | BTENTRY             | ANSI: 12, Unicode: 24 
        /// BBT       |  0              | BBTENTRY            | ANSI: 12, Unicode: 24
        ///           |  Less than 0    | BTENTRY             | ANSI: 12, Unicode: 24
        ///           
        /// </summary>
        protected byte cbEnt;
        /// <summary>
        /// cLevel (1 byte): The depth level of this page. 
        /// Leaf pages have a level of zero, whereas intermediate pages have a level greater than 0. 
        /// This value determines the type of the entries in rgentries, and interpreted as unsigned.
        /// </summary>
        protected byte cLevel;
        /// <summary>
        /// dwPadding (Unicode: 4 bytes): Padding; MUST be set to zero. 
        /// Note there is no padding in the ANSI version of this structure.
        /// </summary>
        protected Int32 dwPadding;
        /// <summary>
        /// pageTrailer (Unicode: 16 bytes; ANSI: 12 bytes): A PAGETRAILER structure (section 2.2.2.7.1). 
        /// The ptype subfield of pageTrailer MUST be set to ptypeBBT for a Block BTree page, or
        /// ptypeNBT for a Node BTree page.
        /// </summary>
        protected byte[] pageTrailer;
        public BTreePage(BTreeType bTreeType)
        {
            this.BTreeType = bTreeType;
        }
    }
}
