using System;
using System.Collections.Generic;

namespace BunSort
{
    class Program
    {
        static void Main(string[] args)
        {
            bool continueProgram = true;
            List<string> history = new List<string>();
            //int historyIndex = 0;



            VaultManager Vault = new VaultManager();
            string userInput = "";
            //Provide the user with the ability to query the current file, also make this later available as an argument
            while(continueProgram){
                StartLoop:
                Console.Write($"> ");
               // ConsoleKeyInfo keyPress = Console.ReadKey();
                // if(keyPress.Key == ConsoleKey.UpArrow){
                //     Console.WriteLine("UP");
                //     SendKeys.SendWait(history[historyIndex-1]);
                //     goto StartLoop;
                // }
                userInput = Console.ReadLine().ToLower();
                Console.WriteLine($"--> {userInput}");
                //Increment History
                history.Add(userInput);
                // historyIndex++;

                //Handle User Input
                string[] command = userInput.Split(' ');
                if(command[0] == "seek"){
                    byte date = 255;
                    int month = 255;
                    ushort year = 255;
                    int parseInt = -1;
                    List<string> characters = new List<string>();
                    List<string> tags = new List<string>();
                    List<string> extensions = new List<string>();
                    bool nsfw = false;
                    for(int i = 1; i < command.Length; i++){
                        if(command[i] == "-year" || command[i] == "-y"){
                            i+= 1;
                            if(i == command.Length)
                                continue;
                            int.TryParse(command[i], out parseInt);
                            year = (ushort) parseInt;
                            continue;
                    }
                        if(command[i] == "-month" || command[i] == "-m"){
                            i +=1;
                            if(i == command.Length)
                                continue;
                            if(!int.TryParse(command[i], out parseInt)){
                                // How else do we parse month?
                            }else{
                                month = (parseInt -1);
                            }
                            continue;
                        }
                        if(command[i] == "-day" || command[i] == "-date" || command[i] == "-d"){
                            i+=1;
                            if(i == command.Length)
                                continue;
                            int.TryParse(command[i], out parseInt);
                            date = (byte) parseInt;
                            continue;
                        }
                        if(command[i] == "-c" || command[i] == "-char" || command[i] == "-chars"){
                            i+=1;
                            while(i < command.Length && command[i].Substring(0, 1) != "-" ){
                                characters.Add(command[i]);
                                Console.WriteLine(command[i]);
                                i++;
                                if(i == command.Length){
                                    Vault.Records.Seek(date, month, year, characters, tags, extensions, nsfw);
                                    goto StartLoop;
                                }
                            }
                        }
                        if(command[i] == "-t" || command[i] == "-tag" || command[i] == "-tag"){
                            i+=1;
                            while(i < command.Length && command[i][0] != '-'){
                                tags.Add(command[i]);
                                i++;
                                if(i == command.Length){
                                    Vault.Records.Seek(date, month, year, characters, tags, extensions, nsfw);
                                    goto StartLoop;
                                }
                            }
                        }
                        if(command[i] == "-e" || command[i] == "-ext"){
                            i+=1;
                            while(command[i][0] != '-' && i < command.Length){
                                extensions.Add(command[i]);
                                i++;
                            }
                        }
                        if(command[i] == "-x" || command[i] == "-hecky" || command[i] == "-nsfw")
                        {
                            nsfw = true;
                        }
                    }
                    Vault.Records.Seek(date, month, year, characters, tags, extensions, nsfw);
                    goto StartLoop;;
                }

                if(command.Length == 2){
                    switch(command[0]){
                        case "read":
                            if(command[1] == "." || command[1] == "*" || command[1] == "all"){
                                Vault.AddAll(false);
                            }
                            break;
                        case "add":
                            break;
                        case "find":
                            break;
                        case "char":
                            Vault.Records.SeekCharacter(command[1]);
                            break;
                        case "dump":
                            if(command[1] == "extensions" || command[1] == "exts" || command[1] == "ext"){
                                Vault.DumpExtensions();
                                break;
                            }
                            if(command[1] == "records")
                                Vault.Records.dump();
                            break;
                        case "_":
                            break;
                    }
                }

                if(command.Length == 3){
                    switch(command[0]){
                        case "read":
                            if(command[1] == "." || command[1] == "*" || command[1] == "all"){
                                if(command[2] == "v" || command[2] == "-v")
                                    Vault.AddAll(true);
                            }
                            break;
                        case "_":
                            break;
                    }
                }

                if(userInput == "write"){
                    Vault.writeVault();
                }

                if(userInput == "quit" || userInput == "exit"){
                    Vault.writeVault();
                    Console.WriteLine("> Goodbye!");
                    return;
                }

            }
        }
    }
}
