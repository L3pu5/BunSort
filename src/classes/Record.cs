using System;
using System.Linq;
using System.Collections.Generic;

public enum Month
{
    JAN,
    FEB,
    MAR,
    APR,
    MAY,
    JUN,
    JUL,
    AUG,
    SEP,
    OCT,
    NOV,
    DEC
}

/// <summary>
/// 
/// </summary>
[Serializable()]
public class ShortDate{
    public ushort Year = 2023;
    public Month Month = Month.JAN;
    public byte Day = 0;

    public override string ToString(){
        return  $"Date: {Day}/{((int) Month) + 1}/{Year}"; 
    }

    /// <summary>
    /// Creates a ShortDate instance.
    /// </summary>
    /// <param name="day">The date of the month</param>
    /// <param name="month">The number of the month</param>
    /// <param name="year">The number of the year</param>
    public ShortDate(byte day, int month, ushort year){
        this.Year = year;
        this.Month = (Month) month;
        this.Day = day;
    }

    public ShortDate(int day, int month, int year){
        this.Year = (ushort) year;
        this.Month = (Month) month;
        this.Day = (byte) day;
    }

    public ShortDate()
    {

    }
}

[Serializable()]
public class Record : ShortDate {
    public string Title = "";
    public string Path = "";
    public List<string> AlternatePaths = new List<string>();
    public List<string> Tags = new List<string>();
    public List<string> Characters = new List<string>();

    public override string ToString(){
        string output = "RECORD: ";
        if(Title != "")
            output += $"Title: {Title} ";
        if(Tags.Contains("NSFW"))
            output += $"NSFW: [X] ";
        else
            output += $"NSFW: [ ] ";
        
        //output += base.ToString();
        return output;
    }

    /// <summary>
    /// Creates a Record of a given object. Extends ShortDate.
    /// </summary>
    /// <param name="day">The date of the month</param>
    /// <param name="month">The number of the month</param>
    /// <param name="year">The number of the year</param>
    /// <returns></returns>
    public Record(byte day, int month, ushort year) : base (day, month, year){

    }

    public Record(){

    }
}

[Serializable()]
public class RecordCollection{
    public List<Record> Records = new List<Record>();

    
    public void SeekCharacter(string character){
        string _character = character.ToLower();
        foreach (Record record in Records.Where(x => x.Characters.Contains(_character))){
            Console.WriteLine(record.ToString());
        }
    }

    public void Seek(byte date, int month,ushort year, List<string> characters, List<string> tags, List<string> extensions, bool nsfw){
        //Descent Seek.
        List<Record> query = new List<Record>(Records);
        //
        if(date != 255){
            query = query.Where(x => x.Day == date).ToList();
        }
        if(month != 255){
            query = query.Where(x => x.Month == (Month) month).ToList();
        }
        if(year != 255){
            query = query.Where(x => x.Year == year).ToList();
        }
        if(characters.Count != 0){
            foreach(string character in characters){
                query = query.Where(x => x.Characters.Contains(character)).ToList();
            }
        }

        if(tags.Count != 0){
            foreach(string tag in tags){
                query= query.Where(x => x.Tags.Contains(tag)).ToList();
            }
        }

        if(extensions.Count != 0){
            //skip
        }
        if(nsfw)
        {
            query = query.Where(x => x.Tags.Contains("NSFW")).ToList();
            //skip
        }
        else{
            query = query.Where(x => !x.Tags.Contains("NSFW")).ToList();
        }

        Console.WriteLine("--------------------------------");
        Console.WriteLine($"Found {query.Count} entries.");
        foreach(Record r in query){
            Console.WriteLine($"{r.ToString()}");
        }
        Console.WriteLine("--------------------------------");
    }

    public void AddRecord(Record record){
        Records.Add(record);
    }

    public bool CheckForRecord(string fileName){
        foreach(Record record in Records){
            if(record.Path == fileName)
                return true;
        }
        return false;
    }

    public void dump(){
        foreach (Record record in Records)
        {
            Console.WriteLine($"{record.ToString()}");
        }
    }
}