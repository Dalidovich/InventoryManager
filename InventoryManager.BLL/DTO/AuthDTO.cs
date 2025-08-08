namespace InventoryManager.BLL.DTO
{
    public record AuthDTO
    {
        public string JWTToken { get; set; }
        public string RefreshToken { get; set; }
        public Guid AccountId { get; set; }

        public AuthDTO(string jWTToken, Guid accountId, string refreshToken)
        {
            JWTToken = jWTToken;
            AccountId = accountId;
            RefreshToken = refreshToken;
        }
    }
}
