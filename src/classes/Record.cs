using System;
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
            Console.WriteLine($"Record: {record.ToString()}");
        }
    }
}