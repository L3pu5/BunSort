using System;

namespace BunSort
{
    class Program
    {
        static void Main(string[] args)
        {
            bool continueProgram = true;
            VaultManager Vault = new VaultManager();
            string userInput = "";
            //Provide the user with the ability to query the current file, also make this later available as an argument
            while(continueProgram){
                Console.Write($"> ");
                userInput = Console.ReadLine();
                Console.WriteLine($"--> {userInput}");
                string[] command = userInput.Split(' ');
                if(command[0] == "seek"){
                    
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
