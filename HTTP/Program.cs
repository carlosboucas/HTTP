using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HTTP
{
    class Program
    {

        enum Menu { Lista = 1, Individual = 2, Sair = 0 }

        static void Main(string[] args)
        {
            bool escolheuSair = false;

            while (escolheuSair == false)
            {
                Console.WriteLine("Como você deseja ver as atividades?");
                Console.WriteLine("1-Lista completa\n2-Por id\n0-Sair");
                string opMenuStr = Console.ReadLine();
                int opMenu = int.Parse(opMenuStr);

                if (opMenu > 0 && opMenu < 3)
                {
                    Menu escolha = (Menu)opMenu;

                    switch (escolha)
                    {
                        case Menu.Lista:
                            ReqList();
                            break;
                        case Menu.Individual:
                            ReqUnica();
                            break;
                        case Menu.Sair:
                            escolheuSair = true;
                            break;
                    }
                }
            }

        }
        static void ReqList()
        {
            foreach (Tarefa tarefa in GetTarefas())
            {
                tarefa.Exibir();
            }

            Console.ReadKey();
            Console.Clear();

        }

        static List<Tarefa> GetTarefas(int id = 0)
        {
            string url = "https://jsonplaceholder.typicode.com/todos";

            if (id != 0)
            {
                url += "?id=" + id;
            }
            var requisicao = WebRequest.Create(url); // + id
            requisicao.Method = "GET";
            using var resposta = requisicao.GetResponse();

            var stream = resposta.GetResponseStream();
            StreamReader leitor = new StreamReader(stream);
            object dados = leitor.ReadToEnd();

            List<Tarefa> tarefas = JsonConvert.DeserializeObject<List<Tarefa>>(dados.ToString());
            stream.Close();
            resposta.Close();

            return tarefas;
        }

        static void ReqUnica()
        {
            Console.WriteLine("Informe o número do id: ");
            int id = int.Parse(Console.ReadLine());

            var tarefas = GetTarefas();

            if(id > tarefas.Count)
            {
                Console.WriteLine($"Atualmente existem {tarefas.Count} tarefas. Favor informar um id válido");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            var tarefa = GetTarefas(id)[0];

            tarefa.Exibir();

            Console.ReadKey();
            Console.Clear();
        }
    }
}