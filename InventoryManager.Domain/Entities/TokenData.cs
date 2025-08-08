namespace InventoryManager.Domain.Entities
{
    public class TokenData
    {
        public string RefreshToken { get; set; }
        public Guid AccountId { get; set; }

        public TokenData()
        {
        }

        public TokenData(string refreshToken, Guid clientId)
        {
            RefreshToken = refreshToken;
            AccountId = clientId;
        }
    }
}
