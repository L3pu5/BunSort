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
                if(command.Length == 2){
                    switch(command[0]){
                        case "read":
                            if(command[1] == "." || command[1] == "*" || command[1] == "all"){
                                Vault.AddAll(false);
                            }
                            break;
                        case "add":
                            if(command[1] == "records"){
                                Vault.Records.AddRecord(new Record(9,9,9));
                            }
                            break;
                        case "dump":
                            if(command[1] == "extensions" || command[1] == "exts" || command[1] == "ext"){
                                Vault.DumpExtensions();
                            }
                            break;
                        case "_":
                            break;
                    }
                }

                if(userInput == "read records"){
                    Vault.Records.dump();
                }

                if(userInput == "write"){
                    Vault.writeVault();
                }

                if(userInput == "read"){
                    Vault.readVault();
                }

                if(userInput == "quit" || userInput == "exit"){
                    Console.WriteLine("> Goodbye!");
                    return;
                }

            }
        }
    }
}
