// <copyright file="Globals.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

namespace BaseAdminProject.Business.Core
{
    public static class Globals
    {
        public const string StatusMessageKey = "StatusMessage";
        public const string StatusMessageTypeKey = "StatusMessageType";

        // Error messages
        public const string RequiredMessage = "O campo {0} é obrigatório";
        public const string EmailRequiredMessage = "Insira um endereço de email válido.";

        // Error Messages Type
        public const string StatusMessageTypeSuccess = "success";
        public const string StatusMessageTypeDanger = "danger";
    }
}
