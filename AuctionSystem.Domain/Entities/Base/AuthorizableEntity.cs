using AuctionSystem.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionSystem.Domain.Entities.Base
{
    public class AuthorizableEntity : EntityBase
    {
        [Column("INITIATOR")]
        public string Initiator { get; set; }
        [Column("INITIATOR_EMAIL")]
        public string InitiatorEmail { get; set; }
        [Column("DATE_INITIATED")]
        public DateTime DateInitiated { get; set; }
        [Column("AUTHORIZER")]
        public string Authorizer { get; set; }
        [Column("AUTHORIZER_EMAIL")]
        public string AuthorizerEmail { get; set; }
        [Column("DATE_AUTHORIZED")]
        public DateTime? DateAuthorized { get; set; }
        [Column("AUTHORIZERS_COMMENT")]
        public string AuthorizersComment { get; set; }
        [Column("AUTH_STATUS")]
        public AuthStatus AuthStatus { get; set; }
    }
}
