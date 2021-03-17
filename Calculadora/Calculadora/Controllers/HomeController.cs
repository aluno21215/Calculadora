using Calculadora.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Calculadora.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Invicação da View, em modo Get
        /// </summary>
        /// <returns></returns>
        [HttpGet] //facultativa, por defeito get
        public IActionResult Index()
        {

            //prepara os valores iniciais do visor
            ViewBag.Visor = "0";
            ViewBag.PrimeiroOperador = "Sim";
            ViewBag.Operador = "";
            ViewBag.PrimeiroOperando = "";
            ViewBag.LimpaVisor = "Sim";

            return View();
        }


        /// <summary>
        /// Invocação da View, em modo Post
        /// </summary>
        /// <param name="botao">operador pelo utilizador</param>
        /// <param name="visor">valor do visor da calculadora</param>
        /// <param name="primeiroOperador">var aux: Já foi escolhido um operador, ou não ...</param>
        /// <param name="primeiroOperando">var aux: Primeiro Operando na a ser utilizado na operação</param>
        /// <param name="operador">var aux: Operador a ser utilizado na operação</param>
        /// <param name="limpaVisor">var aux: Se 'Sim' limpa visor, caso contrário não</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(string botao, string visor, string primeiroOperador,
                                                string primeiroOperando, string operador, string limpaVisor)
        {

            //avaliar o valor associado à variável 'botao'
            switch (botao) {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "0":
                    //atribuir ao 'visor' o valor do 'botao'
                    if (visor == "0" || limpaVisor == "Sim") { visor = botao;}
                    else {
                        visor += botao;
                    }
                    limpaVisor = "Nao";
                    break;

                case "+/-":
                    //faz a inversão do valor do visor
                    if (visor.StartsWith('-')) { visor = visor.Substring(1); }
                    else { visor = "-" + visor; }
                        break;

                case ",":
                    //faz a gestão da parte decimal do número no visor
                    if (!visor.Contains(",")) { visor += ","; }
                    break;

                case "+":
                case "-":
                case "x":
                case ":":
                case "=":

                    limpaVisor = "Sim";
                    if (primeiroOperador != "Sim")
                    {
                        //esta é a 2ª vez (ou mais) que se selecionou um 'operadior'
                        //efetuar a operação com o operador antigo, e os valores dso opearandos
                        double operando1 = Convert.ToDouble(primeiroOperando);
                        double operando2 = Convert.ToDouble(visor);
                        //efetuar a operação aritmética
                        switch (operador)
                        {
                            case "+":
                                visor = operando1 + operando2 + "";
                                break;
                            case "-":
                                visor = operando1 - operando2 + "";
                                break;
                            case "x":
                                visor = operando1 * operando2 + "";
                                break;
                            case ":":
                                visor = operando1 / operando2 + "";
                                break;
                        }
                    }//fim if

                        //armazenar os valores atuais para cálculos futuros
                        //Visor atual, que passa a '1º operando'
                        primeiroOperando = visor;
                        //guardar o valor do 1 operador
                        operador = botao;
                    if(botao != "=") {
                        //assinal o que se vai fazer com os operadores
                        primeiroOperador = "Nao"; }
                    else{ primeiroOperador = "Sim"; }
                    
     
                    break;

                case "C":
                    visor = "0";
                    primeiroOperador = "Sim";
                    operador = "";
                    primeiroOperando = "";
                    limpaVisor = "Sim";
                    break;


            } //fim switch

            // Enviar o valor do 'visor' para a view
            ViewBag.Visor = visor;
            //preciso de manter o 'estado' das vars. aux
            ViewBag.PrimeiroOperador = primeiroOperador;
            ViewBag.Operador = operador;
            ViewBag.PrimeiroOperando = primeiroOperando;
            ViewBag.LimpaVisor = limpaVisor;

            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
