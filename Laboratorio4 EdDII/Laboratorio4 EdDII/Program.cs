using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using JsonSerializer = System.Text.Json.JsonSerializer;
class Program
{
    public class User
    {
        public string name { get; set; }
        public string dpi { get; set; }
        public string datebirth { get; set; }
        public string address { get; set; }
        public string[] companies { get; set; }
        public string[] Chats = new string[10];
        public int[] ChatsLenght = new int[10];
        public int CantChats = 0;
    }

    public class UserRegister
    {
        public string name { get; set; }
        public string dpi { get; set; }
        public string address { get; set; }
        public string datebirth { get; set; }
        public string[] companies = new string[100];
        public string[] Chats = new string[20];
        public int[] ChatsLenght = new int[20];

    }
    bool LLaveConfirmada = false;
    string Encryption(string Message, int[] LLave)
    {
        string[,] DES = new string[LLave.Length, (Message.Length / LLave.Length) + 1];
        int m = 0;
        for (int i = 0; i < (Message.Length / LLave.Length) + 1; i++)
        {
            for (int j = 0; j < LLave.Length; j++)
            {
                if (m < Message.Length)
                {
                    DES[j, i] = Message.Substring(m, 1);
                }
                m++;
            }
        }
        string Encrypted = "";
        for (int i = 0; i < LLave.Length; i++)
        {
            for (int j = 0; j < LLave.Length; j++)
            {
                if (LLave[j] == i)
                {
                    for (int k = 0; k < (Message.Length / LLave.Length) + 1; k++)
                    {
                        Encrypted = Encrypted + DES[j, k];
                    }
                }
            }
        }
        return Encrypted;
    }
    string DesEncryption(string EncMessage, int[] LLave)
    {
        string[,] DES = new string[LLave.Length, (EncMessage.Length / LLave.Length)];
        int Recursión = (EncMessage.Length / LLave.Length) + 1;
        int RecursiónBase = 0;
        int m = 0;
        for (int i = 0; i < LLave.Length; i++)
        {
            for (int j = 0; j < LLave.Length; j++)
            {
                if (LLave[j] == i)
                {
                    for (int k = 0; k < EncMessage.Length / LLave.Length; k++)
                    {
                        if (i == 0)
                        {
                            DES[j, k] = EncMessage.Substring(m, 1);
                        }
                        if (i != 0 && k + RecursiónBase < EncMessage.Length)
                        {
                            DES[j, k] = EncMessage.Substring(m, 1);
                        }
                        m++;
                    }
                }
            }
            RecursiónBase = Recursión + RecursiónBase;
        }
        string DesEncrypted = "";
        for (int i = 0; i < (EncMessage.Length / LLave.Length); i++)
        {
            for (int j = 0; j < LLave.Length; j++)
            {
                if (DES[j, i] != "*")
                {
                    if (DES[j, i] == "_")
                    {
                        DesEncrypted = DesEncrypted + " ";
                    }
                    else
                    {
                        DesEncrypted = DesEncrypted + DES[j, i];
                    }
                }
            }
        }
        DesEncrypted.Replace('_', ' ');
        DesEncrypted.Replace('*', ' ');

        return DesEncrypted;
    }
    public static string ToSHA256(string s)
    {
        using var sha256 = SHA256.Create();
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));

        var sb = new StringBuilder();
        for(int i = 0; i < bytes.Length; i++)
        {
            sb.Append(bytes[i].ToString("x2"));
        }
        return sb.ToString();
    }

    static void Main(string[] args) {
        Console.WriteLine("¿Cuál desea que sea su llave?");
        string Key = Console.ReadLine();
        //Se ordena alfabéticamente la llave//
        int filling = 1;
        int[] Order = new int[Key.Length];
        for (int i = 0; i < Key.Length; i++)
        {
            int Pos = 0;
            for (int j = 0; j < Key.Length; j++)
            {
                string Actual = Key.Substring(j, 1);
                if (Key.Substring(i, 1).CompareTo(Actual) > 0)
                {
                    Pos++;
                }
            }
            Order[i] = Pos;
        }
        bool[] reps = new bool[Order.Length];
        for (int i = 0; i < Order.Length; i++)
        {
            int repeticiones = 0;
            for (int j = i; j >= 0; j--)
            {
                if (Order[i] == Order[j])
                {
                    repeticiones++;
                }
            }
            repeticiones--;
            Order[i] = Order[i] + repeticiones;
        }
        string jsonText = File.ReadAllText(@"C:\Users\oswal\OneDrive\Documentos\Universidad\6. 4to Ciclo\Estructura de datos\Labs\Lab 4\input.json");
        string[] jsonObjects = jsonText.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        int Size = jsonObjects.Length - 1;
        string[] jsonObjectsT = new string[Size];
        string[] jsonObjectsData = new string[Size];
        string[] jsonObjectsAction = new string[Size];
        for (int i = 0; i < Size; i++)
        {
            jsonObjectsT = jsonObjects[i].Split(';', 2);
            jsonObjectsAction[i] = jsonObjectsT[0];
        }
        for (int i = 0; i < Size; i++)
        {
            jsonObjectsT = jsonObjects[i].Split(';', 2);
            jsonObjectsData[i] = jsonObjectsT[1];
        }
        //Se consiguen los datos del JSON//
        User[] Usuario = new User[Size];
        //Se consiguen los datos del JSON//

        //Se consiguen las cartas de recomendación//
        string directorio = @"C:\Users\oswal\OneDrive\Documentos\Universidad\6. 4to Ciclo\Estructura de datos\Labs\Lab 4\inputs"; // Reemplaza esto con la ruta de tu directorio
        // Verifica si el directorio existe
        int SearchRecomendations = 0;
        string[] archivos = Directory.GetFiles(directorio);
        string[] DPIRecomendation = new string[archivos.Length];
        string[] ChatsArchive = new string[archivos.Length];
        int ArchivesAnalyze = 0;
        foreach (string archivo in archivos)
        {
            DPIRecomendation[ArchivesAnalyze] = archivo.Substring(103, 13);
            TextReader Read = new StreamReader(archivo);
            string Temp = Read.ReadToEnd();
            ChatsArchive[ArchivesAnalyze] = Temp;
            ArchivesAnalyze++;
        }
        Program Procesos = new Program();
        SearchRecomendations = archivos.Length;
        int count = 0;
        for (int i = 0; i < Size - 1; i++)
        {
            string eleccion = "";
            User input = JsonSerializer.Deserialize<User>(jsonObjectsData[i])!;
            eleccion = jsonObjectsAction[i];
            switch (eleccion)
            {
                case "INSERT":
                    Usuario[i] = input;
                    for (int j = 0; j < SearchRecomendations; j++)
                    {
                        if (DPIRecomendation[j] == input.dpi)
                        {
                            string Signature = ToSHA256(ChatsArchive[j]);
                            Signature = Procesos.Encryption(Signature, Order);
                            string TempChat = Procesos.Encryption(ChatsArchive[j], Order);
                            string SignedDoc = TempChat + Signature;
                            Usuario[i].ChatsLenght[Usuario[i].CantChats] = TempChat.Length;
                            Usuario[i].Chats[Usuario[i].CantChats] = SignedDoc;
                            Usuario[i].CantChats++;
                        }
                    }
                    count++;
                    break;
                case "DELETE":
                    string DPIDelete = input.dpi;
                    for (int j = 0; j < count; j++)
                    {
                        if (Usuario[j].dpi == DPIDelete)
                        {
                            Usuario[j] = null;
                            for (int k = j; k < count; k++)
                            {
                                Usuario[k] = Usuario[k + 1];
                            }
                            count--;
                        }
                    }

                    break;
                case "PATCH":
                    string DPIPatch = input.dpi;
                    for (int j = 0; j < count; j++)
                    {
                        if (Usuario[j].dpi == DPIPatch)
                        {
                            Usuario[j] = null;
                            Usuario[j] = input;
                            for (int k = 0; k < SearchRecomendations; k++)
                            {
                                if (DPIRecomendation[k] == input.dpi)
                                {
                                    string TempRecomendacion = Procesos.Encryption(ChatsArchive[k], Order);
                                    Usuario[j].Chats[Usuario[j].CantChats] = TempRecomendacion;
                                    Usuario[j].CantChats++;
                                }
                            }
                        }
                    }
                    break;
            }
        }
        
        Console.WriteLine(ChatsArchive[92]);
    }
}
