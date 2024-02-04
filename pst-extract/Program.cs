using core;
using core.Messaging;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;

namespace PSTExtractor;

class Program
{
    public static void Main(string[] args)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        string path = "C:\\Users\\Dell\\Workstation\\SoftwareDevelopment\\Dotnet\\NugetLibraries\\Personal\\PSTExtractionLibrary\\sample-pst\\source.pst";
        Folder rootFolder;
        using (var memoryMappedFile = MemoryMappedFile.CreateFromFile(path, FileMode.Open))
        {
            var pst = new PST(memoryMappedFile);
            var message = new MessageStore(pst, SpecialInternalNId.NID_MESSAGE_STORE);
            rootFolder = new Folder(message.RootFolderEntryId.Nid, new List<string>()
                , pst.NodeBTPage.BTPageEntries, pst.BlockBTPage.BTPageEntries);
            var namedPropertyLookup = new NamedPropertyLookup(pst.NodeBTPage.BTPageEntries, pst.BlockBTPage.BTPageEntries);
        }
        Stack<Folder> folderStack = new Stack<Folder>();
        folderStack.Push(rootFolder);
        while (folderStack.Count > 0)
        {
            var curFolder = folderStack.Pop();
            if (curFolder.SubFolders != null && curFolder.SubFolders.Count != 0)
                foreach (var child in curFolder.SubFolders)
                    folderStack.Push(child);
            foreach (var item in curFolder)
            {
                if (item is MessageObject)
                {
                    var messageObject = item as MessageObject;
                    Console.WriteLine(messageObject.Subject);
                    Console.WriteLine("Sender Name: " + messageObject.SenderName);
                }
                else
                {
                    Console.WriteLine("+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+");
                    Console.WriteLine("Any Type of other object");
                    Console.WriteLine("Any Type of other object");
                    Console.WriteLine("+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+");
                }
            }
        }
        stopWatch.Stop();
        Console.WriteLine("Time Taken " + stopWatch.Elapsed);
    }
}
