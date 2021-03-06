﻿using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;

namespace WebApiRfc.Security
{
    public static class SystemLdap
    {
        private const string SRVR = "LDAP://SEU_LINK_AD";
        /// <summary>
        /// Metodo de cominicação do LDAP
        /// </summary>
        /// <param name="user">Login utilizado pelo usuário Ex:Matricula</param>
        /// <param name="password">Senha usuado pelo usuário</param>
        /// <returns>
        /// SUCESSO: Usuário <NOME_AD> Autenticado!
        /// ERRO: "ERRO: Usuário/Senha Inválido!" - SENHA ERRADA.
        /// </returns>
        public static string createDirectoryEntry(string user, string password)
        {
            var messagem = string.Empty;
            try
            {   
                //Abre a conexão com o AD.
                DirectoryEntry directoryEntry = new DirectoryEntry(SRVR, user, password);
                DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry);
                
                //Busca pela squema do AD
                directorySearcher.Filter = "(SAMAccountName=" + user + ")";
                SearchResult searchResult = directorySearcher.FindOne();

                //Busca pelas propriedades se o usuário e valido.
                if ((Int32)searchResult.Properties["userAccountControl"][0] == 512)
                {
                    //Utilizando as propriedades preenchidas no AD trás a informações que deseja exibir
                    //basta mundar o valor [""] pela propriedade que contem no  'searchResult.Properties'.
                    var name = searchResult.Properties["name"][0];
                    var email = searchResult.Properties["mail"][0];
                    messagem = string.Format("Usuário {0} - {1} Autenticado!", name, email);
                }
                else
                {
                    messagem = "ERRO: Usuário/Senha Inválido!";
                }
            }
            catch (Exception ex)
            {
                messagem = string.Format("Erro: {0}, Link Ajuda: {1}", ex.Message, ex.HelpLink);
            }

            return messagem;
        }
    }
}