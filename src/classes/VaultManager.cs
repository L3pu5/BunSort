using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Collections.Generic;


public class VaultManager{
    //Static Elements
    static string RecordLocation = "bunVault.vault";
    static string SettingsLocation = RecordLocation + ":SETTINGS";
    public RecordCollection Records;
    public VaultSettings Settings;
    
    bool hasRecord = false;

    public void DumpSettings(){
        Console.WriteLine("> Dumping Vault Settings to Terminal:");
        DumpExtensions();
    }

    public void DumpExtensions(){
        Settings.DumpExtensions();
    }

    void checkForVault(){
        if (File.Exists(RecordLocation)){
            hasRecord = true;
        }
    }

    public void readVault(){ 
        Console.WriteLine($"> Found a local vault at {Directory.GetCurrentDirectory()}" );
        //Read in the records
        XmlSerializer serialiser = new XmlSerializer (typeof(RecordCollection)); 
        using (Stream stream = new FileStream(RecordLocation, FileMode.Open, FileAccess.Read, FileShare.None))
        {
            Records = (RecordCollection) serialiser.Deserialize(stream);
        }

        //Read in the vault settings
        serialiser = new XmlSerializer (typeof(VaultSettings)); 
        using (Stream stream = new FileStream(SettingsLocation, FileMode.Open, FileAccess.Read, FileShare.None))
        {
            Settings = (VaultSettings) serialiser.Deserialize(stream);
        }
    }
    
    public void writeVault(){
        Console.WriteLine($"> Writing a local vault at {Directory.GetCurrentDirectory()}" );
        //Write out the records
        XmlSerializer serialiser = new XmlSerializer (typeof(RecordCollection)); 
        using (Stream stream = new FileStream(RecordLocation, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
        {
            serialiser.Serialize(stream, Records);
        }

        //Write out the vautl settings
        serialiser = new XmlSerializer (typeof(VaultSettings)); 
        using (Stream stream = new FileStream(SettingsLocation, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
        {
            serialiser.Serialize(stream, Settings);
        }
    }

    public void AddAll(bool verbose = false){
        foreach(string file in Directory.EnumerateFiles(".")){
            string[] sections = file.Split('.');
            string extension = sections[sections.Length-1];
            if(Settings.AcceptedExtension(extension)){
                if (Records.CheckForRecord(file))
                    continue;
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("New file: " + file +".");
                DateTime creationTime = File.GetCreationTime(file);
                Console.WriteLine("        : " + creationTime);
                Console.WriteLine("Do you wish to add it? [Y/n]");
                string response = Console.ReadLine();
                Record newRecord = new Record();
                if(checkAffirm(response)){
                    //Check for NSFW.
                    Console.WriteLine("Is the piece NSFW?");
                    if(checkAffirm(Console.ReadLine()))
                        newRecord.Tags.Add("NSFW");
                    //Check to see if Date is correct.

                    Console.WriteLine($"Accept date: {creationTime}? [Y/n]");
                    response = Console.ReadLine();
                    if(checkAffirm(response)){
                        newRecord.Day = (byte) creationTime.Day;
                        newRecord.Month = (Month) creationTime.Month;
                        newRecord.Year = (ushort) creationTime.Year;
                    }
                    else{
                        Console.Write("Day [0-31]: ");
                        int parseInt;
                        int.TryParse(Console.ReadLine(), out parseInt);
                        newRecord.Day = (byte) parseInt;
                        Console.Write("Month [1-12]: ");
                        int.TryParse(Console.ReadLine(), out parseInt);
                        newRecord.Month = (Month) (parseInt -1);
                        Console.Write("Year [XXXX]: ");
                        int.TryParse(Console.ReadLine(), out parseInt);
                        newRecord.Year = (ushort) parseInt;
                    }
                }
                else{
                    continue;
                }
                if(verbose){
                    Console.WriteLine("Who are the characters in this piece? Separate them with colons.");
                    response = Console.ReadLine();
                    string[] characters = response.Split(':');
                    foreach(string character in characters){
                        newRecord.Characters.Add(character.Trim().ToLower());
                    }
                    Console.WriteLine("Are there any other tags you would like to add? Separate them with colons.");
                    response = Console.ReadLine();
                    string[] tags = response.Split(':');
                    foreach(string tag in tags){
                        newRecord.Tags.Add(tag.Trim().ToLower());
                    }
                    newRecord.Path = file;
                    newRecord.Title = sections[1].Trim('\\');
                    Records.AddRecord(newRecord);
                }
                else{
                    newRecord.Path = file;
                    newRecord.Title = sections[1].Trim('\\');
                    Records.AddRecord(newRecord);
                }
            }
        }
        Console.WriteLine("---------------------------------------------");
        Console.WriteLine("No new files.");
    }

    bool checkAffirm(string input){
        if(input == "" || input == "y" || input == "Y" || input == "yes" || input == "Yes")
            return true;
        return false;
    }

    public void dumpRecords()
    {
        Records.dump();
    }

    void createVault(){
        Records = new RecordCollection();
        Settings = new VaultSettings();
        //TODO: Remove me
        Settings.AddExtension("clip");
        Settings.AddExtension("png");
    }

    public VaultManager(){

        //Check for an existin vault file in this location;
        checkForVault();
        //If there is no vault, create a new in-memory vault.
        if(!hasRecord){
            createVault();
        } else {
            readVault();
        }

    }
}

[Serializable()]
public class VaultSettings{
    public List<string> Extensions = new List<string>();

    public void DumpExtensions(){
        Console.Write("> Extensions: ");
        foreach(string extension in Extensions){
            Console.Write($".{extension} ");
        }
        Console.Write("\r\n");
    }

    public bool AcceptedExtension(string extension){
        return Extensions.Contains(extension);
    }


    public void AddExtension(string extension)
    {
        if(!Extensions.Contains(extension))
            Extensions.Add(extension);
    }

    public void RemoveExtension(string extension){
        if(Extensions.Contains(extension))
            Extensions.Remove(extension);
    }
}